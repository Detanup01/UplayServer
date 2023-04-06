using Core.JSON;
using Core.SQLite;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Core.HTTP
{
    public class Register
    {
        public static string RegisterCallback(Dictionary<string, string> headers, string body, out string contentType)
        {
            contentType = "application/json; charset=UTF-8";
            var auth = headers["authorization"].Replace("Basic ", "");
            var toauth = Utils.Base64Encode(SHA256.HashData(Encoding.UTF8.GetBytes(auth)).ToString() + Config.SQL.AuthSalt);

            var userIdFromAuth = UserAuth.GetUserIdByAuth(toauth);

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

                if (User.GetUser(id) == null)
                {
                    userId = id;
                    break;
                }
            }

            UserAuth.Add(userId, toauth);


            User user = new()
            {
                Name = name,
                UserId = userId,
                Ownership = new()
                {
                    OwnedGamesIds = { 0 },
                    UbiPlus = 0
                }
            };

            User.SaveUser(userId, user);
            Owners.MakeOwnership(userId, 0, new() { 0 }, new() { 0 });

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
