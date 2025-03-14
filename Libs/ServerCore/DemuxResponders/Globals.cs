namespace ServerCore.DemuxResponders;

public class Globals
{
    public static List<string> Services =
    [
        "utility_service", "steam_service", "client_configuration_service", 
        // our services
        "uplaydll"
    ];
    public static List<string> Connections =
    [
        "ownership_service", "denuvo_service", "store_service",
        "friends_service", "party_service" , "playtime_service",
        "download_service" , "ach_frontend" , "cloudsave_service"
        /*
        ,"channel_service" , "uplay_service" , "pcbang_service", 
        "control_panel", "crash_reporter"
        */
    ];
    public static List<string> AppFlags =
    [
        "Downloadable", "Playable", "DenuvoForceTimeTrial" , "Denuvo", "FromSubscription", "FromExpiredSubscription"
    ];

    public static Dictionary<Guid, Guid> IdToUser = [];
    public static Dictionary<Guid, Guid> UserToId = [];

    public static List<uint> AcceptVersions =
    [
        uint.MinValue, 11194, 11646
    ];
}
