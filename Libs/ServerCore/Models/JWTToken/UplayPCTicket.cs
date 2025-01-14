namespace ServerCore.Models.JWTToken;

public class UplayPCTicket
{
    public string sub { get; set; } = string.Empty;
    public string iss { get; set; } = string.Empty;
    public long exp { get; set; }
    public int uplay_id { get; set; }
    public int platform { get; set; }
}
