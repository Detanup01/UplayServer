using Core.JSON;
using Core.SQLite;
using Google.Protobuf;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Uplay.Ownership;
using static Core.JSON.GameConfig;

namespace Core.DemuxResponsers
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
                    SignList.Add((byte)id);
                    i++;
                    SignList.Add((byte)0xFF);

                }
                var SignatureByte = SignList.ToArray();
                ByteString Signature = ByteString.CopyFrom(SignatureByte);
                var sigb64 = Signature.ToBase64();
                if (user.Ownership.OwnedGamesIds.Count > 30)
                {
                    sigb64 = Utils.GetZstdB64(SignatureByte);
                }

                var userId64 = Utils.GetZstdB64(UserId);
                return ByteString.CopyFrom(Encoding.UTF8.GetBytes(Utils.Base64Encode(userId64 + "_OwnerSignature_" + sigb64)));
            }
            return ByteString.CopyFrom(Encoding.UTF8.GetBytes("T3duZXJTaWduYXR1cmVfSXNGYWlsZWQ="));
        }

        public static ByteString GetSignofOwnership(string UserId,uint ProdId)
        {
            var gameconfig = GameConfig.GetGameConfig(ProdId);
            if (gameconfig != null)
            {
                var userId64 = Utils.GetZstdB64(UserId);
                string gameconfigb64 = Utils.GetZstdB64(JsonConvert.SerializeObject(gameconfig));

                return ByteString.CopyFrom(Encoding.UTF8.GetBytes(Utils.GetZstdB64(Utils.GetDeflateB64(userId64 + "_" + gameconfigb64))));
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
                Utils.WriteFile(Upsteam.ToString(), $"client_{ClientNumb}_party_req.log");
                if (Upsteam != null)
                {
                    if (Upsteam.Request != null)
                    {
                        ReqRSP.Requests(ClientNumb, Upsteam.Request);
                        while (ReqRSP.IsIdDone == false)
                        {

                        }
                        Utils.WriteFile(Upsteam.ToString(), $"client_{ClientNumb}_party_req.log");
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
                File.AppendAllText($"client_{ClientNumb}_ownership_req.log", req.ToString() + "\n");
                ReqId = req.RequestId;
                if (req?.ClaimKeystorageKeyReq != null) { Console.WriteLine(ClientNumb + " " + req.ClaimKeystorageKeyReq); }
                if (req?.ConsumeOwnershipReq != null) { Console.WriteLine(ClientNumb + " " + req.ConsumeOwnershipReq); }
                if (req?.DeprecatedGetLatestManifestsReq != null) { DeprecatedGetLatestManifests(ClientNumb, req.DeprecatedGetLatestManifestsReq); }
                if (req?.DeprecatedGetProductFromCdKeyReq != null) { Console.WriteLine(ClientNumb + " " + req.DeprecatedGetProductFromCdKeyReq); }
                if (req?.GetBatchDownloadUrlsReq != null) { GetBatchDownloadUrls(ClientNumb, req.GetBatchDownloadUrlsReq); }
                if (req?.GetGameTimeTicketReq != null) { Console.WriteLine(ClientNumb + " " + req.GetGameTimeTicketReq); }
                if (req?.GetGameTokenReq != null) { Console.WriteLine(ClientNumb + " " + req.GetGameTokenReq); }
                if (req?.GetGameWithdrawalRightsReq != null) { Console.WriteLine(ClientNumb + " " + req.GetGameWithdrawalRightsReq); }
                if (req?.GetProductConfigReq != null) { Console.WriteLine(ClientNumb + " " + req.GetProductConfigReq); }
                if (req?.GetUplayPcTicketReq != null) { Console.WriteLine(ClientNumb + " " + req.GetUplayPcTicketReq); }
                if (req?.InitializeReq != null) { Initialize(ClientNumb, req.InitializeReq); }
                if (req?.OwnershipTokenReq != null) { OwnershipToken(ClientNumb, req.OwnershipTokenReq); }
                if (req?.RegisterOwnershipByCdKeyReq != null) { Console.WriteLine(ClientNumb + " " + req.RegisterOwnershipByCdKeyReq); }
                if (req?.RegisterOwnershipFromOculusReq != null) { Console.WriteLine(ClientNumb + " " + req.RegisterOwnershipFromOculusReq); }
                if (req?.RegisterOwnershipFromWegameReq != null) { Console.WriteLine(ClientNumb + " " + req.RegisterOwnershipFromWegameReq); }
                if (req?.RegisterOwnershipReq != null) { Console.WriteLine(ClientNumb + " " + req.RegisterOwnershipReq); }
                if (req?.RegisterOwnershipSteamPopReq != null) { Console.WriteLine(ClientNumb + " " + req.RegisterOwnershipSteamPopReq); }
                if (req?.RegisterTemporaryOwnershipReq != null) { Console.WriteLine(ClientNumb + " " + req.RegisterTemporaryOwnershipReq); }
                if (req?.RetryUplayCoreInitializeReq != null) { Console.WriteLine(ClientNumb + " " + req.RetryUplayCoreInitializeReq); }
                if (req?.SignOwnershipReq != null) { SignOwnership(ClientNumb, req.SignOwnershipReq); }
                if (req?.SwitchProductBranchReq != null) { Console.WriteLine(ClientNumb + " " + req.SwitchProductBranchReq); }
                if (req?.UnlockProductBranchReq != null) { Console.WriteLine(ClientNumb + " " + req.UnlockProductBranchReq); }
                if (req?.WaiveGameWithdrawalRightsReq != null) { Console.WriteLine(ClientNumb + " " + req.WaiveGameWithdrawalRightsReq); }
                IsIdDone = true;
            }

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
                        if (Config.DMX.GlobalOwnerShipCheck || user.Ownership.OwnedGamesIds.Contains(OwnershipToken.ProductId))
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
                        if (Config.DMX.GlobalOwnerShipCheck || user.Ownership.OwnedGamesIds.Contains(SignOwnership.ProductId))
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

                if (Config.DMX.ownership.EnableManifestRequest)
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
                            if (File.Exists($"{Config.DMX.DownloadGamePath}{proId}/{gameconfig.latest_manifest}.manifest"))
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

                foreach (var req in GetBatchDownloadUrls.UrlRequests)
                {
                    if (Directory.Exists($"{Config.DMX.DownloadGamePath}/{req.ProductId}"))
                    {
                        foreach (var path in req.RelativeFilePaths)
                        {
                            if (File.Exists($"{Config.DMX.DownloadGamePath}/{req.ProductId}/{path}"))
                            {
                                //send back success and downloadURL
                            }
                        }
                    }
                    else
                    {
                        // send back not owned
                        //Downstream.Response.GetBatchDownloadUrlsRsp.UrlResponses
                    }


                }

            }

            


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
                uint userCon = UserDMX.GetConIdByUserAndName(username, Name);
                var bstr = OwnedGamePusher.ToByteString();

                DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
            }

            public static void UplayCoreGameInitializedPusher(int ClientNumb, UplayCoreGameInitializedPush UplayCoreGameInitializedPusher)
            {
                string username = Globals.IdToUser[ClientNumb];
                uint userCon = UserDMX.GetConIdByUserAndName(username, Name);
                var bstr = UplayCoreGameInitializedPusher.ToByteString();

                DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
            }

            public static void SubscriptionPusher(int ClientNumb, SubscriptionPush SubscriptionPusher)
            {
                string username = Globals.IdToUser[ClientNumb];
                uint userCon = UserDMX.GetConIdByUserAndName(username, Name);
                var bstr = SubscriptionPusher.ToByteString();

                DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
            }
        }
    }
}
