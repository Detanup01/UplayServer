using Google.Protobuf;
using Uplay.Uplay;

namespace Core.DemuxResponders
{
    public class Achievement
    {
        public const string Name = "ach_frontend";
        public class Up
        {
            public static Rsp Rsp = null;
            public static void ReqConverter(Guid ClientNumb, ByteString bytes)
            {
                var reqBytes = bytes.ToArray();
                var req = Req.Parser.ParseFrom(reqBytes);

                if (req != null)
                {
                    ReqRSP.Requests(ClientNumb, req);
                    while (ReqRSP.IsIdDone == false)
                    {

                    }
                    Rsp = ReqRSP.Rsp;
                }
            }
        }
        public class ReqRSP
        {
            public static Rsp Rsp = null;
            public static uint ReqId = 0;
            public static bool IsIdDone = false;
            public static void Requests(Guid ClientNumb, Req req)
            {
                File.AppendAllText($"logs/client_{ClientNumb}_ach_req.log", req.ToString() + "\n");
                ReqId = req.RequestId;
                if (req.AuthenticateReq != null) { Auth(ClientNumb, req.AuthenticateReq); }
                if (req.ReadAchievementsReq != null) { ReadAch(ClientNumb, req.ReadAchievementsReq); }
                if (req.WriteAchievementsReq != null) { WriteAch(ClientNumb, req.WriteAchievementsReq); }
                IsIdDone = true;
            }

            public static void Auth(Guid ClientNumb, AuthenticateReq req)
            {
                //req.OrbitToken;
                Rsp = new()
                {
                    RequestId = ReqId,
                    AuthenticateRsp = new()
                    {
                        Success = true
                    }
                };
            }
            public static void ReadAch(Guid ClientNumb, ReadAchievementsReq req)
            {
                //req.UserId;
                //req.Product.ProductId;
                Rsp = new()
                {
                    RequestId = ReqId,
                    ReadAchievementsRsp = new()
                    {
                        UserId = req.UserId,
                        AchievementBlob = new()
                        {
                            ProductAchievements =
                                {
                                    new ProductAchievements()
                                    {
                                        Product = req.Product,
                                        Achievements = new()
                                        {
                                            Achievements_ =
                                            {
                                                new Uplay.Uplay.Achievement()
                                                {
                                                    AchievementId = 0,
                                                    Timestamp = 0,

                                                }
                                            }
                                        }
                                    }
                                }
                        }
                    }
                };
            }

            public static void WriteAch(Guid ClientNumb, WriteAchievementsReq req)
            {
                Rsp = new()
                {
                    RequestId = ReqId,
                    WriteAchievementsRsp = new()
                    {
                    }
                };
            }

        }
    }
}
