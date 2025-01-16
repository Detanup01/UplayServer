namespace ServerCore.Models.Auth;

public class Demux
{
    [LiteDB.BsonId]
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public uint ConnectionId { get; set; }
    public string ConnectionName { get; set; } = string.Empty;
}
