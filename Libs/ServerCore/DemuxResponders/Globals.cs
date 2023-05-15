namespace Core.DemuxResponders
{
    public class Globals
    {
        public static List<string> Services = new()
        {
            "utility_service", "steam_service", "client_configuration_service", 
            // our services
            "uplaydll"
        };
        public static List<string> Connections = new()
        {
            "ownership_service", "denuvo_service", "store_service",
            "friends_service", "party_service" , "playtime_service",
            "download_service" , "ach_frontend" , "cloudsave_service"
            /*
            ,"channel_service" , "uplay_service" , "pcbang_service", 
            "control_panel", "crash_reporter"
            */
        };
        public static List<string> AppFlags = new()
        {
            "Downloadable", "Playable", "DenuvoForceTimeTrial" , "Denuvo", "FromSubscription", "FromExpiredSubscription"
        };

        public static Dictionary<Guid, string> IdToUser = new();
        public static Dictionary<string, Guid> UserToId = new();

        public static List<uint> AcceptVersions = new()
        {
            uint.MinValue, 10857
        };
    }
}
