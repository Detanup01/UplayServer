using Google.Protobuf;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Security.Cryptography;
using System.Text;
using ZstdNet;

namespace Core
{
    public class Creators
    {

        public static string _ManifestSigner = "START_RandomBytesDoDecryptBecauseIDidntKnowHowToDecrpytTheOriginalDotManifestSignThingy_IfYourReadingThisYouUsingCustomUplayServer_RandomBytesDoDecryptBecauseIDidntKnowHowToDecrpytTheOriginalDotManifestSignThingy_IfYourReadingThisYouUsingCustomUplayServer_RandomBytesDoDecryptBecauseIDidntKnowHowToDecrpytTheOriginalDotManifestSignThingy_IfYourReadingThisYouUsingCustomUplayServer_RandomBytesDoDecryptBecauseIDidntKnowHowToDecrpytTheOriginalDotManifestSignThingy_IfYourReadingThisYouUsingCustomUplayServerFun_END";
        public static string _ManfiestB64 = "U1RBUlRfUmFuZG9tQnl0ZXNEb0RlY3J5cHRCZWNhdXNlSURpZG50S25vd0hvd1RvRGVjcnB5dFRoZU9yaWdpbmFsRG90TWFuaWZlc3RTaWduVGhpbmd5X0lmWW91clJlYWRpbmdUaGlzWW91VXNpbmdDdXN0b21VcGxheVNlcnZlcl9SYW5kb21CeXRlc0RvRGVjcnlwdEJlY2F1c2VJRGlkbnRLbm93SG93VG9EZWNycHl0VGhlT3JpZ2luYWxEb3RNYW5pZmVzdFNpZ25UaGluZ3lfSWZZb3VyUmVhZGluZ1RoaXNZb3VVc2luZ0N1c3RvbVVwbGF5U2VydmVyX1JhbmRvbUJ5dGVzRG9EZWNyeXB0QmVjYXVzZUlEaWRudEtub3dIb3dUb0RlY3JweXRUaGVPcmlnaW5hbERvdE1hbmlmZXN0U2lnblRoaW5neV9JZllvdXJSZWFkaW5nVGhpc1lvdVVzaW5nQ3VzdG9tVXBsYXlTZXJ2ZXJfUmFuZG9tQnl0ZXNEb0RlY3J5cHRCZWNhdXNlSURpZG50S25vd0hvd1RvRGVjcnB5dFRoZU9yaWdpbmFsRG90TWFuaWZlc3RTaWduVGhpbmd5X0lmWW91clJlYWRpbmdUaGlzWW91VXNpbmdDdXN0b21VcGxheVNlcnZlckZ1bl9FTkQ=";

        /// <summary>
        /// Makeing manifest file to output
        /// </summary>
        /// <param name="manifest">Manifest Data</param>
        /// <param name="FileOutput">File path (better to end with .manifest)</param>
        public static void MakeManifest(Uplay.Download.Manifest manifest, string FileOutput)
        {
            var mem = new MemoryStream();
            manifest.WriteTo(mem);
            var writer = File.OpenWrite(FileOutput);
            var binaryWriter = new BinaryWriter(writer, Encoding.UTF8, false);
            binaryWriter.Write((int)manifest.CompressionMethod);
            binaryWriter.Write(344);
            binaryWriter.Write((int)mem.Length);
            binaryWriter.Write(_ManfiestB64);
            writer.Position = 356;

            var ms = new MemoryStream();
            var defl = new Deflater(Deflater.BEST_COMPRESSION, false);
            Stream deflate = new DeflaterOutputStream(ms, defl);
            deflate.Write(mem.ToArray());
            deflate.Close();

            byte[] compressedData = ms.ToArray();
            writer.Write(compressedData);
            writer.Close();
            binaryWriter.Close();
        }

