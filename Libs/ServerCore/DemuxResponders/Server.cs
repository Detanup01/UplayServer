using Core.Extra;
using Google.Protobuf;
using NetCoreServer;
using SharedLib.Server.Json;
using SharedLib.Shared;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using Uplay.Demux;

namespace Core.DemuxResponders
{
    public class DemuxServer
    {
        #region DemuxServer
        public static ConcurrentDictionary<Guid, DMXSession> DMXSessions = new ConcurrentDictionary<Guid, DMXSession>();
        static DMXServer server;
        public static bool IsLocal;
        public static void Start(bool isLocal = true)
        {
            Directory.CreateDirectory("logs");
            IsLocal = isLocal;
            SslContext context;
            if (IsLocal)
                context = new SslContext(SslProtocols.Tls12, Utils.GetCert("dmx_local", "dmx.local.upc.ubisoft.com"));
            else
                context = new SslContext(SslProtocols.Tls12, Utils.GetCert("dmx", "dmx.upc.ubisoft.com"));

            server = new DMXServer(context, IPAddress.Parse(ServerConfig.DemuxIp), ServerConfig.DemuxPort);
            Console.WriteLine("[DMX] Server Started");
            server.Start();
        }

        public static void Stop()
        {
            server.Stop();
            Console.WriteLine("[DMX] Server Stopped");
        }

        public static DMXServer GetServer()
        {
            return server;
        }

        #region SendtoClients
        /// <summary>
        /// Demux Sent To Client
        /// </summary>
        /// <param name="ClientNumber">SSL Client Number</param>
        /// <param name="down">Uplay.Demux Downstream data</param>
        public static void SendToClient(Guid ClientNumber, Downstream down)
        {
            if (DMXSessions.TryGetValue(ClientNumber, out var session) && !session.IsClosed)
            {
                session.Send(Formatters.FormatUpstream(down.ToByteArray()));
            }
        }

        /// <summary>
        /// Demux Sent To Client
        /// </summary>
        /// <param name="ClientNumber">SSL Client Number</param>
        /// <param name="message">Push DataMessage</param>
        public static void SendToClient(Guid ClientNumber, DataMessage message)
        {
            if (DMXSessions.TryGetValue(ClientNumber, out var session) && !session.IsClosed)
            {
                Downstream downstream = new()
                {
                    Push = new()
                    {
                        Data = message
                    }
                };
                session.Send(Formatters.FormatUpstream(downstream.ToByteArray()));
            }
        }

        /// <summary>
        /// Demux Sent To Client
        /// </summary>
        /// <param name="ClientNumber">SSL Client Number</param>
        /// <param name="bstr">ByteString data</param>
        /// <param name="conId">ConnectionId</param>
        public static void SendToClient(Guid ClientNumber, ByteString bstr, uint conId)
        {
            if (DMXSessions.TryGetValue(ClientNumber, out var session) && !session.IsClosed)
            {
                Downstream downstream = new()
                {
                    Push = new()
                    {
                        Data = new()
                        {
                            ConnectionId = conId,
                            Data = bstr
                        }
                    }
                };
                session.Send(Formatters.FormatUpstream(downstream.ToByteArray()));
            }
        }
        #endregion
        #region Multicast
        public static void SendToAllClient(Downstream down)
        {
            server.Multicast(Formatters.FormatUpstream(down.ToByteArray()));
        }
        public static void SendToAllClient(DataMessage message)
        {
            Downstream down = new()
            {
                Push = new()
                {
                    Data = message
                }
            };
            server.Multicast(Formatters.FormatUpstream(down.ToByteArray()));
        }
        public static void SendToAllClient(ByteString bstr, uint conId)
        {
            Downstream down = new()
            {
                Push = new()
                {
                    Data = new()
                    {
                        ConnectionId = conId,
                        Data = bstr
                    }
                }
            };
            server.Multicast(Formatters.FormatUpstream(down.ToByteArray()));
        }
        #endregion
        #endregion
        #region DMXServer
        public class DMXServer : SslServer
        {
            public List<Guid> ConnectedIds { get; set; } = new();
            public DMXServer(SslContext context, IPAddress address, int port) : base(context, address, port) { }

            protected override SslSession CreateSession() { return new DMXSession(this); }

            protected override void OnError(SocketError error)
            {
                Console.WriteLine($"Chat SSL server caught an error with code {error}");
            }
        }
        #endregion
        #region DMXSession
        public class DMXSession : SslSession
        {
            public DMXServer Server;
            public bool IsClosed { get; internal set; } = false;
            public DMXSession(DMXServer server) : base(server)
            {
                Server = server;
            }

