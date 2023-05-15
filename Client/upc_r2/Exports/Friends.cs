using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports
{
    internal class Friends
    {
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_FriendAdd(IntPtr inContext, IntPtr inSearchStringUtf8, IntPtr inOptCallback, IntPtr inOptCallbackData)
        {
            Basics.Log(nameof(UPC_FriendAdd), new object[] { inContext, inSearchStringUtf8, inOptCallback, inOptCallbackData });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_FriendRemove(IntPtr inContext, IntPtr inUserIdUtf8, IntPtr inOptCallback, IntPtr inOptCallbackData)
        {
            Basics.Log(nameof(UPC_FriendRemove), new object[] { inContext, inUserIdUtf8, inOptCallback, inOptCallbackData });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_FriendCheck(IntPtr inContext, IntPtr inUserIdUtf8)
        {
            Basics.Log(nameof(UPC_FriendCheck), new object[] { inContext, inUserIdUtf8 });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_FriendListFree(IntPtr inContext, IntPtr inUserIdUtf8)
        {
            Basics.Log(nameof(UPC_FriendListFree), new object[] { inContext, inUserIdUtf8 });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_FriendListGet(IntPtr inContext, uint inOptOnlineStatusFilter, IntPtr outFriendList, IntPtr inCallback, IntPtr inOptCallbackData)
        {
            Basics.Log(nameof(UPC_FriendListGet), new object[] { inContext, inOptOnlineStatusFilter, outFriendList, inCallback, inOptCallbackData });
            return 0;
        }
    }
}
