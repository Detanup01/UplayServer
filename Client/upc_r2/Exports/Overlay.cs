using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports
{
    internal class Overlay
    {
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_ShowBrowserUrl(IntPtr inContext, IntPtr inBrowserUrlUtf8)
        {
            Basics.Log(nameof(UPC_OverlayBrowserUrlShow), new object[] { inContext, inBrowserUrlUtf8 });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_OverlayBrowserUrlShow(IntPtr inContext, IntPtr inBrowserUrlUtf8, IntPtr inOptCallback, IntPtr inOptCallbackData)
        {
            Basics.Log(nameof(UPC_OverlayBrowserUrlShow), new object[] { inContext, inBrowserUrlUtf8, inOptCallback, inOptCallbackData });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_OverlayFriendInvitationShow(IntPtr inContext, IntPtr inOptIdListUtf8, uint inOptIdListLength)
        {
            Basics.Log(nameof(UPC_OverlayFriendInvitationShow), new object[] { inContext, inOptIdListUtf8, inOptIdListLength });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_OverlayFriendSelectionFree(IntPtr inContext, IntPtr inSelectedFriends)
        {
            Basics.Log(nameof(UPC_OverlayFriendSelectionFree), new object[] { inContext, inSelectedFriends });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_OverlayFriendSelectionShow(IntPtr inContext, IntPtr inIdListUtf8, uint inIdListLength, IntPtr outSelectedFriends, IntPtr inCallback, IntPtr inCallbackData)
        {
            Basics.Log(nameof(UPC_OverlayFriendSelectionShow), new object[] { inContext, inIdListUtf8, inIdListLength, outSelectedFriends, inCallback, inCallbackData });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_OverlayMicroAppShow(IntPtr inContext, IntPtr inOptMicroAppParamList, IntPtr inOptCallback, IntPtr inOptCallbackData)
        {
            Basics.Log(nameof(UPC_OverlayMicroAppShow), new object[] { inContext, inOptMicroAppParamList, inOptCallback, inOptCallbackData });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_OverlayNotificationShow(IntPtr inContext, uint inId)
        {
            Basics.Log(nameof(UPC_OverlayNotificationShow), new object[] { inContext, inId });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_OverlayShow(IntPtr inContext, uint inSection, IntPtr inOptCallback, IntPtr inOptCallbackData)
        {
            Basics.Log(nameof(UPC_OverlayShow), new object[] { inContext, inSection, inOptCallback, inOptCallbackData });
            return 0;
        }
    }
}
