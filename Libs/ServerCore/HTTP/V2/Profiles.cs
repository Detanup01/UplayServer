using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using Newtonsoft.Json;
using ServerCore.DB;
using ServerCore.Models.Responses;
using ServerCore.Models.User;
using ModdableWebServer.Helper;
using ServerCore.Controllers;
using ServerCore.Models;
using SharedLib.Shared;

namespace ServerCore.HTTP.V2;

internal class Profiles
{
    [HTTP("POST", "/v2/profiles/sessions")]
    public static bool CreateSessions(HttpRequest request, ServerStruct serverStruct)
    {
        var auth = serverStruct.Headers["authorization"];
        Guid appId = Guid.Parse(serverStruct.Headers["ubi-appid"]);
        Guid id = SessionController.GetUserFromAuth(auth);
        Guid SessionId =  Guid.NewGuid();
        var user = DBUser.Get<UserCommon>(id);
        if (user == null)
        {
            Console.WriteLine("User not found");
            serverStruct.Response.MakeErrorResponse("User not found!");
            serverStruct.SendResponse();
            return true;
        }
        var token = JWTController.CreateAuthToken(id, SessionId, appId);
        Auth.AddCurrent(id, token, TokenType.AuthToken);
        string time = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds().ToString();
        var devicetoken = JWTController.CreateAuthToken(id, SessionId, appId, "prod", DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds());
        Auth.AddCurrent(id, devicetoken, TokenType.RememberMe_v1);
        var space = App.GetSpaceId(appId);
        serverStruct.Response.MakeGetResponse(
            JsonConvert.SerializeObject(
        new V2SessionsResponse()
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
            accountIssues = null,
            hasAcceptedLegalOptins = true,
            initializeUser = true,
        }), "application/json; charset=UTF-8");
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("DELETE", "/v2/profiles/sessions")]
    public static bool DeleteSessions(HttpRequest request, ServerStruct serverStruct)
    {
        var auth = serverStruct.Headers["authorization"];
        Guid id = SessionController.GetUserFromAuth(auth);
        Auth.DeleteCurrent(id, SessionController.GetTokenTypeFromAuth(auth));
        serverStruct.Response.MakeOkResponse();
        serverStruct.SendResponse();
        return true;
    }


    [HTTP("POST", "/v2/profiles/{profileid}/events")]
    public static bool PostEvents(HttpRequest request, ServerStruct serverStruct)
    {
        Console.WriteLine("PostEvents: " + request.Body);
        serverStruct.Response.MakeGetResponse("{}");
        serverStruct.SendResponse();
        return true;
    }
}
