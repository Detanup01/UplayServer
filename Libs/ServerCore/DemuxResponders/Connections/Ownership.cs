using Google.Protobuf;
using Newtonsoft.Json;
using System.Text;
using Uplay.Ownership;
using ServerCore.DB;
using ServerCore;
using ServerCore.Models;
using ServerCore.Models.User;
using ServerCore.Controllers;
using SharedLib;

namespace ServerCore.DemuxResponders;

public class Ownership
{
    public static ByteString GetOwnerSignature(Guid UserId)
    {
        var owbasic = DBUser.Get<UserOwnershipBasic>(UserId);
        if (owbasic != null)
        {
            List<byte> SignList = [];
            int i = 0;
            foreach (var id in owbasic.OwnedGamesIds)
            {
                byte bi = Convert.ToByte(i.ToString(), 16);
                SignList.Add(bi);
                SignList.Add(byte.Parse(id.ToString()));
                i++;
                SignList.Add((byte)0xFF);
            }
            var SignatureByte = SignList.ToArray();
            ByteString Signature = ByteString.CopyFrom(SignatureByte);
            var sigb64 = Signature.ToBase64();
            if (owbasic.OwnedGamesIds.Count > 30)
            {
                sigb64 = CompressB64.GetZstdB64(SignatureByte);
            }

            var userId64 = CompressB64.GetZstdB64(UserId.ToString());
            return ByteString.CopyFrom(Encoding.UTF8.GetBytes(B64.ToB64(userId64 + "_OwnerSignature_" + sigb64)));
        }
        return ByteString.CopyFrom(Encoding.UTF8.GetBytes("T3duZXJTaWduYXR1cmVfSXNGYWlsZWQ="));
    }

    public static List<uint> FromOwnerSignature(string token)
    {
        List<uint> prodids = [];
        var realtokenb = B64.FromB64(token);
        var realtoken = B64.FromB64(realtokenb);
        var tokensp = realtoken.Split("_OwnerSignature_");
        var userid64 = tokensp[0];
        var sig64 = tokensp[1];
        var userId = B64.FromB64(CompressB64.GetUnZstdB64(Convert.FromBase64String(userid64)));
        var owbasic = DBUser.Get<UserOwnershipBasic>(userId);
        byte[] siglist = [];
        if (owbasic == null)
            return [];
        if (owbasic.OwnedGamesIds.Count > 30)
        {
            siglist = Convert.FromBase64String(CompressB64.GetUnZstdB64(Convert.FromBase64String(sig64)));
        }
        else
        {
            siglist = Convert.FromBase64String(sig64);
        }
        var blist = siglist.ToList();
        int bnum = 0;
        foreach (var b in blist)
        {
            if (bnum == 1)
            {
                prodids.Add(uint.Parse(b.ToString()));
            }
            if (bnum == 2 && b == (byte)0xFF)
            {
                bnum = -1;
            }
            bnum++;
        }
        return prodids;
    }


    public static ByteString GetSignofOwnership(Guid UserId, uint ProdId)
    {
        var ownership = DBUser.Get<UserOwnership>(UserId, x=>x.ProductId == ProdId);
        if (ownership != null)
        {
            var userId64 = CompressB64.GetZstdB64(UserId.ToString());
            string ownershipb64 = CompressB64.GetZstdB64(JsonConvert.SerializeObject(ownership));

            return ByteString.CopyFrom(Encoding.UTF8.GetBytes(CompressB64.GetZstdB64(CompressB64.GetDeflateB64(userId64 + "_" + ownershipb64))));
        }
        return ByteString.CopyFrom(Encoding.UTF8.GetBytes("T3duZXJTaWduYXR1cmVfSXNGYWlsZWQ="));
    }

