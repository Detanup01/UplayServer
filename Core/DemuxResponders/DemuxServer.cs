using Core.Extra;
using Google.Protobuf;
using SharedLib.Server.Json;
using SharedLib.Shared;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using Uplay.Demux;

namespace Core.DemuxResponders
{
    public class DemuxServer
    {
        public TcpListener tcpListener;
        public static Dictionary<int, TcpClient> Clients;
        public static Dictionary<int, SslStream> SslClients;
        public static Dictionary<int, NetworkStream> NetClients;
        public static List<int> ConnectedIds = new();
        private Dictionary<int, string> ConnectedClients_IP = new();
        public int CurrentClientNumber;
        public bool IsClosed { get; internal set; } = false;
        private bool ClientModifing { get; set; } = false;

        object clientLock = new object();
        private bool IsLocal = false;

        public DemuxServer(bool isLocal) : this()
        {
            IsLocal = isLocal;
        }

        public DemuxServer()
        {
            Directory.CreateDirectory("logs");
            Console.WriteLine("[DMXSERVER] Server Started");
            tcpListener = new(IPAddress.Parse(ServerConfig.DemuxIp), ServerConfig.DemuxPort);
            Clients = new();
            SslClients = new();
            NetClients = new();
            CurrentClientNumber = 0;
            tcpListener.Start();
            new Thread(Main).Start();
            new Thread(StateCheck).Start();
            PluginHandle.DemuxParseInitFinish(this);
        }

