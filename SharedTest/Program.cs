namespace SharedTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            SharedLib.Server.DB.Prepare.MakeAll();
            Console.WriteLine(SharedLib.Server.DB.App.GetSpaceId("xxx"));
            SharedLib.Server.DB.App.AddFlags(234, new() { "xx", "dsfsfds" });
            SharedLib.Server.DB.App.EditFlags(234, new() { "xx", "dsfsfds" , "dfsfdsf" });
            Console.WriteLine("done!");
        }
    }
}