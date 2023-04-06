using System.Data.SQLite;

namespace Core.SQLite
{
    public class UserToSession
    {
        public static void Add(string userId, string sessionId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserToSessionDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "INSERT INTO usertosession (\"id\",\"userId\",\"sessionId\") VALUES (NULL,@userId,@sessionId);";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();

        }
        public static void Edit(string userId, string sessionId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserToSessionDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "UPDATE usertosession SET sessionId=@sessionId WHERE userId=@userId;";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@auth", sessionId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();
        }

        public static string GetUserIdBySessionId(string sessionId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserToSessionDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "SELECT userId FROM usertosession WHERE sessionId=@sessionId;";
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
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

        public static string GetSessionIdByUserId(string userId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserToSessionDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "SELECT sessionId FROM usertosession WHERE userId=@userId;";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Prepare();
            string ret = "";
            using (SQLiteDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ret = rdr["sessionId"].ToString();
                }
            }
            cmd.Dispose();
            sqlconnection.Dispose();
            if (ret == null) { ret = ""; }
            return ret;
        }

        public static void DeleteWithUser(string userId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserToSessionDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "DELETE FROM usertosession WHERE userId=@userId;";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();
        }
        public static void DeleteWithSession(string sessionId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserToSessionDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "DELETE FROM usertosession WHERE sessionId=@sessionId;";
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();
        }
    }
}
