using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Store
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_Checkout", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_Checkout(uint aId)
    {
        Basics.Log(nameof(UPLAY_STORE_Checkout), [aId]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_GetPartner", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_GetPartner()
    {
        Basics.Log(nameof(UPLAY_STORE_GetPartner), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_GetProducts", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_GetProducts(IntPtr aOverlapped, IntPtr aOutProductList)
    {
        Basics.Log(nameof(UPLAY_STORE_GetProducts), [aOverlapped, aOutProductList]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_IsEnabled", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_IsEnabled()
    {
        Basics.Log(nameof(UPLAY_STORE_IsEnabled), []);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_ReleaseProductsList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_ReleaseProductsList(IntPtr aProductList)
    {
        Basics.Log(nameof(UPLAY_STORE_ReleaseProductsList), [aProductList]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_ShowProductDetails", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_ShowProductDetails(uint aId)
    {
        Basics.Log(nameof(UPLAY_STORE_ShowProductDetails), [aId]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_STORE_ShowProducts", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_STORE_ShowProducts(uint aTags)
    {
        Basics.Log(nameof(UPLAY_STORE_ShowProducts), [aTags]);
        return true;
    }
}
