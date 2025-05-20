using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Metadata
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_METADATA_ClearContinuousTag", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_METADATA_ClearContinuousTag(IntPtr aStringNameUtf8)
    {
        Log(nameof(UPLAY_METADATA_ClearContinuousTag), [aStringNameUtf8]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_METADATA_SetContinuousTag", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_METADATA_SetContinuousTag(IntPtr aStringNameUtf8, IntPtr aStringValueUtf8)
    {
        Log(nameof(UPLAY_METADATA_SetContinuousTag), [aStringNameUtf8, aStringValueUtf8]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_METADATA_SetSingleEventTag", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_METADATA_SetSingleEventTag(IntPtr aStringNameUtf8, IntPtr aStringValueUtf8)
    {
        Log(nameof(UPLAY_METADATA_SetSingleEventTag), [aStringNameUtf8, aStringValueUtf8]);
        return true;
    }
}
