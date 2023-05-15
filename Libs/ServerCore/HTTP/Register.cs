using Newtonsoft.Json;
using SharedLib.Server.DB;
using SharedLib.Server.Json;
using SharedLib.Server.Json.DB;
using SharedLib.Server.Json.Ext;

namespace Core.HTTP
{
    public class Register
    {
        public static string RegisterCallback(Dictionary<string, string> headers, string body, out string contentType)
        {
            contentType = "application/json; charset=UTF-8";
            var auth = headers["authorization"].Replace("Basic ", "");
            var toauth = Utils.MakeAuth(auth);

            var userIdFromAuth = Auth.GetUserIdByAuth(toauth);

            if (userIdFromAuth != "")
            {
                return JsonConvert.SerializeObject(new RegisterResponse()
                {
                    UserId = userIdFromAuth
                });
            }

            var name = JsonConvert.DeserializeObject<RegisterBody>(body).Name;
            var userId = "";

            // method to be check if there is a userId that are not used
            // hopefully you arent gonna broke that
            while (true)
            {
                var id = Utils.MakeNewID();

                if (DBUser.GetUser(id) == null)
                {
                    userId = id;
                    break;
                }
                Thread.Sleep(10);
            }

            Auth.AddUA(userId, toauth);

            DBUser.Add(new JUser()
            { 
                UserId = userId,
                Name = name,
                Friends = new()
            });
            DBUser.Add(new JOwnershipBasic()
            {
                UserId = userId,
                OwnedGamesIds = { 0 },
                UnlockedBranches = { 
                    { 
                        0, 
                        new()
                        { 
                            0
                        } 
                    } 
                },
                UbiPlus = 0
            });
            var cdkey = CDKey.GenerateKey(0);
            DBUserExt.AddOwnership(0,0,userId,cdkey,new(),new());
            //Owners.MakeOwnership(userId, 0, new() { 0 }, new() { 0 });
            return JsonConvert.SerializeObject(new RegisterResponse()
            {
                UserId = userId
            });
        }
        private class RegisterBody
        {
            public string Name;
        }
        private class RegisterResponse
        {
            public string UserId;
        }
    }
}
