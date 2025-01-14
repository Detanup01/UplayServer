namespace ServerCore.Models.Auth;

public class User2Session
{
    [LiteDB.BsonId]
    public Guid UserId { get; set; }
    public Guid SessionId { get; set; }
}
