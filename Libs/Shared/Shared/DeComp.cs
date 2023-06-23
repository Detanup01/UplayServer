using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using LzhamWrapper;
using ZstdNet;

namespace SharedLib.Shared
{
    public class DeComp
    {
        public static byte[] Decompress(bool IsCompressed, bool IsCustomLzham, string CompressionMethod, byte[] bytesToDecompress, uint outputsize)
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
                    if (IsCustomLzham)
                    {
                        DecompressionParameters d = new()
                        {
                            Flags = LzhamWrapper.Enums.DecompressionFlag.ComputeAdler32,
                            DictionarySize = 26,
                            UpdateRate = LzhamWrapper.Enums.TableUpdateRate.Default
                        };
                        MemoryStream mem = new((int)outputsize);
                        LzhamStream lzhamStream = new LzhamStream(new MemoryStream(bytesToDecompress), d);
                        lzhamStream.CopyTo(mem);
                        lzhamStream.Dispose();
                        return mem.ToArray();
                    }
                    else
                    {
                        DecompressionParameters d = new()
                        {
                            Flags = LzhamWrapper.Enums.DecompressionFlag.ComputeAdler32 | LzhamWrapper.Enums.DecompressionFlag.ReadZlibStream,
                            DictionarySize = 15,
                            UpdateRate = LzhamWrapper.Enums.TableUpdateRate.Default
                        };
                        MemoryStream mem = new((int)outputsize);
                        LzhamStream lzhamStream = new LzhamStream(new MemoryStream(bytesToDecompress), d);
                        lzhamStream.CopyTo(mem);
                        lzhamStream.Dispose();
                        return mem.ToArray();
                    }
            }
            return bytesToDecompress;
        }

        public static byte[] Compress(bool IsCompressed, bool IsCustomLzham, string CompressionMethod, byte[] bytesToCompress, uint outputsize)
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
                    var defl = new Deflater(Deflater.BEST_COMPRESSION, false);
                    Stream deflate = new DeflaterOutputStream(ms, defl);
                    deflate.Write(bytesToCompress);
                    deflate.Close();
                    return ms.ToArray();
                case "Lzham":
                    //50 MB Limit!
                    if (IsCustomLzham && outputsize <= 52428800)
                    {
                        uint adler = 0;
                        CompressionParameters c = new()
                        {
                            Flags = 0,
                            DictionarySize = 26,
                            UpdateRate = LzhamWrapper.Enums.TableUpdateRate.Default,
                            Level = LzhamWrapper.Enums.CompressionLevel.Uber
                        };

                        byte[] output = new byte[outputsize];
                        int outsize = output.Length;
                        Lzham.CompressMemory(c, bytesToCompress, bytesToCompress.Length, 0, output, ref outsize, 0, ref adler);
                        return output.Take(outsize).ToArray();
                    }
                    else
                    { 
                        //return nothing to indicate we have issues!
                        return new byte[] { };
                    }
            }
            return bytesToCompress;
        }

        public static byte[] ZlibCompress(byte[] input)
        {
            return Ionic.Zlib.ZlibStream.CompressBuffer(input);
        }

        public static byte[] ZlibDecompress(byte[] input)
        {
            return Ionic.Zlib.ZlibStream.UncompressBuffer(input);
        }
    }
}
