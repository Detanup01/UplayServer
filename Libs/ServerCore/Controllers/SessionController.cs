namespace ServerCore.Controllers;

public class SessionController
{
    public static TokenType GetTokenTypeFromAuth(string Auth)
    {
        Auth = Auth.ToLower();
        if (Auth.Contains("t=userid|"))
            return TokenType.UserIdToken;
        if (Auth.Contains("basic"))
            return TokenType.BasicAuth;
        if (Auth.Contains("rm_v1"))
            return TokenType.RememberMe_v1;
        if (Auth.Contains("ubi_v1"))
            return TokenType.UbiV1;
        if (Auth.Contains("uplaypc_v1"))
            return TokenType.UplayPCV1;
        return TokenType.None;
    }

    public static string GetTokenStringFromAuth(string Auth)
    {
        string authLow = Auth.ToLower();
        if (authLow.Contains("t=userid|"))
            return Auth.Split("t=UserId|")[1];
        if (authLow.Contains("basic"))
            return Auth.Replace("Basic ", string.Empty);
        if (authLow.Contains("rm_v1"))
            return Auth.Replace("rm_v1 t=", string.Empty);
        if (authLow.Contains("ubi_v1"))
            return Auth.Replace("Ubi_v1 t=", string.Empty);
        if (authLow.Contains("uplaypc_v1"))
            return Auth.Replace("uplaypc_v1 t=", string.Empty);
        return Auth;
    }

    public static Guid GetUserFromAuth(string Auth)
    {
        TokenType type = GetTokenTypeFromAuth(Auth);
        string token = GetTokenStringFromAuth(Auth);
        if (type == TokenType.UserIdToken)
            return Guid.Parse(token);
        return DB.Auth.GetUserIdByToken(token, type);
    }
}
