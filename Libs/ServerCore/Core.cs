using ServerCore.DemuxResponders;
using ServerCore.Extra;
using ServerCore.DB;
using ServerCore.Models;
using ServerCore.HTTP;

namespace ServerCore;

public class CoreRun
{
    public static void Start()
    {
        Prepare.MakeAll();
        JWTController.CreateRSA();
        PluginHandle.LoadPlugins();
        ServerManager.Start();
        Console.WriteLine(ServerConfig.Instance.HTTPS_Url);
        DemuxServer.Start();
        Console.WriteLine(ServerConfig.Instance.DemuxUrl);
    }

    public static void Stop()
    {
        PluginHandle.UnloadPlugins();
        DemuxServer.Stop();
        ServerManager.Stop();
    }
}
