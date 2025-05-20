using ServerCore.Extra;

namespace Creator;

internal class ToManifest
{
    public static Uplay.Download.Manifest Manifest = new();

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
        var filename = fileinfo.FullName.Replace(basepath + "\\", "");
        Uplay.Download.File file = new()
        {
            IsDir = false,
            Name = filename,
            Size = (ulong)fileinfo.Length,
            Slices = { },
            SliceList = { }
        };
        if (Manifest.IsCompressed)
        {
            Creators.CompressFile(filepath, int.Parse(maxsiz), comp, savetopath, sliceversion, prodid, out var file1);
            file = file1;
        }
        else
        {
            if (fileinfo.Length >= int.MaxValue)
            {
                var sr = File.OpenRead(filepath);
                int megabyte = 5242880;
                byte[] buffer = new byte[megabyte];
                int bytesRead = sr.Read(buffer, 0, megabyte);
                Creators.WriteOut((uint)buffer.Length, buffer, savetopath, sliceversion, prodid, file, out var outfile);
                file = outfile;
                while (bytesRead > 0)
                {
                    bytesRead = sr.Read(buffer, 0, megabyte);
                    Creators.WriteOut((uint)buffer.Length, buffer, savetopath, sliceversion, prodid, file, out outfile);
                    file = outfile;
                }
            }
            else
            {
                var all = File.ReadAllBytes(filepath);
                Creators.WriteOut((uint)all.Length, all, savetopath, sliceversion, prodid, file, out var outfile);
                file = outfile;
            }
        }

        Manifest.Chunks[chunknumber].Files.Add(file);

    }

}
