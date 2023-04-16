using Microsoft.Win32.SafeHandles;

namespace LzhamWrapper
{
    public class CompressionHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public CompressionHandle() : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            Finish();
            return true;
        }

        public uint Finish()
        {
            handle = IntPtr.Zero;
            return LzhamInterop.CompressDeinit(handle);
        }
    }
}