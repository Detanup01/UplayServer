using LiteDB;
using ServerCore.Json.DB;

namespace ServerCore.DB;

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
            if (!col.Exists(x => x.Id == store.Id))
            {
                col.Insert(store);
            }
        }
    }

    public static void Edit(uint prodId, JStore store)
    {
        using (var db = new LiteDatabase(DBName))
        {
            var col = db.GetCollection<JStore>(StoreBDB);
            var toReplace = col.FindOne(x => x.productId == prodId);

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
            var toGet = col.FindOne(x => x.productId == prodId);

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

            var toDel = col.FindOne(x => x.productId == prodId);
            if (toDel != null)
            {
                col.Delete(toDel.Id);
            }

        }
    }

    public static void DeleteMany(uint prodId)
    {
        using (var db = new LiteDatabase(DBName))
        {
            var col = db.GetCollection<JStore>(StoreBDB);

            var toDel = col.Find(x => x.productId == prodId);

            if (toDel != null)
            {
                foreach (var item in toDel)
                {
                    col.Delete(item.Id);
                }
            }
        }
    }
    #endregion
}