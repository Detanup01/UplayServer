using ClientKit.Demux;
using ClientKit.Demux.Connection;
using Google.Protobuf;
using SharedLib.Shared;
using static Downloader.Saving;
using File = System.IO.File;
using UDFile = Uplay.Download.File;

namespace Downloader
{
    internal class DLWorker
    {
        public static void CreateNew()
        {
            Config = new Config();

        }

        public static Config Config;

        public static void DownloadWorker(DownloadConnection downloadConnection)
        {
            Console.WriteLine("\n\t\tDownloading Started!");
            int filecounter = 0;
            foreach (var file in Config.FilesToDownload)
            {
                if (file.Size == 0)
                    continue;

                filecounter++;

                List<string> sliceListIds = new();

                foreach (var sl in file.SliceList)
                {
                    sliceListIds.Add(Convert.ToHexString(sl.DownloadSha1.ToArray()));
                }

                List<string> sliceIds = new();

                foreach (var sl in file.Slices)
                {
                    sliceIds.Add(Convert.ToHexString(sl.ToArray()));
                }

                if (CheckCurrentFile(file, downloadConnection))
                    continue;

                var saving = Read();
                saving.Work.FileInfo = new()
                {
                    Name = file.Name,
                    IDs = new()
                    {
                        SliceList = sliceListIds,
                        Slices = sliceIds
                    }
                };
                Save(saving);
                Console.WriteLine($"\t\tFile {file.Name} started ({filecounter}/{Config.FilesToDownload.Count}) [{Formatters.FormatFileSize(file.Size)}]");
                DownloadFile(file, downloadConnection);
            }
            Console.WriteLine($"\t\tDownload for app {Config.ProductId} is done!");
        }

        public static bool CheckCurrentFile(UDFile file, DownloadConnection downloadConnection)
        {
            var saving = Read();
            if (saving.Work.FileInfo.Name != file.Name)
                return false;

            var curId = saving.Work.CurrentId;
            var NextId = saving.Work.NextId;
            var verifile = saving.Verify.Files.Where(x => x.Name == file.Name).FirstOrDefault();
            if (verifile == null)
                return false;

            List<string> slicesToDownload = new();
            int index = 0;
            uint Size = 0;
            if (saving.Compression.HasSliceSHA)
            {
                index = saving.Work.FileInfo.IDs.SliceList.FindIndex(0, x => x == curId);
                index += 1;
                slicesToDownload = saving.Work.FileInfo.IDs.SliceList.Skip(index).ToList();

            }
            else
            {
                index = saving.Work.FileInfo.IDs.Slices.FindIndex(0, x => x == curId);
                index += 1;
                slicesToDownload = saving.Work.FileInfo.IDs.Slices.Skip(index).ToList();
            }

            if (slicesToDownload.Count == 0)
                return false;

            var sizelister = file.SliceList.Take(index).ToList();
            foreach (var sizer in sizelister)
            {
                Size += sizer.Size;
            }


            var fullPath = Path.Combine(Config.VerifyBinPath, file.Name);
            var fileInfo = new System.IO.FileInfo(fullPath);
            if (fileInfo.Length != Size)
            {
                Console.WriteLine("Something isnt right, +/- chunk??");
                File.WriteAllText("x.txt", (uint)fileInfo.Length + " != " + Size + " " + sizelister.Count + " " + index + "\n" + curId + " " + NextId);
                // We try restore chunk after here
                //
                return false;
            }
            Console.WriteLine($"\t\tRedownloading File {file.Name}!");
            RedownloadSlices(slicesToDownload, file, downloadConnection);
            return true;
        }

