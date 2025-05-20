using System.Runtime.InteropServices;

namespace DllLib;

public partial class LoadDll
{
    [LibraryImport("kernel32.dll", StringMarshalling = StringMarshalling.Utf8)]
    internal static partial IntPtr LoadLibrary(string dllToLoad);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool FreeLibrary(IntPtr hModule);

    static readonly Dictionary<string, IntPtr> FileToModule = [];

    public static string PluginPath = "dlls";

    public static void LoadPlugins()
    {
        if (!Directory.Exists(Path.Combine(PathHelper.CurrentPath, PluginPath)))
            return;
        var files = Directory.GetFiles(Path.Combine(PathHelper.CurrentPath, PluginPath), "*.dll");
        foreach (var file in files)
        {
            FileToModule.Add(file, LoadLibrary(file));
        }
    }

    public static void FreePlugins()
    {
        foreach (var file in FileToModule)
        {
            FreeLibrary(file.Value);
        }
        FileToModule.Clear();
    }
}
