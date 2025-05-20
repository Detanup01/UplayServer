using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Serilog;

namespace OnlineConfigService;

public class OnlineService_SVC
{
    [HTTP("GET", "/OnlineConfigService.svc/GetOnlineConfig?onlineConfigID={configid}&target={target}")]
    public static bool GetOnlineConfig(HttpRequest request, ServerStruct serverStruct)
    {
        string configid = serverStruct.Parameters["configid"];
        string target = serverStruct.Parameters["target"];

        var path = Path.Combine("OnlineConfigService", $"{configid}_{target}.json");
        if (!File.Exists(path))
        {
            Log.Warning("OnlineConfigService: Config file not found! ({Path})", path);
            serverStruct.Response.MakeErrorResponse("No file found!");
            serverStruct.SendResponse();
            return true;
        }

        serverStruct.Response.MakeGetResponse(File.ReadAllText(path), "application/json");
        serverStruct.SendResponse();
        return true;
    }
}
