using Core.DemuxResponders;
using Core.Extra;
using ServerCore.DB;
using ServerCore.Json;

namespace Core
{
    public class CoreRun
    {
        public static void Start()
        {
            Prepare.MakeAll();
            jwt.CreateRSA();
            PluginHandle.LoadPlugins();
            HTTPServer.Start();
            Console.WriteLine(ServerConfig.Instance.HTTPS_Url);
            DemuxServer.Start();
            Console.WriteLine(ServerConfig.Instance.DemuxUrl);
        }

        public static void Stop()
        {
            PluginHandle.UnloadPlugins();
            DemuxServer.Stop();
            HTTPServer.Stop();
        }
    }
}
