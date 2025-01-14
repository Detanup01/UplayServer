namespace ServerCore.Models.User;

public class UserOwnershipBasic : UserBase
{
    public List<uint> OwnedGamesIds { get; set; } = [];
    public Dictionary<uint, List<uint>> UnlockedBranches { get; set; } = [];
    public uint UbiPlus { get; set; } = 0;
}
