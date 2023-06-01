using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static upc_r2.Enums;

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
            string userId = Marshal.PtrToStringUTF8(inOptUserIdUtf8);
            UPC_AvatarSize size = (UPC_AvatarSize)inSize;
            Basics.Log(nameof(UPC_AvatarGet), new object[] { userId, size });
            /*
            var cbList = Main.GlobalContext.Callbacks.ToList();
            cbList.Add(new(inCallback, inCallbackData, -2));
            Main.GlobalContext.Callbacks = cbList.ToArray();
            */
            Basics.Log(nameof(UPC_AvatarGet), new object[] { "returner 0" });
            int AvSize = 0;
            switch (size)
            {
                case UPC_AvatarSize.UPC_AvatarSize_64x64:
                    AvSize = 16384;
                    break;
                case UPC_AvatarSize.UPC_AvatarSize_128x128:
                    AvSize = 65536;
                    break;
                case UPC_AvatarSize.UPC_AvatarSize_256x256:
                    AvSize = 262144;
                    break;
                default:
                    break;
            }

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
