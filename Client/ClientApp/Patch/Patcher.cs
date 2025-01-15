using Google.Protobuf;
using ICSharpCode.SharpZipLib.Checksum;
using RestSharp;
using SharedLib.Shared;
using System.Text;

namespace Client.Patch
{
    internal class Patcher
    {
        public static void MainPatch(Uplay.Demux.GetPatchInfoRsp rsp)
        {
            var url = rsp.PatchBaseUrl;
            var latest = rsp.LatestVersion;
            var baseurl = url + latest;
            /*
            if (rsp.PatchTrackId == "DEFAULT")
            {
                var client = new RestClient(baseurl + "/files.txt");
                var request = new RestRequest();
                var bytes = UbiServices.Rest.GetBytes(client, request);
                var patchdata = Encoding.UTF8.GetString(bytes);
                DoUpdate(baseurl, patchdata);
            }     
            */
        }

        public static void DoUpdate(string baseurl, string patchdata)
        {
            //check if more than one file
            if (patchdata.Contains("\n"))
            {
                foreach (var item in patchdata.Split("\n"))
                {
                    Console.WriteLine(item);
                    var splitted = item.Split("\t");
                    var crc = splitted[0];
                    var filename = splitted[1];
                    filename = filename.Trim();

                    UpdateFile(baseurl, crc, filename);
                }
            }
            else //only one file
            {
                var splitted = patchdata.Split("\t");
                var crc = splitted[0];
                var filename = splitted[1];
                filename = filename.Trim();

                UpdateFile(baseurl, crc, filename);
            }
        }

        public static void UpdateFile(string baseurl, string crc, string filename)
        {
            Console.WriteLine("|:" + filename + ":|");
            var data = GetData(baseurl + "/" + filename);
            Directory.CreateDirectory("Patch");
            var dfile = Path.GetDirectoryName(filename);
            if (dfile != null && dfile.Trim() != string.Empty)
            {
                Directory.CreateDirectory(Path.Combine("Patch",dfile));
            }

            Crc32 crc32 = new();
            crc32.Update(data);
            var crcvalue = crc32.Value;
            var crcstring = string.Format("{0:X}", crcvalue);

            if (crcstring == crc)
            {
                Console.WriteLine("File OK!");
                File.WriteAllBytes(Path.Combine("Patch", filename), data);
            }
            else
            {
                Console.WriteLine("CRC MISSMATCH!");
            }
        }


        public static byte[] GetData(string url)
        {
            return [];
            /*
            var client = new RestClient(url);
            var request = new RestRequest();
            var bytes = UbiServices.Rest.GetBytes(client, request);
            if (bytes == null)
                return [];

            return unzstd(bytes);*/
        }

        public static byte[] unzstd(byte[] bytes)
        {
            ByteString byteString = ByteString.FromBase64(Encoding.UTF8.GetString(bytes));
            var patchdata = CompressB64.GetUnZstdB64(byteString.ToArray());
            byteString = ByteString.FromBase64(patchdata);
            return byteString.ToArray();
        }
    }
}
