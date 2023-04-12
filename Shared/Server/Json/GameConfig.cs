using Newtonsoft.Json;

namespace SharedLib.Server.Json
{
    public class GameConfig
    {
        public uint uplay_id { get; set; }
        public string product_name { get; set; } = string.Empty;
        public string configuration { get; set; } = string.Empty;
        public string store_configuration { get; set; } = string.Empty;
        public string latest_manifest { get; set; } = string.Empty;
        public bool staging { get; set; } = false;
        public Session gamesession { get; set; } = new();
        public Branches branches { get; set; } = new();
        public RichPresence richpresence { get; set; } = new();
        public List<uint> associations { get; set; } = new();
        public List<string> appflags { get; set; } = new();
        public string storereference { get; set; } = string.Empty;

        public class Session
        {
            public uint max_size { get; set; } = 4;
        }

        public class Branches
        {
            public List<CustomBranches> product_branches = new();
            public uint active_branch_id { get; set; } = 0;
            public uint current_branch_id { get; set; } = 0;
        }

        public class CustomBranches
        {
            public uint branch_id { get; set; }
            public string branch_name { get; set; } = string.Empty;
            public string branch_password { get; set; } = string.Empty;
        }

        public class RichPresence
        {
            public uint presence_id { get; set; } = 0;
            public List<string> Available_KeyValues = new();
        }

        public static GameConfig? GetGameConfig(uint ProdId)
        {
            GameConfig? GameConfig = null;
            foreach (var file in Directory.GetFiles("ServerFiles/ProductConfigs"))
            {
                var filename = file.Replace("_Config.json", "").Replace("ServerFiles/ProductConfigs\\", "");
                if (ProdId.ToString() == filename)
                {
                    GameConfig = JsonConvert.DeserializeObject<GameConfig>(File.ReadAllText(file));
                }

            }
            return GameConfig;
        }

        public static GameConfig? GetGameConfig(uint ProdId, uint BranchId)
        {
            if (BranchId == uint.MaxValue | BranchId == uint.MinValue)
                return GetGameConfig(ProdId);
            GameConfig? GameConfig = null;
            foreach (var file in Directory.GetFiles("ServerFiles/ProductConfigs"))
            {
                var filename = file.Replace("_Config.json", "").Replace("ServerFiles/ProductConfigs\\", "");
                if (filename.Contains('_'))
                {
                    var filesplit = filename.Split("_");
                    var id = filesplit[0];
                    var branch = filesplit[1];
                    if (ProdId.ToString() == id && BranchId.ToString() == branch)
                    {
                        GameConfig = JsonConvert.DeserializeObject<GameConfig>(File.ReadAllText(file));
                    }
                }
            }
            return GameConfig;
        }
    }
}
