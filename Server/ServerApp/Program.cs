using Core;
using Core.Commands;
using SharedLib.Shared;

namespace Tets
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ubisoft Testing!");
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
            Debug.IsDebug = args.Contains("debug");
            CoreRun.Start();
            
            string endCheck = "not";
            while (endCheck.ToLower() != "exit")
            {
                endCheck = Console.ReadLine();
                if (endCheck.StartsWith("!"))
                {
                    CommandHandler.Run(endCheck);
                }
            }
            CoreRun.Stop();
            Console.ReadLine();
        }

    }
}