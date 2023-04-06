using System.Data.SQLite;

namespace Core.SQLite
{
    public class AppAPI
    {
        public static void Starter()
        {
            var sqlconnection = new SQLiteConnection(Preparing.AppAPIDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "INSERT INTO app_api (\"id\",\"applicationId\",\"name\",\"platform\",\"spaceId\") VALUES (NULL,\"f35adcb5-1911-440c-b1c9-48fdc1701c68\",\"ubi.com pc\",\"PC\",\"ed8600fa-a5d9-48a2-8ab7-d9f41c0c2029\");";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlconnection.Dispose();
        }

        public static string GetSpaceId(string AppId)
        {
            var sqlconnection = new SQLiteConnection(Preparing.AppAPIDB);
            sqlconnection.Open();
            var cmd = sqlconnection.CreateCommand();
            cmd.CommandText = "SELECT spaceId FROM app_api WHERE applicationId=@appid;";
            cmd.Parameters.AddWithValue("@appid", AppId);
            cmd.Prepare();
            string ret = "";
            using (SQLiteDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ret = rdr["spaceId"].ToString();
                }
            }
            cmd.Dispose();
            sqlconnection.Dispose();
            if (ret == null) { ret = ""; }
            return ret;

        }
    }
}
