using Google.Protobuf;
using ServerCore.Extra;
using Uplay.Demux;

namespace ServerCore.DMX;

public static class CoreTask
{
    public static Task<ByteString> ReturnEmptyByteString()
    {
        return Task.FromResult(ByteString.Empty);
    }

    public static Task<Downstream?> ReturnDownstream()
    {
        return Task.FromResult<Downstream?>(null);
    }

    public static Task<Downstream?> RunTask(DmxSession dmxSession, byte[] Data)
    {
        Upstream? upstream;
        switch (Data[0])
        {
            case 0x12:
                upstream = Upstream.Parser.ParseFrom(Data);
                return RunPushTask(dmxSession, upstream.Push);
            case 0x0A:
                upstream = Upstream.Parser.ParseFrom(Data);
                return RunRequestTask(dmxSession, upstream.Request);
            case 0x5F:
                return RunCustomTask(dmxSession, Data);
            default:
                return ReturnDownstream();
        }
    }

    public static Task<Downstream?> RunRequestTask(DmxSession dmxSession, Req req)
    {
        //if (req?.ClientIpOverride != null) { Console.WriteLine(req.ClientIpOverride); }
        if (req?.GetPatchInfoReq != null)
            return DemuxTasks.GetPatchInfo(req.RequestId, dmxSession, req.GetPatchInfoReq);
        if (req?.AuthenticateReq != null)
            return DemuxTasks.Authenticate(req.RequestId, dmxSession, req.AuthenticateReq);
        if (req?.OpenConnectionReq != null)
            return DemuxTasks.OpenConnection(req.RequestId, dmxSession, req.OpenConnectionReq);
        if (req?.ServiceRequest != null)
            return DemuxTasks.Service(req.RequestId, dmxSession, req.ServiceRequest);
        return ReturnDownstream();
    }

    public static Task<Downstream?> RunPushTask(DmxSession dmxSession, Push push)
    {
        if (push.ClientVersion != null)
            return DemuxTasks.ClientVersion(dmxSession, push.ClientVersion);
        if (push.ClientVersion != null)
            return DemuxTasks.PushMessage(dmxSession, push.Data);
        return ReturnDownstream();
    }

    public static Task<Downstream?> RunCustomTask(DmxSession dmxSession, byte[] Data)
    {
        var (protoname, buffer) = Utils.GetCustomProto(Data);
        PluginHandle.DemuxDataReceivedCustom(dmxSession, buffer, protoname);
        return ReturnDownstream();
    }
}
