using Google.Protobuf;
using ServerCore;
using ServerCore.DB;
using ServerCore.Models.User;
using Uplay.Uplaydll;

namespace Core.DemuxResponders;

public class UplayDll
{
    public static readonly string Name = "uplaydll";
    public class Up
    {
        public static Rsp? Rsp = null;
        public static void UpstreamConverter(Guid ClientNumb, ByteString bytes)
        {
            var UpstreamBytes = bytes.ToArray();
            var Upsteam = Req.Parser.ParseFrom(UpstreamBytes);
            Utils.WriteFile(Upsteam.ToString(), $"logs/client_{ClientNumb}_uplaydll_req.log");
            if (Upsteam != null)
            {
                ReqRSP.Requests(ClientNumb, Upsteam);
                while (ReqRSP.IsIdDone == false)
                {

                }
                Utils.WriteFile(ReqRSP.Rsp.ToString(), $"logs/client_{ClientNumb}_uplaydll_rsp.log");
                Rsp = ReqRSP.Rsp;
            }
        }
    }

    public class ReqRSP
    {
        public static Rsp? Rsp = null;
        public static uint ReqId = 0;
        public static bool IsIdDone = false;
        public static void Requests(Guid ClientNumb, Req req)
        {
            if (req?.InitReq != null) { Init(ClientNumb, req.InitReq); }
            if (req?.InitProcessReq != null) { InitProcess(req.InitProcessReq); }
            if (req.Equals(new Req())) { Rsp = new(); }
            IsIdDone = true;
        }

        public static void Init(Guid ClientNumb, InitReq init)
        {
            Rsp = new()
            {
                InitRsp = new()
                {
                    Result = InitResult.Success,
                    UplayPID = init.UplayId
                }
            };

            var UserId = Globals.IdToUser[ClientNumb];

            var user = DBUser.Get<UserCommon>(UserId);

            var auth = Auth.GetTokenByUserId(UserId, TokenType.Ticket);
            Rsp.InitRsp.UpcTicket = auth;
            Rsp.InitRsp.Account = new()
            { 
                AccountId = UserId.ToString(),
                Email = "customuplay@protonmail.com",
                NameOnPlatform = user.Name,
                Password = "CustomUplay12345!",
                Username = user.Name
            };
            Rsp.InitRsp.IsInOfflineMode = true; //force it while testing!
            Rsp.InitRsp.Connected = true;   //idk what this should do

        }


        public static void InitProcess(InitProcessReq init)
        {
            if (init.ApiVersion == uint.MaxValue)
            {
                // App quit
                Rsp = new();
            }
            else
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
