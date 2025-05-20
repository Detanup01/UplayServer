using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Newtonsoft.Json;
using ServerCore.Controllers;
using ServerCore.DB;
using ServerCore.Models;
using ServerCore.Models.Responses;
using ServerCore.Models.User;
using SharedLib;

namespace ServerCore.HTTP.V3;

internal class Profies
{
    [HTTP("POST", "/v3/profiles/sessions")]
    public static bool Sessions(HttpRequest request, ServerStruct serverStruct)
    {
        var auth = serverStruct.Headers["authorization"];
        Guid appId = Guid.Parse(serverStruct.Headers["ubi-appid"]);
        Guid id = SessionController.GetUserFromAuth(auth);

        if (id == Guid.Empty)
        {
            serverStruct.Response.MakeErrorResponse("UserId is Empty!");
            serverStruct.SendResponse();
            return true;
        }

        var user = DBUser.Get<UserCommon>(id);
        if (user == null)
        {
            serverStruct.Response.MakeErrorResponse("User not found!");
            serverStruct.SendResponse();
            return true;
        }

        Guid SessionId = Guid.NewGuid();
        if (serverStruct.Headers.TryGetValue("ubi-sessionid", out string? value))
        {
            SessionId = Guid.Parse(value);
        }

        var token = JWTController.CreateAuthToken(id, SessionId, appId);
        Auth.AddCurrent(id, token, TokenType.AuthToken);
        string time = DateTimeOffset.FromUnixTimeSeconds(JWTController.GetExp(token)).ToString();
        var devicetoken = JWTController.CreateAuthToken(id, SessionId, appId, "prod", DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds());
        Auth.AddCurrent(id, devicetoken, TokenType.RememberMe_v1);
        SessionsResponse rsp = new()
        {
            clientIp = "172.0.0.1",
            clientIpCountry = ServerConfig.Instance.Demux.DefaultCountryCode,
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
            sessionKey = B64.ToB64(SessionId.ToString()),
            spaceId = App.GetSpaceId(appId),
            nameOnPlatform = user.Name,
            rememberDeviceTicket = devicetoken,
        };
        serverStruct.Response.MakeGetResponse(JsonConvert.SerializeObject(rsp), "application/json; charset=UTF-8");
        serverStruct.SendResponse();
        return true;
    }
}
