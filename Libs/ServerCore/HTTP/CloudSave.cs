using Newtonsoft.Json;
using SharedLib.Server.DB;
using SharedLib.Server.Json;
using SharedLib.Server.Json.DB;

namespace ServerCore.HTTP
{
    internal class CloudSave
    {
        public static (string returnString, byte[] byteArray) GET(string URL, Dictionary<string, string> headers, out string contentType)
        {
            var nullbytes = new byte[0] { };
            contentType = "application/json; charset=UTF-8";
            var userId = headers["UserId"];
            if (URL.StartsWith("/"))
                URL = URL.Replace("/cloudsave/", "");
            else
                URL = URL.Replace("cloudsave/", "");

            var urlsplit = URL.Split("/");
            var uplayid = urlsplit[0];
            var itemOrAll = urlsplit[1];
            uint prodId = 0;
            if (!uint.TryParse(uplayid, out prodId))
            {
                return ("{\"error\":\"Error while parsing UplayID!\"}", nullbytes);
            }
            if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || DBUser.GetOwnershipBasic(userId).OwnedGamesIds.Contains(prodId))
            {
                if (itemOrAll.Contains("all"))
                {
                    var cloudsaves = DBUser.GetCloudSaves(userId, prodId);

                    if (cloudsaves == null)
                    {
                        cloudsaves = new();

                        DBUser.Add(new JCloudSave()
                        {
                            uplayId = prodId,
                            UserId = userId
                        });

                    }
                    return (JsonConvert.SerializeObject(cloudsaves), nullbytes);
                }
                else
                {
                    if (itemOrAll.Contains("savegame"))
                    {
                        var cloudsave = DBUser.GetCloudSave(userId, prodId, itemOrAll);
                        if (cloudsave != null)
                        {
                            var path = $"{ServerConfig.Instance.Demux.ServerFilesPath}/{cloudsave.UserId}/{cloudsave.uplayId}/{cloudsave.SaveName}";
                            if (Directory.Exists(Path.GetDirectoryName(path)))
                            {
                                if (File.Exists(path))
                                {
                                    contentType = "binary/octet-stream";
                                    var bytes = File.ReadAllBytes(path);
                                    return ("", SharedLib.Shared.DeComp.ZlibCompress(bytes));
                                }
                            }
                        }
                        else
                        {
                            return ("{\"error\":\"No savegame found!\"}", nullbytes);
                        }
                    }
                    else
                    {
                        if (!uint.TryParse(itemOrAll, out uint itemId))
                        {
                            return ("{\"error\":\"Error while parsing ItemId!\"}", nullbytes);
                        }
                        var cloudsave = DBUser.GetCloudSave(userId, prodId, itemId);
                        if (cloudsave != null)
                        {
                            var path = $"{ServerConfig.Instance.Demux.ServerFilesPath}/{cloudsave.UserId}/{cloudsave.uplayId}/{cloudsave.SaveName}";
                            if (Directory.Exists(Path.GetDirectoryName(path)))
                            {
                                if (File.Exists(path))
                                {
                                    contentType = "binary/octet-stream";
                                    var bytes = File.ReadAllBytes(path);
                                    return ("", SharedLib.Shared.DeComp.ZlibCompress(bytes));
                                }
                            }
                        }
                        else
                        {
                            return ("{\"error\":\"No savegame found!\"}", nullbytes);
                        }
                    }
                    return ("{\"error\":\"No path/file found!\"}", nullbytes);
                }
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
            if (URL.StartsWith("/"))
                URL = URL.Replace("/cloudsave/", "");
            else
                URL = URL.Replace("cloudsave/", "");

            var urlsplit = URL.Split("/");
            var uplayid = urlsplit[0];
            var itemOrName = urlsplit[1];
            uint prodId = 0;
            if (!uint.TryParse(uplayid, out prodId))
            {
                return "Error while parsing UplayID!";
            }
            if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || DBUser.GetOwnershipBasic(userId).OwnedGamesIds.Contains(prodId))
            {
                if (itemOrName.Contains("savegame"))
                {
                    var cloudsave = DBUser.GetCloudSave(userId, prodId, itemOrName);
                    if (cloudsave == null)
                    {
                        var jsave = new JCloudSave()
                        {
                            UserId = userId,
                            uplayId = prodId,
                            SaveId = 0,
                            SaveName = itemOrName
                        };
                        DBUser.Add(jsave);
                        cloudsave = jsave;
                    }
                    var path = $"{ServerConfig.Instance.Demux.ServerFilesPath}/{cloudsave.UserId}/{cloudsave.uplayId}/{cloudsave.SaveName}";
                    if (!Directory.Exists(Path.GetDirectoryName(path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                    }
                    File.WriteAllBytes(path, body);
                    return "OK!";
                }
                else
                {
                    if (!uint.TryParse(itemOrName, out uint itemId))
                    {
                        return "Error while parsing ItemId!";
                    }
                    var cloudsave = DBUser.GetCloudSave(userId, prodId, itemId);
                    if (cloudsave == null)
                    {
                        var jsave = new JCloudSave()
                        {
                            UserId = userId,
                            uplayId = prodId,
                            SaveId = itemId,
                            SaveName = itemId + ".savegame"
                        };
                        DBUser.Add(jsave);
                        cloudsave = jsave;
                    }
                    var path = $"{ServerConfig.Instance.Demux.ServerFilesPath}/{cloudsave.UserId}/{cloudsave.uplayId}/{cloudsave.SaveName}";
                    if (!Directory.Exists(Path.GetDirectoryName(path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                    }
                    File.WriteAllBytes(path, body);
                    return "OK!";

                }
            }
            return "User not owning the game!";
        }

        public static bool DELETE(string URL, Dictionary<string, string> headers, out string errorRSP)
        {
            errorRSP = "General Error";
            var userId = headers["UserId"];
            if (URL.StartsWith("/"))
                URL = URL.Replace("/cloudsave/", "");
            else
                URL = URL.Replace("cloudsave/", "");

            var urlsplit = URL.Split("/");
            var uplayid = urlsplit[0];
            var itemIdString = urlsplit[1];
            uint prodId = 0;
            if (!uint.TryParse(uplayid, out prodId))
            {
                errorRSP = "Error while parsing UplayID";
                return true;
            }
            if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || DBUser.GetOwnershipBasic(userId).OwnedGamesIds.Contains(prodId))
            {
                if (!uint.TryParse(itemIdString, out uint itemId))
                {
                    errorRSP =  "Error while parsing ItemId!";
                    return true;
                }
                var cloudsave = DBUser.GetCloudSave(userId, prodId, itemId);
                if (cloudsave != null)
                {
                    DBUser.DeleteCloudSave(userId, prodId, itemId);
                    var path = $"{ServerConfig.Instance.Demux.ServerFilesPath}/{cloudsave.UserId}/{cloudsave.uplayId}/{cloudsave.SaveName}";
                    if (Directory.Exists(Path.GetDirectoryName(path)) && File.Exists(path))
                    {
                        File.Delete(path);
                        return false;
                    }
                    return false;
                }
            }
            errorRSP = "User not owning the game!";
            return true;
        }
    }
}
