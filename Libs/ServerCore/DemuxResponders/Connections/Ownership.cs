﻿using Google.Protobuf;
using Newtonsoft.Json;
using System.Text;
using Uplay.Ownership;
using SharedLib.Shared;
using SharedLib.Server.DB;
using SharedLib.Server.Json;

namespace Core.DemuxResponders
{
    public class Ownership
    {
        public static ByteString GetOwnerSignature(string UserId)
        {
            var user = User.GetUser(UserId);
            if (user != null)
            {
                List<byte> SignList = new();
                int i = 0;
                foreach (var id in user.Ownership.OwnedGamesIds)
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
                if (user.Ownership.OwnedGamesIds.Count > 30)
                {
                    sigb64 = CompressB64.GetZstdB64(SignatureByte);
                }

                var userId64 = CompressB64.GetZstdB64(UserId);
                return ByteString.CopyFrom(Encoding.UTF8.GetBytes(B64.ToB64(userId64 + "_OwnerSignature_" + sigb64)));
            }
            return ByteString.CopyFrom(Encoding.UTF8.GetBytes("T3duZXJTaWduYXR1cmVfSXNGYWlsZWQ="));
        }

        public static List<uint> FromOwnerSignature(string token)
        {
            List<uint> prodids = new();
            var realtokenb = B64.FromB64(token);
            var realtoken = B64.FromB64(realtokenb);
            var tokensp = realtoken.Split("_OwnerSignature_");
            var userid64 = tokensp[0];
            var sig64 = tokensp[1];
            var userId = B64.FromB64(CompressB64.GetUnZstdB64(Convert.FromBase64String(userid64)));
            var user = User.GetUser(userId);
            byte[] siglist = { };
            if (user.Ownership.OwnedGamesIds.Count > 30)
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


        public static ByteString GetSignofOwnership(string UserId, uint ProdId)
        {
            var gameconfig = GameConfig.GetGameConfig(ProdId);
            if (gameconfig != null)
            {
                var userId64 = CompressB64.GetZstdB64(UserId);
                string gameconfigb64 = CompressB64.GetZstdB64(JsonConvert.SerializeObject(gameconfig));

                return ByteString.CopyFrom(Encoding.UTF8.GetBytes(CompressB64.GetZstdB64(CompressB64.GetDeflateB64(userId64 + "_" + gameconfigb64))));
            }
            return ByteString.CopyFrom(Encoding.UTF8.GetBytes("T3duZXJTaWduYXR1cmVfSXNGYWlsZWQ="));
        }

        public static readonly string Name = "ownership_service";
        public class Up
        {
            public static Downstream Downstream = null;
            public static void UpstreamConverter(int ClientNumb, ByteString bytes)
            {
                var UpstreamBytes = bytes.Skip(4).ToArray();
                var Upsteam = Upstream.Parser.ParseFrom(UpstreamBytes);
                Utils.WriteFile(Upsteam.ToString(), $"logs/client_{ClientNumb}_party_req.log");
                if (Upsteam != null)
                {
                    if (Upsteam.Request != null)
                    {
                        ReqRSP.Requests(ClientNumb, Upsteam.Request);
                        while (ReqRSP.IsIdDone == false)
                        {

                        }
                        Utils.WriteFile(Upsteam.ToString(), $"logs/client_{ClientNumb}_party_req.log");
                        Downstream = ReqRSP.Downstream;
                    }
                }
            }
        }

        public class ReqRSP
        {
            public static Downstream Downstream = null;
            public static uint ReqId = 0;
            public static bool IsIdDone = false;
            public static Dictionary<int, bool> UserInits = new();
            public static void Requests(int ClientNumb, Req req)
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
                if (req?.RegisterTemporaryOwnershipReq != null) { RegisterTemporaryOwnership(ClientNumb, req.RegisterTemporaryOwnershipReq); }
                if (req?.RetryUplayCoreInitializeReq != null) { Console.WriteLine(ClientNumb + " " + req.RetryUplayCoreInitializeReq); }
                if (req?.SignOwnershipReq != null) { SignOwnership(ClientNumb, req.SignOwnershipReq); }
                if (req?.SwitchProductBranchReq != null) { SwitchProductBranch(ClientNumb, req.SwitchProductBranchReq); }
                if (req?.UnlockProductBranchReq != null) { UnlockProductBranch(ClientNumb, req.UnlockProductBranchReq); }
                if (req?.WaiveGameWithdrawalRightsReq != null) { Console.WriteLine(ClientNumb + " " + req.WaiveGameWithdrawalRightsReq); }
                IsIdDone = true;
            }

            public static void x(int ClientNumb, ClaimKeystorageKeyReq claimKeystorageKeyReq)
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
            public static void Initialize(int ClientNumb, InitializeReq Initializer)
            {
                bool IsSuccess = false;
                var userID = Globals.IdToUser[ClientNumb];
                Console.WriteLine(userID + " OW Init");
                var user = User.GetUser(userID);
                if (user != null)
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
                if (user != null)
                {
                    UserInits.Add(ClientNumb, true);
                    Downstream.Response.InitializeRsp.SubscriptionType = user.Ownership.UbiPlus;
                    Downstream.Response.InitializeRsp.SubscriptionState = (SubscriptionState)user.Ownership.UbiPlus;

                    Downstream.Response.InitializeRsp.OwnedGamesContainer.Signature = GetOwnerSignature(userID);
                    if (File.Exists($"ServerFiles/CacheFiles/{userID}.ownershipcache"))
                    {
                        Console.WriteLine("cache");
                        Downstream.Response.InitializeRsp.OwnedGamesContainer.ProductIds.AddRange(Owners.GetOwnershipProdIds(userID));
                        Downstream.Response.InitializeRsp.OwnedGamesContainer.VisibleOrInstallableProductIds.AddRange(Owners.GetOwnershipInstallable(userID));
                        Downstream.Response.InitializeRsp.OwnedGames = Owners.GetOwnershipGames(userID);
                    }
                    else if (File.Exists($"ServerFiles/CacheFiles/{userID}.ownershipInit"))
                    {
                        Console.WriteLine("init");
                        Downstream.Response.InitializeRsp = InitializeRsp.Parser.ParseFrom(File.ReadAllBytes($"ServerFiles/CacheFiles/{userID}.ownershipInit"));
                    }
                    else
                    {
                        Console.WriteLine("else");
                        Owners.MakeOwnershipFromUser(userID, user.Ownership);
                        Downstream.Response.InitializeRsp.OwnedGamesContainer.ProductIds.AddRange(Owners.GetOwnershipProdIds(userID));
                        Downstream.Response.InitializeRsp.OwnedGamesContainer.VisibleOrInstallableProductIds.AddRange(Owners.GetOwnershipInstallable(userID));
                        Downstream.Response.InitializeRsp.OwnedGames = Owners.GetOwnershipGames(userID);
                    }

                }
            }

            public static void OwnershipToken(int ClientNumb, OwnershipTokenReq OwnershipToken)
            {
                bool IsSuccess = false;
                ulong Exp = 0;
                string Token = "";
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var user = User.GetUser(userID);
                    if (user != null)
                    {
                        if (ServerConfig.DMX.GlobalOwnerShipCheck | user.Ownership.OwnedGamesIds.Contains(OwnershipToken.ProductId))
                        {
                            var gameconfig = GameConfig.GetGameConfig(OwnershipToken.ProductId);
                            if (gameconfig != null)
                            {
                                Token = jwt.CreateOwnershipToken(userID, OwnershipToken.ProductId, OwnershipToken.ProductId, gameconfig.branches.active_branch_id, gameconfig.appflags);
                                Exp = (ulong)jwt.GetExp(Token);
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

            public static void SignOwnership(int ClientNumb, SignOwnershipReq SignOwnership)
            {
                bool IsSuccess = false;
                ByteString signature = ByteString.Empty;
                uint BranchId = 0;
                ulong Exp = (ulong)DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();

                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var user = User.GetUser(userID);
                    if (user != null)
                    {
                        if (ServerConfig.DMX.GlobalOwnerShipCheck || user.Ownership.OwnedGamesIds.Contains(SignOwnership.ProductId))
                        {
                            var gameconfig = GameConfig.GetGameConfig(SignOwnership.ProductId);
                            if (gameconfig != null)
                            {
                                BranchId = gameconfig.branches.active_branch_id;
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

            public static void DeprecatedGetLatestManifests(int ClientNumb, DeprecatedGetLatestManifestsReq DeprecatedGetLatestManifests)
            {
                Downstream = new()
                {
                    Response = new()
                    {
                        DeprecatedGetLatestManifestsRsp = new()
                        { }
                    }
                };

                if (ServerConfig.DMX.ownership.EnableManifestRequest)
                {
                    Downstream.Response.DeprecatedGetLatestManifestsRsp.Result = DeprecatedGetLatestManifestsRsp.Types.Result.Success;
                    var prodlist = DeprecatedGetLatestManifests.DeprecatedProductIds.ToList();

                    foreach (var proId in prodlist)
                    {
                        var gameconfig = GameConfig.GetGameConfig(proId);
                        if (gameconfig != null)
                        {

                            DeprecatedGetLatestManifestsRsp.Types.Manifest manifest = new()
                            {
                                ProductId = proId,
                                Manifest_ = gameconfig.latest_manifest
                            };
                            if (File.Exists($"{ServerConfig.DMX.DownloadGamePath}{proId}/{gameconfig.latest_manifest}.manifest"))
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

            public static void GetBatchDownloadUrls(int ClientNumb, GetBatchDownloadUrlsReq GetBatchDownloadUrls)
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
                        if (Directory.Exists($"{ServerConfig.DMX.DownloadGamePath}/{reg.ProductId}") && File.Exists($"{ServerConfig.DMX.DownloadGamePath}/{reg.ProductId}/{path}"))
                        {
                            url.Result = GetBatchDownloadUrlsRsp.Types.Result.Success;
                            url.DownloadUrls.Add(new GetBatchDownloadUrlsRsp.Types.DownloadUrls()
                            {
                                Urls = { $"{ServerConfig.DMX.DownloadGameUrl}/{reg.ProductId}/{path}" }
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

            public static void GetUplayPcTicket(int ClientNumb, GetUplayPCTicketReq getUplayPCTicketReq)
            {
                bool isSucces = false;
                string ticket = "";
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var user = User.GetUser(userID);
                    if (user != null)
                    {
                        if (ServerConfig.DMX.GlobalOwnerShipCheck)
                        {
                            ticket = jwt.CreateUplayTicket(userID, getUplayPCTicketReq.UplayId, (int)getUplayPCTicketReq.Platform);
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

            public static void RegisterTemporaryOwnership(int ClientNumb, RegisterTemporaryOwnershipReq registerTemporaryOwnershipReq)
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

            public static void UnlockProductBranch(int ClientNumb, UnlockProductBranchReq unlockProductBranchReq)
            {
                var pid = unlockProductBranchReq.Branch.ProductId;
                var pass = unlockProductBranchReq.Branch.Password;
                UnlockProductBranchRsp.Types.Result result = UnlockProductBranchRsp.Types.Result.Denied;
                UnlockProductBranchRsp.Types.ProductBranch productBranch = new();
                if (UserInits[ClientNumb])
                {
                    var game = GameConfig.GetGameConfig(pid);
                    if (game != null)
                    {
                        var branches = game.branches.product_branches;
                        foreach (var branch in branches)
                        {
                            if (branch.branch_password == pass)
                            {
                                productBranch.BranchId = branch.branch_id;
                                productBranch.BranchName = branch.branch_name;
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

            public static void SwitchProductBranch(int ClientNumb, SwitchProductBranchReq switchProductBranchReq)
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
                            ownedGames.OwnedGames_.Add(Owners.GetOwnershipGame(userId, switchProductBranchReq.DefaultBranch.ProductId, uint.MaxValue));
                            result = SwitchProductBranchRsp.Types.Result.Success;
                        }
                        if (switchProductBranchReq.SpecifiedBranch != null)
                        {
                            var bId = switchProductBranchReq.SpecifiedBranch.BranchId;
                            var pass = switchProductBranchReq.SpecifiedBranch.Password;
                            var pid = switchProductBranchReq.SpecifiedBranch.ProductId;
                            var game = GameConfig.GetGameConfig(pid, bId);
                            if (game != null)
                            {
                                if (game.branches.current_branch_id == bId)
                                {
                                    var branches = game.branches.product_branches;
                                    foreach (var branch in branches)
                                    {
                                        if (branch.branch_password == pass)
                                        {
                                            ownedGames.OwnedGames_.Add(Owners.GetOwnershipGame(userId, pid, bId));
                                            result = SwitchProductBranchRsp.Types.Result.Success;
                                            break;
                                        }
                                    }
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

            public static void GetProductConfig(int ClientNumb, GetProductConfigReq getProductConfigReq)
            {
                GetProductConfigRsp.Types.Result result = GetProductConfigRsp.Types.Result.InternalServerError;
                string Conf = string.Empty;

                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var user = User.GetUser(userID);
                    if (user != null)
                    {
                        if ((ServerConfig.DMX.GlobalOwnerShipCheck | user.Ownership.OwnedGamesIds.Contains(getProductConfigReq.ProductId)) || ServerConfig.DMX.ownership.EnableConfigRequest)
                        {
                            var gameconfig = GameConfig.GetGameConfig(getProductConfigReq.ProductId);
                            if (gameconfig != null)
                            {
                                Conf = File.ReadAllText("ServerFiles/ProductConfigs/" + gameconfig.configuration);
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
            public static void Pushes(int ClientNumb, Push push)
            {
                if (push?.OwnedGamePush != null) { OwnedGamePusher(ClientNumb, push.OwnedGamePush); }
                if (push?.UplayCoreGameInitializedPush != null) { UplayCoreGameInitializedPusher(ClientNumb, push.UplayCoreGameInitializedPush); }
                if (push?.SubscriptionPush != null) { SubscriptionPusher(ClientNumb, push.SubscriptionPush); }
            }

            public static void OwnedGamePusher(int ClientNumb, OwnedGamePush OwnedGamePusher)
            {
                string username = Globals.IdToUser[ClientNumb];
                uint userCon = Auth.GetConIdByUserAndName(username, Name);
                var bstr = OwnedGamePusher.ToByteString();

                DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
            }

            public static void UplayCoreGameInitializedPusher(int ClientNumb, UplayCoreGameInitializedPush UplayCoreGameInitializedPusher)
            {
                string username = Globals.IdToUser[ClientNumb];
                uint userCon = Auth.GetConIdByUserAndName(username, Name);
                var bstr = UplayCoreGameInitializedPusher.ToByteString();

                DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
            }

            public static void SubscriptionPusher(int ClientNumb, SubscriptionPush SubscriptionPusher)
            {
                string username = Globals.IdToUser[ClientNumb];
                uint userCon = Auth.GetConIdByUserAndName(username, Name);
                var bstr = SubscriptionPusher.ToByteString();

                DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
            }
        }
    }
}