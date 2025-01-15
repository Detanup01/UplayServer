using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using Newtonsoft.Json;
using ServerCore.DB;
using ModdableWebServer.Helper;
using ServerCore.Models.Requests;

namespace ServerCore.HTTP;

internal class User2Session
{
    [HTTP("POST", "/user2session")]
    public static bool user2session(HttpRequest request, ServerStruct serverStruct)
    {
        var des_body = JsonConvert.DeserializeObject<User2SessionRequest>(request.Body);
        if (des_body == null)
        {
            serverStruct.Response.MakeGetResponse("{\"ok\":true}", "application/json; charset=UTF-8");
            serverStruct.SendResponse();
            return true;
        }
        Auth.AddU2S(des_body.UserId, des_body.SessionId);
        serverStruct.Response.MakeGetResponse("{\"ok\":true}", "application/json; charset=UTF-8");
        serverStruct.SendResponse();
        return true;
    }
}
