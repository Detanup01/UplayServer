using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Save
{

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Close", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Close(IntPtr aOutSaveHandle)
    {
        Basics.Log(nameof(UPLAY_SAVE_Close), [aOutSaveHandle]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_GetSavegames", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_GetSavegames(IntPtr aOutGamesList, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_SAVE_GetSavegames), [aOutGamesList, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Open", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Open(uint aSlotId, uint aMode, IntPtr aOutSaveHandle, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_SAVE_Open), [aSlotId, aMode, aOutSaveHandle, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Read", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Read(IntPtr aSaveHandle, uint aNumOfBytesToRead, uint aOffset, IntPtr aOutBuffer, uint aOutNumOfBytesRead, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_SAVE_Read), [aSaveHandle, aNumOfBytesToRead, aOffset, aOutBuffer, aOutNumOfBytesRead, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_ReleaseGameList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_ReleaseGameList(IntPtr aGamesList)
    {
        Basics.Log(nameof(UPLAY_SAVE_ReleaseGameList), [aGamesList]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Remove", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Remove(uint aSlotId, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_SAVE_Remove), [aSlotId, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_SetName", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_SetName(IntPtr aSaveHandle, IntPtr aNameUtf8)
    {
        Basics.Log(nameof(UPLAY_SAVE_SetName), [aSaveHandle, aNameUtf8]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Write", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Write(IntPtr aSaveHandle, uint aNumOfBytesToWrite, IntPtr aBuffer, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_SAVE_Write), [aSaveHandle, aNumOfBytesToWrite, aBuffer, aOverlapped]);
        return true;
    }
}
