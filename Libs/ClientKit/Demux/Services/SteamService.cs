using Google.Protobuf;
using SharedLib.Shared;
using Uplay.SteamService;

namespace ClientKit.Demux.Services
{
    public class SteamService
    {
        #region Base
        private Socket socket;
        public bool isServiceSuccess = false;
        public SteamService(Socket demuxSocket)
        {
            socket = demuxSocket;
            Console.WriteLine("SteamService is Ready");
        }
        #endregion
        #region Request
        public Rsp? SendRequest(Req req)
        {
            Upstream post = new() { Request = req };
            var ServiceRequest = new Uplay.Demux.Req
            {
                ServiceRequest = new()
                {
                    Service = "steam_service",
                    Data = ByteString.CopyFrom(post.ToByteArray())
                },
                RequestId = socket.RequestId
            };
            socket.RequestId++;
            var rsp = socket.SendReq(ServiceRequest);

            if (rsp == null || !rsp.ServiceRsp.Success)
            {
                return null;
            }

            return Formatters.FormatDataNoLength<Downstream>(rsp.ServiceRsp.Data.ToByteArray()).Response;
        }
        #endregion
        #region Function
        public List<SteamUserInfo> GetSteamFriends(string SteamID)
        {
            Req req = new()
            {
                GetSteamFriendsReq = new()
                {
                    SteamId = SteamID
                }
            };
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.GetSteamFriendsRsp.Success;
                return rsp.GetSteamFriendsRsp.SteamFriends.ToList();
            }
            else
            {
                return new();
            }

        }

        public SteamUserInfo GetSteamUserInfo(string SteamID)
        {
            Req req = new()
            {
                GetSteamUserInfoReq = new()
                {
                    SteamId = SteamID
                }
            };
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.GetSteamUserInfoRsp.Success;
                return rsp.GetSteamUserInfoRsp.SteamUserInfo;
            }
            else
            {
                return new();
            }
        }
        #endregion
    }
}
