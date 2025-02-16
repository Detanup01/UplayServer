using System.Runtime.InteropServices;

namespace upc_r1;

public class LoadPlugins
{
    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string dllToLoad);

    [DllImport("kernel32.dll")]
    public static extern bool FreeLibrary(IntPtr hModule);

    static Dictionary<string, IntPtr> FileToModule = [];

    public static void LoadR1Plugins()
    {
        var files = Directory.GetFiles(Path.Combine(Basics.GetCuPath(), "r1"), "*.dll");
        foreach (var file in files)
        {
            FileToModule.Add(file, LoadLibrary(file));
        }
    }

    public static void FreeR1Plugins()
    {
        foreach (var file in FileToModule)
        {
            FreeLibrary(file.Value);
        }
        FileToModule.Clear();
    }
}
