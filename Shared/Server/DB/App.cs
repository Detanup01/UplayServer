using LiteDB;
using SharedLib.Server.Json.DB;

namespace SharedLib.Server.DB
{
    public class App
    {
        public readonly static string DBName = Prepare.DatabasePath + "App.db";
        public readonly static string AppAPI = "AppAPI";
        public readonly static string AppFlags = "AppFlags";

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
        #region AppFlags
        public static void AddFlags(uint productId, List<string> flags)
        {
            AddFlags(new()
            { 
                productId = productId,
                flags = flags
            });
        }

        public static void AddFlags(JAppFlags jAppFlags)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppFlags>(AppFlags);

                if (!col.Exists(X => X == jAppFlags))
                {
                    col.Insert(jAppFlags);
                }
            }
        }

        public static void EditFlags(uint productId, List<string> flags)
        {
            EditFlags(new()
            {
                productId = productId,
                flags = flags
            });
        }

        public static void EditFlags(JAppFlags jAppFlags)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppFlags>(AppFlags);

                var fId = col.Find(X => X.productId == jAppFlags.productId).First().Id;
                jAppFlags.Id = fId;
                col.Update(jAppFlags);
            }
        }

        public static List<string> GetFlags(uint productId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppFlags>(AppFlags);
                var x = col.Query().Where(x => x.productId == productId);
                if (x.Count() > 0)
                    return x.First().flags;
                else
                    return new();
            }
        }

        public static void DeleteFlags(uint productId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppFlags>(AppFlags);

                var found = col.Find(X => X.productId == productId).First();
                col.Delete(found.Id);
            }
        }

        #endregion
    }
}
