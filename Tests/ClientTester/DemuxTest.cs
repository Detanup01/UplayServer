using ClientKit.Demux;
using ClientKit.UbiServices.Records;
using ClientTester.DMX_Test;
using SharedLib.Shared;

namespace ClientTester
{
    internal class DemuxTest
    {
        static Socket socket;
        static LoginJson Login;
        static List<Action> actions = new();
        public static void Run(LoginJson login)
        {
            socket = new();
            socket.NewMessage += Socket_NewMessage;
            Login = login;
            actions.Add(SendVersion);
            actions.Add(VersionCheck);
            actions.Add(Auth);
            actions.Add(DoOwnership);
            actions.Add(VersionCheck);
            actions.ForEach(x => x() );
            socket.NewMessage -= Socket_NewMessage;
            socket.Disconnect();
            Console.WriteLine("DemuxTest Done!");
        }

        static void SendVersion()
        {
            socket.PushVersion();
            if (!socket.IsConnected)
            {
                Console.WriteLine("Socket is closed!");
            }
        }

        static void VersionCheck()
        {
            bool version = socket.VersionCheck();
            if (!version)
            {
                Console.WriteLine("Version is Not same!");
            }
        }

        static void Auth()
        {
            bool authed = socket.Authenticate(Login.Ticket);
            if (!authed)
            {
                Console.WriteLine("User is NOT authed!");
            }
        }

        static void DoOwnership()
        {
            OwnershipTest.Run(socket,new ClientKit.Demux.Connection.OwnershipConnection(socket));
        }
        private static void Socket_NewMessage(object? sender, DMXEventArgs e)
        {
            Console.WriteLine("Socket_NewMessage fired!");
            Debug.WriteDebug(e.ToString()!, "[Socket_NewMessage]");
        }
    }
}
