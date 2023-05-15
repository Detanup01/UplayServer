using Google.Protobuf;
using SharedLib.Shared;
using System.Net.Security;
using System.Net.Sockets;
using Uplay.Demux;

namespace ClientKit.Demux
{
    public class Socket
    {
        #region DemuxEventArgs
        public class DMXEventArgs : EventArgs
        {
            public DMXEventArgs(DataMessage data)
            {
                Data = data;
            }
            public DataMessage Data { get; set; }
        }
        #endregion
        #region Fields
        public event EventHandler<DMXEventArgs> NewMessage;

        public SslStream sslStream;
        public TcpClient tcpClient;
        public NetworkStream network;
        private uint? InternalReadedLenght = null;
        public uint RequestId { get; internal set; }
        public static string ConnectionHost { get; set; } = "dmx.local.upc.ubisoft.com";
        public static int ConnectionPort { get; set; } = 443;
        public int WaitInTimeMS = 10;
        //We using this for same uplay version 
        public uint ClientVersion { get; internal set; } = 10857; //10857
        public bool TestConfig { get; set; } = false;
        public uint TerminateConnectionId { get; set; } = 0;
        public bool IsClosed { get; internal set; } = false;
        public bool IsAuthed { get; internal set; } = false;
        /// <summary>
        /// Connection Dictionary for the Service Names.
        /// </summary>
        public Dictionary<uint, string> ConnectionDict = new();
        /// <summary>
        /// Connection Dictionary for the whole Connection (this)
        /// </summary>
        public Dictionary<uint, object> ConnectionObject = new();

        public static bool StopDataCheck = false;
        private bool _StopCheck = false;
        private bool IsWaitinData = false;
        private const int BUFFERSIZE = 4;
        private byte[] ReadBuffer = null;
        private byte[]? InternalReaded = null;
        private int _numbEventAttempt = 0;
        private Task RecieveTask;
        #endregion
        #region Basic
        public Socket()
        {
            if (Debug.isDebug)
            {
                Directory.CreateDirectory("SendReq");
                Directory.CreateDirectory("SendUpstream");
            }
            try
            {
                tcpClient = new TcpClient();
                tcpClient.ReceiveBufferSize = 2048;
                tcpClient.SendBufferSize = 2048;
                tcpClient.Connect(ConnectionHost, ConnectionPort);
                sslStream = new SslStream(tcpClient.GetStream());
                SslClientAuthenticationOptions sslClientAuthenticationOptions = new()
                {
                    TargetHost = ConnectionHost,
                    RemoteCertificateValidationCallback = delegate { return true; }
                };
                sslStream.AuthenticateAsClient(sslClientAuthenticationOptions);
                RequestId = 1;
                network = tcpClient.GetStream();
                if (!StopDataCheck)
                {
                    ReadBuffer = new byte[BUFFERSIZE];
                    RecieveTask = new(Receive);
                    RecieveTask.Start();
                }
                NewMessage += DemuxSocket_NewMessage;
                Debug.PWDebug("Started.", "DemuxSocket");
            }
            catch (Exception ex)
            {
                Ex.Handler(ex, "ClientSocket");
                Console.WriteLine("Connection to demux has been failed!");
            }

        }

        /// <summary>
        /// Closing the Connection safely
        /// </summary>
        public void Close()
        {
            if (IsClosed)
                return;

            try
            {
                sslStream.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                sslStream.Dispose();
                sslStream.Close();
                network.Dispose();
                network.Close();
                tcpClient.Dispose();
                tcpClient.Close();
                RequestId = 0;
                IsClosed = true;
                IsAuthed = false;
                NewMessage -= DemuxSocket_NewMessage;
                Debug.WriteDebug("Closed.", "DemuxSocket");
            }
            catch (Exception ex)
            {
                Ex.Handler(ex, "ClientSocket");
            }
            Console.WriteLine("[DemuxSocket] Closed.");
        }

