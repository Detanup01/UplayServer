using Google.Protobuf;
using ZstdNet;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace SharedLib.Shared
{
    public class CompressB64
    {
        #region ZSTD
        /// <summary>
        /// Compress string with ZSTD
        /// </summary>
        /// <param name="str">To Compress</param>
        /// <returns>String as Base64</returns>
        public static string GetZstdB64(string str)
        {
            return GetZstdB64(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Compress bytes with ZSTD
        /// </summary>
        /// <param name="bytes">To Compress</param>
        /// <returns>String as Base64</returns>
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

        /// <summary>
        /// Decompress string with ZSTD
        /// </summary>
        /// <param name="str">To Decompress</param>
        /// <returns>String as Base64</returns>
        public static string GetUnZstdB64(string str)
        {
            return GetUnZstdB64(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Decompress bytes with ZSTD
        /// </summary>
        /// <param name="bytes">To Decompress</param>
        /// <returns>String as Base64</returns>
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
        #endregion
        #region Deflate
        /// <summary>
        /// Compress string with Deflate
        /// </summary>
        /// <param name="str">To Compress</param>
        /// <returns>String as Base64</returns>
        public static string GetDeflateB64(string str)
        {
            return GetDeflateB64(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Compress bytes with Deflate
        /// </summary>
        /// <param name="bytes">To Compress</param>
        /// <returns>String as Base64</returns>
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

        /// <summary>
        /// Decompress string with Deflate
        /// </summary>
        /// <param name="str">To Decompress</param>
        /// <returns>String as Base64</returns>
        public static string GetUnDeflateB64(string str)
        {
            return GetUnDeflateB64(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Decompress bytes with Deflate
        /// </summary>
        /// <param name="bytes">To Decompress</param>
        /// <returns>String as Base64</returns>
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
        #endregion
    }
}
