using System.Data.SQLite;

namespace Core.SQLite
{
    public class Preparing
    {
        private static readonly string extractPath = Directory.GetCurrentDirectory();
        private static readonly string DatabasePath = @"URI=file:" + extractPath + "\\DataBase\\";
        public static readonly string UserAuthDB = DatabasePath + "user_auth.db";
        public static readonly string UserToSessionDB = DatabasePath + "user2session.db";
        public static readonly string CurrentDB = DatabasePath + "current_logged.db";
        public static readonly string UserDMXConnectionDB = DatabasePath + "user_dmx.db";
        public static readonly string StoreDB = DatabasePath + "store.db";
        public static readonly string AppAPIDB = DatabasePath + "app_api.db";
        public static readonly string AppFlagsDB = DatabasePath + "app_flags.db";
        public static readonly string SpacesDB = DatabasePath + "spaces.db";
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

            //Current Logged
            var sqlconnection = new SQLiteConnection(CurrentDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS \"current_logged\" (\r\n\t\"id\"\tINTEGER,\r\n\t\"userId\"\tTEXT,\r\n\t\"token\"\tTEXT,\r\n\t\"type\"\tINTEGER,\r\n\tPRIMARY KEY(\"id\" AUTOINCREMENT)\r\n);";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();

            //Users
            sqlconnection = new(UserAuthDB);
            sqlconnection.Open();
            cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS \"user_auth\" (\r\n\t\"id\"\tINTEGER,\r\n\t\"userId\"\tTEXT,\r\n\t\"auth\"\tINTEGER,\r\n\tPRIMARY KEY(\"id\" AUTOINCREMENT)\r\n);";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();

            //User2Session
            sqlconnection = new(UserToSessionDB);
            sqlconnection.Open();
            cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS \"usertosession\" (\r\n\t\"id\"\tINTEGER,\r\n\t\"userId\"\tTEXT,\r\n\t\"sessionId\"\tINTEGER,\r\n\tPRIMARY KEY(\"id\" AUTOINCREMENT)\r\n);";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();

            //User DMX
            sqlconnection = new(UserDMXConnectionDB);
            sqlconnection.Open();
            cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS \"dmx\" (\r\n\t\"id\"\tINTEGER,\r\n\t\"userId\"\tTEXT,\r\n\t\"conId\"\tINTEGER,\r\n\t\"conName\"\tTEXT,\r\n\tPRIMARY KEY(\"id\" AUTOINCREMENT)\r\n);";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();


            //Store
            sqlconnection = new(StoreDB);
            sqlconnection.Open();
            cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS \"store\" (\r\n\t\"id\"\tINTEGER,\r\n\t\"productId\"\tINTEGER,\r\n\t\"storeJson\"\tTEXT,\r\n\tPRIMARY KEY(\"id\" AUTOINCREMENT)\r\n);";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();

            //App API
            sqlconnection = new(AppAPIDB);
            sqlconnection.Open();
            cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS \"app_api\" (\r\n\t\"id\"\tINTEGER,\r\n\t\"applicationId\"\tTEXT,\r\n\t\"description\"\tTEXT,\r\n\t\"name\"\tTEXT,\r\n\t\"displayName\"\tTEXT,\r\n\t\"platform\"\tTEXT,\r\n\t\"genre\"\tTEXT,\r\n\t\"releaseDate\"\tTEXT,\r\n\t\"spaceId\"\tTEXT,\r\n\tPRIMARY KEY(\"id\" AUTOINCREMENT)\r\n);";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();

            //App Flags
            sqlconnection = new(AppFlagsDB);
            sqlconnection.Open();
            cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS \"app_flags\" (\r\n\t\"id\"\tINTEGER,\r\n\t\"productId\"\tINTEGER,\r\n\t\"flags\"\tTEXT,\r\n\tPRIMARY KEY(\"id\" AUTOINCREMENT)\r\n);";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();


            //Spaces
            sqlconnection = new(SpacesDB);
            sqlconnection.Open();
            cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS \"spaces\" (\r\n\t\"id\"\tINTEGER,\r\n\t\"spaceId\"\tTEXT,\r\n\t\"spaceType\"\tTEXT,\r\n\t\"spaceName\"\tTEXT,\r\n\t\"parentSpaceId\"\tTEXT,\r\n\t\"parentSpaceName\"\tTEXT,\r\n\t\"releaseType\"\tTEXT,\r\n\t\"platformType\"\tTEXT,\r\n\t\"dateCreated\"\tTEXT,\r\n\t\"dateLastModified\"\tTEXT,\r\n\tPRIMARY KEY(\"id\" AUTOINCREMENT)\r\n);";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();

            AppAPI.Starter();
        }
    }
}
