using LiteDB;
using SharedLib.Server.Json.DB;

namespace SharedLib.Server.DB
{
    public class Store
    {
        public readonly static string DBName = Prepare.DatabasePath + "Store.db";
        public readonly static string Store = "Store";
    }
}
