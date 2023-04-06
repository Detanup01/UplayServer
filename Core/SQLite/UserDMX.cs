using System.Data.SQLite;

namespace Core.SQLite
{
    public class UserDMX
    {
        public static void Add(string userId, uint conId, string conName)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserDMXConnectionDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "INSERT INTO dmx (\"id\",\"userId\",\"conId\",\"conName\") VALUES (NULL,@userId,@conId,@conName);";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@conId", conId);
            cmd.Parameters.AddWithValue("@conName", conName);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();

        }

        public static uint GetConIdByUserAndName(string userId, string conName)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserDMXConnectionDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "SELECT conId FROM dmx WHERE userId=@userId AND conName=@conName;";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@conName", conName);
            cmd.Prepare();
            uint ret = 0;
            using (SQLiteDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ret = uint.Parse(rdr["conId"].ToString());
                }
            }
            cmd.Dispose();
            sqlconnection.Dispose();
            return ret;
        }

        public static string GetConNameByUserAndId(string userId, uint conId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserDMXConnectionDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "SELECT conName FROM dmx WHERE userId=@userId AND conId=@conId;";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@conId", conId);
            cmd.Prepare();
            string ret = "";
            using (SQLiteDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ret = rdr["conName"].ToString();
                }
            }
            cmd.Dispose();
            sqlconnection.Dispose();
            if (ret == null) { ret = ""; }
            return ret;
        }

        public static uint GetLatestConIdByUser(string userId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserDMXConnectionDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "SELECT conId FROM dmx WHERE userId=@userId;";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Prepare();
            uint ret = 0;
            using (SQLiteDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ret = uint.Parse(rdr["conId"].ToString());
                }
            }
            cmd.Dispose();
            sqlconnection.Dispose();
            return ret;
        }

        public static void Delete(string userId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.UserDMXConnectionDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "DELETE FROM dmx WHERE userId=@userId;";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();
        }
    }
}
