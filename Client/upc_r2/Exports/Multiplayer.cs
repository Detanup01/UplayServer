using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports
{
    internal class Multiplayer
    {
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_MultiplayerInvite(IntPtr inContext, IntPtr inUserIdUtf8, IntPtr inOptCallback, IntPtr inOptCallbackData)
        {
            Basics.Log(nameof(UPC_MultiplayerInvite), new object[] { inContext, inUserIdUtf8, inOptCallback, inOptCallbackData });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_MultiplayerInviteAnswer(IntPtr inContext, IntPtr inSenderIdUtf8, int inIsAccepted, IntPtr inOptCallback, IntPtr inOptCallbackData)
        {
            Basics.Log(nameof(UPC_MultiplayerInviteAnswer), new object[] { inContext, inSenderIdUtf8, inIsAccepted, inOptCallback, inOptCallbackData });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_MultiplayerSessionClear(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_MultiplayerSessionClear), new object[] { inContext });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_MultiplayerSessionFree(IntPtr inContext, IntPtr inMultiplayerSession)
        {
            Basics.Log(nameof(UPC_MultiplayerSessionFree), new object[] { inContext });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_MultiplayerSessionSet(IntPtr inContext, IntPtr inMultiplayerSession)
        {
            Basics.Log(nameof(UPC_MultiplayerSessionSet), new object[] { inContext });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_MultiplayerSessionGet(IntPtr inContext, IntPtr outMultiplayerSession)
        {
            Basics.Log(nameof(UPC_MultiplayerSessionGet), new object[] { inContext });
            return 0;
        }
    }
}
