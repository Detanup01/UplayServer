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
                var log_files = Directory.GetFiles(Environment.CurrentDirectory, "*.log");
                foreach (var logfile in log_files)
                {
                    File.Delete(logfile);
                }
                Directory.Delete("Database", true);

            }
            Debug.isDebug = true;
            CoreRun.Start();
            File.Copy("user_auth.db", "Database/user_auth.db",true);
            /*
            //dGVzdEBnbWFpbC5jb206dGVzdA== is the login auth, test@gmail.com:test
            UserAuth.Add("00000000-0000-0000-0000-000000000000", "ZEdWemRFQm5iV0ZwYkM1amIyMDZkR1Z6ZEE9PV9DVVNUT01ERU1VWA==");
            Console.WriteLine("Login for 0User (00000000-0000-0000-0000-000000000000) \tdGVzdEBnbWFpbC5jb206dGVzdA== \t or \t test@gmail.com:test");


            var token = jwt.CreateAuthToken("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000");
            Add("00000000-0000-0000-0000-000000000000", token, (int)TokenType.Ticket);

            Owners.MakeOwnership("00000000-0000-0000-0000-000000000000", 0, new() { 0, 1, 2 }, new() { 0, 1, 2, 3, 4, 5 });
            */
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