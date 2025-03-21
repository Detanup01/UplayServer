using Google.Protobuf;
using ZstdNet;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace SharedLib;

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
        using MemoryStream mem = new();
        using Compressor compressorZstd = new();
        var zstd = compressorZstd.Wrap(bytes);
        mem.Write(zstd);
        return ByteString.CopyFrom(mem.ToArray()).ToBase64();
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
        using MemoryStream mem = new();
        using Decompressor decompressor = new();
        var unzstd = decompressor.Unwrap(bytes);
        mem.Write(unzstd);
        return ByteString.CopyFrom(mem.ToArray()).ToBase64();
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
        using MemoryStream mem = new();
        var defl = new Deflater(Deflater.BEST_COMPRESSION, false);
        using Stream deflate = new DeflaterOutputStream(mem, defl);
        deflate.Write(bytes);
        deflate.Close();
        return ByteString.CopyFrom(mem.ToArray()).ToBase64();
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
        using MemoryStream mem = new();
        using var inf = new InflaterInputStream(mem);
        inf.ReadExactly(bytes);
        return ByteString.CopyFrom(mem.ToArray()).ToBase64();
    }
    #endregion
}
