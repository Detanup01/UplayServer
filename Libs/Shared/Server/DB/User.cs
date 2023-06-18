using LiteDB;
using SharedLib.Server.Json.DB;

namespace SharedLib.Server.DB
{
    public class DBUser
    {
        readonly static string DBName = Prepare.DatabasePath + "User.db";
        readonly static string USER = "User";
        readonly static string OwnershipBasic = "OwnershipBasic";
        readonly static string Ownership = "Ownership";
        readonly static string Activity = "Activity";
        readonly static string Friend = "Friends";
        readonly static string Playtime = "Playtime";
        readonly static string GameSession = "GameSession";
        readonly static string CloudSave = "CloudSave";

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
        public static List<JUser>? GetUsers()
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JUser>(USER);

                var fId = col.FindAll();
                if (fId != null)
                {
                    return fId.ToList();
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

        public static JOwnership? GetOwnership(string UserId, uint productId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JOwnership>(Ownership);

                var fId = col.FindOne(X => X.UserId == UserId && X.ProductId == productId);
                if (fId != null)
                {
                    return fId;
                }
            }
            return null;
        }

        public static List<JOwnership>? GetOwnershipList(string UserId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JOwnership>(Ownership);

                var fId = col.Find(X => X.UserId == UserId);
                if (fId != null)
                {
                    return fId.ToList();
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

        public static JFriend? GetFriend(string UserId, string FriendUserId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JFriend>(Friend);

                var fId = col.FindOne(X => X.UserId == FriendUserId && X.IdOfFriend == UserId);
                if (fId != null)
                {
                    return fId;
                }
            }
            return null;
        }

        public static List<JFriend>? GetFriends(string UserId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JFriend>(Friend);

                var fId = col.Find(X => X.IdOfFriend == UserId);
                if (fId != null)
                {
                    return fId.ToList();
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

        public static JPlaytime? GetPlaytime(string UserId, uint ProdId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPlaytime>(Playtime);

                var fId = col.FindOne(X => X.UserId == UserId && X.uplayId == ProdId);
                if (fId != null)
                {
                    return fId;
                }
            }
            return null;
        }
        public static List<JPlaytime>? GetPlaytimes(string UserId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPlaytime>(Playtime);

                var fId = col.Find(X => X.UserId == UserId);
                if (fId != null)
                {
                    return fId.ToList();
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
        #region JGameSession
        public static void Add(JCloudSave session)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JCloudSave>(CloudSave);

                if (!col.Exists(X => X.Id == session.Id && X.UserId == session.UserId))
                {
                    col.Insert(session);
                }
            }
        }

        public static void Edit(JCloudSave session)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JCloudSave>(CloudSave);

                var fId = col.FindOne(X => X.Id == session.Id);
                if (fId != null)
                {
                    session.Id = fId.Id;
                    col.Update(session);
                }

            }
        }

        public static List<JCloudSave>? GetCloudSaves(string UserId, uint ProductId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JCloudSave>(CloudSave);

                var fId = col.Find(X => X.UserId == UserId && X.uplayId == ProductId);
                if (fId != null)
                {
                    return fId.ToList();
                }
            }
            return null;
        }

        public static JCloudSave? GetCloudSave(string UserId, uint ProductId, uint SaveId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JCloudSave>(CloudSave);

                var fId = col.FindOne(X => X.UserId == UserId && X.uplayId == ProductId && X.SaveId == SaveId);
                if (fId != null)
                {
                    return fId;
                }
            }
            return null;
        }

        public static JCloudSave? GetCloudSave(string UserId, uint ProductId, string saveName)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JCloudSave>(CloudSave);

                var fId = col.FindOne(X => X.UserId == UserId && X.uplayId == ProductId && X.SaveName == saveName);
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
