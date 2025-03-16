using Google.Protobuf;
using ServerCore.DemuxResponders;
using ServerCore.Models;
using Uplay.ClientConfiguration;

namespace ServerCore.DMX.Services;

public static class ClientConfigurationServiceTask
{
    public static Task<ByteString> RunService(DmxSession dmxSession, ByteString data)
    {
        var upstream = Upstream.Parser.ParseFrom(data);
        if (upstream == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request.GetStatisticsStatusReq != null)
            return StatisticStatus(dmxSession, upstream.Request.RequestId, upstream.Request.GetStatisticsStatusReq);
        return CoreTask.ReturnEmptyByteString();
    }

    public static Task<ByteString> StatisticStatus(DmxSession dmxSession, uint ReqId, GetStatisticsStatusReq getStatisticsStatus)
    {
        // ?? Store User Build Version ??

        List<BuildVersion> buildVersions = new();

        foreach (var ver in Globals.AcceptVersions)
        {
            buildVersions.Add(new() { VersionNumber = ver });
        }

        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                GetStatisticsStatusRsp = new()
                {
                    EnabledBuildVersions = { buildVersions }
                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> DMX_PatchInfo(DmxSession dmxSession, uint ReqId, Uplay.Demux.GetPatchInfoReq getPatchInfo)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                GetPatchInfoRsp = new()
                {
                    TrackType = getPatchInfo.TrackType,
                    TestConfig = getPatchInfo.TestConfig,
                    Success = true,
                    LatestVersion = Globals.AcceptVersions.Last(),
                    PatchTrackId = getPatchInfo.PatchTrackId,
                    PatchBaseUrl = ServerConfig.Instance.HTTPS_Url + "/patch/"
                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> PatchInfo(DmxSession dmxSession, uint ReqId, GetPatchInfoReq getPatchInfo)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                GetPatchInfoRsp = new()
                {
                    TrackType = getPatchInfo.TrackType,
                    TestConfig = getPatchInfo.TestConfig,
                    Success = true,
                    LatestVersion = Globals.AcceptVersions.Last(),
                    PatchTrackId = getPatchInfo.PatchTrackId,
                    PatchBaseUrl = ServerConfig.Instance.HTTPS_Url + "/patch/"
                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }
}
