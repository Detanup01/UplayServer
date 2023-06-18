using Core.DemuxResponders;
using Core.Extra;
using SharedLib.Server.DB;
using SharedLib.Server.Json;

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
            Console.WriteLine(ServerConfig.Instance.HTTPS_Url + ServerConfig.Instance.HTTPS_Port);
            DemuxServer.Start();
            Console.WriteLine(ServerConfig.Instance.DemuxUrl + ServerConfig.Instance.DemuxPort);
        }

        public static void Stop()
        {
            PluginHandle.UnloadPlugins();
            DemuxServer.Stop();
            HTTPServer.Stop();
        }
    }
}
