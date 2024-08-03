using LiteDB;
using SharedLib.Server.Json.DB;
using static SharedLib.Server.Enums;

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
            AddAppPI(new JAppAPI()
            {
                applicationId = "f35adcb5-1911-440c-b1c9-48fdc1701c68",
                name = "ubi.com pc",
                platform = "PC",
                spaceId = "ed8600fa-a5d9-48a2-8ab7-d9f41c0c2029"
            });

            AddAppConfig(new JAppConfig()
            {
                productId = 0,
                configuration = "0.yml",
                store_configuration = "null",
                global_appflags = { AppFlags.Downloadable, AppFlags.Playable },
                app_id = "",
                space_id = "",
                staging = false,
                state = Uplay.Ownership.OwnedGame.Types.State.Playable,
                storereference = "0",
                associations = { },
                config_version = 1,
                download_version = 1,
                platform = Uplay.Ownership.GetUplayPCTicketReq.Types.Platform.Normal,
                product_name = "Example",
                product_type = Uplay.Ownership.OwnedGame.Types.ProductType.Game,
                session_max_size = 4,
                gamecode = "EXAMPLE"
            });
            AddAppBranches(new JAppBranches()
            {
                productId = 0,
                branch_name = "Example",
                latest_manifest = "0"
            });
        }

        #region AppAPI
        public static string GetSpaceId(string AppId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppAPI>(AppAPI);

                var x = col.FindOne(x => x.applicationId == AppId);
                if (x != null)
                    return x.spaceId;
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
                col.Update(jAppConfig);
            }
        }

        public static JAppConfig? GetAppConfig(uint productId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppConfig>(AppConfig);
                var x = col.FindOne(x => x.productId == productId);
                return x;
            }
        }

        public static void DeleteAppConfig(uint productId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppConfig>(AppConfig);

                var found = col.FindOne(X => X.productId == productId);
                if (found != null)
                    col.Delete(found.Id);
            }
        }

        #endregion
        #region AppBranches
        public static void AddAppBranches(JAppBranches appBranches)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppBranches>(AppBranches);

                if (!col.Exists(X => X.productId == appBranches.productId))
                {
                    col.Insert(appBranches);
                }
            }
        }

        public static void EditAppBranches(JAppBranches appBranches)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppBranches>(AppBranches);
                col.Update(appBranches);
            }
        }

        public static JAppBranches? GetAppBranch(uint productId, uint branchId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppBranches>(AppBranches);
                var x = col.FindOne(x => x.productId == productId && x.branch_id == branchId);
                return x;
            }
        }

        public static List<JAppBranches>? GetAppBranches(uint productId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppBranches>(AppBranches);
                var x = col.Find(x => x.productId == productId);
                return x.ToList();
            }
        }

        public static void DeleteAppBranches(uint productId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppBranches>(AppBranches);

                var found = col.FindOne(X => X.productId == productId);
                if (found != null)
                    col.Delete(found.Id);
            }
        }

        #endregion
    }
}
