using SharedLib.Shared;

namespace ServiceApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var curDir = Environment.CurrentDirectory;
            if (ParameterLib.HasParameter(args, "service_update"))
            {
                var patchpath = Path.Combine(curDir, "patch");
                if (Directory.Exists(patchpath))
                {
                    var files = Directory.GetFiles(patchpath, "*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        var dir = Path.GetDirectoryName(file.Replace(patchpath + Path.DirectorySeparatorChar, ""));
                        if (dir != string.Empty)
                            Directory.CreateDirectory(dir);
                        File.Copy(file, curDir + file.Replace(patchpath, ""),true);
                    }
                }
            }

            if (ParameterLib.HasParameter(args, "service_cleanup"))
            {
                var patchpath = Path.Combine(curDir, "patch");
                if (Directory.Exists(patchpath))
                {
                    var files = Directory.GetFiles(patchpath, "*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                    Directory.Delete(patchpath,true);
                }
            }


            if (ParameterLib.HasParameter(args, "cleanup"))
            {
                if (Directory.Exists("SendReq"))
                    Directory.Delete("SendReq", true);

                if (Directory.Exists("SendUpstream"))
                    Directory.Delete("SendUpstream", true);

                if (File.Exists("debug.txt"))
                    File.Delete("debug.txt");

                if (File.Exists("DemuxSocket.log"))
                    File.Delete("DemuxSocket.log");

                if (File.Exists("Receive.txt"))
                    File.Delete("Receive.txt");

                if (File.Exists("ubiservice_rest.txt"))
                    File.Delete("ubiservice_rest.txt");

            }
        }
    }
}