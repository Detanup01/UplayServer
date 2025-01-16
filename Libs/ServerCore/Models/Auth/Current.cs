namespace ServerCore.Models.Auth;

public class Current
{
    [LiteDB.BsonId]
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public TokenType type { get; set; }
}
