using System.Runtime.InteropServices;

namespace upc_r2;

public partial class LoadPlugins
{
    [LibraryImport("kernel32.dll", StringMarshalling = StringMarshalling.Utf8)]
    internal static partial IntPtr LoadLibrary(string dllToLoad);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool FreeLibrary(IntPtr hModule);

    static readonly Dictionary<string, IntPtr> FileToModule = [];

    public static void LoadR2Plugins()
    {
        var files = Directory.GetFiles(Path.Combine(Basics.GetCuPath(), "r2"), "*.dll");
        foreach (var file in files)
        {
            FileToModule.Add(file, LoadLibrary(file));
        }
    }

    public static void FreeR2Plugins()
    {
        foreach (var file in FileToModule)
        {
            FreeLibrary(file.Value);
        }
        FileToModule.Clear();
    }
}
