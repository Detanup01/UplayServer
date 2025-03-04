using ServerCore.Controllers;

namespace ServerCore.HTTP;

internal class CloudSave
{
    public static (string returnString, byte[] byteArray) GET(string URL, Dictionary<string, string> headers, out string contentType)
    {
        var nullbytes = Array.Empty<byte>();
        contentType = "application/json; charset=UTF-8";
        //var userId = Guid.Parse(headers["UserId"]);
        URL = URL.StartsWith('/') ? URL.Replace("/cloudsave/", "") : URL.Replace("cloudsave/", "");

        var urlsplit = URL.Split("/");
        var uplayid = urlsplit[0];
        var itemOrAll = urlsplit[1];
        if (!uint.TryParse(uplayid, out uint prodId))
        {
            return ("{\"error\":\"Error while parsing UplayID!\"}", nullbytes);
        }
        else
        {
            return ("{\"error\":\"User doesnt own the game!\"}", nullbytes);
        }
    }

    public static string PUT(string URL, Dictionary<string, string> headers, byte[] body, out string contentType)
    {
        contentType = "text/plain; charset=utf-8";
        var userId = headers["UserId"];
        URL = URL.StartsWith('/') ? URL.Replace("/cloudsave/", "") : URL.Replace("cloudsave/", "");

        var urlsplit = URL.Split("/");
        var uplayid = urlsplit[0];
        var itemOrName = urlsplit[1];
        if (!uint.TryParse(uplayid, out uint prodId))
        {
            return "Error while parsing UplayID!";
        }
        var IsSuccess = CloudSaveController.PutSave(Guid.Parse(userId), itemOrName, prodId, body);
        return "User not owning the game!";
    }

    public static bool DELETE(string URL, Dictionary<string, string> headers, out string errorRSP)
    {
        errorRSP = "General Error";
        var userId = headers["UserId"];
        URL = URL.StartsWith('/') ? URL.Replace("/cloudsave/", "") : URL.Replace("cloudsave/", "");

        var urlsplit = URL.Split("/");
        var uplayid = urlsplit[0];
        var itemIdString = urlsplit[1];
        if (!uint.TryParse(uplayid, out uint prodId))
        {
            errorRSP = "Error while parsing UplayID";
            return true;
        }
        if (!uint.TryParse(itemIdString, out uint itemId))
        {
            errorRSP = "Error while parsing ItemId!";
            return true;
        }
        bool IsSuccess = CloudSaveController.DeleteSave(Guid.Parse(userId), itemId, prodId);
        if (!IsSuccess)
        {
            errorRSP = "User not owning the game!";
            return true;
        }
        errorRSP = "Success!";
        return true;
    }
}
