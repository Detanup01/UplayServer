using ClientKit.Demux;
using ClientKit.Demux.Connection;
using ClientKit.Lib;
using ClientKit.UbiServices.Records;
using Newtonsoft.Json;
using RestSharp;
using SharedLib.Shared;

namespace Downloader
{
    public class Program
    {
        public static string OWToken = "";
        public static ulong Exp = 0;
        public static OwnershipConnection? ownershipConnection = null;
        public static Socket? socket = null;
        public static bool IsCustomManifest = false;
        public static bool Main(string[] args, Socket? argsocket)
        {
            #region Argument thingy
            DLWorker.CreateNew();
            //var debug = ParameterLib.HasParameter(args, "-debug");
            Debug.IsDebug = true;
            int WaitTime = ParameterLib.GetParameter(args, "-time", 5);
            DLWorker.Config.ProductId = ParameterLib.GetParameter<uint>(args, "-product", 0);
            DLWorker.Config.ManifestId = ParameterLib.GetParameter(args, "-manifest", "");
            string manifest_path = ParameterLib.GetParameter(args, "-manifestpath", "");
            bool hasAddons = ParameterLib.HasParameter(args, "-addons");
            string lang = ParameterLib.GetParameter(args, "-lang", "default");
            DLWorker.Config.DownloadDirectory = ParameterLib.GetParameter(args, "-dir", $"{Directory.GetCurrentDirectory()}\\{DLWorker.Config.ProductId}\\{DLWorker.Config.ManifestId}\\");
            DLWorker.Config.UsingFileList = ParameterLib.HasParameter(args, "-skip");
            DLWorker.Config.UsingOnlyFileList = ParameterLib.HasParameter(args, "-only");
            string skipping = ParameterLib.GetParameter(args, "-skip", "skip.txt");
            string onlygetting = ParameterLib.GetParameter(args, "-only", "only.txt");
            DLWorker.Config.Verify = ParameterLib.GetParameter(args, "-verify", true);
            bool hasVerifyPrint = ParameterLib.HasParameter(args, "-vp");
            DLWorker.Config.DownloadAsChunks = ParameterLib.GetParameter(args, "-onlychunk", false);
            bool hasticket = ParameterLib.HasParameter(args, "-ticket");
            string ticket = "";
            if (hasticket)
            {
                ticket = ParameterLib.GetParameter(args, "-ticket", "");
            }
            Console.WriteLine(JsonConvert.SerializeObject(DLWorker.Config));

            if (DLWorker.Config.UsingFileList && DLWorker.Config.UsingOnlyFileList)
            {
                Console.WriteLine("-skip and -only cannot be used in same time!");
                return false;
            }
            #endregion
            #region Login
            if (ticket == "")
            {
                LoginJson? login = LoginLib.TryLoginWithArgsCLI(args);
                if (login == null)
                {
                    Console.WriteLine("Login failed");
                    Environment.Exit(1);
                }
                ticket = login.Ticket;
            }
            #endregion
            #region Starting Connections, Getting game
            if (argsocket != null)
            {
                socket = argsocket;
            }
            else
            {
                socket = new();
            }

            socket.WaitInTimeMS = WaitTime;
            if (!socket.IsAuthed)
            {
                Console.WriteLine("Is same Version? " + socket.VersionCheck());
                socket.PushVersion();
                bool IsAuthSuccess = socket.Authenticate(ticket);
                Console.WriteLine("Is Auth Success? " + IsAuthSuccess);
                if (!IsAuthSuccess)
                {
                    Console.WriteLine("Oops something is wrong!");
                    return false;
                }
            }

            ownershipConnection = new(socket);
            DownloadConnection downloadConnection = new(socket);
            var owned = ownershipConnection.GetOwnedGames(false);
            if (owned == null || owned.Count == 0)
            {
                Console.WriteLine("No games owned?!");
                return false;
            }
            #endregion
            #region Game printing & Argument Check
            Uplay.Download.Manifest parsedManifest = new();
            RestClient rc = new();

            if (DLWorker.Config.ProductId == 0 && DLWorker.Config.ManifestId == "")
            {
                owned = owned.Where(game => game.LatestManifest.Trim().Length > 0 && game.ProductType == (uint)Uplay.Ownership.OwnedGame.Types.ProductType.Game).ToList();

                Console.WriteLine("-1) Your games:.");
                Console.WriteLine("----------------------");
                int gameIds = 0;
                foreach (var game in owned)
                {
                    Console.WriteLine($"({gameIds}) ProductId ({game.ProductId}) Manifest {game.LatestManifest}");
                    gameIds++;
                }
                Console.WriteLine("Please select:");
                Console.ReadLine();

                int selection = int.Parse(Console.ReadLine()!);
                if (selection == -1)
                {
                    Console.WriteLine("> Input the 20-byte long manifest identifier:");
                    DLWorker.Config.ManifestId = Console.ReadLine()!.Trim();

                    Console.WriteLine("> Input the productId:");
                    DLWorker.Config.ProductId = uint.Parse(Console.ReadLine()!.Trim());
                }
                else if (selection <= gameIds)
                {
                    DLWorker.Config.ManifestId = owned[selection].LatestManifest;
                    DLWorker.Config.ProductId = owned[selection].ProductId;
                }

                DLWorker.Config.DownloadDirectory = ParameterLib.GetParameter(args, "-dir", $"{Directory.GetCurrentDirectory()}\\{DLWorker.Config.ProductId}\\{DLWorker.Config.ManifestId}\\");
                DLWorker.Config.ProductManifest = $"{DLWorker.Config.ProductId}_{DLWorker.Config.ManifestId}";

                if (!Directory.Exists(DLWorker.Config.DownloadDirectory.Replace($"{DLWorker.Config.ManifestId}\\", $"{DLWorker.Config.ManifestId}")))
                {
                    Directory.CreateDirectory(DLWorker.Config.DownloadDirectory.Replace($"{DLWorker.Config.ManifestId}\\", $"{DLWorker.Config.ManifestId}"));
                }

                // Getting ownership token
                var ownershipToken = ownershipConnection.GetOwnershipToken(DLWorker.Config.ProductId);
                if (ownershipConnection.IsServiceSuccess == false) { throw new("Product not owned"); }
                OWToken = ownershipToken.Token;
                Exp = ownershipToken.Expiration;
                Console.WriteLine($"Expires in {GetTimeFromEpoc(Exp)}");
                downloadConnection.InitDownloadToken(OWToken);

                if (manifest_path != "")
                {
                    File.Copy(manifest_path, DLWorker.Config.DownloadDirectory + "uplay_install.manifest", true);
                    parsedManifest = Parsers.ParseManifestFile(manifest_path);
                    var signature = Parsers.GetManifestSignature(manifest_path);
                    if (signature.StartsWith("START") && signature.EndsWith("END"))
                        IsCustomManifest = true;
                }
                else
                {
                    string manifestUrl = downloadConnection.GetUrl(DLWorker.Config.ManifestId, DLWorker.Config.ProductId);

                    if (manifestUrl == "")
                    {
                        throw new("manifestUrl?");
                    }
                    var manifestBytes = rc.DownloadData(new(manifestUrl));
                    if (manifestBytes == null)
                        throw new("Manifest not found?");

                    File.WriteAllBytes(DLWorker.Config.ProductManifest + ".manifest", manifestBytes);
                    parsedManifest = Parsers.ParseManifestFile(DLWorker.Config.ProductManifest + ".manifest");
                    var signature = Parsers.GetManifestSignature(manifest_path);
                    if (signature.StartsWith("START") && signature.EndsWith("END"))
                        IsCustomManifest = true;
                }
                Console.WriteLine(IsCustomManifest);
            }
            #endregion
            #region Game from Argument
            else
            {
                var ownershipToken = ownershipConnection.GetOwnershipToken(DLWorker.Config.ProductId);
                if (ownershipConnection.IsServiceSuccess == false) { throw new("Product not owned"); }
                OWToken = ownershipToken.Item1;
                Exp = ownershipToken.Item2;
                Console.WriteLine($"Expires in {GetTimeFromEpoc(Exp)}");
                downloadConnection.InitDownloadToken(OWToken);
                if (manifest_path != "")
                {
                    File.Copy(manifest_path, DLWorker.Config.DownloadDirectory + "uplay_install.manifest", true);
                    parsedManifest = Parsers.ParseManifestFile(manifest_path);
                }
                else
                {
                    string manifestUrl = downloadConnection.GetUrl(DLWorker.Config.ManifestId, DLWorker.Config.ProductId);

                    var manifestBytes = rc.DownloadData(new(manifestUrl));
                    if (manifestBytes == null)
                        throw new("Manifest not found?");

                    File.WriteAllBytes(DLWorker.Config.ProductManifest + ".manifest", manifestBytes);
                    File.Copy(DLWorker.Config.ProductManifest + ".manifest", DLWorker.Config.DownloadDirectory + "uplay_install.manifest", true);
                    parsedManifest = Parsers.ParseManifestFile(DLWorker.Config.ProductManifest + ".manifest");
                }
            }
            #endregion
            #region Addons check
            if (hasAddons)
            {
                string LicenseURL = downloadConnection.GetUrl(DLWorker.Config.ManifestId, DLWorker.Config.ProductId, "license");
                var License = rc.DownloadData(new(LicenseURL));
                if (License == null)
                    throw new("License not found?");
                File.WriteAllBytes(DLWorker.Config.ProductManifest + ".license", License);

                string MetadataURL = downloadConnection.GetUrl(DLWorker.Config.ManifestId, DLWorker.Config.ProductId, "metadata");
                var Metadata = rc.DownloadData(new(MetadataURL));
                if (Metadata == null)
                    throw new("Metadata not found?");
                File.WriteAllBytes(DLWorker.Config.ProductManifest + ".metadata", Metadata);
            }
            rc.Dispose();
            #endregion
            #region Compression Print
            Console.WriteLine($"\nDownloaded and parsed manifest successfully:");
            Console.WriteLine($"Compression Method: {parsedManifest.CompressionMethod} IsCompressed? {parsedManifest.IsCompressed} Version {parsedManifest.Version}");
            #endregion
            #region Lang Chunks
            List<Uplay.Download.File> files = new();

            if (parsedManifest.Languages.ToList().Count > 0)
            {
                if (lang == "default")
                {
                    Console.WriteLine("Languages to use (just press enter to choose nothing, and all for all chunks)");
                    parsedManifest.Languages.ToList().ForEach(x => Console.WriteLine(x.Code));

                    var langchoosed = Console.ReadLine();

                    if (!string.IsNullOrEmpty(langchoosed))
                    {
                        if (langchoosed == "all")
                        {
                            files = ChunkManager.AllFiles(parsedManifest);
                        }
                        else
                        {
                            files.AddRange(ChunkManager.RemoveNonEnglish(parsedManifest));
                            lang = langchoosed;
                            files.AddRange(ChunkManager.AddLanguage(parsedManifest, lang));
                        }
                    }
                    else
                    {
                        files.AddRange(ChunkManager.RemoveNonEnglish(parsedManifest));

                    }
                }
                else if (lang == "all")
                {
                    files = ChunkManager.AllFiles(parsedManifest);
                }
                else
                {
                    files.AddRange(ChunkManager.RemoveNonEnglish(parsedManifest));
                    files.AddRange(ChunkManager.AddLanguage(parsedManifest, lang));
                }
            }
            else
            {
                files = ChunkManager.AllFiles(parsedManifest);
            }
            #endregion
            #region Skipping files from chunk
            DLWorker.Config.FilesToDownload = DLFile.FileNormalizer(files);
            List<string> skip_files = new();
            if (DLWorker.Config.UsingFileList)
            {
                if (File.Exists(skipping))
                {
                    var lines = File.ReadAllLines(skipping);
                    skip_files.AddRange(lines);
                    Console.WriteLine("Skipping files Added");
                }
                ChunkManager.RemoveSkipFiles(skip_files);
            }
            if (DLWorker.Config.UsingOnlyFileList)
            {
                if (File.Exists(onlygetting))
                {
                    var lines = File.ReadAllLines(onlygetting);
                    skip_files.AddRange(lines);
                    Console.WriteLine("Download only Added");
                }
                DLWorker.Config.FilesToDownload = ChunkManager.AddDLOnlyFiles(skip_files);
            }
            Console.WriteLine("\tFiles Ready to work\n");
            #endregion
            #region Saving
            Saving.Root saving = new();
            DLWorker.Config.VerifyBinPath = Path.Combine(DLWorker.Config.DownloadDirectory, ".UD\\verify.bin");
            Directory.CreateDirectory(Path.GetDirectoryName(DLWorker.Config.VerifyBinPath));
            if (File.Exists(DLWorker.Config.VerifyBinPath))
            {
                var readedBin = Saving.Read();
                if (readedBin == null)
                {
                    saving = Saving.MakeNew(DLWorker.Config.ProductId, DLWorker.Config.ManifestId, parsedManifest);
                }
                else
                {
                    saving = readedBin;
                }
            }
            else
            {
                saving = Saving.MakeNew(DLWorker.Config.ProductId, DLWorker.Config.ManifestId, parsedManifest);
            }
            if (hasVerifyPrint)
            {
                File.WriteAllText(DLWorker.Config.VerifyBinPath + ".json", JsonConvert.SerializeObject(saving));
                Console.ReadLine();
            }
            Saving.Save(saving);
            #endregion
            #region Verify + Downloading
            if (DLWorker.Config.Verify && !DLWorker.Config.DownloadAsChunks)
            {
                Verifier.Verify();
            }
            /*
            var resRoot = AutoRes.MakeNew(DLWorker.Config.ProductId, DLWorker.Config.ManifestId, DLWorker.Config.DownloadDirectory, DLWorker.Config.VerifyBinPath, Path.Combine(DLWorker.Config.DownloadDirectory, "uplay_install.manifest"));
            AutoRes.Save(resRoot);
            */
            DLWorker.DownloadWorker(downloadConnection);
            #endregion
            #region Closing and GoodBye
            Console.WriteLine("Goodbye!");
            Console.ReadLine();
            downloadConnection.Close();
            ownershipConnection.Close();
            socket.Disconnect();
            socket.Dispose();
            return true;
            #endregion
        }
        #region Other Functions

        static DateTime GetTimeFromEpoc(ulong epoc)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds(epoc);
        }

        static ulong GetEpocTime()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (ulong)t.TotalSeconds;

        }

        public static void CheckOW(uint ProdId)
        {
            if (Exp <= GetEpocTime())
            {
                Console.WriteLine("Your token has no more valid, getting new!");
                if (ownershipConnection != null && !ownershipConnection.IsConnectionClosed)
                {
                    var token = ownershipConnection.GetOwnershipToken(ProdId);
                    Console.WriteLine("Is Token get success? " + ownershipConnection.IsServiceSuccess);
                    Exp = token.Item2;
                    OWToken = token.Item1;
                }
            }
        }
        #endregion
    }
}