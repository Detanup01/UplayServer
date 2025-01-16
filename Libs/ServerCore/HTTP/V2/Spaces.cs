using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ServerCore.Models;
using ModdableWebServer.Helper;

namespace ServerCore.HTTP.V2;

internal class Spaces
{
    [HTTP("GET", "/v2/spaces/{spaceid}/parameters")]
    public static bool Parameters(HttpRequest request, ServerStruct serverStruct)
    {
        string path = $"{ServerConfig.Instance.Demux.ServerFilesPath}SpaceConfigs/{serverStruct.Parameters["spaceid"]}.json";
        if (!File.Exists(path))
        {
            Console.WriteLine("File not exists");
            serverStruct.Response.MakeErrorResponse("File not exists", "text/html; charset=UTF-8");
            serverStruct.SendResponse();
            return true;
        }
        serverStruct.Response.MakeGetResponse(File.ReadAllText(path), "application/json");
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/v2/spaces/{spaceid}/configs/events")]
    public static bool ConfigsEvents(HttpRequest request, ServerStruct serverStruct)
    {
        string path = $"{ServerConfig.Instance.Demux.ServerFilesPath}SpaceConfigs/{serverStruct.Parameters["spaceid"]}_events.json";
        if (!File.Exists(path))
        {
            Console.WriteLine("File not exists");
            serverStruct.Response.MakeErrorResponse("File not exists", "text/html; charset=UTF-8");
            serverStruct.SendResponse();
            return true;
        }
        serverStruct.Response.MakeGetResponse(File.ReadAllText(path), "application/json");
        serverStruct.SendResponse();
        return true;
    }
}
