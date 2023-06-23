namespace SharedLib.Server.Json.DB
{
    public class JUserBase
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class JUser : JUserBase
    {
        public string Name { get; set; }
        public List<string> Friends { get; set; } = new();
        public bool IsBanned { get; set; } = false;
    }
    public class JOwnershipBasic : JUserBase
    {
        public List<uint> OwnedGamesIds { get; set; } = new();
        public Dictionary<uint,List<uint>> UnlockedBranches { get; set; } = new();
        public uint UbiPlus { get; set; }
    }

    public class JOwnership : JUserBase
    {
        public uint ProductId { get; set; }
        public bool IsOwned { get; set; }
        public bool IsLockedSubscription { get; set; }
        public string CD_Key { get; set; }
        public uint current_branch_id { get; set; } = 0;
        public List<Enums.AppFlags> appflags { get; set; } = new();
        public List<uint> ActivationIds { get; set; } = new();
        public List<uint> Subscriptions { get; set; } = new();
        public Uplay.Ownership.OwnedGame.Types.PackageOwnershipState PackageState { get; set; }
        public Uplay.Ownership.OwnedGame.Types.SuspensionType Suspension { get; set; }
        public Uplay.Ownership.OwnedGame.Types.ActivationType Activation { get; set; }
        public Uplay.Ownership.OwnedGame.Types.TargetPartner TargetPartner { get; set; }
        public Uplay.Ownership.OwnedGame.Types.DenuvoActivationOverwrite DenuvoActivation { get; set; }
    }

    public class JActivity : JUserBase
    {
        public int Status { get; set; }
        public int OnlineStatus { get; set; } = 0;
        public bool IsPlaying { get; set; }
        public string? ProductName { get; set; }
        public uint? GameId { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
    }

    public class JFriend : JUserBase
    {
        public string? IdOfFriend { get; set; }
        public string? Name { get; set; }
        public string? Nickname { get; set; }
        public bool IsFavorite { get; set; } = false;
        public bool IsBlacklisted { get; set; } = false;
        public int Relation { get; set; } = 0;
    }

    public class JPlaytime : JUserBase
    {
        public uint uplayId { get; set; }
        public uint playTime { get; set; }
    }
    public class JGameSession : JUserBase
    {
        public ulong SessionId { get; set; }
        public string SessionIdV2 { get; set; }
        public string SessionData { get; set; } = string.Empty;
        public bool Joinable { get; set; }
        public uint Size { get; set; }
        public uint MaxSize { get; set; }
    }

    public class JCloudSave : JUserBase
    {
        public uint uplayId { get; set; }
        public uint SaveId { get; set; }
        public string SaveName { get; set; }
    }
}