    public const string Name = "ownership_service";
    public class Up
    {
        public static Downstream? Downstream = null;
        public static void UpstreamConverter(Guid ClientNumb, ByteString bytes)
        {
            var UpstreamBytes = bytes.Skip(4).ToArray();
            var Upsteam = Upstream.Parser.ParseFrom(UpstreamBytes);
            if (Upsteam != null)
            {
                if (Upsteam.Request != null)
                {
                    ReqRSP.Requests(ClientNumb, Upsteam.Request);
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
        public static Dictionary<Guid, bool> UserInits = new();
        public static void Requests(Guid ClientNumb, Req req)
        {
            File.AppendAllText($"logs/client_{ClientNumb}_ownership_req.log", req.ToString() + "\n");
            ReqId = req.RequestId;
            if (req?.ClaimKeystorageKeyReq != null) { Console.WriteLine(ClientNumb + " " + req.ClaimKeystorageKeyReq); }
            if (req?.ConsumeOwnershipReq != null) { Console.WriteLine(ClientNumb + " " + req.ConsumeOwnershipReq); }
            if (req?.DeprecatedGetLatestManifestsReq != null) { DeprecatedGetLatestManifests(ClientNumb, req.DeprecatedGetLatestManifestsReq); }
            if (req?.DeprecatedGetProductFromCdKeyReq != null) { Console.WriteLine(ClientNumb + " " + req.DeprecatedGetProductFromCdKeyReq); }
            if (req?.GetBatchDownloadUrlsReq != null) { GetBatchDownloadUrls(ClientNumb, req.GetBatchDownloadUrlsReq); }
            if (req?.GetGameTimeTicketReq != null) { Console.WriteLine(ClientNumb + " " + req.GetGameTimeTicketReq); }
            if (req?.GetGameTokenReq != null) { Console.WriteLine(ClientNumb + " " + req.GetGameTokenReq); }
            if (req?.GetGameWithdrawalRightsReq != null) { Console.WriteLine(ClientNumb + " " + req.GetGameWithdrawalRightsReq); }
            if (req?.GetProductConfigReq != null) { GetProductConfig(ClientNumb, req.GetProductConfigReq); }
            if (req?.GetUplayPcTicketReq != null) { GetUplayPcTicket(ClientNumb, req.GetUplayPcTicketReq); }
            if (req?.InitializeReq != null) { Initialize(ClientNumb, req.InitializeReq); }
            if (req?.OwnershipTokenReq != null) { OwnershipToken(ClientNumb, req.OwnershipTokenReq); }
            if (req?.RegisterOwnershipByCdKeyReq != null) { Console.WriteLine(ClientNumb + " " + req.RegisterOwnershipByCdKeyReq); }
            if (req?.RegisterOwnershipFromOculusReq != null) { Console.WriteLine(ClientNumb + " " + req.RegisterOwnershipFromOculusReq); }
            if (req?.RegisterOwnershipFromWegameReq != null) { Console.WriteLine(ClientNumb + " " + req.RegisterOwnershipFromWegameReq); }
            if (req?.RegisterOwnershipReq != null) { Console.WriteLine(ClientNumb + " " + req.RegisterOwnershipReq); }
            if (req?.RegisterOwnershipSteamPopReq != null) { Console.WriteLine(ClientNumb + " " + req.RegisterOwnershipSteamPopReq); }
#pragma warning disable CS0612 // Type or member is obsolete
            if (req?.RegisterTemporaryOwnershipReq != null) { RegisterTemporaryOwnership(ClientNumb, req.RegisterTemporaryOwnershipReq); }
#pragma warning restore CS0612 // Type or member is obsolete
            if (req?.RetryUplayCoreInitializeReq != null) { Console.WriteLine(ClientNumb + " " + req.RetryUplayCoreInitializeReq); }
            if (req?.SignOwnershipReq != null) { SignOwnership(ClientNumb, req.SignOwnershipReq); }
            if (req?.SwitchProductBranchReq != null) { SwitchProductBranch(ClientNumb, req.SwitchProductBranchReq); }
            if (req?.UnlockProductBranchReq != null) { UnlockProductBranch(ClientNumb, req.UnlockProductBranchReq); }
            if (req?.WaiveGameWithdrawalRightsReq != null) { Console.WriteLine(ClientNumb + " " + req.WaiveGameWithdrawalRightsReq); }
            IsIdDone = true;
        }

        public static void x(Guid ClientNumb, ClaimKeystorageKeyReq claimKeystorageKeyReq)
        {
            if (UserInits[ClientNumb])
            {

            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    ClaimKeystorageKeyRsp = new()
                    {
                        Result = ClaimKeystorageKeyRsp.Types.Result.Success,
                        ProductKeyPairs = { }
                    }
                }
            };
        }


        #region Functions
        public static void Initialize(Guid ClientNumb, InitializeReq Initializer)
        {
            bool IsSuccess = false;
            var userID = Globals.IdToUser[ClientNumb];
            var owbasic = DBUser.Get<UserOwnershipBasic>(userID);
            if (owbasic != null)
            {
                IsSuccess = true;
            }
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    InitializeRsp = new()
                    {
                        Success = IsSuccess,
                        KeySpamBanSeconds = 10,
                        SubscriptionState = SubscriptionState.NoState,
                        SubscriptionType = 0,
                        //
                        OwnedGames = new()
                        {
                            OwnedGames_ = { }
                        },
                        OwnedGamesContainer = new()
                        {
                            ProductIds = { },
                            VisibleOrInstallableProductIds = { }
                        }
                    }
                }
            };
            if (owbasic != null)
            {
                UserInits.Add(ClientNumb, true);
                Downstream.Response.InitializeRsp.SubscriptionType = owbasic.UbiPlus;
                Downstream.Response.InitializeRsp.SubscriptionState = (SubscriptionState)owbasic.UbiPlus;

                Downstream.Response.InitializeRsp.OwnedGamesContainer.Signature = GetOwnerSignature(userID);
                Downstream.Response.InitializeRsp.OwnedGamesContainer.ProductIds.AddRange(owbasic.OwnedGamesIds);
                Downstream.Response.InitializeRsp.OwnedGamesContainer.VisibleOrInstallableProductIds.AddRange(owbasic.OwnedGamesIds);
                Dictionary<uint, uint>? branches = new();
                if (Initializer.Branches != null)
                {
                    foreach (var item in Initializer.Branches)
                    {
                        var branch = App.GetAppBranch(item.ProductId, item.ActiveBranchId);
                        if (branch != null && item.Passwords.Contains(branch.BranchPassword))
                            branches.Add(item.ProductId,item.ActiveBranchId);
                    }
                }
                if (branches.Any())
                {
                    Downstream.Response.InitializeRsp.OwnedGames = OwnershipController.GetOwnershipGames(userID, branches);
                }
                else
                {
                    Downstream.Response.InitializeRsp.OwnedGames = OwnershipController.GetOwnershipGames(userID, null);
                }
            }
            Console.WriteLine("Initialize Done!");
        }

