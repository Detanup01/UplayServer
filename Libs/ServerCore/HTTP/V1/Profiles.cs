using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ServerCore.Models;
using ModdableWebServer.Helper;
using ServerCore.Models.Responses;
using Newtonsoft.Json;
using SharedLib.Shared;

namespace ServerCore.HTTP.V1;

internal class Profiles
{
    [HTTP("GET", "/v1/profiles/{userid}/entities?{args}")]
    public static bool ConfigsEvents(HttpRequest request, ServerStruct serverStruct)
    {
        string userId = serverStruct.Parameters["userid"];
        if (!serverStruct.Parameters.ContainsKey("spaceId"))
        {
            Console.WriteLine("no spaceId");
            serverStruct.Response.MakeErrorResponse("no spaceId", "text/html; charset=UTF-8");
            serverStruct.SendResponse();
            return true;
        }
        string spaceId = serverStruct.Parameters["spaceId"];

        string path = $"{ServerConfig.Instance.Demux.ServerFilesPath}Saves/{userId}/{spaceId}";
        var profileEntities = new ProfileEntities()
        {
            entities = []
        };
        if (!serverStruct.Parameters.ContainsKey("name"))
        {
            // send all from this space
            foreach (var item in Directory.GetFiles(path))
            {
                var bytes = File.ReadAllBytes(item);
                var ext = Path.GetExtension(item);
                var fileName = Path.GetFileName(item);
                profileEntities.entities.Add(new ProfileEntities.Entity()
                {
                    entityId = Guid.NewGuid().ToString(),
                    lastModified = File.GetLastWriteTime(item).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    name = fileName,
                    profileId = userId,
                    revision = 1,
                    spaceId = spaceId,
                    tags = [],
                    type = fileName,
                    obj = 
                    {
                        { ext, [bytes.Length, Convert.ToBase64String(bytes)] }
                    }
                });
            }
        }
        else
        {
            string filepath_name = serverStruct.Parameters["name"];
            Console.WriteLine(filepath_name);
            var files = Directory.GetFiles(path);
            if (files.Any(x=>x.Contains(filepath_name)))
            {
                string? filePath = files.FirstOrDefault(x=>x.Contains(filepath_name));
                if (string.IsNullOrEmpty(filePath))
                {
                    Console.WriteLine("File not exists");
                    serverStruct.Response.MakeErrorResponse("File not exists", "text/html; charset=UTF-8");
                    serverStruct.SendResponse();
                    return true;
                }
                var bytes = File.ReadAllBytes(filePath);
                var ext = Path.GetExtension(filePath).Replace(".", string.Empty);
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                int length = 0;
                switch (ext)
                {
                    case "LzoXml":
                        length = DeComp.Decompress(true, false, "Lzo", File.ReadAllBytes("playersettings.LzoXml"), 0).Length;
                        break;
                    default:
                        break;
                }
               
                profileEntities.entities.Add(new ProfileEntities.Entity()
                {
                    // Hardcoded entityId, idk if works or not.
                    entityId = "131bc2b3-9377-4c47-acdc-3e04a9bba43b",
                    lastModified = File.GetLastWriteTime(filePath).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    name = fileName,
                    profileId = userId,
                    revision = 1,
                    spaceId = spaceId,
                    tags = [],
                    type = fileName,
                    obj =
                    {
                        { ext, [length, Convert.ToBase64String(bytes)[..^1]] }
                    }
                });
            }
        }
        serverStruct.Response.MakeGetResponse(JsonConvert.SerializeObject(profileEntities), "application/json");
        serverStruct.SendResponse();
        return true;
    }
}
