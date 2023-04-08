namespace SharedLib.Server.DB
{
    public class Prepare
    {
        private static readonly string extractPath = Directory.GetCurrentDirectory();
        public static readonly string DatabasePath = extractPath + "\\DataBase\\";

        public static readonly string UserBannedList = "DataBase\\bannedlist.txt";
        public static List<string> BannedUsers = new();
        public static void MakeAll()
        {
            if (!Directory.Exists(extractPath + "\\Database"))
            {
                Directory.CreateDirectory(extractPath + "\\Database");
            }

            if (!File.Exists(UserBannedList))
            {
                File.AppendAllText(UserBannedList, "");
            }
            //Banned
            BannedUsers = File.ReadAllLines(UserBannedList).ToList();

            App.Create();

        }


    }
}
