using Google.Protobuf;
using Uplay.Uplaydll;

namespace Core.DemuxResponders
{
    public class UplayDll
    {
        public static readonly string Name = "uplaydll";
        public class Up
        {
            public static Rsp Rsp = null;
            public static void UpstreamConverter(Guid ClientNumb, ByteString bytes)
            {
                var UpstreamBytes = bytes.ToArray();
                var Upsteam = Req.Parser.ParseFrom(UpstreamBytes);
                Utils.WriteFile(Upsteam.ToString(), $"logs/client_{ClientNumb}_uplaydll_req.log");
                if (Upsteam != null)
                {
                    if (Upsteam != null)
                    {
                        ReqRSP.Requests(Upsteam);
                        while (ReqRSP.IsIdDone == false)
                        {

                        }
                        Utils.WriteFile(ReqRSP.Rsp.ToString(), $"logs/client_{ClientNumb}_uplaydll_rsp.log");
                        Rsp = ReqRSP.Rsp;
                    }
                }
            }
        }

        public class ReqRSP
        {
            public static Rsp Rsp = null;
            public static uint ReqId = 0;
            public static bool IsIdDone = false;
            public static void Requests(Req req)
            {
                if (req?.InitReq != null) { Init(req.InitReq); }
                if (req?.InitProcessReq != null) { InitProcess(req.InitProcessReq); }
                IsIdDone = true;
            }

            public static void Init(InitReq init)
            {
                Rsp = new()
                {
                    InitRsp = new()
                    {
                        Result = InitResult.Success,
                        UplayPID = init.UplayId
                    }
                };
            }


            public static void InitProcess(InitProcessReq init)
            {
                Rsp = new()
                {
                    InitProcessRsp = new()
                    {
                        Result = InitResult.Success,
                        UplayPID = init.UplayId,
                        OverlayInjectionMethod = OverlayInjectionMethod.None,
                        OverlayEnabled = false,
                        Devmode = false,
                        SdkMonitoringConfig = new()
                        {
                            SdkMonitoringEnabled = false,
                        }
                    }
                };
            }
        }
    }
}
