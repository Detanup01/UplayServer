using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using ServerCore.Models;

namespace ServerCore.HTTP.V1;

internal class Applications
{
    [HTTP("GET", "/v1/applications/{appid}/configuration")]
    public static bool Configuration(HttpRequest request, ServerStruct serverStruct)
    {
        string path = $"{ServerConfig.Instance.Demux.ServerFilesPath}AppConfigs/{serverStruct.Parameters["appid"]}.json";
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
