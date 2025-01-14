using LiteDB;
using ServerCore.Models.Store;

namespace ServerCore.DB;

public class Store
{
    public readonly static string DBName = Prepare.DatabasePath + "Store.db";
    public readonly static string StoreBDB = "Store";
    #region Store
    public static void Add(StoreCore store)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<StoreCore>(StoreBDB);
        if (!col.Exists(x => x.ProductId == store.ProductId))
        {
            col.Insert(store);
        }
    }

    public static void Edit(uint prodId, StoreCore store)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<StoreCore>(StoreBDB);
        var toReplace = col.FindOne(x => x.ProductId == prodId);

        if (toReplace != null)
        {
            toReplace = store;
            col.Update(toReplace);
        }
    }

    public static StoreCore? GetStoreByProdId(uint prodId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<StoreCore>(StoreBDB);
        return col.FindOne(x => x.ProductId == prodId);
    }

    public static List<StoreCore> GetAllStore()
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<StoreCore>(StoreBDB);
        return col.FindAll().ToList();
    }

    public static void Delete(uint prodId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<StoreCore>(StoreBDB);

        var toDel = col.FindOne(x => x.ProductId == prodId);
        if (toDel != null)
        {
            col.Delete((int)prodId);
        }
    }
    #endregion
}