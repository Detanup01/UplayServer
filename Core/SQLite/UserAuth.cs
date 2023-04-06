using System.Data.SQLite;

namespace Core.SQLite
{
    public class UserAuth
    {
        public static void Add(string userId, string auth)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserAuthDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "INSERT INTO user_auth (\"id\",\"userId\",\"auth\") VALUES (NULL,@userId,@auth);";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@auth", auth);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();

        }
        public static void Edit(string userId, string auth)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserAuthDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "UPDATE user_auth SET auth=@auth WHERE userId=@userId;";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@auth", auth);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();
        }

        public static string GetUserIdByAuth(string auth)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserAuthDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "SELECT userId FROM user_auth WHERE auth=@auth;";
            cmd.Parameters.AddWithValue("@auth", auth);
            cmd.Prepare();
            string ret = "";
            using (SQLiteDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ret = rdr["userId"].ToString();
                }
            }
            cmd.Dispose();
            sqlconnection.Dispose();
            if (ret == null) { ret = ""; }
            return ret;
        }

        public static string GetAuthByUserId(string userId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserAuthDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "SELECT auth FROM user_auth WHERE userId=@userId;";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Prepare();
            string ret = "";
            using (SQLiteDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ret = rdr["auth"].ToString();
                }
            }
            cmd.Dispose();
            sqlconnection.Dispose();
            if (ret == null) { ret = ""; }
            return ret;
        }

        public static void Delete(string userId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserAuthDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "DELETE FROM user_auth WHERE userId=@userId;";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();
        }
    }
}
