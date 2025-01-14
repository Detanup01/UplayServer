namespace ServerCore.Models.User;

public class UserGameSession : UserBase
{
    public ulong SessionId { get; set; }
    public string SessionIdV2 { get; set; } = string.Empty;
    public string SessionData { get; set; } = string.Empty;
    public bool Joinable { get; set; }
    public uint Size { get; set; }
    public uint MaxSize { get; set; }
}
