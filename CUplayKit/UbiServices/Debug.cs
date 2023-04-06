namespace CUplayKit.UbiServices
{
    public class Debug
    {
        public static bool isDebug = false;
        public static void PWDebug(object obj)
        {
            if (isDebug == true)
            {
                Console.WriteLine(obj.ToString());
                WriteDebug(obj.ToString());
            }
        }
        public static void PWDebug(object obj, string logname)
        {
            if (isDebug == true)
            {
                Console.WriteLine(obj.ToString());
                WriteDebug(obj.ToString(), logname);
            }
        }
        public static void PrintDebug(object obj)
        {
            if (isDebug == true)
            {
                Console.WriteLine(obj.ToString());
            }
        }

        public static void WriteDebug(string strLog, string logname = "service_debug.txt")
        {
            if (isDebug == true)
            {
                File.AppendAllText(logname, strLog + "\n");
            }
        }

        public static void WriteBytes(byte[] bytes, string logname = "service_debug.txt")
        {
            if (isDebug == true)
            {
                File.WriteAllBytes(logname, bytes);
            }
        }
        public static void WriteText(string text, string logname = "service_debug.txt")
        {
            if (isDebug == true)
            {
                File.WriteAllText(logname, text);
            }
        }
    }
}