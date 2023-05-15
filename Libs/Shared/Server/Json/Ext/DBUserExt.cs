using SharedLib.Server.DB;
using SharedLib.Server.Json.DB;

namespace SharedLib.Server.Json.Ext
{
    public class DBUserExt : DBUser
    {
        public static void AddOwnership(uint appid, uint branchid, string userId, string cdkey, List<uint> Subscriptions, List<uint> ActivationIds)
        {
            Add(new JOwnership()
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
                current_branch_id = branchid,
                IsOwned = true,
                TargetPartner = Uplay.Ownership.OwnedGame.Types.TargetPartner.None
            });
        }

        public static bool IsUserBanned(string UserID)
        {
            var user = GetUser(UserID);
            if (user == null)
                return false;
            return user.IsBanned;
        }

        public static bool IsUserExist(string UserID)
        {
            var user = GetUser(UserID);
            return user != null;
        }

        public static bool RemoveFromFriends(string UserId, string FriendId)
        {
            JUser? user = GetUser(UserId);
            if (user != null)
            {
                user.Friends.Remove(FriendId);
                user = GetUser(FriendId);
                if (user != null)
                {
                    user.Friends.Remove(UserId);
                    return true;
                }
            }
            return false;
        }

        public static void UplayFriendsGameParseToUser(string UserId, Uplay.Friends.Game game)
        {
            var activity = GetActivity(UserId);
            if (activity == null)
                activity = new()
                {
                    UserId = UserId
                };
            activity.GameId = game.UplayId;
            activity.ProductName = game.ProductName;
            activity.IsPlaying = true;
            Edit(activity);
            if (game.GameSession != null)
            {
                var session = GetGameSession(UserId);
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
    }
}
