using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using ServerCore.Models;

namespace ServerCore.HTTP;

internal class Download
{
    [HTTP("GET", "/download/{downloadurl}")]
    public static bool DownloadUrl(HttpRequest request, ServerStruct serverStruct)
    {
        // TODO: Refactor this
        Console.WriteLine("DownloadUrl");
        Console.WriteLine(request.Url);
        string path = $"{ServerConfig.Instance.Demux.DownloadGamePath}{request.Url}";
        if (!File.Exists(path))
        {
            serverStruct.Response.MakeErrorResponse("File not exists", "text/html; charset=UTF-8");
            serverStruct.SendResponse();
            return true;
        }
        serverStruct.Response.MakeGetResponse(File.ReadAllText(path), "application/octet-stream");
        serverStruct.SendResponse();
        return true;
    }
}
