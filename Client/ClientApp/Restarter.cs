using System.Diagnostics;

namespace ClientApp
{
    internal class Restarter
    {
        public static void Restart(object obj)
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C timeout /T 3 && ServiceApp service_update";
            Info.FileName = "cmd.exe";
            Process.Start(Info);
            //Console.Clear();
            Environment.Exit(0);
        }
    }
}
