namespace ServerCore.Models.User;

public class UserOwnership : UserBase
{
    public uint ProductId { get; set; }
    public bool IsOwned { get; set; }
    public bool IsLockedSubscription { get; set; }
    public string CD_Key { get; set; } = string.Empty;
    public uint CurrentBranchId { get; set; } = 0;
    public List<uint> ActivationIds { get; set; } = [];
    public List<uint> Subscriptions { get; set; } = [];
    public List<AppFlags> AppFlags { get; set; } = [];
    public Uplay.Ownership.OwnedGame.Types.PackageOwnershipState PackageState { get; set; }
    public Uplay.Ownership.OwnedGame.Types.SuspensionType Suspension { get; set; }
    public Uplay.Ownership.OwnedGame.Types.ActivationType Activation { get; set; }
    public Uplay.Ownership.OwnedGame.Types.TargetPartner TargetPartner { get; set; }
    public Uplay.Ownership.OwnedGame.Types.DenuvoActivationOverwrite DenuvoActivation { get; set; }
}
