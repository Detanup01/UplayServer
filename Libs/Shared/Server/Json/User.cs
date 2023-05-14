using Newtonsoft.Json;

namespace SharedLib.Server.Json
{
    public class User
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public List<CFriends> Friends { get; set; } = new();
        public COwnership Ownership { get; set; } = new();
        public CActivity Activity { get; set; } = new();
        public List<CPlaytime> Playtime { get; set; } = new();

        public class COwnership
        {
            public List<uint> OwnedGamesIds { get; set; } = new();
            public uint UbiPlus { get; set; }
        }

        public class CFriends
        {
            public string UserId { get; set; } = string.Empty;
            public string? Name { get; set; }
            public string? Nickname { get; set; }
            public bool IsFavorite { get; set; } = false;
            public bool IsBlacklisted { get; set; } = false;
            public int Relation { get; set; } = 0;
            public CActivity Activity { get; set; } = new();
            public List<CPlaytime> Playtime { get; set; } = new();
        }
        public class CActivity
        {
            public int Status { get; set; }
            public bool IsPlaying { get; set; }
            public string? ProductName { get; set; }
            public uint? GameId { get; set; }
            public string? Key { get; set; }
            public string? Value { get; set; }
        }
        public class CPlaytime
        {
            public uint uplayId { get; set; }
            public uint playTime { get; set; }
        }

        public static User? GetUser(string UserId)
        {
            User? users = null;
            foreach (var file in Directory.GetFiles("ServerFiles/Users"))
            {
                var filename = file.Replace(".json", "").Replace("ServerFiles/Users\\", "");
                if (UserId == filename)
                {
                    users = JsonConvert.DeserializeObject<User>(File.ReadAllText(file));
                }

            }
            return users;
        }

        public static void SaveUser(string id, User user)
        {
            File.WriteAllText($"ServerFiles/Users/{id}.json", JsonConvert.SerializeObject(user, Formatting.Indented));
        }
    }
}
