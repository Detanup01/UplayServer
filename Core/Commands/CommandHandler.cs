using SharedLib.Server.Json;
using SharedLib.Shared;
using System.Security.Cryptography;
using System.Text;

namespace Core.Commands
{
    public class CommandHandler
    {
        public static Dictionary<string, Action<object>> Commands = new()
        {
            { "reload" , Reloader.ReloadAll },
            { "cleanserver" , Reloader.CleanServer },
            { "help" , Help },
            { "calculatelogin" , CalculateLogin }

        };

        public static void Run(string CommandName)
        {
            var splitted = CommandName.Split(" ");
            CommandName = splitted[0];
            var Parameter = splitted[1..];
            Console.WriteLine(string.Join(",", Parameter));
            if (Commands.TryGetValue(CommandName.Replace("!", ""), out var action))
            {
                action(Parameter);
            }
        }

        public static void Nothing(object obj)
        {

        }

        public static void Help(object obj)
        {
            Console.WriteLine(string.Join(", ", Commands.Keys.ToList()));
        }

        public static void CalculateLogin(object obj)
        {
            var args = (string[])obj;
            var auth = args[0];
            var toauth = Utils.MakeAuth(auth);
            Console.WriteLine($"Auth for {auth} is:\n{toauth}");
        }
    }
}
