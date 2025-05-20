using System.Diagnostics;

namespace DllLib;

public static class Logs
{
    public static bool IsEnabled;
    public static string LogName = "dlllib.log";
    public static void Log(string actionName, params object[] parameters)
    {
        if (!IsEnabled)
            return;
        File.AppendAllText(Path.Combine(PathHelper.CurrentPath, LogName), $"{Process.GetCurrentProcess().Id} | {actionName} {string.Join(", ", parameters)}\n");
    }
}
