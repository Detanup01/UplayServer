using Newtonsoft.Json;
using ServerCore.Controller;
using ServerCore.DB;
using ServerCore.Models.User;

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

            if (userIdFromAuth != Guid.Empty)
            {
                return JsonConvert.SerializeObject(new RegisterResponse()
                {
                    UserId = userIdFromAuth
                });
            }

            var name = JsonConvert.DeserializeObject<RegisterBody>(body)!.Name;
            Guid userId = Guid.NewGuid();
            Auth.AddUA(userId, toauth);

            DBUser.Add(new UserCommon()
            { 
                UserId = userId,
                Name = name,
                Friends = new()
            });
            DBUser.Add(new UserOwnershipBasic()
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
            var cdkey = CDKeyController.GenerateKey(0);
            DBUserExt.AddOwnership(0, 0, userId,cdkey,new(),new());
            //Owners.MakeOwnership(userId, 0, new() { 0 }, new() { 0 });
            return JsonConvert.SerializeObject(new RegisterResponse()
            {
                UserId = userId,
            });
        }
        private class RegisterBody
        {
            public string Name { get; set; } = string.Empty;
        }
        private class RegisterResponse
        {
            public Guid UserId;
        }
    }
}
