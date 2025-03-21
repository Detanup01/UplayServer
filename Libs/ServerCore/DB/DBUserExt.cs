using ServerCore.Models.User;

namespace ServerCore.DB;

public class DBUserExt : DBUser
{
    public static void AddOwnership(uint appid, uint branchid, Guid userId, string cdkey, List<uint> Subscriptions, List<uint> ActivationIds)
    {
        Add(new UserOwnership()
        {
            UserId = userId,
            Subscriptions = Subscriptions,
            Suspension = Uplay.Ownership.OwnedGame.Types.SuspensionType.None,
            IsLockedSubscription = false,
            DenuvoActivation = Uplay.Ownership.OwnedGame.Types.DenuvoActivationOverwrite.Default,
            PackageState = Uplay.Ownership.OwnedGame.Types.PackageOwnershipState.Full,
            Activation = Uplay.Ownership.OwnedGame.Types.ActivationType.Purchase,
            ActivationIds = ActivationIds,
            CD_Key = cdkey,
            ProductId = appid,
            CurrentBranchId = branchid,
            IsOwned = true,
            TargetPartner = Uplay.Ownership.OwnedGame.Types.TargetPartner.None
        });
    }

    public static bool IsUserBanned(string UserID)
    {
        var user = Get<UserCommon>(UserID);
        if (user == null)
            return false;
        return user.IsBanned;
    }

    public static bool IsUserBanned(Guid UserID)
    {
        var user = Get<UserCommon>(UserID);
        if (user == null)
            return false;
        return user.IsBanned;
    }

    public static bool IsUserExist(string UserID)
    {
        var user = Get<UserCommon>(UserID);
        return user != null;
    }

    public static bool IsUserExist(Guid UserID)
    {
        var user = Get<UserCommon>(UserID);
        return user != null;
    }

    public static bool RemoveFromFriends(Guid UserId, Guid FriendId)
    {
        UserCommon? user = Get<UserCommon>(UserId);
        if (user != null)
        {
            user.Friends.Remove(FriendId);
            Edit(user);
            user = Get<UserCommon>(FriendId);
            if (user != null)
            {
                user.Friends.Remove(UserId);
                Edit(user);
                return true;
            }
        }
        return false;
    }

    public static bool RemoveFromFriends(string UserId, string FriendId)
    {
        return RemoveFromFriends(Guid.Parse(UserId), Guid.Parse(FriendId));
    }

    public static void UplayFriendsGameParseToUser(Guid UserId, Uplay.Friends.Game game)
    {
        var activity = Get<UserActivity>(UserId);
        if (activity == null)
            activity = new()
            {
                UserId = UserId
            };
        activity.GameId = game.UplayId;
        activity.ProductName = game.ProductName;
        activity.IsPlaying = true;
        Edit(activity);
        if (game.GameSession == null)
            return;
        UserGameSession? session = Get<UserGameSession>(UserId);
        if (session == null)
            session = new()
            {
                UserId = UserId
            };
        session.SessionData = game.GameSession.GameSessionData.ToBase64();
        session.SessionId = game.GameSession.GameSessionId;
        session.SessionIdV2 = game.GameSession.GameSessionIdV2;
        session.Joinable = game.GameSession.Joinable;
        session.Size = game.GameSession.Size;
        session.MaxSize = game.GameSession.MaxSize;
        Edit(session);
    }
}