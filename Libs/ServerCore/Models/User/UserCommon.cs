namespace ServerCore.Models.User;

public class UserCommon : UserBase
{
    public string Name { get; set; } = string.Empty;
    public List<Guid> Friends { get; set; } = [];
    public bool IsBanned { get; set; } = false;
}