        /// <summary>
        /// Main Thread to connect and handling client (Parsing to ClientHandler)
        /// </summary>
        /// <param name="obj"></param>
        private void Main(object? obj)
        {
            while (!IsClosed)
            {
                try
                {
                    if (CurrentClientNumber == int.MaxValue)
                    {
                        Console.WriteLine("[DMXSERVER] Max Client Connected, please Restart the server!");
                    }
                    ClientModifing = true;
                    // Here we trying to connect to TCP Client. And adding to Clients
                    TcpClient client = tcpListener.AcceptTcpClient();
                    lock (clientLock)
                    {
                        Debug.PWDebug($"[DMXSERVER] Client connected as Number: {CurrentClientNumber} ({client.Client.RemoteEndPoint})");
                        if (ConnectedClients_IP.ContainsValue(client.Client.RemoteEndPoint.ToString()))
                        {
                            Debug.PWDebug("something not right");
                            client.Close();
                        }
                        ConnectedClients_IP.Add(CurrentClientNumber, client.Client.RemoteEndPoint.ToString());
                        Clients.Add(CurrentClientNumber, client);
                        //Making a new thread to do Client Demux Stuff
                        new Thread(ClientHandler).Start(CurrentClientNumber);
                        CurrentClientNumber++;
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText("logs/demux_main_ex.txt", $"M: {ex.Message}\nS: {ex.Source}\nST: {ex.StackTrace}");
                }
            }
        }
        /// <summary>
        /// Checking all Client State if its Closed or not
        /// </summary>
        /// <param name="obj"></param>
        private void StateCheck(object? obj)
        {
            while (!IsClosed)
            {
                //Need this check bc If we add or remove doesnt handle well
                if (ClientModifing == false)
                {
                    try
                    {
                        //Checking all Clients if waiting for close or Force closed
                        foreach (var client in Clients)
                        {
                            var state = GetState(client.Value, client.Key);
                            if (state == TcpState.CloseWait || state == TcpState.Unknown)// || state == TcpState.TimeWait)
                            {
                                KillClient(client.Value);
                            }
                            else if (state != TcpState.Established)
                            {
                                Directory.CreateDirectory("StateChecks");
                                File.AppendAllText($"StateChecks/c_{client.Key}_{client.Value.Client.RemoteEndPoint.ToString().Replace(":", "_").Replace(".", "_")}.log", $"{state}\n");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText("logs/demux_state_ex.txt", $"M: {ex.Message}\nS: {ex.Source}\nST: {ex.StackTrace}");
                    }
                }
            }
        }

        /// <summary>
        /// Killing (Disconnecting) the Client
        /// </summary>
        /// <param name="client">Client</param>
        private void KillClient(TcpClient client)
        {
            //Killing client, from bottom to top.

            if (Clients.Where(x => x.Value == client).Any())
            {
                var key = Clients.Where(x => x.Value == client).First().Key;
                Debug.PWDebug($"[DMXSERVER] Client connection ended: {client.Client.RemoteEndPoint} ID: {key}");
                if (NetClients.TryGetValue(key, out var net))
                {
                    net.Dispose();
                    net.Close();
                    NetClients.Remove(key);
                }
                if (SslClients.TryGetValue(key, out var ssl))
                {
                    ssl.Dispose();
                    ssl.Close();
                    SslClients.Remove(key);
                }
                ClientModifing = true;
                Clients.Remove(key);
                client.Dispose();
                client.Close();
                ConnectedIds.Remove(key);
                ClientModifing = false;
                ConnectedClients_IP.Remove(key);
                Demux.DeleteClient(key);
            }
            else
            {
                Debug.PWDebug("Whoops something went wrong");
            }

        }

        /// <summary>
        /// Client Handling and Connecting, parsing to OnReceiveData
        /// </summary>
        /// <param name="obj">(int) Client Number</param>
        private void ClientHandler(object? obj)
        {
            if (obj == null) { return; }
            int ClientNumb = (int)obj;
            TcpClient client = Clients[ClientNumb];
            var sslStream = new SslStream(client.GetStream());
            //SSL force to use but still dont use CERT's
            SslServerAuthenticationOptions sslServerAuthenticationOptions = new();
            if (IsLocal)
                sslServerAuthenticationOptions.ServerCertificate = Utils.GetCert("dmx_local", "dmx.local.upc.ubisoft.com");
            else
                sslServerAuthenticationOptions.ServerCertificate = Utils.GetCert("dmx", "dmx.upc.ubisoft.com");
            sslServerAuthenticationOptions.RemoteCertificateValidationCallback = remoteCertificateValidationCallback;
            try
            {
                sslStream.AuthenticateAsServer(sslServerAuthenticationOptions);
                var network = client.GetStream();
                SslClients.Add(ClientNumb, sslStream);
                NetClients.Add(ClientNumb, network);
                ConnectedIds.Add(ClientNumb);
                ClientModifing = false;
                //Check if Server isnt closed
                while (!IsClosed)
                {
                    //State check
                    var state = GetState(client, ClientNumb);
                    if (state == TcpState.CloseWait || state == TcpState.Unknown) //|| state == TcpState.TimeWait)
                    {
                        Console.WriteLine("exit: ");
                        Console.WriteLine(state);
                        break;
                    }
                    else
                    {
                        Directory.CreateDirectory("StateChecks");
                        File.AppendAllText($"StateChecks/c_{ClientNumb}_{client.Client.RemoteEndPoint.ToString().Replace(":", "_").Replace(".", "_")}_clienthanlder.log", $"{state}\n");

                    }

                    bool IsDataAvailable = network.DataAvailable;
                    if (IsDataAvailable && !IsClosed)
                    {
                        byte[] buffer = new byte[4];
                        sslStream.Read(buffer, 0, 4);
                        uint responseLength = Formatters.FormatLength(BitConverter.ToUInt32(buffer, 0));
                        if (responseLength == 0)
                        {
                            // Means to break and shut down
                            break;
                        }
                        if (responseLength > 0)
                        {
                            buffer = new byte[responseLength];

                            for (int i = 0; i < responseLength; i++)
                            {
                                try
                                {
                                    sslStream.Read(buffer, i, 1);
                                }
                                catch
                                {
                                    break;
                                }
                            }
                        }
                        sslStream.Flush();
                        OnDataReceived(ClientNumb, sslStream, buffer);
                        PluginHandle.DemuxDataReceived(ClientNumb, sslStream, buffer);
                    }
                }
                Debug.PrintDebug("Client Closed!");
            }
            catch (Exception ex)
            {
                Debug.PrintDebug("Exception got! " + ex.ToString());
            }

            //KillClient(client); Here we wait to close the other!
        }

        /// <summary>
        /// Closing DemuxServer
        /// </summary>
        public void Close()
        {
            IsClosed = true;
            foreach (var stream in NetClients.Values)
            {
                stream.Dispose();
                stream.Close();
            }
            NetClients.Clear();
            foreach (var sslStream in SslClients.Values)
            {
                sslStream.Dispose();
                sslStream.Close();
            }
            SslClients.Clear();
            foreach (var client in Clients.Values)
            {
                client.Dispose();
                client.Close();
            }
            Clients.Clear();
            tcpListener.Stop();
            ConnectedClients_IP.Clear();
            Console.WriteLine("[DMXSERVER] Server Closed");
        }

        /// <summary>
        /// Receiving Data from Client 
        /// </summary>
        /// <param name="ClientNumb">Client Number</param>
        /// <param name="sslStream">SSLStream Object</param>
        /// <param name="receivedData">Recieved Data as Bytes</param>
        void OnDataReceived(int ClientNumb, SslStream sslStream, byte[] receivedData)
        {
            var bytestring = ByteString.CopyFrom(receivedData);
            var first = bytestring.ToArray()[0];
            int Which = -1; // 0 Push, 1 Req
            switch (first)
            {
                case 0x12:
                    var req = Upstream.Parser.ParseFrom(receivedData);
                    Demux.PushRSP.Push(ClientNumb, req.Push);
                    Which = 0;
                    break;
                case 0x0A:
                    var req2 = Upstream.Parser.ParseFrom(receivedData);
                    Demux.ReqRSP.Requests(ClientNumb, req2.Request);
                    Which = 1;
                    break;
                case 0x5F:
                    //  5F 08 75 70 6c 61 79 64 6c 6c
                    //  5F 8 uplaydll
                    //  Please only use this if you want to communicate NOT Uplay Proto types!
                    Debug.PWDebug($"[DMXSERVER] {ClientNumb}: Custom Request Received!");
                    int ReqNameLenght = int.Parse(System.Text.Encoding.UTF8.GetString(new byte[] { receivedData[1] }));
                    var bytename = receivedData.Skip(2).Take(ReqNameLenght).ToArray();
                    string protoname = System.Text.Encoding.UTF8.GetString(bytename);
                    Debug.PrintDebug($"[DMXSERVER] Request Name: {protoname}");
                    var bytes = receivedData.Skip(2 + ReqNameLenght).ToArray();
                    Custom.Requests(ClientNumb, bytes, protoname);
                    break;
                default:
                    Debug.PWDebug("[DMXSERVER] Unknown First byte : " + first);
                    File.WriteAllBytes($"dmx_bytes_unknowm_{DateTimeOffset.UtcNow}.blog", receivedData);
                    break;
            }

            if (Which == 0 && Demux.PushRSP.Downstream != null)
            {
                File.AppendAllText($"logs/client_{ClientNumb}_rsp.log", Demux.PushRSP.Downstream.ToString() + "\n");
                sslStream.Write(Formatters.FormatUpstream(Demux.PushRSP.Downstream.ToByteArray()));
                sslStream.Flush();
                Demux.PushRSP.Downstream = null;
            }
            if (Which == 1 && Demux.ReqRSP.Downstream != null)
            {
                File.AppendAllText($"logs/client_{ClientNumb}_rsp.log", Demux.ReqRSP.Downstream.ToString() + "\n");
                sslStream.Write(Formatters.FormatUpstream(Demux.ReqRSP.Downstream.ToByteArray()));
                sslStream.Flush();
                Demux.ReqRSP.Downstream = null;
            }

            if (Demux.PushRSP.Downstream != null && Demux.ReqRSP.Downstream != null)
            {
                Console.WriteLine("Something wrong happened!");
                Console.WriteLine(ClientNumb);
                Console.WriteLine(Which);
                Console.WriteLine(Demux.PushRSP.Downstream);
                Console.WriteLine(Demux.ReqRSP.Downstream);
                Console.WriteLine("-----\n");
            }
        }

        /// <summary>
        /// Demux Sent To Client
        /// </summary>
        /// <param name="ClientNumber">SSL Client Number</param>
        /// <param name="down">Uplay.Demux Downstream data</param>
        public static void SendToClient(int ClientNumber, Downstream down)
        {
            if (SslClients.TryGetValue(ClientNumber, out var sslStream))
            {
                sslStream.Write(Formatters.FormatUpstream(down.ToByteArray()));
                sslStream.Flush();
            }

        }

        /// <summary>
        /// Demux Sent To Client
        /// </summary>
        /// <param name="ClientNumber">SSL Client Number</param>
        /// <param name="message">Push DataMessage</param>
        public static void SendToClientDataM(int ClientNumber, DataMessage message)
        {
            if (SslClients.TryGetValue(ClientNumber, out var sslStream))
            {
                Downstream downstream = new()
                {
                    Push = new()
                    {
                        Data = message
                    }
                };
                sslStream.Write(Formatters.FormatUpstream(downstream.ToByteArray()));
                sslStream.Flush();
            }
        }

        /// <summary>
        /// Demux Sent To Client
        /// </summary>
        /// <param name="ClientNumber">SSL Client Number</param>
        /// <param name="bstr">ByteString data</param>
        /// <param name="conId">ConnectionId</param>
        public static void SendToClientBSTR(int ClientNumber, ByteString bstr, uint conId)
        {
            var sslStream = SslClients[ClientNumber];

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
            sslStream.Write(Formatters.FormatUpstream(downstream.ToByteArray()));
            sslStream.Flush();
        }

        /// <summary>
        /// Cert Valid Callback
        /// </summary>
        /// <returns>Always True</returns>
        private bool remoteCertificateValidationCallback(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Get TCP Client State
        /// </summary>
        /// <param name="tcpClient">Client</param>
        /// <returns>TCP State</returns>
        public static TcpState GetState(TcpClient tcpClient, int ClientNumb)
        {
            var foo = IPGlobalProperties.GetIPGlobalProperties()
              .GetActiveTcpConnections()[ClientNumb];
            return foo != null ? foo.State : TcpState.Unknown;
        }
    }
}
