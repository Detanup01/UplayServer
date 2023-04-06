namespace Core.Commands
{
    public class CommandHandler
    {
        public static Dictionary<string, Action> Commands = new()
        {
            { "reload" , Reloader.ReloadAll },
            { "cleanserver" , Reloader.CleanServer }
        
        
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
    }
}
