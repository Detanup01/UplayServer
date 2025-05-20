namespace ServerCore.Commands;

public class CommandHandler
{
    public static Dictionary<string, Action<string[]>> Commands = new()
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

    public static void Nothing(string[] obj)
    {

    }

    public static void Help(string[] obj)
    {
        Console.WriteLine("Commands: " + string.Join(", ", Commands.Keys.ToList()));
    }

    public static void CalculateLogin(string[] obj)
    {
        if (obj.Length == 0)
        {
            Console.WriteLine("Use as: !calculatelogin {auth}\t(Auth must be email:password)");
            return;
        }
        var auth = obj[0];
        var toauth = Utils.MakeAuth(auth);
        Console.WriteLine($"Auth for {auth} is:\n{toauth}");
    }
}
