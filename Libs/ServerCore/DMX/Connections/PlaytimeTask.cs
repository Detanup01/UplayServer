using Google.Protobuf;
using ServerCore.DB;
using ServerCore.Models.User;
using Uplay.Playtime;

namespace ServerCore.DMX.Connections;

public static class PlaytimeTask
{
    public static Task<ByteString> RunConnection(DmxSession dmxSession, ByteString data)
    {
        var upstream = Upstream.Parser.ParseFrom(data);
        if (upstream == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request.GetPlaytimeReq != null)
            return GetPlaytime(dmxSession, upstream.Request.RequestId, upstream.Request.GetPlaytimeReq);
        if (upstream.Request.UpdatePlaytimeReq != null)
            return UpdatePlaytime(dmxSession, upstream.Request.RequestId, upstream.Request.UpdatePlaytimeReq);
        return CoreTask.ReturnEmptyByteString();
    }

    public static Task<ByteString> GetPlaytime(DmxSession dmxSession, uint reqid, GetPlaytimeReq req)
    {
        if (!dmxSession.IsLoggedIn)
            return CoreTask.ReturnEmptyByteString();
        var playtime = DBUser.Get<UserPlaytime>(dmxSession.UserId, x => x.UplayId == req.ProductId);
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = reqid,
                GetPlaytimeRsp = new()
                {
                    Result = Result.ServerError
                }
            }
        };
        if (playtime != null && playtime.UserId.ToString() == req.AccountId)
        {
            downstream.Response.GetPlaytimeRsp.Result = Result.Success;
            downstream.Response.GetPlaytimeRsp.Seconds = playtime.PlayTime;
        }
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> UpdatePlaytime(DmxSession dmxSession, uint reqid, UpdatePlaytimeReq req)
    {
        if (!dmxSession.IsLoggedIn)
            return CoreTask.ReturnEmptyByteString();
        var playtime = DBUser.Get<UserPlaytime>(dmxSession.UserId, x => x.UplayId == req.GameId);
        if (playtime != null)
            playtime.PlayTime += req.SecondsToAdd;

        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = reqid,
                UpdatePlaytimeRsp = new()
                {
                    Result = Result.Success
                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }
}
