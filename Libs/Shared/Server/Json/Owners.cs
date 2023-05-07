using Google.Protobuf;
using Uplay.OwnershipCache;
using SharedLib.Shared;

namespace SharedLib.Server.Json
{
    public class Owners
    {
        public static OwnershipCache GetOwnership(string UserId)
        {
            return Parsers.ParseOwnerShipFile($"ServerFiles/CacheFiles/{UserId}.ownershipcache");
        }

        public static OwnershipCache GetOwnershipTXT(string UserId)
        {
            return OwnershipCache.Parser.ParseJson(File.ReadAllText($"ServerFiles/CacheFiles/{UserId}.ownershipcache.txt"));
        }

        public static List<uint> GetOwnershipProdIds(string UserId)
        {
            return Parsers.ParseOwnerShipFile($"ServerFiles/CacheFiles/{UserId}.ownershipcache").ProductIds.ToList();
        }

        public static List<uint> GetOwnershipInstallable(string UserId)
        {
            return Parsers.ParseOwnerShipFile($"ServerFiles/CacheFiles/{UserId}.ownershipcache").VisibleOrInstallableProductIds.ToList();
        }

        public static void MakeOwnership(string UserId, uint prodId, List<uint> product_associations, List<uint> activation_ids)
        {
            OwnershipCache cache = new();
            cache.ProductIds.Add(prodId);
            cache.VisibleOrInstallableProductIds.Add(prodId);
            OwnedGame ownedGame = new();
            ownedGame.ProductId = prodId;
            ownedGame.UplayId = prodId;
            ownedGame.DownloadId = prodId;
            ownedGame.OrbitId = prodId;
            ownedGame.ProductAssociations.AddRange(product_associations);
            ownedGame.ActivationIds.AddRange(activation_ids);
            ownedGame.CdKey = CDKey.GenerateKey(prodId);
            ownedGame.ProductType = (uint)Uplay.Ownership.OwnedGame.Types.ProductType.Game;
            cache.OwnedGames.Add(ownedGame);
            File.Delete($"ServerFiles/CacheFiles/{UserId}.ownershipcache");
            var wr = File.OpenWrite($"ServerFiles/CacheFiles/{UserId}.ownershipcache");
            ownedGame.WriteTo(wr);
            wr.Close();
            File.WriteAllText($"ServerFiles/CacheFiles/{UserId}.ownershipcache.txt", cache.ToString());
        }

        public static void MakeOwnershipFrom(string UserId, Uplay.Ownership.OwnedGames ownedGames)
        {
            OwnershipCache cache = new();

            foreach (var ownedgame in ownedGames.OwnedGames_)
            {
                cache.VisibleOrInstallableProductIds.Add(ownedgame.ProductId);
                cache.ProductIds.Add(ownedgame.ProductId);

                cache.OwnedGames.Add(new OwnedGame()
                {
                    ProductId = ownedgame.ProductId,
                    UplayId = ownedgame.UplayId,
                    DownloadId = ownedgame.DownloadId,
                    OrbitId = ownedgame.OrbitProductId,
                    CdKey = ownedgame.CdKey,
                    Platform = ownedgame.Platform,
                    ProductType = ownedgame.ProductType,
                    State = ownedgame.State,
                    ProductAssociations = { ownedgame.ProductAssociations },
                    GameCode = ownedgame.GameCode,
                    BrandId = ownedgame.BrandId,
                    PendingKeystorageOwnership = ownedgame.PendingKeystorageOwnership,
                    LegacySpaceId = ownedgame.UbiservicesSpaceId,
                    LegacyAppId = ownedgame.UbiservicesAppId,
                    ActivationIds = { ownedgame.ActivationIds },
                    TargetPartner = (OwnedGame.Types.TargetPartner)ownedgame.TargetPartner,
                    ActivationType = (OwnedGame.Types.ActivationType)ownedgame.ActivationType,
                    UbiServicesAppId = ownedgame.UbiservicesAppId
                });
            }
            File.Delete($"ServerFiles/CacheFiles/{UserId}.ownershipcache");
            var wr = File.OpenWrite($"ServerFiles/CacheFiles/{UserId}.ownershipcache");
            cache.WriteTo(wr);
            wr.Close();
            File.WriteAllText($"ServerFiles/CacheFiles/{UserId}.ownershipcache.txt", cache.ToString());
        }

