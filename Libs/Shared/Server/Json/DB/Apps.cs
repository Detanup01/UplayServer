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

    public class JAppFlags
    {
        public int Id { get; set; }
        public uint productId { get; set; }
        public List<string> flags { get; set; }
    }
}
