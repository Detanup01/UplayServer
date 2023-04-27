using System.Runtime.CompilerServices;

namespace SharedLib.Shared
{
    public class Debug
    {
        public static bool isDebug = true;

        public static void PWDebug(object obj, [CallerMemberName] string memberName = "Shared")
        {
            if (isDebug == true)
            {
                Console.WriteLine(obj.ToString());
                WriteDebug(obj.ToString(),"debug.txt", memberName);
            }
        }

        public static void PWDebug(object obj, string logname, [CallerMemberName] string memberName = "Shared")
        {
            if (isDebug == true)
            {
                Console.WriteLine(obj.ToString());
                WriteDebug(obj.ToString(), logname, memberName);
            }
        }

        public static void PWDebugSafe(object obj, [CallerMemberName] string memberName = "Shared")
        {
            if (isDebug == true)
            {
                Console.WriteLine(obj.ToString());
                WriteDebugAsnyc(obj.ToString(), "debug.txt", memberName);
            }
        }

        public static void PrintDebug(object obj)
        {
            if (isDebug == true)
            {
                Console.WriteLine(obj.ToString());
            }
        }

        public static Task PrintDebugAsync(object msg)
        {
            if (isDebug == true)
            {
                Console.WriteLine(msg.ToString());
            }
            return Task.CompletedTask;
        }

        public static void WriteDebug(string strLog, string logname = "debug.txt", [CallerMemberName] string memberName = "Shared")
        {
            if (isDebug == true)
            {
                File.AppendAllText(logname, memberName + " | " + strLog + "\n");
            }
        }
        public static async void WriteDebugAsnyc(string strLog, string logname = "debug.txt", [CallerMemberName] string memberName = "Shared")
        {
            if (isDebug == true)
            {
                await File.AppendAllTextAsync(logname, memberName + " | " + strLog + "\n");
            }
        }

        public static void WriteAllBytes(byte[] bytes, string logname = "_debug.txt")
        {
            if (isDebug == true)
            {
                File.WriteAllBytes(logname, bytes);
            }
        }
        public static void WriteAllText(string text, string logname = "_debug.txt")
        {
            if (isDebug == true)
            {
                File.WriteAllText(logname, text);
            }
        }
    }
}
