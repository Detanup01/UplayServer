using System.Data.SQLite;

namespace Core.SQLite
{
    public class AppFlags
    {
        public static void Add(uint productId, List<string> flags)
        {
            string outFlags = "";
            foreach (string flag in flags)
            {
                outFlags += flag + ",";
            }
            outFlags = outFlags.TrimEnd(',');
            var sqlconnection = new SQLiteConnection(Preparing.AppFlagsDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "INSERT INTO app_flags (\"id\",\"productId\",\"flags\") VALUES (NULL,@productId,@flags);";
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@flags", outFlags);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();

        }
        public static void Edit(uint productId, List<string> flags)
        {
            string outFlags = "";
            foreach (string flag in flags)
            {
                outFlags += flag + ",";
            }
            outFlags = outFlags.TrimEnd(',');
            var sqlconnection = new SQLiteConnection(Preparing.AppFlagsDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "UPDATE app_flags SET flags=@flags WHERE productId=@productId;";
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@flags", outFlags);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();
        }

        public static List<string> Get(uint productId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.AppFlagsDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "SELECT flags FROM app_flags WHERE productId=@productId;";
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Prepare();
            List<string> flags = new();
            string ret = "";
            using (SQLiteDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ret = rdr["flags"].ToString();
                }
            }
            cmd.Dispose();
            sqlconnection.Dispose();
            if (ret == null) { return new(); }
            flags.AddRange(ret.Split(','));
            return flags;
        }
        public static void Delete(uint productId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.AppFlagsDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "DELETE FROM app_flags WHERE productId=@productId;";
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();
        }
    }
}
