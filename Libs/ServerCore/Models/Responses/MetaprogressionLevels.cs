namespace ServerCore.Models.Responses;

public class MetaprogressionLevels
{
    public List<Level> levels { get; set; } = [];

    public class Level
    {
        public int level { get; set; }
        public int xp { get; set; }
        public List<Reward> rewards { get; set; } = [];
    }

    public class Reward
    {
        public string type { get; set; } = "Units";
        public int quantity { get; set; } = 10;
        public string iconUrl { get; set; } = "https://ubiservices.cdn.ubi.com/45d58365-547f-4b45-ab5b-53ed14cc79ed/Pictures/Connect_UbiConnect-Units.png";
        public string displayName { get; set; } = "<span> 10 </span> Units";
    }
}
