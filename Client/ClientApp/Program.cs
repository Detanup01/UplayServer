using Client.Patch;
using ClientApp.PipeConnection;
using UplayKit;
using UplayKit.Connection;
using UbiServices.Public;
using Google.Protobuf;
using SharedLib.Shared;
using Newtonsoft.Json;

namespace ClientApp;

internal class Program
{
    public static DemuxSocket? Socket;
    public static PipeServer? pipeServer;

    static void Main(string[] args)
    {
        Debug.IsDebug = true;
        pipeServer = new PipeServer();
        pipeServer.Readed += PipeServer_Readed;
        var pipeThread = new Thread(pipeServer.Start);
        pipeThread.Start();
        var reg = V3.CreateAccount("testuser", "testuser", "2000-01-01", "testuser", "EU", "US", "-");
        if (reg != null)
        {
            var UserID = reg;
            Console.WriteLine(JsonConvert.SerializeObject(UserID));
        }
        var login = V3.Login("testuser", "testuser");
        if (login != null)
        {
            var ticket = login.Ticket;
            Console.WriteLine(ticket);
            Socket = new();
            var patch = Socket.GetPatch()!;

            if (patch.LatestVersion != DemuxSocket.ClientVersion)
            {
                Console.WriteLine("Your client is outdated!\nDo you wanna update?");
                Console.WriteLine("\nY/y = Yes | N/n = No (If you dont say it just exit)");

                var choice = Console.ReadLine()!;
                switch (choice.ToLower())
                {
                    case "y":
                        Patcher.MainPatch(patch);
                        Restarter.Restart("");
                        break;
                    default:
                        break;
                }

            }
            else
            {
                Console.WriteLine("Your Client is up-to-date!");
            }
            Socket.PushVersion();
            Console.WriteLine(Socket.Authenticate(ticket));


            if (args.Contains("install") || args.Contains("download"))
            {
                args = [.. args, "-ticket", ticket];
            }
            else
            {
                OwnershipConnection ownershipConnection = new(Socket, login.Ticket, login.SessionId);
                var x = ownershipConnection.Initialize();
                var sig = x!.OwnedGamesContainer.Signature.ToBase64();
#pragma warning disable CS0618 // Type or member is obsolete
                var tmp = ownershipConnection.RegisterTempOwnershipToken(sig);
#pragma warning restore CS0618 // Type or member is obsolete
                Console.WriteLine(tmp!.ProductIds.Count);
            }

            Console.ReadLine();
            Socket.DisconnectAsync();
            Socket.Dispose();
            pipeServer.Readed -= PipeServer_Readed;
            pipeServer.Stop();
            pipeThread.Join(TimeSpan.FromSeconds(1));
            Console.WriteLine("END!");
            Environment.Exit(0);
        }

        /*
        var x = callbacktest.getcontext();
        Console.WriteLine("yey context! " + x);
        callbacktest.updatecontext(x);
        Console.WriteLine("update! " +x);
        callbacktest.Use(x);
        Console.WriteLine("use! " + x);
        callbacktest.updatecontext(x);
        Console.WriteLine("update! " + x);
        callbacktest.freecontext(x);
        Console.WriteLine("free! " + x);*/
        /*
        var reg = V3.Register("slejm","slejm","slejm");
        if (reg != null)
        {
            var UserID = reg.UserId;

        }
       

        var login = V3.Login("slejm", "slejm");
        if (login != null)
        {
            var ticket = login.Ticket;
            Debug.isDebug = true;
            Socket socket = new();
            socket.PushVersion();
            socket.VersionCheck();
            Console.WriteLine(socket.Authenticate(ticket));

            OwnershipConnection ow = new(socket);
            var games = ow.GetOwnedGames();
            if (games != null)
            {
                foreach (var x in games)
                {
                    Console.WriteLine(x.ToString());

                }

            }




            Console.ReadLine();

            socket.Close();
        }*/
    }

    private static void PipeServer_Readed(object? sender, byte[] e)
    {
        var req = SharedLib.Shared.Formatters.FormatDataNoLength<Uplay.Demux.Upstream>(e);
        var rsp = Socket!.SendUpstream(req!);
        pipeServer!.Send(SharedLib.Shared.Formatters.FormatUpstream(rsp.ToByteArray()));
    }
}