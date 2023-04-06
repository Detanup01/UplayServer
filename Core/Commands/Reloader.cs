using Core.DemuxResponsers;
using Core.JSON;


namespace Core.Commands
{
    public class Reloader
    {
        public static void ReloadAll()
        {
            Console.WriteLine("ReloadAll Started");
            //owners
            var cachefiles = Directory.GetFiles("ServerFiles/CacheFiles");
            foreach (var cachefile in cachefiles)
            {
                var file = cachefile.Replace("ServerFiles/CacheFiles\\", "");
                if (file.Contains(".ownershipcache.txt"))
                {
                    file = file.Replace(".ownershipcache.txt","");
                    Owners.MakeOwnershipFromTXT(file);
                }
            }








            Console.WriteLine("ReloadAll Finished");
        }

        public static void CleanServer()
        {
            Console.WriteLine("CleanServer Started");
            DemuxServer.ConnectedIds.ForEach(id =>
            {
                DemuxServer.SendToClient(id,
                    new()
                    {
                        Push = new()
                        {
                            ConnectionClosed = new()
                            {
                                ConnectionId = 0,
                                ErrorCode = Uplay.Demux.ConnectionClosedPush.Types.Connection_ErrorCode.ConnectionForceQuit
                            }
                        }
                    }
                    );
            });
            Console.WriteLine("CleanServer Finished");
        }

    }
}
