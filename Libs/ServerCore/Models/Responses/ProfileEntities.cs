namespace ServerCore.Models.Responses;

public class ProfileEntities
{
    public List<Entity> entities { get; set; } = [];


    public class Entity
    {
        public string entityId { get; set; } = string.Empty;
        public string profileId { get; set; } = string.Empty;
        public string spaceId { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public List<object> tags { get; set; } = [];
        public Dictionary<string, List<object>> obj { get; set; } = [];
        public string lastModified { get; set; } = string.Empty;
        public int revision { get; set; }
    }
}
