using Newtonsoft.Json;
using SharedLib.Server.DB;

namespace Core.HTTP
{
    internal class User2SessionHandler
    {
        public static string Callback(Dictionary<string, string> headers, string body, out string contentType)
        {
            var des_body = JsonConvert.DeserializeObject<Body>(body);

            Auth.AddU2S(des_body.UserId, des_body.SessionId);

            contentType = "application/json; charset=UTF-8";
            return "{\"ok\":true}";
        }

        private class Body
        {
            public string SessionId;
            public string UserId;
        }
    }
}
