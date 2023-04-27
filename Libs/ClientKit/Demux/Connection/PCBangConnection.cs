using Google.Protobuf;
using SharedLib.Shared;
using Uplay.Pcbang;

namespace ClientKit.Demux.Connection
{
    public class PCBangConnection
    {
        #region Base
        public uint connectionId;
        public Socket socket;
        public bool isServiceSuccess = false;
        public bool isConnectionClosed = false;
        public static string ServiceName = "pcbang_service";
        private uint ReqId { get; set; } = 1;
        public PCBangConnection(Socket demuxSocket)
        {
            socket = demuxSocket;

            Connect();
        }

        public void Reconnect()
        {
            if (isConnectionClosed)
                Connect();
        }
        internal void Connect()
        {
            var openConnectionReq = new Uplay.Demux.Req
            {
                OpenConnectionReq = new()
                {
                    ServiceName = ServiceName
                },
                RequestId = socket.RequestId
            };
            socket.RequestId++;

            var rsp = socket.SendReq(openConnectionReq);
            if (rsp == null)
            {
                Console.WriteLine("Test Connection cancelled.");
                Close();
            }
            else
            {
                isServiceSuccess = rsp.OpenConnectionRsp.Success;
                connectionId = rsp.OpenConnectionRsp.ConnectionId;
                if (isServiceSuccess == true)
                {
                    Console.WriteLine("Test Connection successful");
                    socket.AddToObj(connectionId, this);
                    socket.AddToDict(connectionId, ServiceName);
                    isConnectionClosed = false;
                }
            }
        }

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
        public Rsp? SendRequest(Req req)
        {
            if (isConnectionClosed)
                return null;

            Upstream post = new() { Request = req };
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
                return null;

            var ds = Formatters.FormatData<Downstream>(down.Push.Data.Data.ToByteArray());
            Debug.WriteDebug(ds.ToString(), "pcbang.txt");

            if (ds != null || ds?.Response != null)
                return ds.Response;
            return null;
        }
        #endregion
        #region Functions
        public void Init(string provider)
        {
            Req req = new()
            {
                RequestId = ReqId,
                InitializeReq = new()
                {
                    Provider = provider
                }
            };
            ReqId++;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                Console.WriteLine(rsp.InitializeRsp);
            }
        }
        #endregion
    }
}
