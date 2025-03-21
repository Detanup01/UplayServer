namespace ServerCore.Models.User;

public class UserLogin : UserBase
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = "2000-01-01";
    public string NameOnPlatform { get; set; } = string.Empty;
    public string Country { get; set; } = "EU";
    public string PreferredLanguage { get; set; } = "US";
    public string LegalOptinsKey { get; set; } = string.Empty;
}
