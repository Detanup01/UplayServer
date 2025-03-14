using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ServerCore.Models;
using ModdableWebServer.Helper;
using ServerCore.Models.Responses;
using Newtonsoft.Json;
using SharedLib;

namespace ServerCore.HTTP.V1;

internal class Profiles
{
    [HTTP("GET", "/v1/profiles/{userid}/entities?{args}")]
    public static bool ConfigsEvents(HttpRequest _, ServerStruct serverStruct)
    {
        string userId = serverStruct.Parameters["userid"];
        if (!serverStruct.Parameters.TryGetValue("spaceId", out string? spaceId))
        {
            Console.WriteLine("no spaceId");
            serverStruct.Response.MakeErrorResponse("no spaceId", "text/html; charset=UTF-8");
            serverStruct.SendResponse();
            return true;
        }

        string path = $"{ServerConfig.Instance.Demux.ServerFilesPath}Saves/{userId}/{spaceId}";
        var profileEntities = new ProfileEntities()
        {
            entities = []
        };
        if (!serverStruct.Parameters.TryGetValue("name", out string? filepath_name))
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


    [HTTP("POST", "/v1/profiles/{userid}/global/ubiconnect/playsession/api/sessions")]
    public static bool PlaySession(HttpRequest request, ServerStruct serverStruct)
    {
        // body is {"productId":568} we currently dont care
        PlaySessionResponse playSessionResponse = new()
        { 
            createdAt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            expiresAt = DateTime.Now.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
        };
        serverStruct.Response.MakeGetResponse(JsonConvert.SerializeObject(playSessionResponse), "application/json");
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("DELETE", "/v1/profiles/{userid}/global/ubiconnect/playsession/api/sessions?{args}")]
    public static bool DeletePlaySession(HttpRequest request, ServerStruct serverStruct)
    {
        serverStruct.Response.MakeOkResponse();
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/v1/profiles/{userid}/global/ubiconnect/economy/api/unit")]
    public static bool EconomyUnit(HttpRequest request, ServerStruct serverStruct)
    {
        // currently faking economy
        string userId = serverStruct.Parameters["userid"];
        EconomyUnitResponse economyUnitResponse = new()
        { 
            profileId = userId,
            units = 0
        }; 
        serverStruct.Response.MakeGetResponse(JsonConvert.SerializeObject(economyUnitResponse), "application/json");
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/v1/profiles/{userid}/global/ubiconnect/economy/api/metaprogression")]
    public static bool EconomyMetaprogression(HttpRequest request, ServerStruct serverStruct)
    {
        // currently faking economy
        string userId = serverStruct.Parameters["userid"];
        EconomyMetaprogressionResponse economyUnitResponse = new()
        {
            profileId = userId,
            level = 1,
            xp = 100
        };
        serverStruct.Response.MakeGetResponse(JsonConvert.SerializeObject(economyUnitResponse), "application/json");
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/v1/profiles/{userid}/global/ubiconnect/challenges/api?{args}")]
    public static bool Challenges(HttpRequest request, ServerStruct serverStruct)
    {
        //string userId = serverStruct.Parameters["userid"];
        //string spaceId = serverStruct.Parameters["spaceId"];
        ChallengesResponse challenges = new()
        {
            challenges = []
        };
        serverStruct.Response.MakeGetResponse(JsonConvert.SerializeObject(challenges), "application/json");
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/v1/profiles/{userid}/global/ubiconnect/rewards/api?{args}")]
    public static bool Rewards(HttpRequest request, ServerStruct serverStruct)
    {
        //string userId = serverStruct.Parameters["userid"];
        //string spaceId = serverStruct.Parameters["spaceId"];
        RewardsResponse rewardsResponse = new()
        {
            rewards = []
        };
        serverStruct.Response.MakeGetResponse(JsonConvert.SerializeObject(rewardsResponse), "application/json");
        serverStruct.SendResponse();
        return true;
    }
}
