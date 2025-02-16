using Newtonsoft.Json;

namespace ServerCore.Models.Responses;

public class ClubProgressionTitleResponse
{
    public Guid spaceId { get; set; }
    public Guid profileId { get; set; }
    public bool owned { get; set; }
    public OwnedAvailable units { get; set; } = new();
    public OwnedAvailable actions { get; set; } = new();
    public OwnedAvailable badges { get; set; } = new();
    public NewOwnedAvailable rewards { get; set; } = new();
    public GameXp gameXp { get; set; } = new();
    public int challengesCompleted { get; set; }

    public class OwnedAvailable
    {
        public int owned { get; set; }
        public int available { get; set; }
    }
    public class NewOwnedAvailable
    {
        [JsonProperty(PropertyName = "new")]
        public int New { get; set; }
        public int owned { get; set; }
        public int available { get; set; }
    }

    public class Action
    {
        public string groupId { get; set; } = string.Empty;
        public int xp { get; set; }
    }


    public class Breakdown
    {
        public List<Action> actions { get; set; } = [];
        public int challenges { get; set; }
    }

    public class GameXp
    {
        public int owned { get; set; }
        public int threshold { get; set; }
        public Breakdown breakdown { get; set; } = new();
    }
}
