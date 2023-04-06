using Newtonsoft.Json;

namespace Core.JSON.Ext
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
            var user = GetUser(UserId);
            if (user != null)
            {
                user.Activity.GameId = game.UplayId;
                user.Activity.ProductName = game.ProductName;
                if (game.GameSession != null)
                {
                    user.Activity.GameSession.SessionData = game.GameSession.GameSessionData.ToBase64();
                    user.Activity.GameSession.SessionId = game.GameSession.GameSessionId;
                    user.Activity.GameSession.SessionIdV2 = game.GameSession.GameSessionIdV2;
                    user.Activity.GameSession.Joinable = game.GameSession.Joinable;
                    user.Activity.GameSession.Size = game.GameSession.Size;
                    user.Activity.GameSession.MaxSize = game.GameSession.MaxSize;

                }
                SaveUser(UserId, user);
            }
        }
    }
}
