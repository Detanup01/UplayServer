using SharedLib.Server.DB;
using SharedLib.Server.Json.DB;

namespace SharedLib.Server.Json.Ext
{
    public class DBUserExt : DBUser
    {
        public static void AddOwnership(uint appid, string userId, string cdkey)
        {
            Add(new JOwnership()
            {
                UserId = userId,
                Subscriptions = { },
                Suspension = Uplay.Ownership.OwnedGame.Types.SuspensionType.None,
                IsLockedSubscription = false,
                DenuvoActivation = Uplay.Ownership.OwnedGame.Types.DenuvoActivationOverwrite.Default,
                PackageState = Uplay.Ownership.OwnedGame.Types.PackageOwnershipState.Full,
                Activation = Uplay.Ownership.OwnedGame.Types.ActivationType.Purchase,
                ActivationIds = { },
                CD_Key = cdkey,
                ProductId = appid,
                IsOwned = true,
                TargetPartner = Uplay.Ownership.OwnedGame.Types.TargetPartner.None
            });
        }
    }
}
