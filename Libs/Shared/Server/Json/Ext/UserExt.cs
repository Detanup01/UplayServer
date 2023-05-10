using SharedLib.Server.DB;

namespace SharedLib.Server.Json
{
    public class UserExt : User
    {
        public static bool RemoveFromFriends(string UserId, string FriendId)
        {
            var user = GetUser(UserId);
            if (user != null)
            {
                var fr = user.Friends.Where(x => x.UserId == FriendId).FirstOrDefault();
                if (fr != null)
                {
                    user.Friends.Remove(fr);
                    SaveUser(UserId, user);
                    return true;
                }
            }
            return false;
        }

        public static bool SetAllUserActivity(string UserId)
        {
            var user = GetUser(UserId);
            if (user != null)
            {
                foreach (var fr in user.Friends)
                {
                    var fuser = GetUser(fr.UserId);
                    if (fuser != null)
                    {
                        var fuserf = fuser.Friends.Find(x => x.UserId == UserId);
                        if (fuserf != null)
                        {
                            fuserf.Activity = user.Activity;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public static bool IsUserExist(string UserId)
        {
            foreach (var file in Directory.GetFiles("ServerFiles/Users"))
            {
                if (file.Replace(".json", "").Replace("ServerFiles/Users\\", "") == UserId)
                {
                    return true;
                }
            }
            return false;
        }

        public static void UplayFriendsGameParseToUser(string UserId, Uplay.Friends.Game game)
        {
            var activity = DBUser.GetActivity(UserId);
            if (activity == null)
                activity = new()
                { 
                    UserId = UserId
                };
            activity.GameId = game.UplayId;
            activity.ProductName = game.ProductName;
            activity.IsPlaying = true;
            DBUser.Edit(activity);
            if (game.GameSession != null)
            {
                var session = DBUser.GetGameSession(UserId);
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
                DBUser.Edit(session);
            }
        }
    }
}
