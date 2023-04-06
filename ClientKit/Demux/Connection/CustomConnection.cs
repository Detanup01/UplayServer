using Google.Protobuf;

namespace ClientKit.Demux.Connection
{
    public class CustomConnection
    {
        #region Base
        private uint connectionId;
        private Socket socket;
        public bool isServiceSuccess = false;
        public bool isConnectionClosed = false;
        public bool initDone = false;
        public static string ServiceName = "";
        public uint ReqId { get; set; } = 1;
        public CustomConnection(string serviceName, Socket demuxSocket)
        {
            ServiceName = serviceName;
            socket = demuxSocket;
            Connect();
        }

        /// <summary>
        /// Reconnect the CustomConnection
        /// </summary>
        public void Reconnect()
        {
            if (isConnectionClosed)
                Connect();
        }

        internal void Connect()
        {
            var openConnectionReq = new Uplay.Demux.Req
            {
                RequestId = socket.RequestId,
                OpenConnectionReq = new()
                {
                    ServiceName = ServiceName
                }
            };
            socket.RequestId++;
            var rsp = socket.SendReq(openConnectionReq);
            if (rsp == null)
            {
                Console.WriteLine("Custom Connection cancelled.");
                Close();
            }
            else
            {
                isServiceSuccess = rsp.OpenConnectionRsp.Success;
                connectionId = rsp.OpenConnectionRsp.ConnectionId;
                if (isServiceSuccess == true)
                {
                    Console.WriteLine("Custom Connection successful.");
                    socket.AddToObj(connectionId, this);
                    socket.AddToDict(connectionId, ServiceName);
                    isConnectionClosed = false;
                }
            }
        }

        /// <summary>
        /// Closing CustomConnection
        /// </summary>
        public void Close()
        {
            if (socket.TerminateConnectionId == connectionId)
            {
                Console.WriteLine($"Connection terminated via Socket {ServiceName}");
            }
            socket.RemoveConnection(connectionId);
            isServiceSuccess = false;
            connectionId = uint.MaxValue;
            isConnectionClosed = true;
        }
        #endregion
        #region Request
        public V? SendPostRequest<T, V>(T post)
            where V : IMessage<V>, new()
            where T : IMessage<T>, new()
        {
            if (isConnectionClosed)
                return default;

            Uplay.Demux.Upstream up = new()
            {
                Push = new()
                {
                    Data = new()
                    {
                        ConnectionId = connectionId,
                        Data = ByteString.CopyFrom(Formatters.FormatUpstream(post.ToByteArray()))
                    }
                }
            };

            var down = socket.SendUpstream(up);
            if (isConnectionClosed || down == null || !down.Push.Data.HasData)
                return default;

            var ds = Formatters.FormatData<V>(down.Push.Data.Data.ToByteArray());
            Debug.WriteDebug(ds.ToString(), "custom.txt");

            if (ds != null)
                return ds;
            return default;
        }
        #endregion
    }
}
