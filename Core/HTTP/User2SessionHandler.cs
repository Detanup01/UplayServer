using Newtonsoft.Json;
using Core.SQLite;

namespace Core.HTTP
{
    internal class User2SessionHandler
    {
        public static string Callback(Dictionary<string, string> headers, string body, out string contentType)
        {
            var des_body = JsonConvert.DeserializeObject<Body>(body);

            UserToSession.Add(des_body.UserId, des_body.SessionId);

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
