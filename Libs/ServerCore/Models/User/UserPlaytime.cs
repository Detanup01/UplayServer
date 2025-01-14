namespace ServerCore.Models.User;

public class UserPlaytime : UserBase
{
    public uint UplayId { get; set; }
    public ulong PlayTime { get; set; }
}
