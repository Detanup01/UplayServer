using Core.DemuxResponders;
using ServerCore.Models;
using SharedLib.Shared;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Core
{
    public class Utils
    {
        public static void WriteFile(string strLog, string FileName)
        {
            FileInfo logFileInfo = new FileInfo(FileName);
            DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            using FileStream fileStream = new FileStream(FileName, FileMode.Append);
            using StreamWriter log = new StreamWriter(fileStream);
            log.WriteLine(strLog);
        }

        public static string MakeNewID()
        {
            return $"{Randoming(8)}-{Randoming(4)}-{Randoming(4)}-{Randoming(4)}-{Randoming(12)}";
        }

        public static DateTime ConvertFromUnixTimestampToLocal(long timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp).ToLocalTime();
        }

        public static string MakeAuth(string auth)
        {
            return B64.ToB64(Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(auth).Concat(Encoding.UTF8.GetBytes(ServerConfig.Instance.sql.AuthSalt)).ToArray())));
        }

        public static string Randoming(int lenght)
        {
            Random res = new Random();
            String str = "abcdef0123456789";
            String ran = "";

            for (int i = 0; i < lenght; i++)
            {

                // Selecting a index randomly
                int x = res.Next(16);

                // Appending the character at the 
                // index to the random string.
                ran = ran + str[x];
            }

            return ran;
        }

        public static X509Certificate GetCert(string certname, string password)
        {
            Debug.PWDebug($"[GetCert] {certname} {password}");
            X509Certificate2 cert = new(File.ReadAllBytes($"cert/{certname}.pfx"), password);
            return cert;
        }

        public static (string protoname, byte[] buffer) GetCustomProto(Guid Id, byte[] buffer)
        {
            Debug.PWDebug($"{Id}: Custom Request Received!", "DMXSERVER");
            int ReqNameLenght = int.Parse(Encoding.UTF8.GetString(new byte[] { buffer[1] }));
            var bytename = buffer.Skip(2).Take(ReqNameLenght).ToArray();
            string protoname = Encoding.UTF8.GetString(bytename);
            Debug.PrintDebug($"[DMXSERVER] Request Name: {protoname}");
            var bytes = buffer.Skip(2 + ReqNameLenght).ToArray();
            return (protoname, bytes);


        }
    }
}
