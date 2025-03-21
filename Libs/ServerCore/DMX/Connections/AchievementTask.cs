using Google.Protobuf;
using Serilog;
using Uplay.Uplay;

namespace ServerCore.DMX.Connections;

public static class AchievementTask
{
    private readonly static List<DmxSession> SessionsLoggedIn = [];

    public static Task<ByteString> RunConnection(DmxSession dmxSession, ByteString data)
    {
        var req = Req.Parser.ParseFrom(data);
        if (req == null)
            return CoreTask.ReturnEmptyByteString();
        if (req.AuthenticateReq != null)
            return Auth(dmxSession, req.RequestId, req.AuthenticateReq);
        if (req.ReadAchievementsReq != null)
            return ReadAchi(dmxSession, req.RequestId, req.ReadAchievementsReq);
        if (req.WriteAchievementsReq != null)
            return WriteAchi(dmxSession, req.RequestId, req.WriteAchievementsReq);
        return CoreTask.ReturnEmptyByteString();
    }

    public static Task<ByteString> Auth(DmxSession dmxSession, uint ReqId, AuthenticateReq req)
    {
        Log.Information("AchievementTask: Token: {Token}", req.OrbitToken);
        SessionsLoggedIn.Add(dmxSession);
        //req.OrbitToken;
        Rsp rsp = new()
        {
            RequestId = ReqId,
            AuthenticateRsp = new()
            {
                Success = true
            }
        };
        return Task.FromResult(rsp.ToByteString());
    }

    public static Task<ByteString> ReadAchi(DmxSession dmxSession, uint ReqId, ReadAchievementsReq req)
    {
        // Todo add logic for achi
        Rsp rsp = new()
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
                                    new Achievement()
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
        return Task.FromResult(rsp.ToByteString());
    }

    public static Task<ByteString> WriteAchi(DmxSession dmxSession, uint ReqId, WriteAchievementsReq req)
    {
        // Todo add logic for achi
        Rsp rsp = new()
        {
            RequestId = ReqId,
            WriteAchievementsRsp = new()
            {

            }
        };
        return Task.FromResult(rsp.ToByteString());
    }
}
