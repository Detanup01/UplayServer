using LiteDB;
using ServerCore.Models.User;
using System.Linq.Expressions;

namespace ServerCore.DB;

public class DBUser
{
    readonly static string DBName = Path.Combine(Prepare.DatabasePath, "User.db");
    readonly static string USER = "User";
    readonly static string OwnershipBasic = "OwnershipBasic";
    readonly static string Ownership = "Ownership";
    readonly static string Activity = "Activity";
    readonly static string Friend = "Friends";
    readonly static string Playtime = "Playtime";
    readonly static string GameSession = "GameSession";
    readonly static string CloudSave = "CloudSave";
    readonly static string Login = "Login";

    #region JUser
    public static void Add<T>(T user) where T : UserBase
    {
        string collectionName = GetCollectionName<T>();
        if (string.IsNullOrEmpty(collectionName))
            return;
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<T>(collectionName);

        if (!col.Exists(X => X.UserId == user.UserId))
            col.Insert(user);
    }

    public static void Edit<T>(T user) where T : UserBase
    {
        string collectionName = GetCollectionName<T>();
        if (string.IsNullOrEmpty(collectionName))
            return;
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<T>(collectionName);

        var fId = col.FindOne(X => X.UserId == user.UserId);
        if (fId == null)
            return;
        user.UserId = fId.UserId;
        col.Update(user);
    }
    public static T? Get<T>(Guid UserId) where T : UserBase
    {
        string collectionName = GetCollectionName<T>();
        if (string.IsNullOrEmpty(collectionName))
            return default;
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<T>(collectionName);

        return col.FindOne(X => X.UserId == UserId);
    }

    public static T? Get<T>(Expression<Func<T, bool>> predicate) where T : UserBase
    {
        string collectionName = GetCollectionName<T>();
        if (string.IsNullOrEmpty(collectionName))
            return default;
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<T>(collectionName);

        return col.FindOne(predicate);
    }

    public static T? Get<T>(Guid UserId, Func<T, bool> expression) where T : UserBase
    {
        string collectionName = GetCollectionName<T>();
        if (string.IsNullOrEmpty(collectionName))
            return default;
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<T>(collectionName);

        return col.FindOne(X => X.UserId == UserId && expression.Invoke(X));
    }

    public static T? Get<T>(string UserId) where T : UserBase
    {
        return Get<T>(Guid.Parse(UserId));
    }

    public static T? Get<T>(string UserId, Func<T, bool> expression) where T : UserBase
    {
        return Get<T>(Guid.Parse(UserId), expression);
    }

    public static List<T> GetList<T>(Guid UserId) where T : UserBase
    {
        string collectionName = GetCollectionName<T>();
        if (string.IsNullOrEmpty(collectionName))
            return [];
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<T>(collectionName);

        return col.Find(x => x.UserId == UserId).ToList();
    }

    public static List<T> GetList<T>(Guid UserId, Func<T, bool> expression) where T : UserBase
    {
        string collectionName = GetCollectionName<T>();
        if (string.IsNullOrEmpty(collectionName))
            return [];
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<T>(collectionName);

        return col.Find(x => x.UserId == UserId && expression.Invoke(x)).ToList();
    }

    public static void Delete<T>(Guid UserId, Func<T, bool> expression) where T : UserBase
    {
        string collectionName = GetCollectionName<T>();
        if (string.IsNullOrEmpty(collectionName))
            return;
        using var db = new LiteDatabase(DBName);
        var col = db.GetCollection<T>(collectionName);

        col.DeleteMany(X => X.UserId == UserId && expression.Invoke(X));
    }

    public static string GetCollectionName<T>() where T : UserBase
    {
        switch (typeof(T).Name)
        {
            case "UserCommon":
                return USER;
            case "UserActivity":
                return Activity;
            case "UserOwnershipBasic":
                return OwnershipBasic;
            case "UserOwnership":
                return Ownership;
            case "UserFriend":
                return Friend;
            case "UserPlaytime":
                return Playtime;
            case "UserGameSession":
                return GameSession;
            case "UserCloudSave":
                return CloudSave;
            case "UserLogin":
                return Login;
            default:
                return string.Empty;
        }
    }
    #endregion
}