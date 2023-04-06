namespace Core.Commands
{
    public class CommandHandler
    {
        public static Dictionary<string, Action> Commands = new()
        {
            { "reload" , Reloader.ReloadAll },
            { "cleanserver" , Reloader.CleanServer },
            { "help" , Help }

        };

        public static void Run(string CommandName)
        {
            if (Commands.TryGetValue(CommandName.Replace("!",""), out var action))
            {
                action();
            }
        }

        public static void Nothing()
        {

        }

        public static void Help()
        {
            Console.WriteLine(string.Join(", ", Commands.Keys.ToList()));
        }
    }
}
