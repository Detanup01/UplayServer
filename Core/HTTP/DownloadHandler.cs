namespace Core.HTTP
{
    internal class DownloadHandler
    {
        public static byte[] DownloadHandlerCallback(string url, out string contentType)
        {
            byte[] returner = { };
            url = url.Replace("/download/", "");
            if (File.Exists($"{Config.DMX.DownloadGamePath}{url}"))
            {
                returner = File.ReadAllBytes($"{Config.DMX.DownloadGamePath}{url}");
            }
            contentType = "application/octet-stream";
            return returner;
        }
    }
}