        #endregion
        #region Event
        public void StopCheck()
        {
            _StopCheck = true;
            RecieveTask.Dispose();
        }

        public void StartCheck()
        {
            _StopCheck = false;
            RecieveTask = new(Receive);
            RecieveTask.Start();
        }

        private void Receive()
        {
            if (!IsClosed)
            {
                if (!_StopCheck)
                {
                    Debug.PWDebug($"Started!", "Receive");
                    sslStream.BeginRead(ReadBuffer, 0, BUFFERSIZE, new AsyncCallback(EndReceive), null);
                }
            }
        }

        private void EndReceive(IAsyncResult ar)
        {
            try
            {
                if (!IsClosed)
                {
                    int nBytes;
                    nBytes = sslStream.EndRead(ar);
                    if (nBytes > 0)
                    {
                        Debug.PWDebug($"Must be 4!: {ReadBuffer.Length} / {nBytes}", "Receive");
                        var _InternalReadedLenght = Formatters.FormatLength(BitConverter.ToUInt32(ReadBuffer, 0));
                        Debug.PWDebug($"Response Length is {_InternalReadedLenght}!", "Receive");
                        var _InternalReaded = new byte[(int)_InternalReadedLenght];
                        var readed = 0;
                        for (int i = 0; i < _InternalReadedLenght; i++)
                            readed += sslStream.Read(_InternalReaded, i, 1);

                        Debug.PWDebug($"Readed lenght: {_InternalReaded.Length} / {readed}", "Receive");
                        if (readed != _InternalReaded.Length)
                        {
                            Console.WriteLine("We broke It!");
                        }

                        Debug.PWDebug($"Response is Readed!", "Receive");
                        if (IsWaitinData)
                        {
                            Debug.PWDebug("We have the data, writing down (IsWaitinData)", "Receive");
                            InternalReaded = _InternalReaded;
                            InternalReadedLenght = _InternalReadedLenght;
                            Debug.PWDebug("Restarting...", "Receive");
                            Receive();
                            return;
                        }
                        var downstream = Formatters.FormatDataNoLength<Downstream>(_InternalReaded);
                        if (downstream != null)
                        {
                            Debug.PWDebug(downstream.ToString(), "Receive.txt");
                            if (IsWaitinData)
                            {
                                Debug.PWDebug("We have the data, writing down (IsWaitinData)", "Receive");
                                InternalReaded = _InternalReaded;
                                InternalReadedLenght = _InternalReadedLenght;
                                Debug.PWDebug("Restarting...", "Receive");
                                Receive();
                                return;
                            }
                            if (downstream.Response != null)
                            {
                                Debug.PWDebug("We have the data, writing down", "Receive");
                                InternalReaded = _InternalReaded;
                                InternalReadedLenght = _InternalReadedLenght;
                            }
                            if (downstream.Push != null)
                            {
                                if (downstream.Push.ClientOutdated != null)
                                {
                                    Debug.PWDebug("Your Client is Outdated!", "Receive");
                                    TerminateConnection(0);
                                }
                                if (downstream.Push.ConnectionClosed != null)
                                {
                                    TerminateConnection(downstream.Push.ConnectionClosed.ConnectionId, downstream.Push.ConnectionClosed.ErrorCode);
                                }
                                if (downstream.Push.Data != null && !IsWaitinData)
                                {
                                    NewMessage?.Invoke(this, new(downstream.Push.Data));
                                }
                                if (downstream.Push.KeepAlive != null)
                                {
                                    this.KeepAlivePush();
                                }
                            }
                        }
                        Debug.PWDebug("Restarting...", "Receive");
                        Receive();
                    }
                    else
                    {
                        Debug.PWDebug("nBytes is 0!", "Receive");
                        if (_numbEventAttempt < 3)
                        {
                            _numbEventAttempt++;
                            Receive();
                            return;
                        }
                        else
                        {
                            Debug.PWDebug("Something isn't Right!", "Receive");
                            TerminateConnection(0);
                        }

                    }
                }
                else
                {
                    Debug.PWDebug("Demux got closed we dont want to read again!", "Receive");
                }
            }
            catch (SocketException socketex)
            {
                if (socketex.SocketErrorCode == SocketError.ConnectionReset)
                {
                    TerminateConnection(0);
                }
            }
            catch (Exception ex)
            {
                Ex.Handler(ex, "ClientSocket");
            }
        }
        private void DemuxSocket_NewMessage(object? sender, DMXEventArgs e)
        {
            Debug.WriteDebug(e.Data.ToString(), "NewMessage");
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
                Close();
                return;
            }
            var CloseMethod = ConnectionObject[connectionID].GetType().GetMethod("Close");
            if (CloseMethod != null)
            {
                CloseMethod.Invoke(ConnectionObject[connectionID], null);
            }
        }

