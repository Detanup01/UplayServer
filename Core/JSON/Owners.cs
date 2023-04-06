using Google.Protobuf;
using Uplay.OwnershipCache;

namespace Core.JSON
{
    public class Owners
    {
        public static OwnershipCache GetOwnership(string UserId)
        {
            return Extra.Parsers.ParseOwnerShipFile($"ServerFiles/CacheFiles/{UserId}.ownershipcache");
        }
        public static List<uint> GetOwnershipProdIds(string UserId)
        {
            return Extra.Parsers.ParseOwnerShipFile($"ServerFiles/CacheFiles/{UserId}.ownershipcache").ProductIds.ToList();
        }
        public static List<uint> GetOwnershipInstallable(string UserId)
        {
            return Extra.Parsers.ParseOwnerShipFile($"ServerFiles/CacheFiles/{UserId}.ownershipcache").VisibleOrInstallableProductIds.ToList();
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

                OwnedGame ownedGame = new();
                ownedGame.ProductId = ownedgame.ProductId;
                ownedGame.UplayId = ownedgame.UplayId;
                ownedGame.DownloadId = ownedgame.DownloadId;
                ownedGame.OrbitId = ownedgame.OrbitProductId;
                ownedGame.CdKey = ownedgame.CdKey;
                ownedGame.Platform = ownedgame.Platform;
                ownedGame.ProductType = ownedgame.ProductType;
                ownedGame.State = ownedgame.State;
                ownedGame.ProductAssociations.AddRange(ownedgame.ProductAssociations);
                ownedGame.GameCode = ownedgame.GameCode;
                ownedGame.BrandId = ownedgame.BrandId;
                ownedGame.PendingKeystorageOwnership = ownedgame.PendingKeystorageOwnership;
                ownedGame.LegacySpaceId = ownedgame.UbiservicesSpaceId;
                ownedGame.LegacyAppId = ownedgame.UbiservicesAppId;
                ownedGame.ActivationIds.AddRange(ownedgame.ActivationIds);
                ownedGame.TargetPartner = (OwnedGame.Types.TargetPartner)ownedgame.TargetPartner;
                ownedGame.ActivationType = (OwnedGame.Types.ActivationType)ownedgame.ActivationType;
                ownedGame.UbiServicesAppId = ownedgame.UbiservicesAppId;
                cache.OwnedGames.Add(ownedGame);
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

                OwnedGame ownedGame = new();
                ownedGame.ProductId = ownedgame;
                ownedGame.UplayId = ownedgame;
                ownedGame.DownloadId = ownedgame;
                ownedGame.OrbitId = ownedgame;
                ownedGame.ProductAssociations.AddRange(GameConfig.GetGameConfig(ownedgame).associations);
                ownedGame.ActivationIds.AddRange(GameConfig.GetGameConfig(ownedgame).associations);
                ownedGame.ActivationType = OwnedGame.Types.ActivationType.Purchase;
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
            var ownedGames = new Uplay.Ownership.OwnedGames();
            var owship = GetOwnership(UserId);

            for (int owhelper = 0; owhelper < owship.OwnedGames.Count; owhelper++)
            {
                Uplay.Ownership.OwnedGame ownedgame = new();


                ownedgame.ProductId = owship.OwnedGames[owhelper].ProductId;
                ownedgame.ActivationIds.AddRange(owship.OwnedGames[owhelper].ActivationIds);
                ownedgame.ProductAssociations.AddRange(owship.OwnedGames[owhelper].ProductAssociations);
                var config = GameConfig.GetGameConfig(owship.OwnedGames[owhelper].ProductId);
                if (config != null)
                {
                    ownedgame.ActiveBranchId = config.branches.active_branch_id;
                    ownedgame.Configuration = File.ReadAllText("ServerFiles/ProductConfigs/" + config.configuration);
                    ownedgame.LatestManifest = config.latest_manifest;
                    ownedgame.Owned = true;
                    ownedgame.State = 3;
                    ownedgame.GameCode = "CUG";
                    ownedgame.Balance = 0;
                    ownedgame.BrandId = 0;
                    ownedgame.EncryptionKey = "";
                    ownedgame.PackageOwnershipState = Uplay.Ownership.OwnedGame.Types.PackageOwnershipState.Full;
                    ownedgame.DenuvoActivationOverwrite = Uplay.Ownership.OwnedGame.Types.DenuvoActivationOverwrite.Default;
                    ownedgame.LockedBySubscription = false;
                    ownedgame.ActivationType = Uplay.Ownership.OwnedGame.Types.ActivationType.Purchase;
                    ownedgame.ActiveBranchId = config.branches.active_branch_id;
                }
                ownedGames.OwnedGames_.Add(ownedgame);
            }

            return ownedGames;
        }
    }
}
