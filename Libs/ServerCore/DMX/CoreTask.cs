using Uplay.Demux;
using ServerCore.Extra;

namespace ServerCore.DMX;

public static class CoreTask
{
    public static Task<Downstream?> RunTask(Guid id, byte[] Data)
    {
        Upstream? upstream = null;
        switch (Data[0])
        {
            case 0x12:
                upstream = Upstream.Parser.ParseFrom(Data);
                return RunPushTask(id, upstream.Push);
            case 0x0A:
                upstream = Upstream.Parser.ParseFrom(Data);
                return RunRequestTask(id, upstream.Request);
            case 0x5F:
                return RunCustomTask(id, Data);
            default:
                return Task.FromResult<Downstream?>(null);
        }
    }

    public static Task<Downstream?> RunRequestTask(Guid id, Req req)
    {
        //if (req?.ClientIpOverride != null) { Console.WriteLine(req.ClientIpOverride); }
        if (req?.GetPatchInfoReq != null)
            return DemuxTasks.GetPatchInfo(req.RequestId, id, req.GetPatchInfoReq);
        if (req?.AuthenticateReq != null)
            return DemuxTasks.Authenticate(req.RequestId, id, req.AuthenticateReq);
        return Task.FromResult<Downstream?>(null);
    }

    public static Task<Downstream?> RunPushTask(Guid id, Push push)
    {
        if (push.ClientVersion != null)
            return DemuxTasks.ClientVersion(id, push.ClientVersion);
        return Task.FromResult<Downstream?>(null);
    }

    public static Task<Downstream?> RunCustomTask(Guid id, byte[] Data)
    {
        var customproto = Utils.GetCustomProto(id, Data);
        PluginHandle.DemuxDataReceivedCustom(id, customproto.buffer, customproto.protoname);
        return Task.FromResult<Downstream?>(null);
    }
}
