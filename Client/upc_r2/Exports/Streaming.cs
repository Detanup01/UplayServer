using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace upc_r2.Exports;

internal class Streaming
{
    [UnmanagedCallersOnly(EntryPoint = "UPC_StreamingCurrentUserCountryFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StreamingCurrentUserCountryFree(IntPtr inContext, IntPtr intUtf8Country)
    {
        Basics.Log(nameof(UPC_StreamingCurrentUserCountryFree), [inContext, intUtf8Country]);
        return -4;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StreamingCurrentUserCountryGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StreamingCurrentUserCountryGet(IntPtr inContext, IntPtr outUtf8Country, IntPtr callback, IntPtr callbackData)
    {
        Basics.Log(nameof(UPC_StreamingCurrentUserCountryGet), [inContext, outUtf8Country, callback, callbackData]);
        return -4;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StreamingDeviceTypeGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StreamingDeviceTypeGet(IntPtr inContext, IntPtr outType, IntPtr callback, IntPtr callbackData)
    {
        Basics.Log(nameof(UPC_StreamingDeviceTypeGet), [inContext, outType, callback, callbackData]);
        return -4;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StreamingInputGamepadTypeGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StreamingInputGamepadTypeGet(IntPtr inContext, IntPtr outType, IntPtr callback, IntPtr callbackData)
    {
        Basics.Log(nameof(UPC_StreamingInputGamepadTypeGet), [inContext, outType, callback, callbackData]);
        return -4;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StreamingInputTypeGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StreamingInputTypeGet(IntPtr inContext, IntPtr outType, IntPtr callback, IntPtr callbackData)
    {
        Basics.Log(nameof(UPC_StreamingInputTypeGet), [inContext, outType, callback, callbackData]);
        return -4;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StreamingNetworkDelayForInputGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StreamingNetworkDelayForInputGet(IntPtr inContext, IntPtr outdelayMs, IntPtr callback, IntPtr callbackData)
    {
        Basics.Log(nameof(UPC_StreamingNetworkDelayForInputGet), [inContext, outdelayMs, callback, callbackData]);
        return -4;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StreamingNetworkDelayForVideoGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StreamingNetworkDelayForVideoGet(IntPtr inContext, IntPtr outdelayMs, IntPtr callback, IntPtr callbackData)
    {
        Basics.Log(nameof(UPC_StreamingNetworkDelayForVideoGet), [inContext, outdelayMs, callback, callbackData]);
        return -4;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StreamingNetworkDelayRoundtripGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StreamingNetworkDelayRoundtripGet(IntPtr inContext, IntPtr outdelayMs, IntPtr callback, IntPtr callbackData)
    {
        Basics.Log(nameof(UPC_StreamingNetworkDelayRoundtripGet), [inContext, outdelayMs, callback, callbackData]);
        return -4;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StreamingResolutionGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StreamingResolutionGet(IntPtr inContext, IntPtr outResolution, IntPtr callback, IntPtr callbackData)
    {
        Basics.Log(nameof(UPC_StreamingResolutionGet), [inContext, outResolution, callback, callbackData]);
        return -4;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StreamingResolutionFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StreamingResolutionFree(IntPtr inContext, IntPtr inResolution)
    {
        Basics.Log(nameof(UPC_StreamingResolutionFree), [inContext, inResolution]);
        return -4;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StreamingTypeGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StreamingTypeGet(IntPtr inContext, IntPtr outType)
    {
        Basics.Log(nameof(UPC_StreamingTypeGet), [inContext, outType]);
        var mem = Marshal.AllocHGlobal(1);
        Marshal.WriteByte(mem, (byte)Uplay.Uplaydll.StreamingType.None);
        Marshal.WriteIntPtr(outType, 0, mem);
        return 0;
    }
}
