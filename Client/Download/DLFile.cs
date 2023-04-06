namespace Downloader
{
    internal class DLFile
    {
        public static List<Uplay.Download.File> FileNormalizer(List<Uplay.Download.File> files)
        {
            files.ForEach(x => x.Name.Replace('\\', '/'));
            return files;
        }
    }
}
