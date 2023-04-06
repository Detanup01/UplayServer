using Uplay.Download;
using File = Uplay.Download.File;

namespace Downloader
{
    internal class ChunkManager
    {
        public static List<File> RemoveNonEnglish(Manifest parsedManifest)
        {
            List<Chunk> chunks = parsedManifest.Chunks.Where(x => String.IsNullOrEmpty(x.Language)).ToList();

            List<File> files = new();

            foreach (var chunk in chunks)
                files = files.Concat(chunk.Files).ToList();

            return files;
        }

        public static List<File> AllFiles(Manifest parsedManifest)
        {
            var chunks = parsedManifest.Chunks.ToList();
            List<File> files = new();

            foreach (var chunk in chunks)
                files = files.Concat(chunk.Files).ToList();

            return files;
        }

        public static List<File> AddLanguage(Manifest parsedManifest, string Lang)
        {
            List<Chunk> chunks = parsedManifest.Chunks.Where(x => x.Language.Equals(Lang)).ToList();

            List<File> files = new();

            foreach (var chunk in chunks)
                files = files.Concat(chunk.Files).ToList();

            return files;
        }

        public static void RemoveSkipFiles(List<string> skip_files)
        {
            List<File> to_remove = new();
            if (skip_files.Count != 0)
            {
                foreach (var file in DLWorker.Config.FilesToDownload)
                {
                    foreach (var skip in skip_files)
                    {
                        if (file.Name.Contains(skip))
                        {
                            to_remove.Add(file);
                        }
                        /*
                        if (skip.StartsWith("regex:"))
                        {
                            var rgx = new Regex(skip.Substring(6), RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            var m = rgx.Match(file.Name);

                            if (!m.Success)
                                to_remove.Add(file);
                        }
                        */
                    }
                }
                foreach (var remove in to_remove)
                {
                    DLWorker.Config.FilesToDownload.Remove(remove);
                }

            }
        }

        public static List<File> AddDLOnlyFiles(List<string> add_files)
        {
            List<File> output = new();
            if (add_files.Count != 0)
            {
                foreach (var file in DLWorker.Config.FilesToDownload)
                {
                    foreach (var add in add_files)
                    {
                        if (file.Name.Contains(add))
                        {
                            output.Add(file);
                        }
                    }
                }
            }

            return output;
        }
    }
}
