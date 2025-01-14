namespace ServerCore.Models.JWTToken;

public class AuthService
{
    public string sub { get; set; } = string.Empty;
    public string iss { get; set; } = string.Empty;
    public long exp { get; set; }
    public string session { get; set; } = string.Empty;
    public string app { get; set; } = string.Empty;
    public string env { get; set; } = string.Empty;
}
