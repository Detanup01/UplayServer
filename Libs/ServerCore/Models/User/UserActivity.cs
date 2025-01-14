namespace ServerCore.Models.User;

public class UserActivity : UserBase
{
    public int Status { get; set; }
    public int OnlineStatus { get; set; } = 0;
    public bool IsPlaying { get; set; }
    public string? ProductName { get; set; }
    public uint? GameId { get; set; }
    public string? Key { get; set; }
    public string? Value { get; set; }
}
