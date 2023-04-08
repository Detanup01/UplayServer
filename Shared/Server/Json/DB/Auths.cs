namespace SharedLib.Server.Json.DB
{
    public class JCurrent
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string token { get; set; }
        public Enums.TokenType type { get; set; }
    }

    public class JAuth
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string authtoken { get; set; }
    }

    public class JSession
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string sessionId { get; set; }
    }
    public class JCon
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string sessionId { get; set; }
    }
}
