using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using Google.Protobuf;
using NetCoreServer;
using SharedLib.Shared;
using Uplay.Demux;

namespace ClientKit.Demux
{
    public class Socket : SslClient
    {
        #region Basics
        public static string ConnectionHost { get; set; } = "dmx.local.upc.ubisoft.com";
        public static int ConnectionPort { get; set; } = 443;
        public static bool AllowVersionMissmatch = false;
        /// <summary>
        /// Connection Dictionary for the Service Names.
        /// </summary>
        public Dictionary<uint, string> ConnectionDict = new();
        /// <summary>
        /// Connection Dictionary for the whole Connection (this)
        /// </summary>
        public Dictionary<uint, object> ConnectionObject = new();
        bool IsWaitingData;
        byte[]? InternalReaded = null;
        public event EventHandler<DMXEventArgs> NewMessage;
        public event EventHandler<byte[]> CustomProtobufEvent;
        public uint TerminateConnectionId { get; set; } = 0;
        public uint RequestId { get; internal set; }
        public uint ClientVersion { get; internal set; } = 10857;
        public bool IsAuthed { get; internal set; } = false;
        public bool TestConfig { get; set; } = false;

        public int WaitInTimeMS = 10;

        public Socket() : base(new SslContext(SslProtocols.Tls12), new DnsEndPoint(ConnectionHost, ConnectionPort, System.Net.Sockets.AddressFamily.InterNetwork)) 
        {
            Start();
        }

        public Socket(SslContext context) : base(context, new DnsEndPoint(ConnectionHost, ConnectionPort, System.Net.Sockets.AddressFamily.InterNetwork))
        {
            Start();
        }
        void Start()
        {
            Console.WriteLine(Address);
            Console.WriteLine(Port);
            var x = Endpoint as DnsEndPoint;
            Console.WriteLine(x.AddressFamily);
            if (Debug.IsDebug)
            {
                Directory.CreateDirectory("SendReq");
                Directory.CreateDirectory("SendUpstream");
            }
            NewMessage += NewSocket_NewMessage;
            Context.ClientCertificateRequired = false;
            Context.CertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            var Connected = Connect();
            ReceiveAsync();
            Console.WriteLine(Connected);
        }

        #endregion
        #region Override
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            Debug.PWDebug($"OnReceived! Bytes to send: {BytesSent} | Bytes sending: {BytesSending} | Bytes Pending {BytesPending} | Bytes Recieved {BytesReceived}");
            var _InternalReadedLenght = Formatters.FormatLength(BitConverter.ToUInt32(buffer[..4], 0));
            var _InternalReaded = buffer.Skip(4).Take((int)_InternalReadedLenght).ToArray();
            if (!IsWaitingData)
            {
                if (_InternalReaded[0] == 0x12) //  First byte is a Push byte!
                {
                    var downstream = Formatters.FormatDataNoLength<Downstream>(_InternalReaded);
                    CheckTheConnection(downstream);
                    if (downstream != null && downstream.Push.Data != null)
                        NewMessage?.Invoke(this, new DMXEventArgs(downstream.Push.Data));
                    if (downstream != null && downstream.Push.KeepAlive != null)
                        this.KeepAlivePush();
                }
                else if (_InternalReaded[0] == 0x5F)
                {
                    Debug.PWDebug("Custom Protobuf EVENT Currently NOT Supported!", "WARN");
                    CustomProtobufEvent?.Invoke(this, _InternalReaded);
                }
                else
                {
                    Debug.PWDebug("Unknown byte! " + _InternalReaded[0], "ERROR");
                    File.WriteAllBytes("All_Received_Bytes", buffer);
                }
            }
            else
            {
                InternalReaded = _InternalReaded;
            }
            ReceiveAsync();
        }

        protected override void OnDisconnected()
        {
            NewMessage -= NewSocket_NewMessage;
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine("ERROR: " + error);
        }

        protected override void OnDisconnecting()
        {
            Console.WriteLine("DISCONNECTING!!");
        }

        #endregion
        #region Sending
        /// <summary>
        /// Sending Request
        /// </summary>
        /// <param name="req">Demux Req</param>
        /// <returns>Demux Rsp or null</returns>
        public Rsp? SendReq(Req req)
        {
            Debug.WriteAllText(req.ToString(), $"SendReq/{req.RequestId}_req.txt");
            Upstream post = new() { Request = req };
            var up = Formatters.FormatUpstream(post.ToByteArray());
            IsWaitingData = true;
            long sentBytes = Send(up);
            if (sentBytes == up.Length)
            {
                while (InternalReaded == null)
                {
                    Console.WriteLine("SendReq waiting...");
                    Thread.Sleep(WaitInTimeMS);
                }
                IsWaitingData = false;
                var downstream = Formatters.FormatDataNoLength<Downstream>(InternalReaded);
                InternalReaded = null;
                if (downstream?.Response != null)
                {
                    Debug.WriteAllText(downstream.Response.ToString(), $"SendReq/{req.RequestId}_rsp.txt");
                    return downstream.Response;
                }
            }
            IsWaitingData = false;
            return null;
        }

