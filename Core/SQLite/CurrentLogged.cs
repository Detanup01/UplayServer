using System.Data.SQLite;

namespace Core.SQLite
{
    public static class CurrentLogged
    {
        public enum TokenType
        {
            Orbit = 1,
            Token = 2,
            Ticket = 3
        }

        public static void Add(string userId, string token, int tokentype)
        {
            var sqlconnection = new SQLiteConnection(Preparing.CurrentDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "INSERT INTO current_logged (\"userId\",\"token\",\"type\") VALUES (@userId,@token,@tokentype);";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@tokentype", tokentype);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();

        }
        public static void Edit(string userId, string token, int tokentype)
        {
            var sqlconnection = new SQLiteConnection(Preparing.CurrentDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "UPDATE current_logged SET token=@token WHERE userId=@userId AND type=@tokentype;";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@tokentype", tokentype);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();
        }

        public static string GetUserIdByToken(string token, int tokentype)
        {
            var sqlconnection = new SQLiteConnection(Preparing.CurrentDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "SELECT userId FROM current_logged WHERE token=@token AND type=@tokentype;";
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@tokentype", tokentype);
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

        public static void Delete(string userId, int tokentype)
        {
            var sqlconnection = new SQLiteConnection(Preparing.CurrentDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "DELETE FROM current_logged WHERE userId=@userId AND type=@tokentype;";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@tokentype", tokentype);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();
        }

        public static void Delete(string userId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.CurrentDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "DELETE FROM current_logged WHERE userId=@userId";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();
        }
    }
}
