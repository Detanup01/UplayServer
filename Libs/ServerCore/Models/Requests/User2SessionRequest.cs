namespace ServerCore.Models.Requests;

public class User2SessionRequest
{
    public Guid SessionId { get; set; }
    public Guid UserId { get; set; }
}
