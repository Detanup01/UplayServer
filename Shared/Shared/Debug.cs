using System.Runtime.CompilerServices;

namespace SharedLib.Shared
{
    public class Debug
    {
        public static bool isDebug = true;

        public static void PWDebug(object obj)
        {
            if (isDebug == true)
            {
                Console.WriteLine(obj.ToString());
                WriteDebug(obj.ToString());
            }
        }

        public static void PWDebugSafe(object obj)
        {
            if (isDebug == true)
            {
                Console.WriteLine(obj.ToString());
                WriteDebugAsnyc(obj.ToString());
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

        public static void WriteDebug(string strLog, [CallerMemberName] string memberName = "Shared", string logname = "debug.txt")
        {
            if (isDebug == true)
            {
                File.AppendAllText(logname, memberName + " | " + strLog + "\n");
            }
        }
        public static async void WriteDebugAsnyc(string strLog, [CallerMemberName] string memberName = "Shared", string logname = "debug.txt")
        {
            if (isDebug == true)
            {
                await File.AppendAllTextAsync(logname, memberName + " | " + strLog + "\n");
            }
        }
    }
}