        public static void RedownloadSlices(List<string> slicesToDownload, UDFile file, DownloadConnection downloadConnection)
        {
            var fullPath = Path.Combine(Config.VerifyBinPath, file.Name);

            var prevBytes = File.ReadAllBytes(fullPath);

            var fs = File.OpenWrite(fullPath);
            fs.Position = prevBytes.LongLength;
            if (slicesToDownload.Count > 30)
            {
                Console.WriteLine("\tBig Slice! " + slicesToDownload.Count);
                var splittedList = slicesToDownload.SplitList(10);
                for (int listcounter = 0; listcounter < splittedList.Count; listcounter++)
                {
                    var spList = splittedList[listcounter];
                    var dlbytes = ByteDownloader.DownloadBytes(file, spList.ToList(), downloadConnection);
                    foreach (var barray in dlbytes)
                    {
                        fs.Write(barray);
                        fs.Flush(true);
                    }
                    if (listcounter % 10 == 0)
                    {
                        Console.WriteLine("%");
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(10);
                }
            }
            else
            {
                var dlbytes = ByteDownloader.DownloadBytes(file, slicesToDownload, downloadConnection);
                foreach (var barray in dlbytes)
                {
                    fs.Write(barray);
                    fs.Flush(true);
                }
            }
            fs.Close();
        }

        public static void DownloadFile(UDFile file, DownloadConnection downloadConnection)
        {
            var saving = Read();
            var fullPath = Path.Combine(Config.DownloadDirectory, file.Name);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            var fs = File.OpenWrite(fullPath);
            if (saving.Compression.HasSliceSHA)
            {
                if (file.SliceList.Count > 30)
                {
                    Console.WriteLine("\tBig Slice! " + file.SliceList.Count);
                    var splittedList = file.SliceList.ToList().SplitList<Uplay.Download.Slice>(10);
                    for (int listcounter = 0; listcounter < splittedList.Count; listcounter++)
                    {
                        var spList = splittedList[listcounter];
                        var dlbytes = ByteDownloader.DownloadBytes(file.Name, spList.ToList(), downloadConnection);
                        Console.WriteLine(spList.Count + " =?= " + dlbytes.Count);
                        for (int i = 0; i < spList.Count; i++)
                        {
                            var sp = spList[i];
                            var barray = dlbytes[i];
                            if (Config.DownloadAsChunks)
                            {
                                var sliceId = Convert.ToHexString(sp.DownloadSha1.ToArray());
                                var slicepath = SliceManager.GetSlicePath(sliceId, (uint)saving.Version);
                                File.WriteAllBytes(Path.Combine(Config.DownloadDirectory, slicepath), barray);
                            }
                            else
                            {
                                fs.Write(barray);
                                fs.Flush(true);
                            }
                        }
                        if (listcounter % 10 == 0)
                        {
                            Thread.Sleep(1000);
                        }
                        Thread.Sleep(10);
                    }
                }
                else
                {
                    var dlbytes = ByteDownloader.DownloadBytes(file.Name, file.SliceList.ToList(), downloadConnection);
                    Console.WriteLine(file.SliceList.ToList().Count + " =?= " + dlbytes.Count);
                    for (int i = 0; i < file.SliceList.ToList().Count; i++)
                    {
                        var sp = file.SliceList.ToList()[i];
                        var barray = dlbytes[i];
                        if (Config.DownloadAsChunks)
                        {
                            var sliceId = Convert.ToHexString(sp.DownloadSha1.ToArray());
                            var slicepath = SliceManager.GetSlicePath(sliceId, (uint)saving.Version);
                            File.WriteAllBytes(Path.Combine(Config.DownloadDirectory, slicepath), barray);
                        }
                        else
                        {
                            fs.Write(barray);
                            fs.Flush(true);
                        }
                    }
                }
            }
            else
            {
                if (file.Slices.Count > 30)
                {
                    Console.WriteLine("\tBig Slice! " + file.Slices.Count);
                    var splittedList = file.Slices.ToList().SplitList<ByteString>(10);
                    for (int listcounter = 0; listcounter < splittedList.Count; listcounter++)
                    {
                        var spList = splittedList[listcounter];
                        var dlbytes = ByteDownloader.DownloadBytes(file, spList.ToList(), downloadConnection);
                        Console.WriteLine(spList.ToList().Count + " =?= " + dlbytes.Count);
                        for (int i = 0; i < spList.ToList().Count; i++)
                        {
                            var sp = spList.ToList()[i];
                            var barray = dlbytes[i];
                            if (Config.DownloadAsChunks)
                            {
                                var sliceId = Convert.ToHexString(sp.ToArray());
                                var slicepath = SliceManager.GetSlicePath(sliceId, (uint)saving.Version);
                                File.WriteAllBytes(Path.Combine(Config.DownloadDirectory, slicepath), barray);
                            }
                            else
                            {
                                fs.Write(barray);
                                fs.Flush(true);
                            }
                        }

                        if (listcounter % 10 == 0)
                        {
                            Thread.Sleep(1000);
                        }
                        Thread.Sleep(10);
                    }

                }
                else
                {
                    var dlbytes = ByteDownloader.DownloadBytes(file, file.Slices.ToList(), downloadConnection);
                    Console.WriteLine(file.Slices.Count + " =?= " + dlbytes.Count);
                    for (int i = 0; i < file.Slices.ToList().Count; i++)
                    {
                        var sp = file.Slices.ToList()[i];
                        var barray = dlbytes[i];
                        if (Config.DownloadAsChunks)
                        {
                            var sliceId = Convert.ToHexString(sp.ToArray());
                            var slicepath = SliceManager.GetSlicePath(sliceId, (uint)saving.Version);
                            File.WriteAllBytes(Path.Combine(Config.DownloadDirectory, slicepath), barray);
                        }
                        else
                        {
                            fs.Write(barray);
                            fs.Flush(true);
                        }
                    }
                }

            }
            fs.Close();
            Console.WriteLine($"\t\tFile {file.Name} finished");
            if (Config.DownloadAsChunks)
            {
                //we delete the file because we arent even writing to it :)
                File.Delete(fullPath);
            }

        }
    }
}
