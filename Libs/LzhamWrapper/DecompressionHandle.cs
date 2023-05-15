using Microsoft.Win32.SafeHandles;

namespace LzhamWrapper
{
    public class DecompressionHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public DecompressionHandle() : base(true)
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
            return LzhamInterop.DecompressDeinit(handle);
        }
    }
}
