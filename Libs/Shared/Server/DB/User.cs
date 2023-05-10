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

        #region JUser
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

        public static void Edit(JUser user)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JUser>(USER);

                var fId = col.FindOne(X=> X.Id == user.Id);
                if (fId != null)
                {
                    user.Id = fId.Id;
                    col.Update(user);
                }

            }
        }

        public static JUser? GetUser(string UserId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JUser>(USER);

                var fId = col.FindOne(X => X.UserId == UserId);
                if (fId != null)
                {
                    return fId;
                }
            }
            return null;
        }
        #endregion
        #region JOwnershipBasic
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

        public static void Edit(JOwnershipBasic ownershipBasic)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JOwnershipBasic>(OwnershipBasic);

                var fId = col.FindOne(X => X.Id == ownershipBasic.Id);
                if (fId != null)
                {
                    ownershipBasic.Id = fId.Id;
                    col.Update(ownershipBasic);
                }

            }
        }

        public static JOwnershipBasic? GetOwnershipBasic(string UserId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JOwnershipBasic>(OwnershipBasic);

                var fId = col.FindOne(X => X.UserId == UserId);
                if (fId != null)
                {
                    return fId;
                }
            }
            return null;
        }
        #endregion
        #region JOwnership
        public static void Add(JOwnership ownership)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JOwnership>(Ownership);

                if (!col.Exists(X => X.Id == ownership.Id && X.UserId == ownership.UserId))
                {
                    col.Insert(ownership);
                }
            }
        }

        public static void Edit(JOwnership ownership)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JOwnership>(Ownership);

                var fId = col.FindOne(X => X.Id == ownership.Id);
                if (fId != null)
                {
                    ownership.Id = fId.Id;
                    col.Update(ownership);
                }

            }
        }

        public static JOwnership? GetOwnership(string UserId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JOwnership>(Ownership);

                var fId = col.FindOne(X => X.UserId == UserId);
                if (fId != null)
                {
                    return fId;
                }
            }
            return null;
        }
        #endregion
        #region JActivity
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

        public static void Edit(JActivity activity)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JActivity>(Activity);

                var fId = col.FindOne(X => X.Id == activity.Id);
                if (fId != null)
                {
                    activity.Id = fId.Id;
                    col.Update(activity);
                }

            }
        }

        public static JActivity? GetActivity(string UserId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JActivity>(Activity);

                var fId = col.FindOne(X => X.UserId == UserId);
                if (fId != null)
                {
                    return fId;
                }
            }
            return null;
        }
        #endregion
        #region JFriend
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

        public static void Edit(JFriend friend)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JFriend>(Friend);

                var fId = col.FindOne(X => X.Id == friend.Id);
                if (fId != null)
                {
                    friend.Id = fId.Id;
                    col.Update(friend);
                }

            }
        }

        public static JFriend? GetFriend(string UserId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JFriend>(Friend);

                var fId = col.FindOne(X => X.UserId == UserId);
                if (fId != null)
                {
                    return fId;
                }
            }
            return null;
        }
        #endregion
        #region JPlaytime
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

        public static void Edit(JPlaytime playtime)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPlaytime>(Playtime);

                var fId = col.FindOne(X => X.Id == playtime.Id);
                if (fId != null)
                {
                    playtime.Id = fId.Id;
                    col.Update(playtime);
                }

            }
        }

        public static JPlaytime? GetPlaytime(string UserId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPlaytime>(Playtime);

                var fId = col.FindOne(X => X.UserId == UserId);
                if (fId != null)
                {
                    return fId;
                }
            }
            return null;
        }
        #endregion
        #region JGameSession
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

        public static void Edit(JGameSession session)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JGameSession>(Playtime);

                var fId = col.FindOne(X => X.Id == session.Id);
                if (fId != null)
                {
                    session.Id = fId.Id;
                    col.Update(session);
                }

            }
        }

        public static JGameSession? GetGameSession(string UserId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JGameSession>(Playtime);

                var fId = col.FindOne(X => X.UserId == UserId);
                if (fId != null)
                {
                    return fId;
                }
            }
            return null;
        }
        #endregion
    }
}
