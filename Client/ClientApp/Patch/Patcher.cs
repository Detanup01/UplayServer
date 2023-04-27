using Google.Protobuf;
using ICSharpCode.SharpZipLib.Checksum;
using RestSharp;
using SharedLib.Shared;
using System.Text;

namespace Client.Patch
{
    internal class Patcher
    {
        public static void Main(Uplay.Demux.GetPatchInfoRsp rsp)
        {
            var url = rsp.PatchBaseUrl;
            var latest = rsp.LatestVersion;
            var baseurl = url + latest;
            if (rsp.PatchTrackId == "DEFAULT")
            {
                var client = new RestClient(baseurl + "/files.txt");
                var request = new RestRequest();
                var bytes = ClientKit.UbiServices.Rest.GetBytes(client, request);

                File.WriteAllBytes($"patch_{latest}.txt", bytes);

                var patchdata = Encoding.UTF8.GetString(bytes);
                DoUpdate(baseurl, patchdata);
            }     
        }

        public static void DoUpdate(string baseurl, string patchdata)
        {
            //check if more than one file
            if (patchdata.Contains("\n"))
            {

            }
            else //only one file
            {
                var splitted = patchdata.Split("\t");
                var crc = splitted[0];
                var filename = splitted[1];

                var data = GetData(baseurl + "/" + filename);

                Crc32 crc32 = new();
                crc32.Update(data);
                var crcvalue = crc32.Value;
                Console.WriteLine(crcvalue + " =?= " + crc);
            }
        }

        public static byte[] GetData(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest();
            var bytes = ClientKit.UbiServices.Rest.GetBytes(client, request);
            if (bytes == null)
                return new byte[] { };

            var patchdata = CompressB64.GetUnZstdB64(bytes);
            ByteString byteString = ByteString.FromBase64(patchdata);
            return byteString.ToArray();
        }
    }
}
