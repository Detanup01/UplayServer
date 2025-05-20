using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Product
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PRODUCT_GetProductList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PRODUCT_GetProductList(IntPtr aOverlapped, IntPtr aOutProductList)
    {
        Log(nameof(UPLAY_PRODUCT_GetProductList), [aOverlapped, aOutProductList]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PRODUCT_ReleaseProductList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PRODUCT_ReleaseProductList(IntPtr aProductList)
    {
        Log(nameof(UPLAY_PRODUCT_ReleaseProductList), [aProductList]);
        return false;
    }
}
