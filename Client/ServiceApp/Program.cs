using SharedLib.Shared;

namespace ServiceApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (ParameterLib.HasParameter(args, "service_update"))
            {
                var curDir = Environment.CurrentDirectory;
                var files = Directory.GetFiles(Path.Combine(curDir,"patch"));
                foreach (var file in files)
                {
                    Console.WriteLine(file);
                }
            }
            if (ParameterLib.HasParameter(args, "service_cleanup"))
            {

            }
        }
    }
}