namespace SharedLib.Server.Json.DB
{
    public class JAppAPI
    {
        public int Id { get; set; }
        public string applicationId { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public string platform { get; set; }
        public string genre { get; set; }
        public string releaseDate { get; set; }
        public string spaceId { get; set; }
    }

    public class JAppConfig
    {
        public int Id { get; set; }
        public uint productId { get; set; }
        public uint config_version { get; set; } = 0;
        public uint download_version { get; set; } = 0;
        public uint current_branch_id { get; set; } = 0;
        public uint session_max_size { get; set; } = 4;
        public string product_name { get; set; } = string.Empty;
        public string space_id { get; set; } = string.Empty;
        public string app_id { get; set; } = string.Empty;
        public string configuration { get; set; } = string.Empty;
        public string store_configuration { get; set; } = string.Empty;
        public string storereference { get; set; } = string.Empty;
        public bool staging { get; set; } = false;
        public List<uint> associations { get; set; } = new();
        public List<string> appflags { get; set; } = new();
        public Uplay.Ownership.GetUplayPCTicketReq.Types.Platform platform { get; set; } = Uplay.Ownership.GetUplayPCTicketReq.Types.Platform.Normal;
        public Uplay.Ownership.OwnedGame.Types.ProductType product_type { get; set; } = Uplay.Ownership.OwnedGame.Types.ProductType.Game;
        public Uplay.Ownership.OwnedGame.Types.State state { get; set; } = Uplay.Ownership.OwnedGame.Types.State.Released;
    }

    public class JAppBranches
    {
        public int Id { get; set; }
        public uint productId { get; set; }
        public uint branch_id { get; set; } = 0;
        public string branch_name { get; set; } = string.Empty;
        public string branch_password { get; set; } = string.Empty;
        public string latest_manifest { get; set; } = string.Empty;
        public string encryption_key { get; set; } = string.Empty;
    }
}
