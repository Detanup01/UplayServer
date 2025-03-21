using System.Runtime.InteropServices;

namespace upc_r1;

public partial class LoadPlugins
{
    [LibraryImport("kernel32.dll", StringMarshalling = StringMarshalling.Utf8)]
    internal static partial IntPtr LoadLibrary(string dllToLoad);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool FreeLibrary(IntPtr hModule);

    static readonly Dictionary<string, IntPtr> FileToModule = [];

    public static void LoadR1Plugins()
    {
        if (!Directory.Exists(Path.Combine(Basics.GetCuPath(), "r1")))
            return;
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
