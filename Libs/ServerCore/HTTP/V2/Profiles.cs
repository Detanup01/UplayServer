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
using System.Net.Http.Headers;
using SharedLib;

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

    //?locale=en&spaceId=6edd234a-abff-4e90-9aab-b9b9c6e49ff
    [HTTP("GET", "/v2/profiles/{userid}/club/progression/title?{args}")]
    public static bool Rewards(HttpRequest request, ServerStruct serverStruct)
    {
        var userId = Guid.Parse(serverStruct.Parameters["userid"]);
        var spaceId = Guid.Parse(serverStruct.Parameters["spaceId"]);
        ClubProgressionTitleResponse clubProgressionTitleResponse = new()
        { 
            spaceId = spaceId,
            profileId = userId,
            owned = true,
            actions = new()
            { 
                owned = 0,
                available = 50
            },
            badges = new()
            {
                owned = 0,
                available = 50
            },
            rewards = new()
            {
                owned = 0,
                available = 50,
                New = 0,
            },
            units = new()
            {
                owned = 0,
                available = 50
            },
            challengesCompleted = 0,
            gameXp = new()
            {
                owned = 0,
                threshold = 6000,
                breakdown = new()
                {
                    challenges = 0,
                    actions =
                    [
                        new()
                        {
                            groupId = "ACU_GAME_PROGRESSION",
                            xp = 0
                        },
                        new()
                        {
                            groupId = "DLC_PROGRESSION",
                            xp = 0
                        },
                        new()
                        {
                            groupId = "CLUB",
                            xp = 0
                        },
                    ]
                }
            }
        };
        serverStruct.Response.MakeGetResponse(JsonConvert.SerializeObject(clubProgressionTitleResponse), "application/json");
        serverStruct.SendResponse();
        return true;
    }
}
