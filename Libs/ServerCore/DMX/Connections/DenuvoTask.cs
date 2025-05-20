using Google.Protobuf;
using Uplay.DenuvoService;

namespace ServerCore.DMX.Connections;

public static class DenuvoTask
{
    public static Task<ByteString> RunConnection(DmxSession dmxSession, ByteString data)
    {
        var upstream = Upstream.Parser.ParseFrom(data);
        if (upstream == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request.GetGameTimeTokenReq != null)
            return GetGameTimeToken(dmxSession, upstream.Request.RequestId, upstream.Request.GetGameTimeTokenReq);
        if (upstream.Request.GetGameTokenReq != null)
            return GetGameToken(dmxSession, upstream.Request.RequestId, upstream.Request.GetGameTokenReq);
        return CoreTask.ReturnEmptyByteString();
    }

    public static Task<ByteString> GetGameTimeToken(DmxSession dmxSession, uint ReqId, GetGameTimeTokenReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                Result = Rsp.Types.Result.ServerError,
                GetGameTimeTokenRsp = new()
                {

                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> GetGameToken(DmxSession dmxSession, uint ReqId, GetGameTokenReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                Result = Rsp.Types.Result.ServerError,
                GetGameTokenRsp = new()
                {

                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }
}
