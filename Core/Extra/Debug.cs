namespace Core.Extra
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

        public static void WriteDebug(string strLog, string logname = "debug.txt")
        {
            if (isDebug == true)
            {
                File.AppendAllText(logname, strLog+"\n");
            }
        }
    }
}
