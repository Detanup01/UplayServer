namespace SharedLib.Server.Json.DB
{
    public class JCurrent
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string token { get; set; }
        public Enums.TokenType type { get; set; }
    }

    public class JUA
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string authtoken { get; set; }
    }

    public class JU2S
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string sessionId { get; set; }
    }

    public class JDMX
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public uint conId { get; set; }
        public string conName { get; set; }
    }
}
