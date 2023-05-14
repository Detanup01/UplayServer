using Google.Protobuf;
using Uplay.OwnershipCache;
using SharedLib.Shared;
using SharedLib.Server.DB;

namespace SharedLib.Server.Json
{
    public class Owners
    {
        public static OwnershipCache GetOwnershipTXT(string UserId)
        {
            return OwnershipCache.Parser.ParseJson(File.ReadAllText($"ServerFiles/CacheFiles/{UserId}.ownershipcache.txt"));
        }

        public static Uplay.Ownership.OwnedGames GetOwnershipGames(string UserId, Dictionary<uint, uint>? branches)
        {
            var ownedGames = new Uplay.Ownership.OwnedGames()
            { 
                OwnedGames_ = { }
            };

            var owlist = DBUser.GetOwnershipList(UserId);
            if (owlist != null) 
            {
                foreach (var ow in owlist)
                {
                    var app = App.GetAppConfig(ow.ProductId);
                    if (app == null)
                        continue;

                    uint branch = 0;
                    var branchList = App.GetAppBranches(ow.ProductId);
                    if (branchList == null)
                        continue;
                    if (branches != null)
                        branches.TryGetValue(ow.ProductId, out branch);

                    var appbranch = branchList.Find(x=>x.branch_id == branch);

                    if (appbranch == null)
                        continue;

                    ownedGames.OwnedGames_.Add(new Uplay.Ownership.OwnedGame()
                    {
                        PendingKeystorageOwnership = false,
                        ProductId = ow.ProductId,
                        ActivationIds = { ow.ActivationIds },
                        Owned = ow.IsOwned,
                        UplayId = ow.ProductId,
                        PackageOwnershipState = ow.PackageState,
                        LockedBySubscription = ow.IsLockedSubscription,
                        SubscriptionTypes = { ow.Subscriptions },
                        CdKey = ow.CD_Key,
                        OrbitProductId = ow.ProductId,
                        DenuvoActivationOverwrite = ow.DenuvoActivation,
                        SuspensionType = ow.Suspension,
                        ActivationType = ow.Activation,
                        TargetPartner = ow.TargetPartner,
                        State = (uint)app.state,
                        StoreData = new()
                        {
                            StoreRef = app.storereference,
                            PromotionScore = 0,
                            Associations = { app.associations },
                            Configuration = app.store_configuration
                        },
                        IngameStoreData = new()
                        {
                            StoreRef = app.storereference,
                            PromotionScore = 0,
                            Associations = { app.associations },
                            Configuration = app.store_configuration
                        },
                        UbiservicesSpaceId = app.space_id,
                        UbiservicesAppId = app.app_id,
                        ActiveBranchId = branch,
                        ProductAssociations = { app.associations },
                        Balance = 0,
                        BrandId = 0,
                        Configuration = app.configuration,
                        ConfigVersion = app.config_version,
                        DeprecatedTestConfig = false,
                        DownloadId = app.productId,
                        DownloadVersion = app.download_version,
                        GameCode = app.gamecode,
                        OrbitGameVersion = app.productId,
                        OrbitGameVersionUrl = "",
                        Platform = (uint)app.platform,
                        ProductType = (uint)app.product_type,
                        TitleId = 0,
                        LatestManifest = appbranch.latest_manifest,
                        EncryptionKey = appbranch.encryption_key,
                        AvailableBranches = 
                        { 
                            new Uplay.Ownership.OwnedGame.Types.ProductBranch()
                            { 
                                BranchId = appbranch.branch_id,
                                BranchName = appbranch.branch_name
                            }
                        }

                    });
                }
            }
            return ownedGames;
        }

        public static Uplay.Ownership.OwnedGame GetOwnershipGame(string UserId, uint productId, uint branchId)
        {
            var owship = GetOwnershipTXT(UserId);
            var og = owship.OwnedGames.Where(x=>x.ProductId == productId).FirstOrDefault();
            Uplay.Ownership.OwnedGame ownedgame = new()
            {
                ProductId = og.ProductId,
                ActivationIds = { og.ActivationIds },
                ProductAssociations = { og.ActivationIds },
                CdKey = og.CdKey,
                Owned = true,
                State = 3,
                GameCode = og.GameCode,
                Balance = 0,
                BrandId = 0,
                EncryptionKey = string.Empty,
                LockedBySubscription = false,
                ActivationType = Uplay.Ownership.OwnedGame.Types.ActivationType.Purchase,
                DenuvoActivationOverwrite = Uplay.Ownership.OwnedGame.Types.DenuvoActivationOverwrite.Default,
                PackageOwnershipState = Uplay.Ownership.OwnedGame.Types.PackageOwnershipState.Full
            };
            var config = GameConfig.GetGameConfig(og.ProductId, branchId);
            if (config != null)
            {
                ownedgame.ActiveBranchId = config.branches.active_branch_id;
                ownedgame.Configuration = File.ReadAllText("ServerFiles/ProductConfigs/" + config.configuration);
                ownedgame.LatestManifest = config.latest_manifest;
                ownedgame.ActiveBranchId = config.branches.active_branch_id;
            }

            return ownedgame;
        }

        //TODO: OWnership Ownedgames parse and save to DB
    }
}
