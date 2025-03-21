using Google.Protobuf;
using Uplay.Party;

namespace ServerCore.DMX.Connections;

public static class PartyTask
{
    public static Task<ByteString> RunConnection(DmxSession dmxSession, ByteString data)
    {
        var upstream = Upstream.Parser.ParseFrom(data);
        if (upstream == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request.StartSessionReq != null)
            return StartSession(dmxSession, upstream.Request.RequestId, upstream.Request.StartSessionReq);
        return CoreTask.ReturnEmptyByteString();
    }

    public static Task<ByteString> StartSession(DmxSession dmxSession, uint ReqId, StartSessionReq req)
    {
        uint cookie = 25;
        if (req.HasCookie)
        {
            cookie = req.Cookie;
        }
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                StartSessionRsp = new()
                {
                    Cookie = cookie
                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }
}
