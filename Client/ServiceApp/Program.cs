using SharedLib.Shared;
using System.Runtime.InteropServices;

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

                if (Directory.Exists("logs"))
                    Directory.Delete("logs", true);

                if (File.Exists("UbiServices_Rest.txt"))
                    File.Delete("UbiServices_Rest.txt");

            }
            if (ParameterLib.HasParameter(args, "test"))
            {
                UPC_Init(0,0);


            }
        }

        [DllImport("upc_r2_loader64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "UPC_Init")]
        public static extern int UPC_Init(uint version, int appID);
    }
}