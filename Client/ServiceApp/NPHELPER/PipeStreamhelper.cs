using System.IO.Pipes;

namespace ServiceApp.NPHELPER
{
    internal class PipeStreamhelper
    {
        public static void Starter()
        {
            PipeNames.Add("\\terminal_1_uplay_service_ipc_pipe_");
            PipeNames.Add("\\terminal_1_uplay_protocol_ipc_pipe_");
            PipeNames.Add("\\terminal_1_uplay_overlay_ipc_pipe_");
            PipeNames.Add("\\terminal_1_uplay_ipc_pipe_");
            PipeNames.Add("\\terminal_1_uplay_crash_reporter_ipc_pipe_");
            PipeNames.Add("\\terminal_1_uplay_aux_ipc_pipe_");
            PipeNames.Add("\\terminal_1_uplay_api_process_ipc_pipe_");
            PipeNames.Add("\\terminal_1_orbit_ipc_pipe_");
            PipeNames.Add("\\terminal_1_game_start_ipc_pipe_");
            foreach (string name in PipeNames)
            {
                NamedPipeServer longRunning = new NamedPipeServer(name);
                NameKill.Add(name, longRunning);
                new Thread(new ThreadStart(longRunning.ExecuteLongRunningTask)).Start();
            }
            Console.ReadLine();
            Console.WriteLine("After this we quit!");
            Console.ReadLine();
            PipeNames.ForEach(delegate (string x)
            {
                NamedServerPipeStop(x);
            });
        }

        private static void NamedServerPipeStop(string pipename)
        {
            NamedPipeServer? pipeServer;
            if (NameKill.TryGetValue(pipename, out pipeServer))
            {
                pipeServer.Cancel = true;
            }
            Console.WriteLine("[Server | " + pipename + "] Disconnect!");
        }
        public static Dictionary<string, NamedPipeServerStream> NameServer = new Dictionary<string, NamedPipeServerStream>();

        // Token: 0x04000004 RID: 4
        public static Dictionary<string, NamedPipeClientStream> NameClient = new Dictionary<string, NamedPipeClientStream>();

        // Token: 0x04000005 RID: 5
        public static Dictionary<string, bool> NameServerBool = new Dictionary<string, bool>();

        // Token: 0x04000006 RID: 6
        public static Dictionary<string, bool> NameClientBool = new Dictionary<string, bool>();

        // Token: 0x04000007 RID: 7
        public static Dictionary<string, NamedPipeServer> NameKill = new Dictionary<string, NamedPipeServer>();

        // Token: 0x04000008 RID: 8
        public static List<string> PipeNames = new List<string>();
    }
}
