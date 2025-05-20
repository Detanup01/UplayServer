using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Options
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OPTIONS_Apply", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OPTIONS_Apply(IntPtr aFileHandle, IntPtr aKeyValueList, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_OPTIONS_Apply), [aFileHandle, aKeyValueList, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OPTIONS_Close", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OPTIONS_Close(IntPtr aFileHandle)
    {
        Log(nameof(UPLAY_OPTIONS_Close), [aFileHandle]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OPTIONS_Enumerate", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OPTIONS_Enumerate(IntPtr aFileHandle, IntPtr aOutKeyValueList, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_OPTIONS_Enumerate), [aFileHandle, aOutKeyValueList, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OPTIONS_Get", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OPTIONS_Get(IntPtr aKeyValueList, IntPtr aKey)
    {
        Log(nameof(UPLAY_OPTIONS_Get), [aKeyValueList, aKey]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OPTIONS_Open", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OPTIONS_Open(IntPtr aName)
    {
        Log(nameof(UPLAY_OPTIONS_Open), [aName]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OPTIONS_ReleaseKeyValueList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OPTIONS_ReleaseKeyValueList(IntPtr aKeyValueList)
    {
        Log(nameof(UPLAY_OPTIONS_ReleaseKeyValueList), [aKeyValueList]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OPTIONS_Set", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OPTIONS_Set(IntPtr aKeyValueList, IntPtr aKey, IntPtr aValue)
    {
        Log(nameof(UPLAY_OPTIONS_Set), [aKeyValueList, aKey, aValue]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OPTIONS_SetInGameState", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OPTIONS_SetInGameState(uint aFlags)
    {
        Log(nameof(UPLAY_OPTIONS_SetInGameState), [aFlags]);
        return false;
    }
}
