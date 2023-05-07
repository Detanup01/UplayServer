using LiteDB;
using SharedLib.Server.Json.DB;

namespace SharedLib.Server.DB
{
    public class DBUser
    {
        public readonly static string DBName = Prepare.DatabasePath + "User.db";
        public readonly static string USER = "User";
        public readonly static string OwnershipBasic = "OwnershipBasic";
        public readonly static string Ownership = "Ownership";
        public readonly static string Activity = "Activity";
        public readonly static string Friend = "Friend";
        public readonly static string Playtime = "Playtime";
        public readonly static string GameSession = "GameSession";

        public static void Add(JUser user)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JUser>(USER);

                if (!col.Exists(X => X.Id == user.Id && X.UserId == user.UserId))
                {
                    col.Insert(user);
                }
            }
        }

        public static void Add(JOwnershipBasic ownershipBasic)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JOwnershipBasic>(OwnershipBasic);

                if (!col.Exists(X => X.Id == ownershipBasic.Id && X.UserId == ownershipBasic.UserId))
                {
                    col.Insert(ownershipBasic);
                }
            }
        }


        public static void Add(JActivity activity)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JActivity>(Activity);

                if (!col.Exists(X => X.Id == activity.Id && X.UserId == activity.UserId))
                {
                    col.Insert(activity);
                }
            }
        }

        public static void Add(JFriend friend)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JFriend>(Friend);

                if (!col.Exists(X => X.Id == friend.Id && X.UserId == friend.UserId))
                {
                    col.Insert(friend);
                }
            }
        }

        public static void Add(JPlaytime playtime)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPlaytime>(Playtime);

                if (!col.Exists(X => X.Id == playtime.Id && X.UserId == playtime.UserId))
                {
                    col.Insert(playtime);
                }
            }
        }

        public static void Add(JGameSession session)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JGameSession>(GameSession);

                if (!col.Exists(X => X.Id == session.Id && X.UserId == session.UserId))
                {
                    col.Insert(session);
                }
            }
        }
    }
}
