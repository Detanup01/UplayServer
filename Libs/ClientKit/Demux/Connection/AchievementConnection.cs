using Google.Protobuf;
using SharedLib.Shared;
using Uplay.Uplay;

namespace ClientKit.Demux.Connection
{
    public class AchievementConnection
    {
        #region Base
        private uint connectionId;
        private Socket socket;
        public bool isServiceSuccess = false;
        public bool isConnectionClosed = false;
        public bool initDone = false;
        public static readonly string ServiceName = "ach_frontend";
        private uint ReqId { get; set; } = 1;
        public AchievementConnection(Socket demuxSocket)
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
                Console.WriteLine("Achievement Connection cancelled.");
                Close();
            }
            else
            {
                isServiceSuccess = rsp.OpenConnectionRsp.Success;
                connectionId = rsp.OpenConnectionRsp.ConnectionId;
                if (isServiceSuccess == true)
                {
                    Console.WriteLine("Achievement Connection successful.");
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

            Uplay.Demux.Upstream up = new()
            {
                Push = new()
                {
                    Data = new()
                    {
                        ConnectionId = connectionId,
                        Data = ByteString.CopyFrom(Formatters.FormatUpstream(req.ToByteArray()))
                    }
                }
            };

            var down = socket.SendUpstream(up);
            if (isConnectionClosed || down == null || !down.Push.Data.HasData)
                return null;

            var ds = Formatters.FormatData<Rsp>(down.Push.Data.Data.ToByteArray());
            Debug.WriteDebug(ds.ToString(), "AchievementConnection");

            if (ds != null)
                return ds;
            return null;
        }
        #endregion
        #region Functions
        public void Auth(string token)
        {
            Req req = new()
            {
                RequestId = ReqId,
                AuthenticateReq = new()
                {
                    OrbitToken = token
                }
            };
            ReqId++;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.AuthenticateRsp.Success;
                Debug.PWDebug(rsp.AuthenticateRsp.Success);
            }
            else
            {
                isServiceSuccess = false;
            }
        }

        public List<ProductAchievements> GetAchievements(string userId, string ProductId, string PlatformId)
        {
            Req req = new()
            {
                RequestId = ReqId,
                ReadAchievementsReq = new()
                {
                    UserId = userId,
                    Product = new()
                    {
                        ProductId = ProductId,
                        PlatformId = PlatformId
                    }
                }
            };
            ReqId++;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = true;
                return rsp.ReadAchievementsRsp.AchievementBlob.ProductAchievements.ToList();
            }
            else
            {
                isServiceSuccess = false;
                return new() { };
            }
        }

        public bool WriteAchievement(List<ProductAchievements> productAchievements)
        {
            Req req = new()
            {
                RequestId = ReqId,
                WriteAchievementsReq = new()
                {
                    AchievementBlob = new()
                    {
                        ProductAchievements = { productAchievements }
                    }
                }
            };
            ReqId++;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = true;
            }
            else
            {
                isServiceSuccess = false;
            }
            return isServiceSuccess;
        }
        #endregion
    }
}
