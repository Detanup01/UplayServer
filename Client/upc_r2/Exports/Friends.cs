using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports;

internal class Friends
{
    [UnmanagedCallersOnly(EntryPoint = "UPC_FriendAdd", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_FriendAdd(IntPtr inContext, IntPtr inSearchStringUtf8, IntPtr inOptCallback, IntPtr inOptCallbackData)
    {
        Basics.Log(nameof(UPC_FriendAdd), [inContext, inSearchStringUtf8, inOptCallback, inOptCallbackData]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_FriendRemove", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_FriendRemove(IntPtr inContext, IntPtr inUserIdUtf8, IntPtr inOptCallback, IntPtr inOptCallbackData)
    {
        Basics.Log(nameof(UPC_FriendRemove), [inContext, inUserIdUtf8, inOptCallback, inOptCallbackData]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_FriendCheck", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_FriendCheck(IntPtr inContext, IntPtr inUserIdUtf8)
    {
        Basics.Log(nameof(UPC_FriendCheck), [inContext, inUserIdUtf8]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_FriendCheck_Extended", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_FriendCheck_Extended(IntPtr inContext, IntPtr inUserIdUtf8, IntPtr checkPtr)
    {
        Basics.Log(nameof(UPC_FriendCheck_Extended), [inContext, inUserIdUtf8]);
        var mem = Marshal.AllocHGlobal(1);
        Marshal.WriteByte(mem, 1);
        Marshal.WriteIntPtr(checkPtr, 0, mem);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_FriendListFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_FriendListFree(IntPtr inContext, IntPtr inUserIdUtf8)
    {
        Basics.Log(nameof(UPC_FriendListFree), [inContext, inUserIdUtf8]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_FriendListGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_FriendListGet(IntPtr inContext, uint inOptOnlineStatusFilter, IntPtr outFriendList, IntPtr inCallback, IntPtr inOptCallbackData)
    {
        Basics.Log(nameof(UPC_FriendListGet), [inContext, inOptOnlineStatusFilter, outFriendList, inCallback, inOptCallbackData]);
        return 0;
    }
}
