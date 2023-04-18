using SharedLib.Server.Json;
using SharedLib.Shared;

namespace Core.HTTP
{
    internal class StoreHandler
    {
        public static string StoreHandlerCallback(string url, out string contentType)
        {
            contentType = "text/plain; charset=utf-8";
            return "FAILED";
        }
    }
}
