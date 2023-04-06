using Google.Protobuf;

namespace Creator
{
    internal class ToManifest
    {
        public static Uplay.Download.Manifest Manifest = null;

        public static void MakeNewManifest(string prodId)
        {
            Manifest = new()
            {
                IsCompressed = true,
                Chunks =
                {
                    new Uplay.Download.Chunk()
                    {
                        Disc = "Basic",
                        Files = { },
                        Id = 0,
                        Type = Uplay.Download.Chunk.Types.ChunkType.Required,
                        UplayId = uint.Parse(prodId)
                    }

                }
            };
        }

        public static void MakeNewManifest()
        {
            Manifest = new()
            {
                IsCompressed = true,
                Chunks =
                {
                    new Uplay.Download.Chunk()
                    {
                        Disc = "Basic",
                        Files = { },
                        Id = 0,
                        Type = Uplay.Download.Chunk.Types.ChunkType.Required
                    }

                },
            };
        }


        public static void WorkFile(string filepath, string maxsiz, string compression, string prodid, string savetopath, string sliceversion, string basepath, int chunknumber)
        {
            Uplay.Download.CompressionMethod comp = Uplay.Download.CompressionMethod.Deflate;
            if (compression == "None")
            {
                Manifest.IsCompressed = false;
            }
            else
            {
                switch (compression)
                {
                    case "Deflate":
                        comp = Uplay.Download.CompressionMethod.Deflate;
                        break;
                    case "Zstd":
                        comp = Uplay.Download.CompressionMethod.Zstd;
                        break;
                    case "Lzham":
                        comp = Uplay.Download.CompressionMethod.Lzham;
                        break;
                    default:
                        break;

                }
            }

            if (Manifest.HasCompressionMethod && Manifest.CompressionMethod != comp)
            {
                Console.WriteLine("Compression is not same as before!");
                return;
            }
            Manifest.CompressionMethod = comp;
            if (Manifest.HasVersion && Manifest.Version != uint.Parse(sliceversion))
            {
                Console.WriteLine("Version is not same as before!");
                return;
            }
            Manifest.Version = uint.Parse(sliceversion);
            var fileinfo = new FileInfo(filepath);
            List<uint> uncomplenght = new();
            List<byte[]> bytes = new();
            if (Manifest.IsCompressed)
            {
                Core.Creators.CompressFile(filepath, long.Parse(maxsiz), comp, out uncomplenght, out bytes);
            }
            else
            {
                if (fileinfo.Length >= int.MaxValue)
                {
                    var sr = File.OpenRead(filepath);
                    int megabyte = 1024 * 1024;
                    byte[] buffer = new byte[megabyte];
                    int bytesRead = sr.Read(buffer, 0, megabyte);
                    while (bytesRead > 0)
                    {
                        bytes.Add(buffer);
                        uncomplenght.Add((uint)bytesRead);
                        bytesRead = sr.Read(buffer, 0, megabyte);
                    }
                }
                else
                {
                    uncomplenght.Add((uint)fileinfo.Length);
                    bytes.Add(File.ReadAllBytes(filepath));
                }
            }



            var filename = fileinfo.FullName.Replace(basepath + "\\", "");

            Uplay.Download.File file = new()
            {
                IsDir = false,
                Name = filename,
                Size = (ulong)fileinfo.Length,
                Slices = { },
                SliceList = { }
            };

            for (int slicecounter = 0; slicecounter < uncomplenght.Count; slicecounter++)
            {
                var uncompsize = uncomplenght[slicecounter];
                var slicebytes = bytes[slicecounter];
                var sliceid = Core.Creators.GenerateSliceID(slicebytes);
                var sliceid_b = Convert.FromHexString(sliceid);

                if (sliceversion == "3")
                {
                    Uplay.Download.Slice slice = new()
                    {
                        DownloadSha1 = ByteString.CopyFrom(sliceid_b),
                        Size = uncompsize,
                        DownloadSize = (uint)slicebytes.Length
                    };
                    file.SliceList.Add(slice);
                    var pathtowrite = $"{savetopath}/Download/{prodid}/slices_v3/{Core.Utils.FormatSliceHashChar(sliceid)}";
                    Directory.CreateDirectory(pathtowrite);
                    File.WriteAllBytes(pathtowrite + "/" + sliceid, slicebytes);
                }
                else
                {
                    var pathtowrite = $"{savetopath}/Download/{prodid}/slices";
                    Directory.CreateDirectory(pathtowrite);
                    File.WriteAllBytes(pathtowrite + "/" + sliceid, slicebytes);
                }
                file.Slices.Add(ByteString.CopyFrom(sliceid_b));
            }

            Manifest.Chunks[chunknumber].Files.Add(file);

        }

    }
}
