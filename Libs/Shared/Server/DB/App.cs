using LiteDB;
using SharedLib.Server.Json.DB;

namespace SharedLib.Server.DB
{
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
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppAPI>(AppAPI);
                if (!col.Exists(x => x.applicationId == "f35adcb5-1911-440c-b1c9-48fdc1701c68"))
                {
                    var appAPI = new JAppAPI
                    {
                        applicationId = "f35adcb5-1911-440c-b1c9-48fdc1701c68",
                        name = "ubi.com pc",
                        platform = "PC",
                        spaceId = "ed8600fa-a5d9-48a2-8ab7-d9f41c0c2029"
                    };
                    col.Insert(appAPI);
                }
                var appconf = db.GetCollection<JAppConfig>(AppConfig);
                if (!appconf.Exists(x => x.productId == 0))
                {
                    appconf.Insert(new JAppConfig()
                    { 
                        productId = 0,
                        configuration = "0.yml",
                        store_configuration = "null",
                        appflags = { "Downloadable", "Playable" },
                        app_id = "",
                        space_id = "",
                        staging = false,
                        state = Uplay.Ownership.OwnedGame.Types.State.Released,
                        storereference = "0",
                        associations = { },
                        config_version = 1,
                        download_version = 1,
                        platform = Uplay.Ownership.GetUplayPCTicketReq.Types.Platform.Normal,
                        product_name = "Example",
                        product_type = Uplay.Ownership.OwnedGame.Types.ProductType.Game,
                        session_max_size = 4

                    });
                }
                var appbranch = db.GetCollection<JAppBranches>(AppBranches);
                if (!appbranch.Exists(x => x.productId == 0))
                {
                    appbranch.Insert(new JAppBranches()
                    {
                        productId = 0,
                        branch_name = "Example",
                        latest_manifest = "0"
                    });
                }
            }
        }

        #region AppAPI
        public static string GetSpaceId(string AppId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppAPI>(AppAPI);

                var x = col.Query().Where(x => x.applicationId == AppId);
                if (x.Count() > 0)
                    return x.First().spaceId;
                else
                    return "";
            }
        }
        public static void AddAppPI(JAppAPI jAppAPI)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppAPI>(AppAPI);

                if (!col.Exists(X => X == jAppAPI))
                {
                    col.Insert(jAppAPI);
                }
            }
        }
        #endregion
        #region AppConfig
        public static void AddAppConfig(JAppConfig jAppConfig)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppConfig>(AppConfig);

                if (!col.Exists(X => X.productId == jAppConfig.productId))
                {
                    col.Insert(jAppConfig);
                }
            }
        }

        public static void EditAppConfig(JAppConfig jAppConfig)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppConfig>(AppConfig);

                var fId = col.FindOne(X => X.productId == jAppConfig.productId).Id;
                jAppConfig.Id = fId;
                col.Update(jAppConfig);
            }
        }

        public static JAppConfig GetAppConfig(uint productId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppConfig>(AppConfig);
                var x = col.Query().Where(x => x.productId == productId);
                if (x.Count() > 0)
                    return x.First();
                else
                    return new() { productId = productId };
            }
        }

        public static void DeleteAppConfig(uint productId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppConfig>(AppConfig);

                var found = col.Find(X => X.productId == productId).First();
                col.Delete(found.Id);
            }
        }

        #endregion
    }
}
