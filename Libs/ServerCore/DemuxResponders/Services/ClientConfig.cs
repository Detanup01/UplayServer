using Google.Protobuf;
using ServerCore.Models;
using Uplay.ClientConfiguration;

namespace ServerCore.DemuxResponders;

public class ClientConfig
{
    public static readonly string Name = "client_configuration_service";
    public class Up
    {
        public static void Reset()
        {
            Downstream = null;
            ReqRSP.Downstream = null;
            ReqRSP.IsIdDone = false;
        }

        public static Downstream? Downstream = null;
        public static void UpstreamConverter(ByteString bytes)
        {
            var UpstreamBytes = bytes.ToArray();
            var Upsteam = Upstream.Parser.ParseFrom(UpstreamBytes);

            if (Upsteam != null)
            {
                if (Upsteam.Request != null)
                {
                    ReqRSP.Requests(Upsteam.Request);
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
        public static Downstream? Downstream = null;
        public static uint ReqId = 0;
        public static bool IsIdDone = false;
        public static void Requests(Req req)
        {
            ReqId = req.RequestId;
            if (req?.GetStatisticsStatusReq != null) { StatisticStatus(req.GetStatisticsStatusReq); }
            if (req?.GetPatchInfoReq != null) { DMX_PatchInfo(req.GetPatchInfoReq); }
            if (req?.GetPatchInfoReqV2 != null) { PatchInfo(req.GetPatchInfoReqV2); }
            IsIdDone = true;
        }

        public static void StatisticStatus(GetStatisticsStatusReq getStatisticsStatus)
        {
            // ?? Store User Build Version ??

            List<BuildVersion> buildVersions = new();

            foreach (var ver in Globals.AcceptVersions)
            {
                buildVersions.Add(new() { VersionNumber = ver });
            }

            Downstream = new()
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

        }

        public static void DMX_PatchInfo(Uplay.Demux.GetPatchInfoReq getPatchInfo)
        {
            Downstream = new()
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
        }

        public static void PatchInfo(GetPatchInfoReq getPatchInfo)
        {
            Downstream = new()
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
        }

    }
}
