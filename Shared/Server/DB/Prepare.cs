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

            //  Other DB Init
            App.Init();

            //0User Auth with: 0USER@test:test
            Auth.AddUA("00000000-0000-0000-0000-000000000000", "YzFjNFU2d2RHZGJ4c0hjMkZjZUFpUlM2MWxVOE9tSndRK2hNbTJYS1E5WT1fQ1VTVE9NREVNVVg=");

            //fUser Auth with: fUSER@test: test
            Auth.AddUA("ffffffff-ffff-ffff-ffff-ffff-ffffffffffff", "YzFjNFU2d2RHZGJ4c0hjMkZjZUFpUlM2MWxVOE9tSndRK2hNbTJYS1E5WT1fQ1VTVE9NREVNVVg=");
        }
    }
}