        /// <summary>
        /// Sending Request
        /// </summary>
        /// <param name="upstream">Upstream</param>
        /// <returns>Downstream or Null</returns>
        public Downstream? SendUpstream(Upstream up)
        {
            Debug.WriteAllText(up.ToString(), $"SendUpstream/{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_up.txt");
            var upbytes = Formatters.FormatUpstream(up.ToByteArray());
            long sentBytes = Send(upbytes);
            IsWaitingData = true;
            if (sentBytes == upbytes.Length)
            {
                while (InternalReaded == null)
                {
                    Console.WriteLine("SendUpstream waiting...");
                    Thread.Sleep(WaitInTimeMS);
                }
                IsWaitingData = false;
                var downstream = Formatters.FormatDataNoLength<Downstream>(InternalReaded);
                InternalReaded = null;
                if (downstream != null)
                {
                    Debug.WriteAllText(downstream.ToString(), $"SendUpstream/{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_down.txt");
                    return downstream;
                }
            }
            IsWaitingData = false;
            return null;
        }

        /// <summary>
        /// Sending Bytes Request
        /// </summary>
        /// <param name="post">Bytes Request</param>
        /// <returns>Bytes Response</returns>
        public byte[]? SendBytes(byte[] post)
        {
            long sentBytes = Send(Formatters.FormatUpstream(post));
            IsWaitingData = true;
            if (sentBytes == post.Length)
            {
                while (InternalReaded == null)
                {
                    Thread.Sleep(WaitInTimeMS);
                }
                IsWaitingData = false;
                var returner = InternalReaded;
                InternalReaded = null;
                return returner;
            }
            IsWaitingData = false;
            return null;
        }

        /// <summary>
        /// Send push to the socket.
        /// </summary>
        /// <param name="push">Pushed object</param>
        public void SendPush(Push push)
        {
            Upstream up = new() { Push = push };
            Send(Formatters.FormatUpstream(up.ToByteArray()));
            Debug.PWDebug("Write was successful!");
        }
        #endregion
        #region Connection Handling
        /// <summary>
        /// Adding to ConnectionDict
        /// </summary>
        /// <param name="connectionID">Id</param>
        /// <param name="ConnectionName">Name</param>
        public void AddToDict(uint connectionID, string ConnectionName)
        {
            ConnectionDict.Add(connectionID, ConnectionName);
            Debug.PWDebug($"Connection added {ConnectionName} as ID {connectionID}");
        }

        /// <summary>
        /// Adding to ConnectionObject
        /// </summary>
        /// <param name="connectionID">Id</param>
        /// <param name="ConnectionObj">this</param>
        public void AddToObj(uint connectionID, object ConnectionObj)
        {
            ConnectionObject.Add(connectionID, ConnectionObj);
            Debug.PWDebug($"Connection added {ConnectionObj} as ID {connectionID}");
        }
        /// <summary>
        /// Remove Connection from Dictionaries
        /// </summary>
        /// <param name="connectionID">ID to remove</param>
        public void RemoveConnection(uint connectionID)
        {
            ConnectionDict.Remove(connectionID);
            ConnectionObject.Remove(connectionID);
        }

        /// <summary>
        /// Terminating connection (Closing) from the Dictionaries
        /// </summary>
        /// <param name="connectionID">Connection Id</param>
        /// <param name="errorCode">Error Code</param>
        public void TerminateConnection(uint connectionID, ConnectionClosedPush.Types.Connection_ErrorCode errorCode = ConnectionClosedPush.Types.Connection_ErrorCode.ConnectionForceQuit)
        {
            TerminateConnectionId = connectionID;
            Debug.PWDebug(connectionID);
            Debug.PWDebug($"Connection Terminated ID: {connectionID}, Reason: {errorCode}");
            if (connectionID == 0)
            {
                Disconnect();
                return;
            }
            var CloseMethod = ConnectionObject[connectionID].GetType().GetMethod("Close");
            if (CloseMethod != null)
            {
                CloseMethod.Invoke(ConnectionObject[connectionID], null);
            }
        }

        #endregion
        #region Other Stuff
        public void CheckTheConnection(Downstream? downstream)
        {
            if (downstream?.Push != null)
            {
                if (downstream.Push?.ConnectionClosed != null)
                {
                    if (downstream.Push.ConnectionClosed.HasConnectionId)
                    {
                        Console.WriteLine("Connection closed");
                        TerminateConnection(downstream.Push.ConnectionClosed.ConnectionId, downstream.Push.ConnectionClosed.ErrorCode);
                    }
                    if (downstream.Push.ClientOutdated != null)
                    {
                        Console.WriteLine("Your Client is Outdated!");
                        if (!AllowVersionMissmatch)
                            TerminateConnection(0);
                    }
                }
            }
        }

        private void NewSocket_NewMessage(object? sender, DMXEventArgs e)
        {
            Debug.WriteDebug(e.Data.ToString(), "NewMessage");
        }
        #endregion
    }
}
