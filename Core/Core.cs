using Core.DemuxResponders;
using Core.Extra;
using Core.SQLite;

namespace Core
{
    public class CoreRun
    {
        public static DemuxServer server = null;
        public static void Start(bool IsLocal = true)
        {
            Preparing.MakeAll();
            jwt.CreateRSA();
            PluginHandle.LoadPlugins();
            HTTPServer.Start();
            Console.WriteLine(Config.HTTPSUrl);
            server = new(IsLocal);
            Console.WriteLine(Config.DemuxUrl);
        }

        public static void Stop()
        {
            PluginHandle.UnloadPlugins();
            server.Close();
            HTTPServer.Stop();
        }
    }
}
