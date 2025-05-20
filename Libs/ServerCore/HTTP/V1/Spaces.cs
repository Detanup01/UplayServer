using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Newtonsoft.Json;
using ServerCore.Models;
using ServerCore.Models.Responses;

namespace ServerCore.HTTP.V1;

internal class Spaces
{
    [HTTP("GET", "/v1/spaces/{spaceid}/parameters")]
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

    [HTTP("GET", "/v1/spaces/global/ubiconnect/economy/api/metaprogression?{args}")]
    public static bool EconomyMetaprogressionLevels(HttpRequest request, ServerStruct serverStruct)
    {
        // currently faking economy
        string levels = serverStruct.Parameters["levels"];
        MetaprogressionLevels metaprogression = new()
        {
            levels = []
        };
        var split_lvl = levels.Split(",");
        int i = 0;
        foreach (var lvl in split_lvl)
        {
            int int_level = int.Parse(lvl);
            metaprogression.levels.Add(new MetaprogressionLevels.Level()
            {
                level = int_level,
                xp = 100 + i,
                rewards = [new()]
            });
            i += 50;
        }
        serverStruct.Response.MakeGetResponse(JsonConvert.SerializeObject(metaprogression), "application/json");
        serverStruct.SendResponse();
        return true;
    }
}
