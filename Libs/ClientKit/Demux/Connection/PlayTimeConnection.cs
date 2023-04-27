using Google.Protobuf;
using SharedLib.Shared;
using Uplay.Playtime;

namespace ClientKit.Demux.Connection
{
    public class PlayTimeConnection
    {
        #region Base
        public uint connectionId;
        public Socket socket;
        public bool isServiceSuccess = false;
        public bool isConnectionClosed = false;
        public static readonly string ServiceName = "playtime_service";
        private uint ReqId { get; set; } = 1;
        public PlayTimeConnection(Socket demuxSocket)
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
                Console.WriteLine("Playtime Connection cancelled.");
                Close();
            }
            else
            {
                isServiceSuccess = rsp.OpenConnectionRsp.Success;
                connectionId = rsp.OpenConnectionRsp.ConnectionId;
                if (isServiceSuccess == true)
                {
                    Console.WriteLine("Playtime Connection successful");
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
            Debug.WriteDebug(ds.ToString(), "playtime.txt");

            if (ds != null || ds?.Response != null)
                return ds.Response;
            return null;
        }
        #endregion
        #region Functions
        public Result UpdatePlayTime(uint prodId, uint Seconds, bool IsActive)
        {
            Req updateReq = new()
            {
                UpdatePlaytimeReq = new()
                {
                    ProductIds = { prodId },
                    SecondsToAdd = Seconds,
                    IsActivePlaySession = IsActive,
                    GameId = prodId

                },
                RequestId = ReqId,
            };
            ReqId++;
            var rsp = SendRequest(updateReq);
            if (rsp != null)
            {
                return rsp.UpdatePlaytimeRsp.Result;
            }
            else
            {
                return Result.ServerError;
            }
        }

        public GetPlaytimeRsp GetPlayTime(uint prodId, string AccountId)
        {
            Req updateReq = new()
            {
                GetPlaytimeReq = new()
                {
                    AccountId = AccountId,
                    ProductId = prodId
                },
                RequestId = ReqId,
            };
            ReqId++;
            var rsp = SendRequest(updateReq);
            if (rsp != null)
            {
                return rsp.GetPlaytimeRsp;
            }
            else
            {
                return new() { Result = Result.ServerError };
            }
        }

        public GetFriendsPlaytimeRsp GetFriendsPlayTime(uint prodId, List<string> AccountIds)
        {
            Req GetFriendsPlaytimeReq = new()
            {
                GetFriendsPlaytimeReq = new()
                {
                    GameId = prodId,
                    MyFriends = { AccountIds }
                },
                RequestId = ReqId,
            };
            ReqId++;
            var rsp = SendRequest(GetFriendsPlaytimeReq);
            if (rsp != null)
            {
                return rsp.GetFriendsPlaytimeRsp;
            }
            else
            {
                return new() { Result = Result.ServerError };
            }
        }
        #endregion
    }
}
