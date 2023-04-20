using LiteDB;
using SharedLib.Server.Json.DB;
using static Uplay.Statistics.GameCloudSaveSyncObjectData.Types;

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
                        Id = x+1,
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

                var toReplace = col.FindOne(x => x.userId == userId);

                if (toReplace != null)
                {
                    toReplace.authtoken = auth;
                    col.Update(toReplace);
                }
            }
        }

        public static string GetUserIdByAuth(string auth)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JUA>(UA);

                var toGet = col.FindOne(x => x.authtoken == auth);

                if (toGet != null)
                {
                    return toGet.userId;
                }
                else
                    return "";
            }
        }

        public static string GetAuthByUserId(string userId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JUA>(UA);

                var toGet = col.FindOne(x => x.userId == userId);

                if (toGet != null)
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

                var toDel = col.FindOne(x => x.userId == userId);

                if (toDel != null)
                {
                    col.Delete(toDel.Id);
                }
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
                        Id = x+1,
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

                var toReplace = col.FindOne(x => x.userId == userId);

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

                var toGet = col.FindOne(x => x.sessionId == sessionId);

                if (toGet != null)
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

                var toGet = col.FindOne(x => x.userId == userId);

                if (toGet != null)
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

                var toDel = col.FindOne(x => x.sessionId == sessionId);

                if (toDel != null)
                {
                    col.Delete(toDel.Id);
                }
            }
        }

        public static void DeleteU2SWithUser(string userId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JU2S>(U2S);

                var toDel = col.FindOne(x => x.userId == userId);


                if (toDel != null)
                {
                    col.Delete(toDel.Id);
                }
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
                        Id = x+1,
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

                var toGet = col.Find(x => x.userId == userId && x.conName == conName);

                if (toGet != null)
                {
                    if (toGet.Any())
                    {
                        var conList = toGet.ToList().OrderBy(x=>x.conId).ToList();
                        return conList.First().conId;
                    }
                    return 0;
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

                var toGet = col.FindOne(x => x.userId == userId && x.conId == conId);

                if (toGet != null)
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

                var toGet = col.Find(x => x.userId == userId);

                if (toGet != null)
                {
                    if (toGet.Any())
                    {
                        var conList = toGet.ToList().OrderBy(x => x.conId).ToList();
                        return conList.First().conId;
                    }
                    return 0;
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

                var toDel = col.FindOne(x => x.userId == userId);

                if (toDel != null)
                {
                    col.Delete(toDel.Id);
                }
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
                        Id = x+1,
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
                var toReplace = col.FindOne(x => x.userId == userId && x.type == tokentype);

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
                var toGet = col.FindOne(x => x.token == token && x.type == tokentype);

                if (toGet != null)
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

                var toDel = col.FindOne(x => x.userId == userId);

                if (toDel != null)
                {
                    col.Delete(toDel.Id);
                }
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
