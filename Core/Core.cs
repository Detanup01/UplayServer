using Core.DemuxResponders;
using Core.Extra;
using SharedLib.Server.DB;
using SharedLib.Server.Json;

namespace Core
{
    public class CoreRun
    {
        public static DemuxServer server = null;
        public static void Start(bool IsLocal = true)
        {
            Prepare.MakeAll();
            jwt.CreateRSA();
            PluginHandle.LoadPlugins();
            HTTPServer.Start();
            Console.WriteLine(ServerConfig.HTTPSUrl);
            server = new(IsLocal);
            Console.WriteLine(ServerConfig.DemuxUrl);
        }

        public static void Stop()
        {
            PluginHandle.UnloadPlugins();
            server.Close();
            HTTPServer.Stop();
        }
    }
}
