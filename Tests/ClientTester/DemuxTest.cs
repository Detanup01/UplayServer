using ClientKit.Demux;
using ClientKit.UbiServices.Records;

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
            Login = login;
            actions.Add(SendVersion);
            actions.Add(VersionCheck);
            actions.ForEach(x => x() );
            socket.Close();
            Console.WriteLine("DemuxTest Done!");
        }

        static void SendVersion()
        {
            socket.PushVersion();
            if (socket.IsClosed)
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
    }
}
