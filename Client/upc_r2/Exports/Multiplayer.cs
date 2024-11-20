using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports;

internal class Multiplayer
{
    [UnmanagedCallersOnly(EntryPoint = "UPC_MultiplayerInvite", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_MultiplayerInvite(IntPtr inContext, IntPtr inUserIdUtf8, IntPtr inOptCallback, IntPtr inOptCallbackData)
    {
        Basics.Log(nameof(UPC_MultiplayerInvite), [inContext, inUserIdUtf8, inOptCallback, inOptCallbackData]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_MultiplayerInviteAnswer", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_MultiplayerInviteAnswer(IntPtr inContext, IntPtr inSenderIdUtf8, int inIsAccepted, IntPtr inOptCallback, IntPtr inOptCallbackData)
    {
        Basics.Log(nameof(UPC_MultiplayerInviteAnswer), [inContext, inSenderIdUtf8, inIsAccepted, inOptCallback, inOptCallbackData]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_MultiplayerSessionClear", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_MultiplayerSessionClear(IntPtr inContext)
    {
        Basics.Log(nameof(UPC_MultiplayerSessionClear), [inContext]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_MultiplayerSessionClear_Extended", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_MultiplayerSessionClear_Extended(IntPtr inContext, IntPtr unk1, IntPtr unk2)
    {
        Basics.Log(nameof(UPC_MultiplayerSessionClear_Extended), [inContext, unk1, unk2]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_MultiplayerSessionFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_MultiplayerSessionFree(IntPtr inContext, IntPtr inMultiplayerSession)
    {
        Basics.Log(nameof(UPC_MultiplayerSessionFree), [inContext]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_MultiplayerSessionSet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_MultiplayerSessionSet(IntPtr inContext, IntPtr inMultiplayerSession)
    {
        Basics.Log(nameof(UPC_MultiplayerSessionSet), [inContext, inContext]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_MultiplayerSessionGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_MultiplayerSessionGet(IntPtr inContext, IntPtr outMultiplayerSession)
    {
        Basics.Log(nameof(UPC_MultiplayerSessionGet), [inContext, outMultiplayerSession]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_MultiplayerSessionSet_Extended", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_MultiplayerSessionSet_Extended(IntPtr inContext, IntPtr inMultiplayerSession, IntPtr unk1, IntPtr unk2)
    {
        Basics.Log(nameof(UPC_MultiplayerSessionSet_Extended), [inContext, inMultiplayerSession, unk1, unk2]);
        return 0;
    }
}
