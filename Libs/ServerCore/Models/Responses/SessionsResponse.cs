namespace ServerCore.Models.Responses;

public class SessionsResponse
{
    public string platformType { get; set; } = string.Empty;
    public string ticket { get; set; } = string.Empty;
    public string? twoFactorAuthenticationTicket { get; set; }
    public Guid profileId { get; set; }
    public Guid userId { get; set; }
    public string nameOnPlatform { get; set; } = string.Empty;
    public string environment { get; set; } = "prod";
    public string expiration { get; set; } = string.Empty;
    public Guid spaceId { get; set; }
    public string clientIp { get; set; } = string.Empty;
    public string clientIpCountry { get; set; } = string.Empty;
    public string serverTime { get; set; } = string.Empty;
    public Guid sessionId { get; set; }
    public string sessionKey { get; set; } = string.Empty;
    public string? rememberMeTicket { get; set; }
    public string? rememberDeviceTicket { get; set; }
}
