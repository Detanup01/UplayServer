using System.Runtime.InteropServices;

namespace SharedLib.Shared.Lzham
{
    public class LzhamPoint
    {
        [DllImport("x64/lzham_api", EntryPoint = "decompress")]
        public static extern int Decompress_Uplay_x64([In] byte[] input, ulong inputsize, [Out] byte[] output, ulong outputsize);

        [DllImport("x86/lzham_api", EntryPoint = "decompress")]
        public static extern int Decompress_Uplay_x86([In] byte[] input, ulong inputsize, [Out] byte[] output, ulong outputsize);
    }
}
