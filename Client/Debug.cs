namespace Client
{
    public class Debug
    {
        public static bool isDebug = true;
        public static void PWSDebug(object obj)
        {
            if (isDebug == true)
            {
                Console.WriteLine(obj.ToString());
                WriteDebug(obj.ToString());
            }
        }

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

        public static void ShowDebug(object obj)
        {

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
                FileInfo logFileInfo = new(logname);
                DirectoryInfo logDirInfo = new(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                using FileStream fileStream = new(logname, FileMode.Append);
                using StreamWriter log = new(fileStream);
                log.WriteLine(strLog);
            }
        }
    }
}