using Newtonsoft.Json;
using ServerCore.DB;
using ServerCore.Models;
using ServerCore.Models.User;
using System.Text;

namespace ServerCore.Controllers;

public class CloudSaveController
{

    public static byte[] GetSave(Guid UserId, string itemOrName, uint productId, out bool UseBinaryOctetStream)
    {
        UseBinaryOctetStream = false;
        byte[] emptyBytes = [];
        if (!OwnershipController.IsOwned(UserId, productId))
            return emptyBytes;
        if (itemOrName.Contains("all"))
        {
            var cloudsaves = DBUser.GetList<UserCloudSave>(UserId, x => x.UplayId == productId);

            if (cloudsaves == null)
            {
                cloudsaves = new();

                DBUser.Add(new UserCloudSave()
                {
                    UplayId = productId,
                    UserId = UserId
                });

            }
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cloudsaves));
        }
        else
        {
            var cloudSave = GetCloudSave(UserId, itemOrName, productId);
            if (cloudSave == null)
                return emptyBytes;
            var path = $"{ServerConfig.Instance.Demux.ServerFilesPath}/{cloudSave.UserId}/{cloudSave.UplayId}/{cloudSave.SaveName}";
            emptyBytes = File.ReadAllBytes(path);
            UseBinaryOctetStream = true;
            return emptyBytes;
        }
    }

    public static bool PutSave(Guid UserId, string itemOrName, uint productId, byte[] body)
    {
        if (!OwnershipController.IsOwned(UserId, productId))
            return false;
        var cloudSave = GetCloudSave(UserId, itemOrName, productId);
        if (cloudSave == null)
            return false;
        var path = $"{ServerConfig.Instance.Demux.ServerFilesPath}/{cloudSave.UserId}/{cloudSave.UplayId}/{cloudSave.SaveName}";
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        }
        File.WriteAllBytes(path, body);
        return true;
    }

    public static bool DeleteSave(Guid UserId, uint itemId, uint productId)
    {
        if (!OwnershipController.IsOwned(UserId, productId))
            return false;
        var cloudsave = DBUser.Get<UserCloudSave>(UserId, x => x.SaveId == itemId && x.UplayId == productId);
        if (cloudsave == null)
            return false;
        DBUser.Delete<UserCloudSave>(UserId, x => x.SaveId == itemId && x.UplayId == productId);
        var path = $"{ServerConfig.Instance.Demux.ServerFilesPath}/{cloudsave.UserId}/{cloudsave.UplayId}/{cloudsave.SaveName}";
        if (Directory.Exists(Path.GetDirectoryName(path)) && File.Exists(path))
        {
            File.Delete(path);
            return true;
        }
        return false;
    }

    public static UserCloudSave? GetCloudSave(Guid UserId, string itemOrName, uint productId)
    {
        UserCloudSave? cloudsave = null;
        if (itemOrName.Contains("savegame"))
        {
            cloudsave = DBUser.Get<UserCloudSave>(UserId, x => x.SaveName == itemOrName && x.UplayId == productId);
            if (cloudsave == null)
            {
                cloudsave = new UserCloudSave()
                {
                    UserId = UserId,
                    UplayId = productId,
                    SaveId = 0,
                    SaveName = itemOrName
                };
                DBUser.Add(cloudsave);
            }
        }
        else
        {
            if (!uint.TryParse(itemOrName, out uint itemId))
            {
                return cloudsave;
            }
            cloudsave = DBUser.Get<UserCloudSave>(UserId, x => x.SaveId == itemId && x.UplayId == productId);
            if (cloudsave == null)
            {
                cloudsave = new UserCloudSave()
                {
                    UserId = UserId,
                    UplayId = productId,
                    SaveId = itemId,
                    SaveName = itemId + ".savegame"
                };
                DBUser.Add(cloudsave);
            }
        }
        return cloudsave;
    }
}
