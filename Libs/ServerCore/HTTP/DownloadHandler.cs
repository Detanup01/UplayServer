using ServerCore.Json;

namespace Core.HTTP
{
    internal class DownloadHandler
    {
        public static byte[] DownloadHandlerCallback(string url, out string contentType)
        {
            byte[] returner = { };
            url = url.Replace("/download", "");
            if (File.Exists($"{ServerConfig.Instance.Demux.DownloadGamePath}{url}"))
            {
                returner = File.ReadAllBytes($"{ServerConfig.Instance.Demux.DownloadGamePath}{url}");
            }
            contentType = "application/octet-stream";
            return returner;
        }
    }
}
