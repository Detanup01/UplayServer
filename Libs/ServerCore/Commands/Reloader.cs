using Core.DemuxResponders;
using SharedLib.Server.Json;

namespace Core.Commands
{
    public class Reloader
    {
        public static void ReloadAll(object obj)
        {
            Console.WriteLine("ReloadAll Started");
            ServerConfig.LoadConfig();
            Console.WriteLine("ReloadAll Finished");
        }

        public static void CleanServer(object obj)
        {
            Console.WriteLine("CleanServer Started");

            DemuxServer.SendToAllClient(new Uplay.Demux.Downstream()
            {
                Push = new()
                {
                    ConnectionClosed = new()
                    {
                        ConnectionId = 0,
                        ErrorCode = Uplay.Demux.ConnectionClosedPush.Types.Connection_ErrorCode.ConnectionForceQuit
                    }
                }
            });

            Console.WriteLine("CleanServer Finished");
        }

    }
}
