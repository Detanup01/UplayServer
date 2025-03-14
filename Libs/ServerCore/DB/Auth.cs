using LiteDB;
using ServerCore.Models.Auth;

namespace ServerCore.DB;

public class Auth
{
    public readonly static string DBName = Path.Combine(Prepare.DatabasePath, "Auth.db");
    public readonly static string UA = "UA"; // (User Auth)
    public readonly static string U2S = "U2S"; // (User To Session)
    public readonly static string DMX = "DMX"; // (Demux)
    public readonly static string Current = "Current";

    #region UA (User Auth)
    public static void AddUA(Guid userId, string auth)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<UserAuth>(UA);
        if (!col.Exists(x => x.AuthToken == auth))
        {
            var x = col.Count();
            col.Insert(new UserAuth()
            {
                UserId = userId,
                AuthToken = auth
            });
        }
    }

    public static void EditUA(Guid userId, string auth)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<UserAuth>(UA);

        var toReplace = col.FindOne(x => x.UserId == userId);

        if (toReplace != null)
        {
            toReplace.AuthToken = auth;
            col.Update(toReplace);
        }
    }

    public static Guid GetUserIdByAuth(string auth)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<UserAuth>(UA);

        var toGet = col.FindOne(x => x.AuthToken == auth);

        if (toGet != null)
        {
            return toGet.UserId;
        }
        else
            return Guid.Empty;
    }

    public static string GetAuthByUserId(Guid userId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<UserAuth>(UA);

        var toGet = col.FindOne(x => x.UserId == userId);

        if (toGet != null)
        {
            return toGet.AuthToken;
        }
        else
            return string.Empty;
    }

    public static void DeleteUA(Guid userId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<UserAuth>(UA);

        col.Delete(userId);
    }
    #endregion

    #region U2S (User To Session)
    public static void AddU2S(Guid userId, Guid sessionId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<User2Session>(U2S);
        if (!col.Exists(x => x.UserId == userId))
        {
            var x = col.Count();
            col.Insert(new User2Session()
            {
                UserId = userId,
                SessionId = sessionId
            });
        }
    }
    public static void EditU2S(Guid userId, Guid sessionId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<User2Session>(U2S);

        var toReplace = col.FindOne(x => x.UserId == userId);

        if (toReplace != null)
        {
            toReplace.SessionId = sessionId;
            col.Update(toReplace);
        }
    }

    public static Guid GetUserIdBySessionId(Guid sessionId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<User2Session>(U2S);

        var toGet = col.FindOne(x => x.SessionId == sessionId);

        if (toGet != null)
        {
            return toGet.UserId;
        }
        else
            return Guid.Empty;
    }

    public static Guid GetSessionIdByUserId(Guid userId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<User2Session>(U2S);

        var toGet = col.FindOne(x => x.UserId == userId);

        if (toGet != null)
        {
            return toGet.SessionId;
        }
        else
            return Guid.Empty;
    }

    public static void DeleteU2SWithSession(Guid sessionId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<User2Session>(U2S);

        var toDel = col.FindOne(x => x.SessionId == sessionId);

        if (toDel != null)
        {
            col.Delete(toDel.UserId);
        }
    }

    public static void DeleteU2SWithUser(Guid userId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<User2Session>(U2S);

        col.Delete(userId);
    }
    #endregion

    #region DMX (Demux)
    public static void AddDMX(Guid userId, uint conId, string conName)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<Demux>(DMX);
        if (!col.Exists(x => x.UserId == userId && x.ConnectionId == conId && x.ConnectionName == conName))
        {
            col.Insert(new Demux()
            {
                UserId = userId,
                ConnectionId = conId,
                ConnectionName = conName
            });
        }
    }
    public static uint GetConIdByUserAndName(Guid userId, string conName)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<Demux>(DMX);

        var toGet = col.Find(x => x.UserId == userId && x.ConnectionName == conName);

        if (toGet != null)
        {
            if (toGet.Any())
            {
                var conList = toGet.ToList().OrderBy(x => x.ConnectionId).ToList();
                return conList.First().ConnectionId;
            }
            return uint.MaxValue;
        }
        else
            return uint.MaxValue;
    }
    public static string GetConNameByUserAndId(Guid userId, uint conId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<Demux>(DMX);

        var toGet = col.FindOne(x => x.UserId == userId && x.ConnectionId == conId);

        if (toGet != null)
        {
            return toGet.ConnectionName;
        }
        else
            return string.Empty;
    }
    public static uint GetLatestConIdByUser(Guid userId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<Demux>(DMX);

        var toGet = col.Find(x => x.UserId == userId);

        if (toGet != null)
        {
            if (toGet.Any())
            {
                var conList = toGet.ToList().OrderBy(x => x.ConnectionId).ToList();
                return conList.First().ConnectionId;
            }
            return uint.MaxValue;
        }
        else
            return uint.MaxValue;
    }

    public static void DeleteDMXWithUserId(Guid userId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<Demux>(DMX);

        var toDel = col.Find(x => x.UserId == userId);

        if (toDel != null)
        {
            foreach (var item in toDel)
            {
                col.Delete(userId);
            }
        }
    }
    #endregion

    #region Current
    public static void AddCurrent(Guid userId, string token, TokenType tokentype)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<Current>(Current);
        if (!col.Exists(x => x.UserId == userId && x.Token == token && x.type == tokentype))
        {
            col.Insert(new Current()
            {
                UserId = userId,
                Token = token,
                type = tokentype
            });
        }
    }

    public static void EditCurrent(Guid userId, string token, TokenType tokentype)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<Current>(Current);
        var toReplace = col.FindOne(x => x.UserId == userId && x.type == tokentype);

        if (toReplace != null)
        {
            toReplace.Token = token;
            col.Update(toReplace);
        }
    }

    public static Guid GetUserIdByToken(string token, TokenType tokentype)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<Current>(Current);
        var toGet = col.FindOne(x => x.Token == token && x.type == tokentype);

        if (toGet != null)
        {
            return toGet.UserId;
        }
        else
            return Guid.Empty;
    }

    public static string GetTokenByUserId(Guid userId, TokenType tokentype)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<Current>(Current);
        var toGet = col.FindOne(x => x.UserId == userId && x.type == tokentype);

        if (toGet != null)
        {
            return toGet.Token;
        }
        else
            return string.Empty;
    }

    public static void DeleteCurrent(Guid userId, TokenType tokentype)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<Current>(Current);

        var toDel = col.FindOne(x => x.UserId == userId && x.type == tokentype);

        if (toDel != null)
        {
            col.Delete(toDel.UserId);
        }
    }

    public static void DeleteCurrentWithUserId(Guid userId)
    {
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<Current>(Current);

        var toDel = col.Find(x => x.UserId == userId);

        if (toDel != null)
        {
            foreach (var item in toDel)
            {
                col.Delete(item.UserId);
            }
        }
    }
    #endregion
}