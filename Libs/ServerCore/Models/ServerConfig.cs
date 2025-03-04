using Newtonsoft.Json;

namespace ServerCore.Models;

public class ServerConfig
{
    private static ServerConfig? _Instance;
    public static ServerConfig Instance
    {
        get
        {
            _Instance ??= LoadConfig();
            return _Instance;
        }
        set { _Instance = value; }
    }

    public string DemuxUrl = $"dmx.local.upc.ubisoft.com:443";
    public string HTTPS_Url = $"https://local-ubiservices.ubi.com:443";

    public Cert CERT = new();
    public class Cert
    {
        public string ServicesCertPassword = "CustomUplay";
    }

    public DMX Demux = new();
    public class DMX
    {
        public string ServerFilesPath = "ServerFiles/";
        public string DownloadGamePath = "ServerFiles/Download/";
        public string DefaultCountryCode = "HU";
        public string DefaultContinentCode = "EU";
        public bool GlobalOwnerShipCheck = true;
        public OwnershipClass Ownership = new();
        public class OwnershipClass
        {
            public bool EnableManifestRequest = true;
            public bool EnableConfigRequest = true;
        }
    }
    public SQL sql = new();
    public class SQL
    {
        public string AuthSalt = "_CUSTOMDEMUX";
    }


    /// <summary>
    /// Loading existing server config
    /// or if not exist it makes one
    /// </summary>
    /// <returns>New ServerConfig</returns>
    public static ServerConfig LoadConfig()
    {
        if (!File.Exists("ServerConfig.json"))
        {
            _Instance = new();
            File.WriteAllText("ServerConfig.json", JsonConvert.SerializeObject(_Instance, Formatting.Indented));
            return _Instance;
        }
        _Instance = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText("ServerConfig.json"))!;
        return _Instance;
    }

}