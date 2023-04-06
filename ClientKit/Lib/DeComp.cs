using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.IO.Compression;
using ZstdNet;

namespace ClientKit.Lib
{
    public class DeComp
    {
        public static byte[] Decompress(bool IsCompressed, string CompressionMethod, byte[] bytesToDecompress, ulong outputsize)
        {
            if (!IsCompressed)
            {
                return bytesToDecompress;
            }

            switch (CompressionMethod) // check compression method
            {
                case "Zstd":
                    Decompressor decompressorZstd = new();
                    byte[] returner = decompressorZstd.Unwrap(bytesToDecompress);
                    decompressorZstd.Dispose();
                    return returner;
                case "Deflate":
                    InflaterInputStream decompressor = new InflaterInputStream(new MemoryStream(bytesToDecompress), new(false));
                    MemoryStream ms = new((int)outputsize);
                    decompressor.CopyTo(ms);
                    decompressor.Dispose();
                    return ms.ToArray();
                case "Lzham":
                    return bytesToDecompress;
                    //return LzhamWrapper.Decompress(bytesToDecompress, outputsize);
            }
            return bytesToDecompress;
        }

        public static byte[] Compress(bool IsCompressed, string CompressionMethod, byte[] bytesToCompress, ulong outputsize)
        {
            if (!IsCompressed)
            {
                return bytesToCompress;
            }

            switch (CompressionMethod) // check compression method
            {
                case "Zstd":
                    Compressor compressZstd = new();
                    byte[] returner = compressZstd.Wrap(bytesToCompress);
                    compressZstd.Dispose();
                    return returner;
                case "Deflate":
                    MemoryStream ms = new();
                    ZLibStream compressor = new ZLibStream(new MemoryStream(bytesToCompress), CompressionLevel.SmallestSize);
                    ms.CopyTo(compressor);
                    compressor.Close();
                    return ms.ToArray();
                case "Lzham":
                    //return LzhamWrapper.Compress(downloadedSlice, outputsize);
                    return bytesToCompress;
            }
            return bytesToCompress;
        }
    }
}
