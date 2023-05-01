using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports
{
    internal class Avatar
    {
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_AvatarFree(IntPtr inContext, IntPtr inImageRGBA)
        {
            Basics.Log(nameof(UPC_AvatarFree), new object[] { inContext, inImageRGBA });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_AvatarGet(IntPtr inContext, IntPtr inOptUserIdUtf8, uint inSize, IntPtr outImageRGBA, IntPtr inCallback, IntPtr inCallbackData)
        {
            Basics.Log(nameof(UPC_AvatarGet), new object[] { inContext, inOptUserIdUtf8, inSize, outImageRGBA, inCallback, inCallbackData });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_BlacklistAdd(IntPtr inContext, IntPtr inUserIdUtf8, IntPtr inOptCallback, IntPtr inOptCallbackData)
        {
            Basics.Log(nameof(UPC_BlacklistAdd), new object[] { inContext, inUserIdUtf8, inOptCallback, inOptCallbackData });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_BlacklistHas(IntPtr inContext, IntPtr inUserIdUtf8)
        {
            Basics.Log(nameof(UPC_BlacklistHas), new object[] { inContext, inUserIdUtf8 });
            return 0;
        }
    }
}
