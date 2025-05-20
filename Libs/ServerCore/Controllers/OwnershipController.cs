using Google.Protobuf;
using Newtonsoft.Json;
using ServerCore.DB;
using ServerCore.Models;
using ServerCore.Models.App;
using ServerCore.Models.User;
using SharedLib;
using System.Text;
using Uplay.Ownership;

namespace ServerCore.Controllers;

public static class OwnershipController
{

    public static bool IsOwned(Guid UserId, uint ProductId)
    {
        return ServerConfig.Instance.Demux.GlobalOwnerShipCheck ||
            (DBUser.Get<UserOwnershipBasic>(UserId) != null && DBUser.Get<UserOwnershipBasic>(UserId)!.OwnedGamesIds.Contains(ProductId));
    }

    public static OwnedGames GetOwnershipGames(Guid UserId, Dictionary<uint, uint> branches)
    {
        OwnedGames ownedGames = new()
        {
            OwnedGames_ = { }
        };

        var owlist = DBUser.GetList<UserOwnership>(UserId);
        if (owlist == null)
            return ownedGames;
        foreach (var ow in owlist)
        {
            var app = App.GetAppConfig(ow.ProductId);
            if (app == null)
                continue;

            uint branch = 0;
            var branchList = App.GetAppBranches(ow.ProductId);
            if (branchList == null)
                continue;
            if (branches.Count != 0)
                branches.TryGetValue(ow.ProductId, out branch);

            var appbranch = branchList.Find(x => x.BranchId == branch);
            if (appbranch == null)
                continue;

            var game = GetOwnershipGame(UserId, ow.ProductId, branch);
            Console.WriteLine(game);
            if (game == null)
                continue;
            ownedGames.OwnedGames_.Add(game);
        }
        return ownedGames;
    }

    public static OwnedGame? GetOwnershipGame(Guid UserId, uint productId, uint branchId)
    {
        var owbasic = DBUser.Get<UserOwnershipBasic>(UserId);
        if (owbasic == null)
            return null;

        var ow = DBUser.Get<UserOwnership>(UserId, x => x.ProductId == productId);
        if (ow == null)
            return null;

        var app = App.GetAppConfig(productId);
        if (app == null)
            return null;

        uint branch = 0;
        var appbranch = App.GetAppBranch(productId, branchId);
        if (appbranch == null)
            return null;

        List<OwnedGame.Types.ProductBranch> productBranches = [];

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
                        BranchName = branch_app.BranchName

                    });
                }
            }
        }

        var conf = Path.Combine("ServerFiles", "ProductConfigs", app.Configuration);
        if (File.Exists(conf))
            app.Configuration = File.ReadAllText(conf);

        var og = new OwnedGame()
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
            State = (uint)app.ProductState,
            StoreData = new()
            {
                StoreRef = app.StoreReference,
                PromotionScore = 0,
                Associations = { app.Associations },
                Configuration = app.StoreConfiguration
            },
            IngameStoreData = new()
            {
                StoreRef = app.StoreReference,
                PromotionScore = 0,
                Associations = { app.Associations },
                Configuration = app.StoreConfiguration
            },
            ActiveBranchId = branch,
            ProductAssociations = { app.Associations },
            Balance = 0,
            BrandId = 0,
            Configuration = app.Configuration,
            ConfigVersion = app.ConfigVersion,
            DeprecatedTestConfig = false,
            DownloadId = app.DownloadVersion,
            DownloadVersion = app.DownloadVersion,
            GameCode = app.GameCode,
            OrbitGameVersion = app.ProductId,
            OrbitGameVersionUrl = "",
            Platform = (uint)app.Platform,
            ProductType = (uint)app.ProductType,
            TitleId = 0,
            AvailableBranches = { productBranches }
        };

        if (app.SpaceId != Guid.Empty)
            og.UbiservicesSpaceId = app.SpaceId.ToString();

        if (app.AppId != Guid.Empty)
            og.UbiservicesAppId = app.AppId.ToString();

        if (!string.IsNullOrEmpty(appbranch.LatestManifest))
            og.LatestManifest = appbranch.LatestManifest;

        if (!string.IsNullOrEmpty(appbranch.EncryptionKey))
            og.EncryptionKey = appbranch.EncryptionKey;

        return og;
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

            File.WriteAllText(Path.Combine("ServerFiles", "ProductConfigs", $"{games.ProductId}.yml"), games.Configuration, System.Text.Encoding.UTF8);


            App.AddAppBranches(new AppBranches()
            {
                LatestManifest = games.LatestManifest,
                BranchId = games.ActiveBranchId,
                EncryptionKey = games.EncryptionKey,
                ProductId = games.ProductId,
                BranchName = branchname
            });

            App.AddAppConfig(new AppConfig()
            {
                ProductId = games.ProductId,
                Staging = false,
                ProductState = (OwnedGame.Types.State)games.State,
                SpaceId = Guid.Parse(games.UbiservicesSpaceId),
                AppFlags = [AppFlags.Downloadable, AppFlags.Playable],
                AppId = Guid.Parse(games.UbiservicesSpaceId),
                Associations = [.. games.ProductAssociations],
                Configuration = $"{games.ProductId}.yml",
                DownloadVersion = games.DownloadVersion,
                GameCode = games.GameCode,
                ConfigVersion = games.ConfigVersion,
                ProductType = (OwnedGame.Types.ProductType)games.ProductType,
                Platform = (GetUplayPCTicketReq.Types.Platform)games.Platform,
                ProductName = $"Product {games.ProductId}",
            });
        }
    }

    public static ByteString GetOwnerSignature(Guid UserId)
    {
        var owbasic = DBUser.Get<UserOwnershipBasic>(UserId);
        if (owbasic == null)
            return ByteString.CopyFrom(Encoding.UTF8.GetBytes("T3duZXJTaWduYXR1cmVfSXNGYWlsZWQ="));
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

        if (owbasic == null)
            return [];
        byte[] siglist = [];
        if (owbasic.OwnedGamesIds.Count > 30)
            siglist = Convert.FromBase64String(CompressB64.GetUnZstdB64(Convert.FromBase64String(sig64)));
        else
            siglist = Convert.FromBase64String(sig64);
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
        var ownership = DBUser.Get<UserOwnership>(UserId, x => x.ProductId == ProdId);
        if (ownership == null)
            return ByteString.CopyFrom(Encoding.UTF8.GetBytes("T3duZXJTaWduYXR1cmVfSXNGYWlsZWQ="));
        var userId64 = CompressB64.GetZstdB64(UserId.ToString());
        string ownershipb64 = CompressB64.GetZstdB64(JsonConvert.SerializeObject(ownership));

        return ByteString.CopyFrom(Encoding.UTF8.GetBytes(CompressB64.GetZstdB64(CompressB64.GetDeflateB64(userId64 + "_" + ownershipb64))));
    }
}
