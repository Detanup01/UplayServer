using LiteDB;
using ServerCore.Models.App;

namespace ServerCore.DB;

public class App
{
    public readonly static string DBName = Prepare.DatabasePath + "App.db";
    public readonly static string AppAPI = "AppAPI";
    public readonly static string AppConfig = "AppConfig"; 
    public readonly static string AppBranches = "AppBranches";

    /// <summary>
    /// Create App.db and filling with Basic data
    /// </summary>
    public static void Init()
    {
        AddAppPI(new AppAPI()
        {
            ApplicationId = Guid.Parse("f35adcb5-1911-440c-b1c9-48fdc1701c68"),
            Name = "ubi.com pc",
            Platform = "PC",
            SpaceId = Guid.Parse("ed8600fa-a5d9-48a2-8ab7-d9f41c0c2029")
        });

        AddAppConfig(new AppConfig()
        {
            ProductId = 0,
            Configuration = "0.yml",
            StoreConfiguration = "null",
            AppFlags = { AppFlags.Downloadable, AppFlags.Playable },
            AppId = Guid.NewGuid(),
            SpaceId = Guid.NewGuid(),
            Staging = false,
            ProductState = Uplay.Ownership.OwnedGame.Types.State.Playable,
            StoreReference = "0",
            Associations = { },
            ConfigVersion = 1,
            DownloadVersion = 1,
            Platform = Uplay.Ownership.GetUplayPCTicketReq.Types.Platform.Normal,
            ProductName = "Example",
            ProductType = Uplay.Ownership.OwnedGame.Types.ProductType.Game,
            SessionMaxSize = 4,
            GameCode = "EXAMPLE"
        });
        AddAppBranches(new AppBranches()
        {
            ProductId = 0,
            BranchName = "Example",
            LatestManifest = "0"
        });

        AddAppPI(new AppAPI()
        {
            ApplicationId = Guid.Parse("043a4bd9-9336-4cc3-b51c-1da703f89f87"),
            Name = "The Crew PC",
            Platform = "PC",
            SpaceId = Guid.Parse("ee2ed4a6-4d8c-4d20-8777-c6355c21666f"),
            DisplayName = "The Crew PC",
        });
    }

    #region AppAPI
    public static Guid GetSpaceId(Guid AppId)
    {
        using (LiteDatabase db = new(DBName))
        {
            var col = db.GetCollection<AppAPI>(AppAPI);

            var x = col.FindOne(x => x.ApplicationId == AppId);
            if (x != null)
                return x.SpaceId;
            else
                return Guid.Empty;
        }
    }
    public static void AddAppPI(AppAPI jAppAPI)
    {
        using LiteDatabase db = new(DBName);
        var col = db.GetCollection<AppAPI>(AppAPI);

        if (!col.Exists(X => X == jAppAPI))
        {
            col.Insert(jAppAPI);
        }
    }
    #endregion
    #region AppConfig
    public static void AddAppConfig(AppConfig jAppConfig)
    {
        using LiteDatabase db = new(DBName);
        var col = db.GetCollection<AppConfig>(AppConfig);

        if (!col.Exists(X => X.ProductId == jAppConfig.ProductId))
        {
            col.Insert(jAppConfig);
        }
    }

    public static void EditAppConfig(AppConfig jAppConfig)
    {
        using LiteDatabase db = new(DBName);
        var col = db.GetCollection<AppConfig>(AppConfig);
        col.Update(jAppConfig);
    }

    public static AppConfig? GetAppConfig(uint productId)
    {
        using LiteDatabase db = new(DBName);
        var col = db.GetCollection<AppConfig>(AppConfig);
        return col.FindOne(x => x.ProductId == productId);
    }

    public static void DeleteAppConfig(uint productId)
    {
        using LiteDatabase db = new(DBName);
        var col = db.GetCollection<AppConfig>(AppConfig);

        col.Delete((int)productId);
    }

    #endregion
    #region AppBranches
    public static void AddAppBranches(AppBranches appBranches)
    {
        using LiteDatabase db = new(DBName);
        var col = db.GetCollection<AppBranches>(AppBranches);

        if (!col.Exists(X => X.ProductId == appBranches.ProductId))
        {
            col.Insert(appBranches);
        }
    }

    public static void EditAppBranches(AppBranches appBranches)
    {
        using LiteDatabase db = new(DBName);
        var col = db.GetCollection<AppBranches>(AppBranches);
        col.Update(appBranches);
    }

    public static AppBranches? GetAppBranch(uint productId, uint branchId)
    {
        using LiteDatabase db = new(DBName);
        var col = db.GetCollection<AppBranches>(AppBranches);
        return col.FindOne(x => x.ProductId == productId && x.BranchId == branchId);
    }

    public static List<AppBranches>? GetAppBranches(uint productId)
    {
        using LiteDatabase db = new(DBName);
        var col = db.GetCollection<AppBranches>(AppBranches);
        return col.Find(x => x.ProductId == productId).ToList();
    }

    public static void DeleteAppBranches(uint productId)
    {
        using LiteDatabase db = new(DBName);
        var col = db.GetCollection<AppBranches>(AppBranches);

        col.Delete((int)productId);
    }

    #endregion
}