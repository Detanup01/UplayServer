using System.Runtime.CompilerServices;
using LLibrary;

namespace SharedLib.Shared
{
    public class Debug
    {
        public static L logger = new(true);

        public static bool isDebug = true;

        public static void PWDebug(object obj, string label = "INFO", [CallerMemberName] string memberName = "Shared")
        {
            if (isDebug == true)
            {
                Console.WriteLine($"[{label}] {obj}");
                logger.Log(label, obj.ToString() + " | " + memberName);
            }
        }

        public static void PrintDebug(object obj)
        {
            if (isDebug == true)
            {
                Console.WriteLine(obj.ToString());
            }
        }

        public static void WriteDebug(string strLog, string label = "debug", [CallerMemberName] string memberName = "Shared")
        {
            if (isDebug == true)
            {
                logger.Log(label, strLog + " | " + memberName);
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

        public static void AppendAllText(string text, string logname = "_debug.txt")
        {
            if (isDebug == true)
            {
                try
                {
                    File.AppendAllText(logname, text + "\n");
                }
                catch
                {
                    PWDebug(text, "ERROR-APPEND");
                }
            }
        }
    }
}
