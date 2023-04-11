using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SharedLib.Shared
{
    public class Ex
    {
        /// <summary>
        /// Handling Exception better, and logging out
        /// </summary>
        /// <param name="ex">The Exception</param>
        /// <param name="FancyExName">Your fancy name (Shared,Client,Core,etc)</param>
        /// <param name="caller">Do not use this param, only if you do nameof(MyFunc)</param>
        public static void Handler(Exception ex, string FancyExName = "Shared", [CallerMemberName] string caller = "")
        {
            if (ex != null)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                foreach (var method in new StackTrace().GetFrames())
                {
                    if (method.GetMethod().Name == caller)
                    {
                        caller = $"{method.GetMethod().ReflectedType.Name}.{caller}";
                        break;
                    }
                }
                string ToWrite = "";
                ToWrite += DateTime.UtcNow.ToString("yyyy.MM.dd hh:mm:ss") + " (UTC) | ";
                ToWrite += DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss") + " (LOCAL)";
                ToWrite += $"\nInner: {ex.InnerException}";
                ToWrite += $"\nStackTrace: {ex.StackTrace}";
                ToWrite += $"\nMessage: {ex.Message}";
                ToWrite += $"\nSource: {ex.Source}";
                ToWrite += $"\nHResult: {ex.HResult}";
                ToWrite += $"\nHelpLink: {ex.HelpLink}";
                try
                {
                    ToWrite += $"\nTargetSite: {JsonConvert.SerializeObject(ex.TargetSite)}";
                    ToWrite += $"\nData: {JsonConvert.SerializeObject(ex.Data)}";
                }
                catch
                {
                    ToWrite += $"\nData: {JsonConvert.SerializeObject(ex.Data)}";
                }

                File.WriteAllText($"ex_{FancyExName}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm_ss")}.txt", $"Caller: {caller}\n{ToWrite}");
            }
        }
    }
}
