using LiteDB;
using SharedLib.Server.Json.DB;

namespace SharedLib.Server.DB
{
    public class Store
    {
        public readonly static string DBName = Prepare.DatabasePath + "Store.db";
        public readonly static string StoreBDB = "Store";
        #region Store
        public static void Add(JStore store)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JStore>(StoreBDB);
                if (!col.Exists(x => x == store))
                {
                    var x = col.Count();
                    col.Insert(store);
                }
            }
        }

        public static void Edit(uint prodId, JStore store)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JStore>(StoreBDB);
                var toReplace = col.Find(x => x.productId == prodId).First();

                if (toReplace != null)
                {
                    toReplace = store;
                    col.Update(toReplace);
                }
            }
        }

        public static JStore? GetStoreByProdId(uint prodId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JStore>(StoreBDB);
                var toGet = col.Find(x => x.productId == prodId).First();

                if (toGet != null)
                {
                    return toGet;
                }
                else
                    return null;

            }
        }

        public static List<JStore>? GetAllStore()
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JStore>(StoreBDB);
                var toGet = col.FindAll().ToList();

                if (toGet != null)
                {
                    return toGet;
                }
                else
                    return null;

            }
        }


        public static void Delete(uint prodId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JStore>(StoreBDB);

                var toDel = col.Find(x => x.productId == prodId).Last();

                col.Delete(toDel.Id);
            }
        }

        public static void DeleteMany(uint prodId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JStore>(StoreBDB);

                col.DeleteMany(x=>x.productId == prodId);
            }
        }
        #endregion
    }
}