        public static void MakeOwnershipFromTXT(string UserId)
        {
            var ownership = OwnershipCache.Parser.ParseJson(File.ReadAllText($"ServerFiles/CacheFiles/{UserId}.ownershipcache.txt"));
            Console.WriteLine(ownership.ToString());
            File.Delete($"ServerFiles/CacheFiles/{UserId}.ownershipcache");
            var wr = File.OpenWrite($"ServerFiles/CacheFiles/{UserId}.ownershipcache");
            ownership.WriteTo(wr);
            wr.Close();
        }

        public static void MakeOwnershipFromUser(string UserId, User.COwnership cOwnership)
        {
            OwnershipCache cache = new();
            foreach (var ownedgame in cOwnership.OwnedGamesIds)
            {
                cache.VisibleOrInstallableProductIds.Add(ownedgame);
                cache.ProductIds.Add(ownedgame);

                OwnedGame ownedGame = new()
                { 
                    ProductId = ownedgame,
                    UplayId = ownedgame,
                    DownloadId = ownedgame,
                    OrbitId = ownedgame,
                    ActivationType = OwnedGame.Types.ActivationType.Purchase
                };
                var config = GameConfig.GetGameConfig(ownedgame);
                if (config != null)
                {
                    ownedGame.ProductAssociations.AddRange(config.associations);
                    ownedGame.ActivationIds.AddRange(config.associations);
                }
                cache.OwnedGames.Add(ownedGame);
            }
            File.Delete($"ServerFiles/CacheFiles/{UserId}.ownershipcache");
            var wr = File.OpenWrite($"ServerFiles/CacheFiles/{UserId}.ownershipcache");
            cache.WriteTo(wr);
            wr.Close();
            File.WriteAllText($"ServerFiles/CacheFiles/{UserId}.ownershipcache.txt", cache.ToString());
        }

        public static Uplay.Ownership.OwnedGames GetOwnershipGames(string UserId)
        {
            return GetOwnershipGames(UserId, uint.MaxValue);
        }

        public static Uplay.Ownership.OwnedGames GetOwnershipGames(string UserId, uint branchId)
        {
            var ownedGames = new Uplay.Ownership.OwnedGames()
            { 
                OwnedGames_ = { }
            };
            var owship = GetOwnershipTXT(UserId);
            var owcount = owship.OwnedGames.Count();
            for (int owhelper = 0; owhelper < owcount; owhelper++)
            {
                var og = owship.OwnedGames[owhelper];
                Uplay.Ownership.OwnedGame ownedgame = new()
                {
                    ProductId = og.ProductId,
                    ActivationIds = { og.ActivationIds },
                    ProductAssociations = { og.ActivationIds },
                    Owned = true,
                    State = 3,
                    GameCode = og.GameCode,
                    Balance = 0,
                    BrandId = 0,
                    EncryptionKey = string.Empty,
                    LockedBySubscription = false,
                    ActivationType = Uplay.Ownership.OwnedGame.Types.ActivationType.Purchase,
                    DenuvoActivationOverwrite = Uplay.Ownership.OwnedGame.Types.DenuvoActivationOverwrite.Default,
                    PackageOwnershipState = Uplay.Ownership.OwnedGame.Types.PackageOwnershipState.Full,
                    ProductType = og.ProductType
                };
                var config = GameConfig.GetGameConfig(og.ProductId, branchId);
                if (config != null)
                {
                    ownedgame.ActiveBranchId = config.branches.active_branch_id;
                    ownedgame.Configuration = File.ReadAllText("ServerFiles/ProductConfigs/" + config.configuration);
                    ownedgame.LatestManifest = config.latest_manifest;
                }
                ownedGames.OwnedGames_.Add(ownedgame);
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
    }
}
