using Google.Protobuf;
using SharedLib.Server.DB;
using Uplay.Playtime;

namespace Core.DemuxResponders
{
    public class Playtime
    {
        public class Up
        {
            public static Downstream Downstream = null;
            public static void UpstreamConverter(Guid ClientNumb, ByteString bytes)
            {
                var UpstreamBytes = bytes.Skip(4).ToArray();
                var Upsteam = Upstream.Parser.ParseFrom(UpstreamBytes);
                if (Upsteam != null)
                {
                    if (Upsteam.Request != null)
                    {
                        ReqRSP.Requests(ClientNumb, Upsteam.Request);
                        while (ReqRSP.IsIdDone == false)
                        {

                        }
                        Downstream = ReqRSP.Downstream;
                    }
                }
            }
        }


        public class ReqRSP
        {
            public static Downstream Downstream = null;
            public static uint ReqId = 0;
            public static bool IsIdDone = false;
            public static void Requests(Guid ClientNumb, Req req)
            {
                if (req?.GetPlaytimeReq != null) { GetPlaytime(ClientNumb, req.GetPlaytimeReq); }
                if (req?.UpdatePlaytimeReq != null) { UpdatePlaytime(ClientNumb, req.UpdatePlaytimeReq); }
                IsIdDone = true;
            }

            public static void GetPlaytime(Guid ClientNumb, GetPlaytimeReq req)
            {
                Downstream = new()
                {
                    Response = new()
                    {
                        GetPlaytimeRsp = new()
                        {
                            Result = Result.ServerError
                        }
                    }
                };
            }

            public static void UpdatePlaytime(Guid ClientNumb, UpdatePlaytimeReq req)
            {
                var UserId = Globals.IdToUser[ClientNumb];
                var playtime = DBUser.GetPlaytime(UserId, req.GameId);
                if (playtime != null)
                    playtime.playTime += req.SecondsToAdd;

                Downstream = new()
                {
                    Response = new()
                    {
                        UpdatePlaytimeRsp = new()
                        {
                            Result = Result.Success
                        }
                    }
                };
            }
        }
    }
}
