namespace ServerCore.Models.User;

public class UserFriend : UserBase
{
    public Guid? IdOfFriend { get; set; }
    public string? Name { get; set; }
    public string? Nickname { get; set; }
    public bool IsFavorite { get; set; } = false;
    public bool IsBlacklisted { get; set; } = false;
    public int Relation { get; set; } = 0;
}
