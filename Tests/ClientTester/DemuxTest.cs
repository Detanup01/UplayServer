using ClientKit.Demux;
using ClientKit.UbiServices.Records;

namespace ClientTester
{
    internal class DemuxTest
    {
        static Socket socket;
        static LoginJson Login;
        static List<Task> tasks = new List<Task>();
        public static async void Run(LoginJson login)
        {
            socket = new();
            Login = login;
            tasks.Add(SendVersion());
            tasks.Add(VersionCheck());
            tasks.Add(SendVersion());
            tasks.Add(SendVersion());
            tasks.Add(SendVersion());
            await Task.WhenAll(tasks);
            socket.Close();
            Console.WriteLine("DemuxTest Done!");
        }

        public static async Task SendVersion()
        {
            socket.PushVersion();
            if (socket.IsClosed)
            {
                Console.WriteLine("Socket is closed!");
            }
            return;
        }

        public static async Task VersionCheck()
        {
            bool version = socket.VersionCheck();
            if (!version)
            {
                Console.WriteLine("Version is Not same!");
            }
            return;
        }
    }
}
