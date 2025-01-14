namespace ServerCore.Models.Auth;

public class UserAuth
{
    [LiteDB.BsonId]
    public Guid UserId { get; set; }
    public string AuthToken { get; set; } = string.Empty;
}
