using System.Runtime.CompilerServices;

namespace SharedLib;

public static class Ex
{
    /// <summary>
    /// Handling Exception better, and logging out
    /// </summary>
    /// <param name="ex">The Exception</param>
    /// <param name="FancyExName">Your fancy name (Shared,Client,Core,etc)</param>
    /// <param name="caller">Do not use this param, only if you do nameof(MyFunc)</param>
    public static void Handler(this Exception ex, string FancyExName = "Shared", [CallerMemberName] string caller = "")
    {
        if (ex != null)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;

            string ToWrite = "";
            ToWrite += DateTime.UtcNow.ToString("yyyy.MM.dd hh:mm:ss") + " (UTC) | ";
            ToWrite += DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss") + " (LOCAL)";
            ToWrite += $"\nInner: {ex.InnerException}";
            ToWrite += $"\nStackTrace: {ex.StackTrace}";
            ToWrite += $"\nMessage: {ex.Message}";
            ToWrite += $"\nSource: {ex.Source}";
            ToWrite += $"\nHResult: {ex.HResult}";
            ToWrite += $"\nHelpLink: {ex.HelpLink}";

            File.WriteAllText($"ex_{FancyExName}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm_ss")}.txt", $"Caller: {caller}\n{ToWrite}");
        }
    }
}
