using Newtonsoft.Json;
using System.Text;
using ZstdNet;

namespace Downloader
{
    public class AutoRes
    {
        public class Root
        {
            [JsonProperty("DownloadingList", NullValueHandling = NullValueHandling.Ignore)]
            public List<DownloadingList> DownloadingList { get; set; }
        }
        public class DownloadingList
        {
            [JsonProperty("ProductId", NullValueHandling = NullValueHandling.Ignore)]
            public uint ProductId { get; set; }

            [JsonProperty("ManifestId", NullValueHandling = NullValueHandling.Ignore)]
            public string ManifestId { get; set; }

            [JsonProperty("DownloadPath", NullValueHandling = NullValueHandling.Ignore)]
            public string DownloadPath { get; set; }

            [JsonProperty("VerifyBinPath", NullValueHandling = NullValueHandling.Ignore)]
            public string VerifyBinPath { get; set; }

            [JsonProperty("ManifestPath", NullValueHandling = NullValueHandling.Ignore)]
            public string ManifestPath { get; set; }

            [JsonProperty("Priority", NullValueHandling = NullValueHandling.Ignore)]
            public uint Priority { get; set; } = uint.MinValue;

            [JsonProperty("CurrentState", NullValueHandling = NullValueHandling.Ignore)]
            public State CurrentState { get; set; } = State.Unknown;
        }

        public enum State : int
        {
            Unknown = -1,
            Pausing,
            Resuming,
            Started,
            Finished
        }

        public static void Save(Root root)
        {
            var ser = JsonConvert.SerializeObject(root);
            var bytes = Encoding.UTF8.GetBytes(ser);
            Compressor compressor = new();
            var returner = compressor.Wrap(bytes);
            compressor.Dispose();
            System.IO.File.WriteAllBytes("UD_AutoRes.bin", returner);
        }
        public static Root? Read()
        {
            var filebytes = System.IO.File.ReadAllBytes("UD_AutoRes.bin");
            Decompressor decompressorZstd = new();
            var decompressed = decompressorZstd.Unwrap(filebytes);
            decompressorZstd.Dispose();
            var ser = Encoding.UTF8.GetString(decompressed);
            var root = JsonConvert.DeserializeObject<Root>(ser);
            return root;
        }

        public static Root MakeNew(uint productId, string manifest, string dlpath, string verbinpath, string mpath)
        {
            return new()
            {
                DownloadingList = new()
                {
                   new()
                   {
                       ProductId = productId,
                       ManifestId = manifest,
                       DownloadPath = dlpath,
                       VerifyBinPath = verbinpath,
                       ManifestPath = mpath,
                       Priority = uint.MaxValue,
                       CurrentState = State.Unknown
                   }
                }
            };
        }
    }
}
/*
{
    "DownloadingList" : [
        {
            "ProductId": 0,
            "ManifestId": "",
            "DownloadPath": "",
            "VerifyBinPath" : "", //(DownloadPath + //.UD/verify.bin)
            "ManifestPath": "",
            "Priority": 0,
            "CurrentState": 5
        }
    ]
}
*/
