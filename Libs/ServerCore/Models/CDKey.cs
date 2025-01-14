namespace ServerCore.Models;

public class CDKey
{
    public uint ProductId { get; set; }
    public string Key { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
}
