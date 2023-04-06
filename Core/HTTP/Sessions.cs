using Core.JSON;
using Core.SQLite;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using static Core.SQLite.CurrentLogged;

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
                auth = SHA256.HashData(Encoding.UTF8.GetBytes(auth)).ToString();
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
                id = GetUserIdByToken(auth, (int)TokenType.Ticket);
            }
            else
            {
                id = UserAuth.GetUserIdByAuth(Utils.Base64Encode(auth + Config.SQL.AuthSalt));
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

            Add(id, token, (int)TokenType.Ticket);

            string time = Utils.ConvertFromUnixTimestampToLocal(jwt.GetExp(token)).ToString();

            var devicetoken = jwt.CreateAuthToken(id, SessionId, appId, "prod", DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds());

            SessionsResponse rsp = new()
            {
                clientIp = "172.0.0.1",
                clientIpCountry = Config.DMX.DefaultCountryCode,
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
                sessionKey = Utils.Base64Encode(SessionId),
                spaceId = AppAPI.GetSpaceId(appId),
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
