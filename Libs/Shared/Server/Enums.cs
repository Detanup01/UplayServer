namespace SharedLib.Server;

public class Enums
{
    public enum TokenType
    {
        Orbit = 1,
        Token = 2,
        Ticket = 3
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
}
