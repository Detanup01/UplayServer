namespace ServerCore.Models.User;

public class UserBase
{
    [LiteDB.BsonId]
    public Guid UserId { get; set; }
}
