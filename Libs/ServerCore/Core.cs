using ServerCore.Controllers;
using ServerCore.DB;
using ServerCore.Extra;
using ServerCore.Models;

namespace ServerCore;

public class CoreRun
{
    public static void Start()
    {
        Prepare.MakeAll();
        JWTController.CreateRSA();
        PluginHandle.LoadPlugins();
        ServerController.Start();
        DemuxController.Start();
        Console.WriteLine(ServerConfig.Instance.DemuxUrl);
        Console.WriteLine(ServerConfig.Instance.HTTPS_Url);
    }

    public static void Stop()
    {
        PluginHandle.UnloadPlugins();
        DemuxController.Stop();
        ServerController.Stop();
    }
}
