namespace ServerCore.Models.Requests;

public class ValidateCreation
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string dateOfBirth { get; set; } = string.Empty;
    public string nameOnPlatform { get; set; } = string.Empty;
    public string country { get; set; } = string.Empty;
    public string preferredLanguage { get; set; } = string.Empty;
    public string legalOptinsKey { get; set; } = string.Empty;
}
