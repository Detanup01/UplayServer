using Uplay.Ownership;
using SharedLib.Server.DB;

namespace SharedLib.Server.Json
{
    public class Owners
    {
        public static OwnedGames GetOwnershipGames(string UserId, Dictionary<uint, uint>? branches)
        {
            var ownedGames = new OwnedGames()
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

                    var game = GetOwnershipGame(UserId, ow.ProductId, branch);
                    if (game == null)
                        continue;
                    
                    ownedGames.OwnedGames_.Add(game);
                }
            }
            return ownedGames;
        }

        public static OwnedGame? GetOwnershipGame(string UserId, uint productId, uint branchId)
        {
            var owbasic = DBUser.GetOwnershipBasic(UserId);
            if (owbasic == null)
                return null;

            var ow = DBUser.GetOwnership(UserId, productId);
            if (ow == null)
                return null;

            var app = App.GetAppConfig(productId);
            if (app == null)
                return null;

            uint branch = 0;
            var appbranch = App.GetAppBranch(productId, branchId);
            if (appbranch == null)
                return null;


            List<OwnedGame.Types.ProductBranch> productBranches = new();

            if (owbasic.UnlockedBranches.TryGetValue(productId, out var branchlist))
            {
                foreach (var branch_id in branchlist)
                {
                    var branch_app = App.GetAppBranch(productId, branch_id);
                    if (branch_app != null)
                    {
                        productBranches.Add(new OwnedGame.Types.ProductBranch()
                        { 
                            BranchId = branch_id,
                            BranchName = branch_app.branch_name

                        });
                    }
                }
            }

            return new OwnedGame()
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
                Configuration = File.ReadAllText("ServerFiles/ProductConfigs/" + app.configuration),
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
                AvailableBranches = { productBranches },
                UbiservicesDynamicConfig = new()    //Currently No option to set these values
                {
                    GfnAppId = app.app_id,
                    LunaAppId = app.app_id
                }
            };
        }

        /// <summary>
        /// Adding Dumped OwnedGames to the Database
        /// </summary>
        /// <param name="file"></param>
        public static void AddOwnershipGameToDB(string file)
        {
            var ow = OwnedGames.Parser.ParseFrom(File.ReadAllBytes(file));

            foreach (var games in ow.OwnedGames_)
            {
                string branchname = string.Empty;
                var currentbrach = games.AvailableBranches.ToList().Find(x => x.BranchId == games.ActiveBranchId);
                if (currentbrach != null)
                    branchname = currentbrach.BranchName;

                File.WriteAllText($"ServerFiles/ProductConfigs/{games.ProductId}.yml", games.Configuration, System.Text.Encoding.UTF8);


                App.AddAppBranches(new DB.JAppBranches()
                { 
                    latest_manifest = games.LatestManifest,
                    branch_id = games.ActiveBranchId,
                    encryption_key = games.EncryptionKey,
                    productId = games.ProductId,
                    branch_name = branchname
                });

                App.AddAppConfig(new DB.JAppConfig()
                { 
                    productId = games.ProductId,
                    staging = false,
                    state = (OwnedGame.Types.State)games.State,
                    space_id = games.UbiservicesSpaceId,
                    global_appflags = new() { Enums.AppFlags.Downloadable, Enums.AppFlags.Playable },
                    app_id = games.UbiservicesSpaceId,
                    associations = games.ProductAssociations.ToList(),
                    configuration = $"{games.ProductId}.yml",
                    download_version = games.DownloadVersion,
                    gamecode = games.GameCode,
                    config_version = games.ConfigVersion,
                    product_type = (OwnedGame.Types.ProductType)games.ProductType,
                    platform = (GetUplayPCTicketReq.Types.Platform)games.Platform,
                    product_name = $"Product {games.ProductId}",
                });
            }       
        }
    }
}
