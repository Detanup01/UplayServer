using Newtonsoft.Json;
using System.Text;
using ZstdNet;

namespace Downloader
{
    internal class Saving
    {
        public class Root
        {
            [JsonProperty("UplayId")]
            public int UplayId;

            [JsonProperty("ManifestSHA1")]
            public string ManifestSHA1;

            [JsonProperty("Version")]
            public int Version;

            [JsonProperty("Compression")]
            public Compression Compression;

            [JsonProperty("Work")]
            public Work Work;

            [JsonProperty("Verify")]
            public Verify Verify;
        }
        public class Compression
        {
            [JsonProperty("Method")]
            public string Method;

            [JsonProperty("IsCompressed")]
            public bool IsCompressed;

            [JsonProperty("HasSliceSHA")]
            public bool HasSliceSHA;
        }
        public class Work
        {
            [JsonProperty("FileInfo")]
            public FileInfo FileInfo;

            [JsonProperty("CurrentId")]
            public string CurrentId;

            [JsonProperty("NextId")]
            public string NextId;
        }
        public class Verify
        {
            [JsonProperty("Files")]
            public List<File> Files;
        }
        public class File
        {
            [JsonProperty("Name")]
            public string Name;

            [JsonProperty("SliceInfo")]
            public List<SliceInfo> SliceInfo;
        }
        public class SliceInfo
        {
            [JsonProperty("CompressedSHA")]
            public string CompressedSHA;

            [JsonProperty("DecompressedSHA")]
            public string DecompressedSHA;

            [JsonProperty("DownloadedSize")]
            public int DownloadedSize;

            [JsonProperty("DecompressedSize")]
            public int DecompressedSize;
        }

        public class FileInfo
        {
            [JsonProperty("Name")]
            public string Name;

            [JsonProperty("IDs")]
            public IDs IDs;
        }

        public class IDs
        {
            [JsonProperty("Slices")]
            public List<string> Slices;

            [JsonProperty("SliceList")]
            public List<string> SliceList;
        }

        public static void Save(Root root, string FileName)
        {
            var ser = JsonConvert.SerializeObject(root);
            var bytes = Encoding.UTF8.GetBytes(ser);
            Compressor compressor = new();
            var returner = compressor.Wrap(bytes);
            compressor.Dispose();
            System.IO.File.WriteAllBytes(FileName, returner);
        }

        public static void Save(Root root)
        {
            var ser = JsonConvert.SerializeObject(root);
            var bytes = Encoding.UTF8.GetBytes(ser);
            Compressor compressor = new();
            var returner = compressor.Wrap(bytes);
            compressor.Dispose();
            System.IO.File.WriteAllBytes(DLWorker.Config.VerifyBinPath, returner);
        }

        public static Root? Read(string FileName)
        {
            var filebytes = System.IO.File.ReadAllBytes(FileName);
            Decompressor decompressorZstd = new();
            var decompressed = decompressorZstd.Unwrap(filebytes);
            decompressorZstd.Dispose();
            var ser = Encoding.UTF8.GetString(decompressed);
            var root = JsonConvert.DeserializeObject<Root>(ser);
            return root;
        }

        public static Root? Read()
        {
            var filebytes = System.IO.File.ReadAllBytes(DLWorker.Config.VerifyBinPath);
            Decompressor decompressorZstd = new();
            var decompressed = decompressorZstd.Unwrap(filebytes);
            decompressorZstd.Dispose();
            var ser = Encoding.UTF8.GetString(decompressed);
            var root = JsonConvert.DeserializeObject<Root>(ser);
            return root;
        }

        public static Root MakeNew(uint productId, string manifest, Uplay.Download.Manifest parsedManifest)
        {
            return new()
            {
                UplayId = (int)productId,
                ManifestSHA1 = manifest,
                Version = (int)parsedManifest.Version,
                Compression = new()
                {
                    IsCompressed = parsedManifest.IsCompressed,
                    Method = parsedManifest.CompressionMethod.ToString(),
                    HasSliceSHA = parsedManifest.Chunks.First().Files.First().SliceList.First().HasDownloadSha1
                },
                Work = new()
                {
                    FileInfo = new()
                    {
                        IDs = new()
                        {
                            SliceList = new(),
                            Slices = new()
                        }
                    }
                },
                Verify = new()
                {
                    Files = new()
                }
            };
        }
    }
}
/*
{
    "UplayId": 0,
    "ManifestSHA1": "",
    "Version": 3,
    "Compression": {
        "Method":"Zstd",
        "IsCompressed":True
    },
    "Work":{
        "FileInfo":{
            "Name":"x.txt",
            "IDs":{
                "Slices": [],
                "SliceList": [ "" ]
            }
        },
        "CurrentId": "",
        "NextId": ""

    },
    "Verify": {
        "Files": [
            {
                "Name":"x.txt",
                "DecompressedSlices": [ {"CompressedSHA":"","DecompressedSHA":"","DownloadedSize":10} ]
            }
        ]

    }
}
 */