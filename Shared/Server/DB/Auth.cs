using LiteDB;
using SharedLib.Server.Json.DB;

namespace SharedLib.Server.DB
{
    public class Auth
    {
        public readonly static string DBName = Prepare.DatabasePath + "Auth.db";
        public readonly static string UA = "UA"; // (User Auth)
        public readonly static string U2S = "U2S"; // (User To Session)
        public readonly static string DMX = "DMX"; // (Demux)
        public readonly static string Current = "Current";

        #region UA (User Auth)
        public static void AddUA(string userId, string auth)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JUA>(UA);
                if (!col.Exists(x => x.authtoken == auth))
                {
                    var x = col.Count();
                    col.Insert(new JUA()
                    {
                        Id = x,
                        userId = userId,
                        authtoken = auth
                    });
                }
            }
        }

        public static void EditUA(string userId, string auth)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JUA>(UA);

                var toReplace = col.Find(x => x.userId == userId).First();

                if (toReplace != null)
                {
                    toReplace.authtoken = auth;
                    col.Update(toReplace);
                }
            }
        }

        public static string GetUserIdByAuth(string userId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JUA>(UA);

                var toGet = col.Find(x => x.userId == userId).First();

                if (toGet != null && !string.IsNullOrEmpty(toGet.userId))
                {
                    return toGet.userId;
                }
                else
                    return "";
            }
        }

        public static string GetAuthByUserId(string auth)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JUA>(UA);

                var toGet = col.Find(x => x.authtoken == auth).First();

                if (toGet != null && !string.IsNullOrEmpty(toGet.authtoken))
                {
                    return toGet.authtoken;
                }
                else
                    return "";
            }
        }

        public static void DeleteUA(string userId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JUA>(UA);

                var toDel = col.Find(x => x.userId == userId).First();

                col.Delete(toDel.Id);
            }
        }
        #endregion

        #region U2S (User To Session)
        public static void AddU2S(string userId, string sessionId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JU2S>(U2S);
                if (!col.Exists(x => x.userId == userId))
                {
                    var x = col.Count();
                    col.Insert(new JU2S()
                    {
                        Id = x,
                        userId = userId,
                        sessionId = sessionId
                    });
                }
            }
        }
        public static void EditU2S(string userId, string sessionId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JU2S>(U2S);

                var toReplace = col.Find(x => x.userId == userId).First();

                if (toReplace != null)
                {
                    toReplace.sessionId = sessionId;
                    col.Update(toReplace);
                }
            }
        }

        public static string GetUserIdBySessionId(string sessionId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JU2S>(U2S);

                var toGet = col.Find(x => x.sessionId == sessionId).First();

                if (toGet != null && !string.IsNullOrEmpty(toGet.userId))
                {
                    return toGet.userId;
                }
                else
                    return "";
            }
        }

        public static string GetSessionIdByUserId(string userId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JU2S>(U2S);

                var toGet = col.Find(x => x.userId == userId).First();

                if (toGet != null && !string.IsNullOrEmpty(toGet.sessionId))
                {
                    return toGet.sessionId;
                }
                else
                    return "";
            }
        }

        public static void DeleteU2SWithSession(string sessionId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JU2S>(U2S);

                var toDel = col.Find(x => x.sessionId == sessionId).First();

                col.Delete(toDel.Id);
            }
        }

        public static void DeleteU2SWithUser(string userId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JU2S>(U2S);

                var toDel = col.Find(x => x.userId == userId).First();

                col.Delete(toDel.Id);
            }
        }
        #endregion

        #region DMX (Demux)
        public static void AddDMX(string userId, uint conId, string conName)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JDMX>(DMX);
                if (!col.Exists(x => x.userId == userId && x.conId == conId && x.conName == conName))
                {
                    var x = col.Count();
                    col.Insert(new JDMX()
                    {
                        Id = x,
                        userId = userId,
                        conId = conId,
                        conName = conName
                    });
                }
            }
        }
        public static uint GetConIdByUserAndName(string userId, string conName)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JDMX>(DMX);

                var toGet = col.Find(x => x.userId == userId && x.conName == conName).First();

                if (toGet != null)
                {
                    return toGet.conId;
                }
                else
                    return 0;
            }
        }
        public static string GetConNameByUserAndId(string userId, uint conId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JDMX>(DMX);

                var toGet = col.Find(x => x.userId == userId && x.conId == conId).First();

                if (toGet != null && !string.IsNullOrEmpty(toGet.conName))
                {
                    return toGet.conName;
                }
                else
                    return "";
            }
        }
        public static uint GetLatestConIdByUser(string userId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JDMX>(DMX);

                var toGet = col.Find(x => x.userId == userId).Last();

                if (toGet != null)
                {
                    return toGet.conId;
                }
                else
                    return 0;
            }
        }

        public static void DeleteDMX(string userId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JDMX>(DMX);

                var toDel = col.Find(x => x.userId == userId).First();

                col.Delete(toDel.Id);
            }
        }

        public static void DeleteDMXWithUserId(string userId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JDMX>(DMX);

                col.DeleteMany(userId);
            }
        }
        #endregion

        #region Current
        public static void AddCurrent(string userId, string token, Enums.TokenType tokentype)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JCurrent>(Current);
                if (!col.Exists(x => x.userId == userId && x.token == token && x.type == tokentype))
                {
                    var x = col.Count();
                    col.Insert(new JCurrent()
                    {
                        Id = x,
                        userId = userId,
                        token = token,
                        type = tokentype
                    });
                }
            }
        }

        public static void EditCurrent(string userId, string token, Enums.TokenType tokentype)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JCurrent>(Current);
                var toReplace = col.Find(x => x.userId == userId && x.type == tokentype).First();

                if (toReplace != null)
                {
                    toReplace.token = token;
                    col.Update(toReplace);
                }
            }
        }

        public static string GetUserIdByToken(string token, Enums.TokenType tokentype)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JCurrent>(Current);
                var toGet = col.Find(x => x.token == token && x.type == tokentype).First();

                if (toGet != null && !string.IsNullOrEmpty(toGet.userId))
                {
                    return toGet.userId;
                }
                else
                    return "";

            }
        }

        public static void DeleteCurrent(string userId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JCurrent>(Current);

                var toDel = col.Find(x => x.userId == userId).Last();

                col.Delete(toDel.Id);
            }
        }

        public static void DeleteCurrentWithUserId(string userId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JCurrent>(Current);

                col.DeleteMany(userId);
            }
        }
        #endregion
    }
}
