using ServerCore.ServerAndSession;

namespace ServerCore.DMX;

public class DmxSession(UplaySession session)
{
    public UplaySession Session => session;

    public Guid SessionId => Session.Id;

    public Guid UserId { get; set; } = Guid.Empty;

    public bool IsLoggedIn => UserId != Guid.Empty;

    public override string ToString()
    {
        return $"(DmxSession) Sid: {SessionId}, UserId: {UserId}";
    }
}