            protected override void OnConnected()
            {
                File.AppendAllText($"logs/client_{Id}.log", "Socket Connected: " + Socket.RemoteEndPoint + " | " + Socket.LocalEndPoint + "\n");
                DMXSessions.TryAdd(Id, this);
                Debug.PWDebug($"SSL session with Id {Id} connected!");
                Server.ConnectedIds.Add(Id);
                IsClosed = false;
            }

            protected override void OnDisconnected()
            {
                File.AppendAllText($"logs/client_{Id}.log", "Socket Disconnected: \n");
                DMXSessions.Remove(Id, out _);
                Debug.PWDebug($"SSL session with Id {Id} disconnected!");
                Server.ConnectedIds.Remove(Id);
                Demux.DeleteClient(Id);
                IsClosed = true;
            }

            protected override void OnReceived(byte[] buffer, long offset, long size)
            {
                File.AppendAllText($"logs/client_{Id}.log", "bufferl: " + buffer.Length + " offset: " + offset + " size: " + size + "\n");
                Debug.PWDebug($"SSL session with Id {Id} received!");
                var buf = buffer.Take((int)size);
                buffer = buf.Skip(4).ToArray();
                var ret = PluginHandle.DemuxDataReceived(Id, buffer);
                if (ret.Contains(true))
                    return;
                var first = buffer[0];
                Debug.WriteDebug("First: " + first);
                int Which = -1; // 0 Push, 1 Req
                switch (first)
                {
                    case 0x12:
                        Debug.WriteDebug("PUSH");
                        var req = Upstream.Parser.ParseFrom(buffer);
                        Demux.PushRSP.Push(Id, req.Push);
                        Which = 0;
                        break;
                    case 0x0A:
                        Debug.WriteDebug("REQ");
                        var req2 = Upstream.Parser.ParseFrom(buffer);
                        Demux.ReqRSP.Requests(Id, req2.Request);
                        Which = 1;
                        break;
                    case 0x5F:
                        //  5F 08 75 70 6c 61 79 64 6c 6c
                        //  5F 8 uplaydll
                        //  Please only use this if you want to communicate NOT Uplay Proto types!
                        var customproto = Utils.GetCustomProto(Id,buffer);
                        ret = PluginHandle.DemuxDataReceivedCustom(Id, customproto.buffer, customproto.protoname);
                        if (ret.Contains(true))
                            return;
                        break;
                    default:
                        Debug.PWDebug("Unknown First byte : " + first, "DMXSERVER");
                        File.WriteAllBytes($"dmx_bytes_unknowm_{DateTimeOffset.UtcNow}.blog", buffer);
                        break;
                }

                if (!IsClosed)
                {
                    if (Which == 0 && Demux.PushRSP.Downstream != null)
                    {
                        Debug.PWDebug("Push RSP sent back!", "DMXSERVER");
                        File.AppendAllText($"logs/client_{Id}_rsp.log", Demux.PushRSP.Downstream.ToString() + "\n");
                        Send(Formatters.FormatUpstream(Demux.PushRSP.Downstream.ToByteArray()));
                        Demux.PushRSP.Downstream = null;
                    }
                    if (Which == 1 && Demux.ReqRSP.Downstream != null)
                    {
                        Debug.PWDebug("Req RSP Sent back!", "DMXSERVER");
                        File.AppendAllText($"logs/client_{Id}_rsp.log", Demux.ReqRSP.Downstream.ToString() + "\n");
                        Send(Formatters.FormatUpstream(Demux.ReqRSP.Downstream.ToByteArray()));
                        Demux.ReqRSP.Downstream = null;
                    }

                    if (Demux.PushRSP.Downstream != null && Demux.ReqRSP.Downstream != null)
                    {
                        Console.WriteLine("Something wrong happened!");
                        Console.WriteLine(Id);
                        Console.WriteLine(Which);
                        Console.WriteLine(Demux.PushRSP.Downstream);
                        Console.WriteLine(Demux.ReqRSP.Downstream);
                        Console.WriteLine("-----\n");
                    }
                }
                else
                {
                    Debug.WriteDebug("Closed!");
                }
            }

            protected override void OnError(SocketError error) => Console.WriteLine($"SSL session caught an error with code {error}");
        }
        #endregion
    }
}
