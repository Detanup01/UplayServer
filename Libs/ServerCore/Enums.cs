namespace ServerCore;

public enum TokenType
{
    None,
    Orbit,
    Token,
    Ticket,

    AuthToken,
    UbiV1 = AuthToken,
    UplayPCV1,
    RememberMe_v1,
    BasicAuth,
    // This token only used to just say the current UserId
    UserIdToken,
}

public enum AppFlags
{
    NotAvailable,
    Downloadable,
    Playable,
    DenuvoForceTimeTrial,
    Denuvo,
    FromSubscription,
    FromExpiredSubscription
}