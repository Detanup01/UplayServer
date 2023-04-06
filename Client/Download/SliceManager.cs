using ClientKit.Demux;
using ClientKit.Demux.Connection;
using ClientKit.Lib;
using Uplay.Download;

namespace Downloader
{
    internal class SliceManager
    {
        public static List<string> SliceWorker(List<Slice> slices, DownloadConnection downloadConnection, uint Version)
        {
            List<string> listOfSliceIds = new();
            foreach (var slice in slices)
            {
                if (slice == null)
                {
                    Console.WriteLine("Your slice is Null?");
                }

                if (slice.HasFileOffset) { Console.WriteLine("[!!!] FILE OFFSET! " + slice.FileOffset); }
                string sliceId = Convert.ToHexString(slice.DownloadSha1.ToArray());
                listOfSliceIds.Add(GetSlicePath(sliceId, Version));
            }
            return GetUrlsForSlices(listOfSliceIds, downloadConnection);
        }

        public static List<string> SliceWorker(List<string> slices, DownloadConnection downloadConnection, uint Version)
        {
            List<string> listOfSliceIds = new();
            foreach (var slice in slices)
            {
                listOfSliceIds.Add(GetSlicePath(slice, Version));
            }
            return GetUrlsForSlices(listOfSliceIds, downloadConnection);
        }

        public static string GetSlicePath(string Slice, uint Version)
        {
            if (Version == 3)
            {
                return ($"slices_v3/{Formatters.FormatSliceHashChar(Slice)}/{Slice}");
            }
            else if (Version == 2)
            {
                return ($"slices_v2/{Slice}");
            }
            else
            {
                return ($"slices/{Slice}");
            }
        }


        public static List<string> GetUrlsForSlices(List<string> listOfSliceIds, DownloadConnection downloadConnection)
        {
            Program.CheckOW(DLWorker.Config.ProductId);
            if (downloadConnection.isConnectionClosed)
            {
                downloadConnection.Reconnect();
                if (downloadConnection.isServiceSuccess)
                {
                    bool InitTrue = downloadConnection.InitDownloadToken(Program.OWToken);

                    if (!InitTrue)
                    {
                        Console.WriteLine("The ownership Token has been expired! Quiting and please try again!");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    Console.WriteLine("Cannot reconnect!");
                    Environment.Exit(1);
                }

            }
            return downloadConnection.GetUrlList(DLWorker.Config.ProductId, listOfSliceIds);
        }

        public static byte[] Decompress(Saving.Root saved, byte[] downloadedSlice, ulong outputsize)
        {
            return DeComp.Decompress(saved.Compression.IsCompressed, saved.Compression.Method, downloadedSlice, outputsize);
        }
    }
}
