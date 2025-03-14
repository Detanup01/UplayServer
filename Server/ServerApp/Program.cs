using ModdableWebServer.Helper;
using ServerCore;
using ServerCore.Commands;
using SharedLib;

namespace ServerApp;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Contains("clean"))
        {
            Console.WriteLine("cleaned!");
            var log_files = Directory.GetFiles(Environment.CurrentDirectory, "*.log", SearchOption.AllDirectories);
            foreach (var logfile in log_files)
            {
                File.Delete(logfile);
            }
            Directory.Delete("Database", true);
        }
        if (args.Contains("debug"))
        {
            DebugPrinter.EnableLogs = true;
            DebugPrinter.PrintToConsole = true;
        }
        

        CoreRun.Start();
        
        string endCheck = "not";
        while (!endCheck.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
        {
            endCheck = Console.ReadLine()!;
            if (endCheck.StartsWith('!'))
            {
                CommandHandler.Run(endCheck);
            }
        }
        CoreRun.Stop();
        Console.ReadLine();
    }

}