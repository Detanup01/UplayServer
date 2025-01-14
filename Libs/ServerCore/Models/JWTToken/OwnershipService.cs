namespace ServerCore.Models.JWTToken;

public class OwnershipService
{
    public string sub { get; set; } = string.Empty;
    public string iss { get; set; } = string.Empty;
    public long exp { get; set; }
    public int uplay_id { get; set; }
    public int product_id { get; set; }
    public int branch_id { get; set; }
    public List<string> flags { get; set; } = [];
}