        public static void OwnershipToken(Guid ClientNumb, OwnershipTokenReq OwnershipToken)
        {
            bool IsSuccess = false;
            ulong Exp = 0;
            string Token = "";
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var owbasic = DBUser.Get<UserOwnershipBasic>(userID);
                if (owbasic != null)
                {
                    if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || owbasic.OwnedGamesIds.Contains(OwnershipToken.ProductId))
                    {
                        var ownership = DBUser.Get<UserOwnership>(userID, x => x.ProductId == OwnershipToken.ProductId);
                        var appconfig = App.GetAppConfig(OwnershipToken.ProductId);
                        if (appconfig != null && ownership != null)
                        {
                            if (ownership.AppFlags.Count == 0)
                            {
                                ownership.AppFlags = appconfig.AppFlags;
                            }

                            Token = JWTController.CreateOwnershipToken(userID, OwnershipToken.ProductId, OwnershipToken.ProductId, ownership.CurrentBranchId, ownership.AppFlags);
                            Exp = (ulong)JWTController.GetExp(Token);
                            IsSuccess = true;
                        }
                    }
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    OwnershipTokenRsp = new()
                    {
                        Success = IsSuccess,
                        Expiration = Exp,
                        Token = Token
                    }
                }
            };
        }

        public static void SignOwnership(Guid ClientNumb, SignOwnershipReq SignOwnership)
        {
            bool IsSuccess = false;
            ByteString signature = ByteString.Empty;
            uint BranchId = 0;
            ulong Exp = (ulong)DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();

            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var ownershipBasic = DBUser.Get<UserOwnershipBasic>(userID);
                var ownership = DBUser.Get<UserOwnership>(userID, x=>x.ProductId == SignOwnership.ProductId);
                if (ownershipBasic != null && ownership != null)
                {
                    if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || ownershipBasic.OwnedGamesIds.Contains(SignOwnership.ProductId))
                    {
                        var gameconfig = App.GetAppConfig(SignOwnership.ProductId);
                        if (gameconfig != null)
                        {
                            BranchId = ownership.CurrentBranchId;
                            signature = GetSignofOwnership(userID, SignOwnership.ProductId);
                            IsSuccess = true;
                        }
                    }
                }
            }
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    SignOwnershipRsp = new()
                    {
                        Success = IsSuccess,
                        BranchId = BranchId,
                        Signature = signature,
                        Expiration = Exp
                    }
                }
            };
        }

        public static void DeprecatedGetLatestManifests(Guid ClientNumb, DeprecatedGetLatestManifestsReq DeprecatedGetLatestManifests)
        {
            Downstream = new()
            {
                Response = new()
                {
                    DeprecatedGetLatestManifestsRsp = new()
                    { }
                }
            };

            if (ServerConfig.Instance.Demux.Ownership.EnableManifestRequest)
            {
                Downstream.Response.DeprecatedGetLatestManifestsRsp.Result = DeprecatedGetLatestManifestsRsp.Types.Result.Success;
                var prodlist = DeprecatedGetLatestManifests.DeprecatedProductIds.ToList();

                foreach (var proId in prodlist)
                {
                    var branch = App.GetAppBranch(proId, 0);
                    if (branch != null)
                    {
                        DeprecatedGetLatestManifestsRsp.Types.Manifest manifest = new()
                        {
                            ProductId = proId,
                            Manifest_ = branch.LatestManifest
                        };
                        if (File.Exists($"{ServerConfig.Instance.Demux.DownloadGamePath}{proId}/{branch.LatestManifest}.manifest"))
                        {
                            manifest.Result = DeprecatedGetLatestManifestsRsp.Types.Manifest.Types.Result.Success;
                        }
                        else
                        {
                            manifest.Result = DeprecatedGetLatestManifestsRsp.Types.Manifest.Types.Result.NotFound;
                        }

                        Downstream.Response.DeprecatedGetLatestManifestsRsp.Manifests.Add(manifest);
                    }
                }
            }
            else
            {
                Downstream.Response.DeprecatedGetLatestManifestsRsp.Result = DeprecatedGetLatestManifestsRsp.Types.Result.ServerError;
            }
        }

        public static void GetBatchDownloadUrls(Guid ClientNumb, GetBatchDownloadUrlsReq GetBatchDownloadUrls)
        {
            Downstream = new()
            {
                Response = new()
                {
                    GetBatchDownloadUrlsRsp = new()
                    {
                        TtlSeconds = uint.MaxValue
                    }
                }
            };

            for (int i = 0; i < GetBatchDownloadUrls.UrlRequests.Count; i++)
            {
                var reg = GetBatchDownloadUrls.UrlRequests[i];

                for (int j = 0; j < reg.RelativeFilePaths.Count; j++)
                {
                    var path = reg.RelativeFilePaths[j];
                    GetBatchDownloadUrlsRsp.Types.UrlResponse url = new();
                    if (Directory.Exists($"{ServerConfig.Instance.Demux.DownloadGamePath}/{reg.ProductId}") && File.Exists($"{ServerConfig.Instance.Demux.DownloadGamePath}/{reg.ProductId}/{path}"))
                    {
                        url.Result = GetBatchDownloadUrlsRsp.Types.Result.Success;
                        url.DownloadUrls.Add(new GetBatchDownloadUrlsRsp.Types.DownloadUrls()
                        {
                            Urls = { $"{ServerConfig.Instance.HTTPS_Url}/download/{reg.ProductId}/{path}" }
                        });
                    }
                    else
                    {
                        url.Result = GetBatchDownloadUrlsRsp.Types.Result.ProductNotOwned;
                        Downstream.Response.GetBatchDownloadUrlsRsp.UrlResponses.Add(url);
                    }
                }
            }
        }

        public static void GetUplayPcTicket(Guid ClientNumb, GetUplayPCTicketReq getUplayPCTicketReq)
        {
            bool isSucces = false;
            string ticket = "";
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var ownershipBasic = DBUser.Get<UserOwnershipBasic>(userID);
                if (ownershipBasic != null)
                {
                    if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || ownershipBasic.OwnedGamesIds.Contains(getUplayPCTicketReq.UplayId))
                    {
                        ticket = JWTController.CreateUplayTicket(userID, getUplayPCTicketReq.UplayId, (int)getUplayPCTicketReq.Platform);
                        isSucces = true;
                    }
                }
            }
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    GetUplayPcTicketRsp = new()
                    {
                        ErrorCode = 0,
                        Success = isSucces,
                        UplayPcTicket = ticket

                    }
                }
            };
        }

        [Obsolete]
        public static void RegisterTemporaryOwnership(Guid ClientNumb, RegisterTemporaryOwnershipReq registerTemporaryOwnershipReq)
        {
            bool IsSuccess = false;
            List<uint> prodids = new();
            if (UserInits[ClientNumb])
            {
                prodids = FromOwnerSignature(registerTemporaryOwnershipReq.Token);
                IsSuccess = true;
            }


            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    RegisterTemporaryOwnershipRsp = new()
                    {
                        Success = IsSuccess,
                        ProductIds = { prodids }
                    }
                }
            };
        }

        public static void UnlockProductBranch(Guid ClientNumb, UnlockProductBranchReq unlockProductBranchReq)
        {
            var pid = unlockProductBranchReq.Branch.ProductId;
            var pass = unlockProductBranchReq.Branch.Password;
            UnlockProductBranchRsp.Types.Result result = UnlockProductBranchRsp.Types.Result.Denied;
            UnlockProductBranchRsp.Types.ProductBranch productBranch = new();
            if (UserInits[ClientNumb])
            {
                var branches = App.GetAppBranches(pid);
                if (branches != null)
                {
                    foreach (var branch in branches)
                    {
                        if (branch.BranchPassword == pass)
                        {
                            productBranch.BranchId = branch.BranchId;
                            productBranch.BranchName = branch.BranchName;
                            result = UnlockProductBranchRsp.Types.Result.Success;
                            break;
                        }
                    }
                };
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    UnlockProductBranchRsp = new()
                    {
                        Result = result,
                        Branch = { productBranch }
                    }
                }
            };
        }

        public static void SwitchProductBranch(Guid ClientNumb, SwitchProductBranchReq switchProductBranchReq)
        {
            OwnedGames ownedGames = new()
            {
                OwnedGames_ = { }
            };
            SwitchProductBranchRsp.Types.Result result = SwitchProductBranchRsp.Types.Result.Denied;
            if (UserInits[ClientNumb])
            {
                if (Globals.IdToUser.TryGetValue(ClientNumb, out var userId))
                {
                    if (switchProductBranchReq.DefaultBranch != null)
                    {
                        ownedGames.OwnedGames_.Add(OwnershipController.GetOwnershipGame(userId, switchProductBranchReq.DefaultBranch.ProductId, uint.MaxValue));
                        result = SwitchProductBranchRsp.Types.Result.Success;
                    }
                    if (switchProductBranchReq.SpecifiedBranch != null)
                    {
                        var bId = switchProductBranchReq.SpecifiedBranch.BranchId;
                        var pass = switchProductBranchReq.SpecifiedBranch.Password;
                        var pid = switchProductBranchReq.SpecifiedBranch.ProductId;

                        var branch = App.GetAppBranch(pid, bId);
                        if (branch != null)
                        {
                            if (branch.BranchPassword == pass)
                            {
                                ownedGames.OwnedGames_.Add(OwnershipController.GetOwnershipGame(userId, pid, bId));
                                result = SwitchProductBranchRsp.Types.Result.Success;
                            }

                        }
                    }
                }
            }


            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    SwitchProductBranchRsp = new()
                    {
                        Result = result,
                        Products = ownedGames
                    }
                }
            };
        }

        public static void GetProductConfig(Guid ClientNumb, GetProductConfigReq getProductConfigReq)
        {
            GetProductConfigRsp.Types.Result result = GetProductConfigRsp.Types.Result.InternalServerError;
            string Conf = string.Empty;

            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var user = DBUser.Get<UserOwnershipBasic>(userID);
                if (user != null)
                {
                    if (ServerConfig.Instance.Demux.Ownership.EnableConfigRequest || user.OwnedGamesIds.Contains(getProductConfigReq.ProductId))
                    {
                        var gameconfig = App.GetAppConfig(getProductConfigReq.ProductId);
                        if (gameconfig != null)
                        {
                            Conf = File.ReadAllText("ServerFiles/ProductConfigs/" + gameconfig.Configuration);
                            result = GetProductConfigRsp.Types.Result.Success;
                        }
                    }
                }
            }
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    GetProductConfigRsp = new()
                    {
                        Result = result,
                        Configuration = Conf
                    }
                }
            };
        }


        #endregion
    }

    public class Pusher
    {
        public static void Pushes(Guid UserId, Push push)
        {
            if (Globals.UserToId.TryGetValue(UserId, out var ClientNumb))
            {
                if (push?.OwnedGamePush != null) { OwnedGamePusher(ClientNumb, push.OwnedGamePush); }
                if (push?.UplayCoreGameInitializedPush != null) { UplayCoreGameInitializedPusher(ClientNumb, push.UplayCoreGameInitializedPush); }
                if (push?.SubscriptionPush != null) { SubscriptionPusher(ClientNumb, push.SubscriptionPush); }
            }
        }

        public static void OwnedGamePusher(Guid ClientNumb, OwnedGamePush OwnedGamePusher)
        {
            var username = Globals.IdToUser[ClientNumb];
            uint userCon = Auth.GetConIdByUserAndName(username, Name);
            var bstr = OwnedGamePusher.ToByteString();

            DemuxController.SendToClient(ClientNumb, bstr, userCon);
        }

        public static void UplayCoreGameInitializedPusher(Guid ClientNumb, UplayCoreGameInitializedPush UplayCoreGameInitializedPusher)
        {
            var username = Globals.IdToUser[ClientNumb];
            uint userCon = Auth.GetConIdByUserAndName(username, Name);
            var bstr = UplayCoreGameInitializedPusher.ToByteString();

            DemuxController.SendToClient(ClientNumb, bstr, userCon);
        }

        public static void SubscriptionPusher(Guid ClientNumb, SubscriptionPush SubscriptionPusher)
        {
            var username = Globals.IdToUser[ClientNumb];
            uint userCon = Auth.GetConIdByUserAndName(username, Name);
            var bstr = SubscriptionPusher.ToByteString();

            DemuxController.SendToClient(ClientNumb, bstr, userCon);
        }
    }
}