        #endregion
        #region Ssl Communication with Demux

        /// <summary>
        /// Sending Request in Demux
        /// </summary>
        /// <param name="req">Demux Req</param>
        /// <returns>Demux Rsp</returns>
        /// <exception cref="Exception">If the Response Length from Response is 0</exception>
        public Rsp? SendReq(Req req)
        {
            try
            {
                Upstream post = new() { Request = req };
                var up = Formatters.FormatUpstream(post.ToByteArray());
                byte[] buffer = new byte[4];
                uint responseLength = 0;
                Debug.WriteAllText(post.ToString(), $"SendReq/{req.RequestId}_req.log.txt");
                Debug.WriteAllBytes(up, $"SendReq/{req.RequestId}_req.blog.txt");
                Debug.PWDebug("We sent our Request!", "SendReq");
                sslStream.Write(up);
                IsWaitinData = true;
                while (true)
                {
                    var _InternalReadedLenght = InternalReadedLenght;
                    var _InternalReaded = InternalReaded;
                    if (_InternalReadedLenght != null && _InternalReaded != null)
                    {
                        Debug.PWDebug("[SendReq] We receive from internal readed!");
                        responseLength = (uint)_InternalReadedLenght;
                        buffer = new byte[(int)_InternalReadedLenght];
                        buffer = _InternalReaded.ToArray();
                        break;
                    }
                    Thread.Sleep(WaitInTimeMS);
                }
                InternalReaded = null;
                InternalReadedLenght = null;
                IsWaitinData = false;
                Debug.PWDebug($"Final Response Length: {responseLength}/{buffer.Length}", "SendReq");
                //Fail save!
                if (responseLength == 0)
                    return null;

                Debug.WriteAllBytes(buffer, $"SendReq/{req.RequestId}_rsp.blog.txt");
                var downstream = Formatters.FormatDataNoLength<Downstream>(buffer);
                Debug.WriteAllText(downstream.ToString(), $"SendReq/{req.RequestId}_rsp.log.txt");
                CheckTheConnection(downstream);
                if (downstream != null)
                {
                    if (downstream.Response != null)
                    {
                        Debug.WriteAllText(downstream.Response.ToString(), $"SendReq/{req.RequestId}_rsp_rsp.log.txt");
                        return downstream.Response;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Ex.Handler(ex, "ClientSocket");
                return null;
            }
        }

        /// <summary>
        /// Sending Request outside of Demux
        /// </summary>
        /// <param name="upstream">Any Upstream</param>
        /// <returns>Any Downstream or Null</returns>
        /// <exception cref="Exception">If the Response Length from Response is 0</exception>
        public Downstream? SendUpstream(Upstream upstream)
        {
            try
            {
                var time = DateTime.Now.ToFileTime();
                byte[] buffer = new byte[4];
                uint responseLength;
                var up = Formatters.FormatUpstream(upstream.ToByteArray());
                Debug.WriteAllText(upstream.ToString(), $"SendUpstream/{time}_req.log.txt");
                Debug.WriteAllBytes(up, $"SendUpstream/{time}_req.blog.txt");
                Debug.PWDebug("We sent our Upstream!", "SendUpstream");
                sslStream.Write(up);
                IsWaitinData = true;
                while (true)
                {
                    var _InternalReadedLenght = InternalReadedLenght;
                    var _InternalReaded = InternalReaded;
                    if (_InternalReadedLenght != null && _InternalReaded != null)
                    {
                        Debug.PWDebug("We receive from internal readed!", "SendUpstream");
                        responseLength = (uint)_InternalReadedLenght;
                        buffer = new byte[(int)_InternalReadedLenght];
                        buffer = _InternalReaded.ToArray();
                        break;
                    }
                    Thread.Sleep(WaitInTimeMS);
                }
                InternalReaded = null;
                InternalReadedLenght = null;
                IsWaitinData = false;
                sslStream.Flush();
                Debug.PWDebug($"Final Response Length: {responseLength}/{buffer.Length}", "SendUpstream");
                //Fail save!
                if (responseLength == 0)
                    return null;

                Debug.WriteAllBytes(buffer, $"SendUpstream/{time}_rsp.blog.txt");
                var downstream = Formatters.FormatDataNoLength<Downstream>(buffer);
                Debug.WriteAllText(downstream.ToString(), $"SendUpstream/{time}_rsp.log.txt");
                CheckTheConnection(downstream);
                return downstream;
            }
            catch (Exception ex)
            {
                Ex.Handler(ex, "ClientSocket");
                return null;
            }
        }

        /// <summary>
        /// Sending Bytes Request
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public byte[]? SendBytes(byte[] post)
        {
            try
            {
                byte[] buffer = new byte[4];
                uint responseLength = 0;
                Debug.PWDebug("We sent our Request!", "SendBytes");
                sslStream.Write(post);
                IsWaitinData = true;
                while (true)
                {
                    var _InternalReadedLenght = InternalReadedLenght;
                    var _InternalReaded = InternalReaded;
                    if (_InternalReadedLenght != null && _InternalReaded != null)
                    {
                        Debug.PWDebug("We receive from internal readed!", "SendBytes");
                        responseLength = (uint)_InternalReadedLenght;
                        buffer = new byte[(int)_InternalReadedLenght];
                        buffer = _InternalReaded.ToArray();
                        break;
                    }
                    if (network.DataAvailable)
                    {
                        sslStream.Read(buffer, 0, 4);
                        responseLength = Formatters.FormatLength(BitConverter.ToUInt32(buffer, 0));
                        Console.WriteLine("Response Length: " + responseLength, "SendBytes");
                        if (responseLength == 0) { throw new Exception("Response Length from Demux is 0, something is not right"); }
                        if (responseLength > 0)
                        {
                            buffer = new byte[responseLength];
                            sslStream.Read(buffer, 0, (int)responseLength);
                            break;
                        }
                    }
                    Thread.Sleep(WaitInTimeMS);
                }
                InternalReaded = null;
                InternalReadedLenght = null;
                IsWaitinData = false;
                Debug.PWDebug($"Final Response Length: {responseLength}/{buffer.Length}", "SendBytes");
                //Fail save!
                if (responseLength == 0)
                    return null;

                return buffer;
            }
            catch (Exception ex)
            {
                Ex.Handler(ex, "ClientSocket");
                return null;
            }
        }

        /// <summary>
        /// Send push to the socket.
        /// </summary>
        /// <param name="push">Pushed object</param>
        public void SendPush(Push push)
        {
            try
            {
                Upstream up = new() { Push = push };
                sslStream.Write(Formatters.FormatUpstream(up.ToByteArray()));
                sslStream.Flush();
                Debug.PWDebug("Write was successful!");
            }
            catch (Exception ex)
            {
                Ex.Handler(ex, "ClientSocket");
            }
        }

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
                        TerminateConnection(0);
                    }
                }
            }
        }
        #endregion
    }
}
