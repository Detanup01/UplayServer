using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ServerCore.Models;
using SharedLib.Shared;
using ModdableWebServer.Helper;
using Newtonsoft.Json;
using ServerCore.DB;
using ServerCore.Models.Responses;
using ServerCore.Models.User;
using ServerCore.Controller;
using ServerCore.Models.Requests;

namespace ServerCore.HTTP.V3;

internal class Profie
{
    [HTTP("POST", "/v3/profiles/sessions")]
    public static bool Sessions(HttpRequest request, ServerStruct serverStruct)
    {
        var auth = serverStruct.Headers["authorization"];
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
        Guid appId = Guid.Parse(serverStruct.Headers["ubi-appid"]);
        Guid id = Guid.Empty;
        if (IsToken)
        {
            id = Auth.GetUserIdByToken(auth, TokenType.Ticket);
        }
        else
        {
            id = Auth.GetUserIdByAuth(Utils.MakeAuth(auth));
        }

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

        Guid SessionId = Guid.Empty;
        if (serverStruct.Headers.ContainsKey("ubi-sessionid"))
        {
            SessionId = Guid.Parse(serverStruct.Headers["ubi-sessionid"]);
        }

        var token = JWTController.CreateAuthToken(id, SessionId, appId);

        Auth.AddCurrent(id, token, TokenType.Ticket);

        string time = DateTimeOffset.FromUnixTimeSeconds(JWTController.GetExp(token)).ToString();

        var devicetoken = JWTController.CreateAuthToken(id, SessionId, appId, "prod", DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds());

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


    [HTTP("POST", "/v3/profiles/sessions")]
    public static bool Register(HttpRequest request, ServerStruct serverStruct)
    {
        var auth = serverStruct.Headers["authorization"].Replace("Basic ", "");
        var toauth = Utils.MakeAuth(auth);

        var userIdFromAuth = Auth.GetUserIdByAuth(toauth);

        if (userIdFromAuth != Guid.Empty)
        {
            serverStruct.Response.MakeGetResponse(JsonConvert.SerializeObject(new RegisterResponse()
            {
                UserId = userIdFromAuth
            }), "application/json; charset=UTF-8");
            serverStruct.SendResponse();
            return true;
        }

        var name = JsonConvert.DeserializeObject<RegisterRequest>(request.Body)!.Name;
        Guid userId = Guid.NewGuid();
        Auth.AddUA(userId, toauth);

        DBUser.Add(new UserCommon()
        {
            UserId = userId,
            Name = name,
            Friends = []
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
        DBUserExt.AddOwnership(0, 0, userId, cdkey, [], []);
        //Owners.MakeOwnership(userId, 0, new() { 0 }, new() { 0 });
        serverStruct.Response.MakeGetResponse(JsonConvert.SerializeObject(new RegisterResponse()
        {
            UserId = userIdFromAuth
        }), "application/json; charset=UTF-8");
        serverStruct.SendResponse();
        return true;
    }
}
