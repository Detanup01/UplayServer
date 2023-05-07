using Core.DemuxResponders;
using SharedLib.Server.DB;
using SharedLib.Server.Json;

namespace Core.Commands
{
    public class Reloader
    {
        public static void ReloadAll(object obj)
        {
            Console.WriteLine("ReloadAll Started");
            //owners
            var cachefiles = Directory.GetFiles("ServerFiles/CacheFiles");
            foreach (var cachefile in cachefiles)
            {
                var file = cachefile.Replace("ServerFiles/CacheFiles\\", "");
                if (file.Contains(".ownershipcache.txt"))
                {
                    file = file.Replace(".ownershipcache.txt", "");
                    Owners.MakeOwnershipFromTXT(file);
                }
            }

            DBUser.Add(new SharedLib.Server.Json.DB.JUser() 
            { 
                Id = 0,
                UserId = "00000000-0000-0000-0000-000000000000",
                Friends = new(),
                Name = "0USER"
            });

            DBUser.Add(new SharedLib.Server.Json.DB.JOwnershipBasic()
            {
                Id = 0,
                UserId = "00000000-0000-0000-0000-000000000000",
                OwnedGamesIds = new() { 0 },
                UbiPlus = 0
            });

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
