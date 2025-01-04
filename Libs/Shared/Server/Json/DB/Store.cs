#if SERVER
namespace SharedLib.Server.Json.DB;

public class JStore
{
    public int Id { get; set; }
    public uint productId { get; set; }
    public string configuration { get; set; } = string.Empty;
    public string reference { get; set; } = string.Empty;
    public string userBlob { get; set; } = string.Empty;
    public int partner { get; set; }
    public List<uint> ownershipAssociations { get; set; } = [];
    public List<uint> associations { get; set; } = [];
    public int Type { get; set; }
}
#endif