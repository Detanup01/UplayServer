using ServerCore.DB;
using ServerCore.Json;

namespace Core.Commands
{
    public class CommandHandler
    {
        public static Dictionary<string, Action<object>> Commands = new()
        {            
            { "help" , Help },
            { "reload" , Reloader.ReloadAll },
            { "cleanserver" , Reloader.CleanServer },
            { "calculatelogin" , CalculateLogin },
            { "generatecdkey" , Generator.GenerateCDKey }
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
            Console.WriteLine("Commands: " + string.Join(", ", Commands.Keys.ToList()));
        }

        public static void CalculateLogin(object obj)
        {
            var args = (string[])obj;
            if (args.Length == 0)
            {
                Console.WriteLine("Use as: !calculatelogin {auth}\t(Auth must be email:password)");
                return;
            }
            var auth = args[0];
            var toauth = Utils.MakeAuth(auth);
            Console.WriteLine($"Auth for {auth} is:\n{toauth}");
        }
    }
}
