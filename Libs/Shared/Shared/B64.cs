using System.Text;

namespace SharedLib.Shared
{
    public class B64
    {
        public static string ToB64(string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        public static string FromB64(string plainText)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(plainText));
        }
    }
}
