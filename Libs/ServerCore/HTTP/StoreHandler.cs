using ServerCore.DB;
using ServerCore.Json.DB;
using ServerCore;
using ServerCore.Controller;
using ServerCore.Models;

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
                JOwnershipBasic? ownershipBasic = null;
                var reason = "FAILED";
                var productid = url.Replace("/store/?p=", "");
                if (!uint.TryParse(productid, out var pid))
                {
                    Console.WriteLine("Failed to convert ProductId to uint");
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
                        var config = App.GetAppConfig(pid);
                        if (config != null)
                        {
                            ownershipBasic.OwnedGamesIds.Add(pid);
                            DBUserExt.AddOwnership(pid,uint.MinValue, ownershipBasic.UserId, CDKeyController.GenerateKey(pid),new(),new());
                            //Owners.MakeOwnershipFromUser(user.UserId, user.Ownership);
                            reason = "SUCCESS";
                        }
                        else
                        {
                            reason = "FAILED (No ProductId)";
                        }
                    }
                }

                var storepath = Path.Combine(ServerConfig.Instance.Demux.ServerFilesPath, "Web/Store/BaseStore.html");
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
                var storepath = Path.Combine(ServerConfig.Instance.Demux.ServerFilesPath, $"Web/Store/{url}.html");

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
