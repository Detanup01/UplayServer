using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using ServerCore.Models;

namespace ServerCore.HTTP;

internal class Store
{

    [HTTP("GET", "/store/{reference}")]
    public static bool StoreReference(HttpRequest request, ServerStruct serverStruct)
    {
        // TODO: Refactor this
        Console.WriteLine("STOOOOOREEEE");
        Console.WriteLine(request.Url);
        Console.WriteLine(serverStruct.Parameters["reference"]);
        var storepath = Path.Combine(ServerConfig.Instance.Demux.ServerFilesPath, $"Web/Store/{serverStruct.Parameters["reference"]}.html");

        if (!File.Exists(storepath))
        {
            serverStruct.Response.MakeErrorResponse("File not exists", "text/html; charset=UTF-8");
            serverStruct.SendResponse();
            return true;
        }
        serverStruct.Response.MakeGetResponse(File.ReadAllText(storepath), "text/html; charset=UTF-8");
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/storebrowse/?{args}")]
    public static bool StoreArgs(HttpRequest request, ServerStruct serverStruct)
    {
        Console.WriteLine("STOOOOOREEEE");
        Console.WriteLine(request.Url);
        Console.WriteLine(serverStruct.Parameters["p"]);
        var storepath = Path.Combine(ServerConfig.Instance.Demux.ServerFilesPath, $"Web/Store/{serverStruct.Parameters["p"]}.html");

        if (!File.Exists(storepath))
        {
            serverStruct.Response.MakeErrorResponse("File not exists", "text/html; charset=UTF-8");
            serverStruct.SendResponse();
            return true;
        }
        serverStruct.Response.MakeGetResponse(File.ReadAllText(storepath), "text/html; charset=UTF-8");
        serverStruct.SendResponse();
        return true;
    }
}
