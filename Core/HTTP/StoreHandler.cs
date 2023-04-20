using SharedLib.Server.DB;
using SharedLib.Server.Json;
using SharedLib.Shared;
using static SharedLib.Server.Enums;

namespace Core.HTTP
{
    internal class StoreHandler
    {
        public static string StoreHandlerCallback(Dictionary<string, string> headers, string url, out string contentType)
        {
            Console.WriteLine("STOOOOOREEEE");
            string result = "FAILED";
            contentType = "text/plain; charset=utf-8";
            // url schema:
            /*
            {basic}/store/?p={productId}
            
            {basic}/store/{storereference}
            
            */
            if (url.Contains("?"))
            {
                User user = null;
                var reason = "FAILED";
                var productid = url.Replace("/store/?p=", "");
                if (!uint.TryParse(productid, out var pid))
                { 
                
                }
                if (headers.TryGetValue("authorization", out var auth))
                {
                    auth = auth.Split(" t=")[1];
                    var id = Auth.GetUserIdByToken(auth, TokenType.Ticket);
                    user = User.GetUser(id);
                }

                if (user != null)
                {
                    if (user.Ownership.OwnedGamesIds.Contains(pid))
                    {
                        reason = "FAILED (Already own)";
                    }
                    else
                    {
                        var config = GameConfig.GetGameConfig(pid);
                        if (config != null)
                        {
                            user.Ownership.OwnedGamesIds.Add(pid);
                            User.SaveUser(user.UserId, user);
                            Owners.MakeOwnershipFromUser(user.UserId, user.Ownership);
                            reason = "SUCCESS";
                        }
                        else
                        {
                            reason = "FAILED (No ProductId)";
                        }
                    }
                }

                var storepath = Path.Combine(ServerConfig.DMX.ServerFilesPath, "Web/Store/BaseStore.html");
                result = File.ReadAllText(storepath);

                result = result.Replace("__REPLACEME_PRODUCT__", productid);
                result = result.Replace("__REPLACEME_RESULT__", reason);
            }
            else
            {
                url = url.Replace("/store/", "");
                var storepath = Path.Combine(ServerConfig.DMX.ServerFilesPath, $"Web/Store/{url}.html");

                if (File.Exists(storepath))
                {
                    result = File.ReadAllText(storepath);
                    contentType = "text/html; charset=UTF-8";
                }

            }




            return result;
        }
    }
}
