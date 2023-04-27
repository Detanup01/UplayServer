using Newtonsoft.Json;
using SharedLib.Server.DB;
using static SharedLib.Server.Enums;
using SharedLib.Server.Json;
using SharedLib.Shared;

namespace Core.HTTP
{
    public class Sessions
    {
        public static string SessionsCallback(Dictionary<string, string> headers, string body, out string contentType, out bool IsFailed)
        {
            contentType = "application/json; charset=UTF-8";
            IsFailed = false;
            var auth = headers["authorization"];
            bool IsToken = false;
            if (auth.Contains("Basic"))
            {
                auth = auth.Replace("Basic ", "");
            }
            else if (auth.Contains(" t="))
            {
                //token renew
                auth = auth.Split(" t=")[1];
                IsToken = true;
            }


            var appId = headers["ubi-appid"];
            var id = "";
            if (IsToken)
            {
                id = Auth.GetUserIdByToken(auth, TokenType.Ticket);
            }
            else
            {
                id = Auth.GetUserIdByAuth(Utils.MakeAuth(auth));
            }

            if (id == "")
            {
                IsFailed = true;
                return "";
            }

            var SessionId = Utils.MakeNewID();
            if (headers.ContainsKey("ubi-sessionid"))
            {
                SessionId = headers["ubi-sessionid"];
            }


            var token = jwt.CreateAuthToken(id, SessionId, appId);

            Auth.AddCurrent(id, token, TokenType.Ticket);

            string time = Utils.ConvertFromUnixTimestampToLocal(jwt.GetExp(token)).ToString();

            var devicetoken = jwt.CreateAuthToken(id, SessionId, appId, "prod", DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds());

            SessionsResponse rsp = new()
            {
                clientIp = "172.0.0.1",
                clientIpCountry = ServerConfig.DMX.DefaultCountryCode,
                twoFactorAuthenticationTicket = "",
                serverTime = DateTime.Now.ToString("yyyy-MM-dd"),
                environment = "Prod",
                platformType = "uplay",
                ticket = token,
                rememberMeTicket = token,
                sessionId = SessionId,
                expiration = time,
                profileId = id,
                userId = id,
                sessionKey = B64.ToB64(SessionId),
                spaceId = App.GetSpaceId(appId),
                nameOnPlatform = User.GetUser(id).Name,
                rememberDeviceTicket = devicetoken,
            };
            contentType = "application/json; charset=UTF-8";
            return JsonConvert.SerializeObject(rsp);
        }

        private class SessionsResponse
        {
            public string platformType;
            public string ticket;
            public string? twoFactorAuthenticationTicket;
            public string profileId;
            public string userId;
            public string nameOnPlatform;
            public string environment;
            public string expiration;
            public string spaceId;
            public string clientIp;
            public string clientIpCountry;
            public string serverTime;
            public string sessionId;
            public string sessionKey;
            public string? rememberMeTicket;
            public string? rememberDeviceTicket;
        }
    }
}
