using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Store
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_Checkout", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_Checkout(uint aId)
    {
        Log(nameof(UPLAY_STORE_Checkout), [aId]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_GetPartner", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPLAY_STORE_GetPartner()
    {
        Log(nameof(UPLAY_STORE_GetPartner), []);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_GetProducts", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_GetProducts(IntPtr aOverlapped, IntPtr aOutProductList)
    {
        Log(nameof(UPLAY_STORE_GetProducts), [aOverlapped, aOutProductList]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_IsEnabled", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_IsEnabled()
    {
        Log(nameof(UPLAY_STORE_IsEnabled), []);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_ReleaseProductsList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_ReleaseProductsList(IntPtr aProductList)
    {
        Log(nameof(UPLAY_STORE_ReleaseProductsList), [aProductList]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_ShowProductDetails", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_ShowProductDetails(uint aId)
    {
        Log(nameof(UPLAY_STORE_ShowProductDetails), [aId]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_ShowProducts", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_ShowProducts(uint aTags)
    {
        Log(nameof(UPLAY_STORE_ShowProducts), [aTags]);
        return false;
    }
}
