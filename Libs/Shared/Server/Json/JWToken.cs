#if SERVER

namespace SharedLib.Server.Json; 

public class JWToken
{
    public class auth_service
    {
        public string sub { get; set; } = string.Empty;
        public string iss { get; set; } = string.Empty;
        public long exp { get; set; }
        public string session { get; set; } = string.Empty;
        public string app { get; set; } = string.Empty;
        public string env { get; set; } = string.Empty;
    }

    public class ownership_service
    {
        public string sub { get; set; } = string.Empty;
        public string iss { get; set; } = string.Empty;
        public long exp { get; set; }
        public int uplay_id { get; set; }
        public int product_id { get; set; }
        public int branch_id { get; set; }
        public List<string> flags { get; set; } = [];
    }

    public class UplayPCTicket
    {
        public string sub { get; set; } = string.Empty;
        public string iss { get; set; } = string.Empty;
        public long exp { get; set; }
        public int uplay_id { get; set; }
        public int platform { get; set; }
    }
}
#endif