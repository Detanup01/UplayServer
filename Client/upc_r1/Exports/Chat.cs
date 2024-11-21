using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Chat
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_CHAT_GetHistory", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_CHAT_GetHistory(IntPtr aAccountIdUtf8, uint aMaxNumberOfMessages, IntPtr aOutHistoryList, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_CHAT_GetHistory), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_CHAT_Init", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_CHAT_Init(int aFlags)
    {
        Basics.Log(nameof(UPLAY_CHAT_Init), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_CHAT_ReleaseHistoryList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_CHAT_ReleaseHistoryList(IntPtr aHistoryList)
    {
        Basics.Log(nameof(UPLAY_CHAT_ReleaseHistoryList), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_CHAT_SendMessage", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_CHAT_SendMessage(IntPtr aAccountIdUtf8, IntPtr aMessageUtf8, IntPtr aData)
    {
        Basics.Log(nameof(UPLAY_CHAT_SendMessage), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_CHAT_SetMessagesRead", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_CHAT_SetMessagesRead(IntPtr aAccountIdUtf8)
    {
        Basics.Log(nameof(UPLAY_CHAT_SetMessagesRead), []);
        return true;
    }
}
