using SharedLib.Server.Json;
using SharedLib.Shared;

namespace Core.HTTP
{
    internal class PatchHandler
    {
        public static byte[] PatchHandlerCallback(string url, out string contentType)
        {
            byte[] returner = { };
            url = url.Replace("/patch", "");
            if (File.Exists($"{ServerConfig.Instance.Demux.ServerFilesPath}Patch{url}"))
            {
                returner = File.ReadAllBytes($"{ServerConfig.Instance.Demux.ServerFilesPath}Patch{url}");
                if (!url.Contains("files.txt"))
                {
                    returner = System.Text.Encoding.UTF8.GetBytes(CompressB64.GetZstdB64(returner));
                }
            }
            contentType = "application/octet-stream";
            return returner;
        }
    }
}
