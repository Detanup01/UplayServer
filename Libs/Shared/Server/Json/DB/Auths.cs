#if SERVER

namespace SharedLib.Server.Json.DB;

public class JCurrent
{
    public int Id { get; set; }
    public string userId { get; set; } = string.Empty;
    public string token { get; set; } = string.Empty;
    public Enums.TokenType type { get; set; }
}

public class JUA
{
    public int Id { get; set; }
    public string userId { get; set; } = string.Empty;
    public string authtoken { get; set; } = string.Empty;
}

public class JU2S
{
    public int Id { get; set; }
    public string userId { get; set; } = string.Empty;
    public string sessionId { get; set; } = string.Empty;
}

public class JDMX
{
    public int Id { get; set; }
    public string userId { get; set; } = string.Empty;
    public uint conId { get; set; }
    public string conName { get; set; } = string.Empty;
}
#endif