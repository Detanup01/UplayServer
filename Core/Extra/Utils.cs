using Google.Protobuf;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ZstdNet;

namespace Core
{
    public class Utils
    {
        public static char FormatSliceHashChar(string sliceId)
        {
            char[] base32 = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v' };
            byte reversedValue = byte.Parse($"{sliceId[1]}{sliceId[0]}", System.Globalization.NumberStyles.HexNumber);
            bool isEven = reversedValue % 2 == 0;
            int offset = (int)Math.Floor((decimal)reversedValue / 16);
            int halfOffset = isEven ? 0 : 16;
            return base32[offset + halfOffset];
        }

        public static uint ByteSwapUInt(uint x)
        {
            return ((x & 0x000000ff) << 24) +
                   ((x & 0x0000ff00) << 8) +
                   ((x & 0x00ff0000) >> 8) +
                   ((x & 0xff000000) >> 24);
        }
        public static byte[] FormatUpstream(byte[] rawMessage)
        {
            return BitConverter.GetBytes(ByteSwapUInt((uint)rawMessage.Length)).Concat(rawMessage).ToArray();
        }

        public static void WriteFile(string strLog, string FileName)
        {
            FileInfo logFileInfo = new FileInfo(FileName);
            DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            using FileStream fileStream = new FileStream(FileName, FileMode.Append);
            using StreamWriter log = new StreamWriter(fileStream);
            log.WriteLine(strLog);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.ASCII.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string FromB64(string plainText)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(plainText));
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

            X509Certificate2 cert = new(File.ReadAllBytes($"cert/{certname}.pfx"), password);
            return cert;
        }

        public static string GetZstdB64(string str)
        {
            return GetZstdB64(System.Text.Encoding.UTF8.GetBytes(str));
        }

        public static string GetZstdB64(byte[] bytes)
        {

            MemoryStream mem = new();
            Compressor compressorZstd = new();
            var zstd = compressorZstd.Wrap(bytes);
            compressorZstd.Dispose();
            mem.Write(zstd);
            ByteString bs = ByteString.CopyFrom(mem.ToArray());
            var bs64 = bs.ToBase64();
            mem.Close();
            return bs64;
        }

        public static string GetUnZstdB64(string str)
        {
            return GetUnZstdB64(System.Text.Encoding.UTF8.GetBytes(str));
        }

        public static string GetUnZstdB64(byte[] bytes)
        {
            MemoryStream mem = new();
            Decompressor decompressor = new();
            var unzstd = decompressor.Unwrap(bytes);
            decompressor.Dispose();
            mem.Write(unzstd);
            ByteString bs = ByteString.CopyFrom(mem.ToArray());
            var bs64 = bs.ToBase64();
            mem.Close();
            return bs64;
        }

        public static string GetDeflateB64(string str)
        {
            return GetDeflateB64(System.Text.Encoding.UTF8.GetBytes(str));
        }

        public static string GetDeflateB64(byte[] bytes)
        {
            MemoryStream mem = new();
            var defl = new Deflater(Deflater.BEST_COMPRESSION, false);
            Stream deflate = new DeflaterOutputStream(mem, defl);
            deflate.Write(bytes);
            deflate.Close();
            ByteString bs = ByteString.CopyFrom(mem.ToArray());
            var bs64 = bs.ToBase64();
            mem.Close();
            return bs64;
        }

        public static string GetUnDeflateB64(string str)
        {
            return GetUnDeflateB64(System.Text.Encoding.UTF8.GetBytes(str));
        }

        public static string GetUnDeflateB64(byte[] bytes)
        {
            MemoryStream mem = new();
            var inf = new InflaterInputStream(mem);
            inf.Read(bytes);
            inf.Close();
            ByteString bs = ByteString.CopyFrom(mem.ToArray());
            var bs64 = bs.ToBase64();
            mem.Close();
            return bs64;
        }
    }
}
