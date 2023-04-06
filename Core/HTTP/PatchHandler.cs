namespace Core.HTTP
{
    internal class PatchHandler
    {
        public static byte[] PatchHandlerCallback(string url, out string contentType)
        {
            //  /patch/$version/files.txt
            // This file should contains the files for update
            //
            //  file    CRC32
            //
            //  All files should be compressed as zlib except the txt!!
            contentType = "application/octet-stream";
            return new byte[] { 0x00 };
        }
    }
}