        /// <summary>
        /// Compressing File to chunks
        /// </summary>
        /// <param name="FileName">Name (or path) to a file that being to compress</param>
        /// <param name="maxSize">Chunk Max Sizes</param>
        /// <param name="compression">Compression Method</param>
        /// <param name="sliceIds">ID's to ManifestData</param>
        /// <param name="numberofpart">Number of parts (same as sliceIds.Count)</param>
        public static void CompressFile(string FileName, long maxSize, Uplay.Download.CompressionMethod compression, out List<uint> uncompressedLength, out List<byte[]> compressedbytes)
        {
            uncompressedLength = new();
            compressedbytes = new();
            if (maxSize == 0) return;

            var sr = File.OpenRead(FileName);
            int megabyte = 1024 * 1024;
            byte[] buffer = new byte[megabyte];
            int bytesRead = sr.Read(buffer, 0, megabyte);
            while (bytesRead > 0)
            {
                //DoSomething(buffer, bytesRead);
                bytesRead = sr.Read(buffer, 0, megabyte);
            }

            byte[] fileData = File.ReadAllBytes(FileName);

            List<byte[]> sliceData = new();

            if (fileData.Length <= (int)maxSize)
            {
                var mem = new MemoryStream();
                // check compression method
                switch (compression)
                {
                    case Uplay.Download.CompressionMethod.Zstd:
                        Compressor compressorZstd = new();
                        var zstd = compressorZstd.Wrap(fileData);
                        compressorZstd.Dispose();
                        mem.Write(zstd);
                        break;
                    case Uplay.Download.CompressionMethod.Deflate:
                        var defl = new Deflater(Deflater.BEST_COMPRESSION, false);
                        Stream deflate = new DeflaterOutputStream(mem, defl);
                        deflate.Write(fileData);
                        deflate.Close();
                        break;
                    case Uplay.Download.CompressionMethod.Lzham:
                        mem.Write(fileData);
                        break;
                }
                var arr = mem.ToArray();
                var sliceid = GenerateSliceID(arr);
                uncompressedLength.Add((uint)fileData.Length);
                sliceData.Add(arr);
                compressedbytes.Add(arr);
            }
            else
            {
                foreach (byte[] copySlice in fileData.Split((int)maxSize))
                {
                    var mem = new MemoryStream();
                    // check compression method
                    switch (compression)
                    {
                        case Uplay.Download.CompressionMethod.Zstd:
                            Compressor compressorZstd = new();
                            var zstd = compressorZstd.Wrap(copySlice);
                            compressorZstd.Dispose();
                            mem.Write(zstd);
                            break;
                        case Uplay.Download.CompressionMethod.Deflate:
                            var defl = new Deflater(Deflater.BEST_COMPRESSION, false);
                            Stream deflate = new DeflaterOutputStream(mem, defl);
                            deflate.Write(copySlice);
                            deflate.Close();
                            break;
                        case Uplay.Download.CompressionMethod.Lzham:
                            mem.Write(copySlice);
                            break;
                    }
                    var arr = mem.ToArray();
                    var sliceid = GenerateSliceID(arr);
                    uncompressedLength.Add((uint)copySlice.Length);
                    sliceData.Add(arr);
                    compressedbytes.Add(arr);
                }
            }
        }

        /// <summary>
        /// Generating Slice IDs
        /// </summary>
        /// <param name="input">Bytes of Data</param>
        /// <returns>SHA1 Hash</returns>
        public static string GenerateSliceID(byte[] input)
        {
            var ret = SHA1.HashData(input);
            var returner = BitConverter.ToString(ret).Replace("-", "");
            return returner;
        }

        /// <summary>
        /// Create Achievements
        /// </summary>
        /// <param name="achId">List of Ids</param>
        /// <param name="FileName">Filename to write it</param>
        public static void CreateAch(List<uint> achId, string FileName)
        {
            Uplay.Uplaydll.AchievementsBlob achievements = new();

            foreach (var id in achId)
            {
                Uplay.Uplaydll.AchievementInBlob achievement = new();

                achievement.AchievementId = id;
                achievement.TitleId = id;
                achievement.DescriptionId = id;
                achievement.DescriptionId = id;
                achievements.Achievements.Add(achievement);
            }

            File.WriteAllBytes(FileName, achievements.ToByteArray());
        }
    }
}