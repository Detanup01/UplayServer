using SharedLib.Server.DB;
using SharedLib.Server.Json;
using SharedLib.Server.Json.DB;
using SharedLib.Server.Json.Ext;
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
            {basic}/store/?p={productId} [Accept: "*"]
            
            {basic}/store/{storereference}
            
            */
            if (url.Contains("?"))
            {
                JOwnershipBasic ownershipBasic = null;
                var reason = "FAILED";
                var productid = url.Replace("/store/?p=", "");
                if (!uint.TryParse(productid, out var pid))
                { 
                
                }
                if (headers.TryGetValue("authorization", out var auth))
                {
                    Console.WriteLine(auth);
                    auth = auth.Split("t=")[1];
                    var id = Auth.GetUserIdByToken(auth, TokenType.Ticket);
                    ownershipBasic = DBUser.GetOwnershipBasic(id);
                }

                if (ownershipBasic != null)
                {
                    if (ownershipBasic.OwnedGamesIds.Contains(pid))
                    {
                        reason = "FAILED (Already own)";
                    }
                    else
                    {
                        var config = GameConfig.GetGameConfig(pid);
                        if (config != null)
                        {
                            ownershipBasic.OwnedGamesIds.Add(pid);
                            DBUserExt.AddOwnership(pid,0, ownershipBasic.UserId, CDKey.GenerateKey(pid),new(),new());
                            //Owners.MakeOwnershipFromUser(user.UserId, user.Ownership);
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

                if (headers["accept"].Contains("*")) //to make it html
                {
                    contentType = "text/html; charset=UTF-8";
                }
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
