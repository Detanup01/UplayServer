using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ServerCore.Models;
using SharedLib.Shared;
using ModdableWebServer.Helper;

namespace ServerCore.HTTP;

internal class Patch
{
    [HTTP("GET", "/patch/{args}")]
    public static bool PatchHandler(HttpRequest request, ServerStruct serverStruct)
    {
        // TODO: Refactor this
        byte[] returner = [];
        if (File.Exists($"{ServerConfig.Instance.Demux.ServerFilesPath}Patch{request.Url}"))
        {
            returner = File.ReadAllBytes($"{ServerConfig.Instance.Demux.ServerFilesPath}Patch{request.Url}");
            if (!request.Url.Contains("files.txt"))
            {
                returner = System.Text.Encoding.UTF8.GetBytes(CompressB64.GetZstdB64(returner));
            }
        }
        serverStruct.Response.MakeGetResponse(returner, "application/octet-stream");
        serverStruct.SendResponse();
        return true;
    }
}
