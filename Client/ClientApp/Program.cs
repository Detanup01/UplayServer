using Client.Patch;
using ClientApp;
using ClientApp.PipeConnection;
using ClientKit.Demux;
using ClientKit.Demux.Connection;
using ClientKit.UbiServices.Public;
using ClientKit.UbiServices.Records;
using Google.Protobuf;
using Newtonsoft.Json;
using RestSharp;
using SharedLib.Shared;
using System.IO.Pipes;
using System.Reflection.Metadata;

namespace Client
{
    internal class Program
    {
        public static Socket Socket;
        public static PipeServer pipeServer;

        static void Main(string[] args)
        {
            Debug.IsDebug = true;
            /*
            var client = new RestClient("https://local-ubiservices.ubi.com:7777/store/?p=0");
            var request = new RestRequest();

            request.AddHeader("authorization", $"ubi_v1 t=VDF6UWlacDJvTDdCOE5IcXR3bHNYRE8yeDlvWjkvYzh6M083R0hER0hGaz0=");

            var rsp = ClientKit.UbiServices.Rest.GetString(client,request);

            File.WriteAllText("resp", rsp);
            */
            pipeServer = new PipeServer();
            pipeServer.Readed += PipeServer_Readed;
            var pipeThread = new Thread(pipeServer.Start);
            pipeThread.Start();
            var reg = V3.Register("testuser", "testuser", "testuser");
            if (reg != null)
            {
                var UserID = reg.UserId;
                Console.WriteLine(UserID);
            }
            var login = V3.Login("testuser", "testuser");
            if (login != null)
            {
                var ticket = login.Ticket;
                Console.WriteLine(ticket);
                Socket = new();
                var patch = Socket.GetPatch();

                if (patch.LatestVersion != Socket.ClientVersion)
                {
                    Console.WriteLine("Your client is outdated!\nDo you wanna update?");
                    Console.WriteLine("\nY/y = Yes | N/n = No (If you dont say it just exit)");

                    var choice = Console.ReadLine();
                    switch (choice.ToLower())
                    {
                        case "y":
                            Patcher.Main(patch);
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
                Console.WriteLine(Socket.IsAuthed);


                if (args.Contains("install") || args.Contains("download"))
                {
                    args = args.Append("-ticket").Append(ticket).ToArray();
                    Downloader.Program.Main(args, Socket);
                }
                else
                {
                    OwnershipConnection ownershipConnection = new(Socket);
                    var x = ownershipConnection.Initialize(new List<Uplay.Ownership.InitializeReq.Types.ProductBranchData>() { });
                    var sig = x.OwnedGamesContainer.Signature.ToBase64();
                    var tmp = ownershipConnection.RegisterTempOwnershipToken(sig);
                    Console.WriteLine(tmp.ProductIds.Count);
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
            var req = Formatters.FormatDataNoLength<Uplay.Demux.Upstream>(e);
            var rsp = Socket.SendUpstream(req);
            pipeServer.Send(Formatters.FormatUpstream(rsp.ToByteArray()));
        }
    }
}