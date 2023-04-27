namespace SharedLib.Server.Json
{
    public class ServerConfig
    {
        public static string DemuxIp = "127.0.0.1";
        public static int DemuxPort = 443;
        public static string DemuxUrl = $"dmx.local.upc.ubisoft.com:{DemuxPort}";
        public static string HTTPS_Ip = "127.0.0.1";
        public static int HTTPS_Port = 7777;
        public static string HTTPSUrl = $"https://local-ubiservices.ubi.com:{HTTPS_Port}";
        public class DMX
        {
            public static string PatchBaseUrl = HTTPSUrl + "/patch/";
            public static string DownloadGameUrl = HTTPSUrl + "/download/";
            public static string ServerFilesPath = Directory.GetCurrentDirectory() + "/ServerFiles/";
            public static string DownloadGamePath = ServerFilesPath + "Download/";
            public static string DefaultCountryCode = "HU";
            public static string DefaultContinentCode = "EU";
            public static bool GlobalOwnerShipCheck = true;
            public class ownership
            {
                public static bool EnableManifestRequest = true;
                public static bool EnableConfigRequest = true;
            }
        }
        public class SQL
        {
            public static string AuthSalt = "_CUSTOMDEMUX";
        }
    }
}
