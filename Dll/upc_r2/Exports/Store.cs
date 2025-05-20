using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports;

internal class Store
{
    [UnmanagedCallersOnly(EntryPoint = "UPC_StoreCheckout", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StoreCheckout(IntPtr inContext, uint inId)
    {
        Log(nameof(UPC_StoreCheckout), [inContext, inId]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StoreIsEnabled", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StoreIsEnabled(IntPtr inContext)
    {
        Log(nameof(UPC_StoreIsEnabled), [inContext]);
        return 1;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StoreIsEnabled_Extended", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StoreIsEnabled_Extended(IntPtr inContext, IntPtr outIsEnabled)
    {
        Log(nameof(UPC_StoreIsEnabled_Extended), [inContext]);
        Marshal.WriteInt32(outIsEnabled, 0, 1);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StoreLanguageSet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StoreLanguageSet(IntPtr inContext, IntPtr inLanguageCountryCode)
    {
        Log(nameof(UPC_StoreLanguageSet), [inContext, inLanguageCountryCode]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorePartnerGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StorePartnerGet(IntPtr inContext)
    {
        Log(nameof(UPC_StorePartnerGet), [inContext]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorePartnerGet_Extended", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StorePartnerGet_Extended(IntPtr inContext, IntPtr outPartner)
    {
        Log(nameof(UPC_StorePartnerGet_Extended), [inContext, outPartner]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StoreProductDetailsShow", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StoreProductDetailsShow(IntPtr inContext, uint inId)
    {
        Log(nameof(UPC_StoreProductDetailsShow), [inContext, inId]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StoreProductListFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StoreProductListFree(IntPtr inContext, IntPtr inProductList)
    {
        Log(nameof(UPC_StoreProductListFree), [inContext, inProductList]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StoreProductListGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StoreProductListGet(IntPtr inContext, IntPtr outProductList, IntPtr inCallback, IntPtr inCallbackData)
    {
        Log(nameof(UPC_StoreProductListGet), [inContext, outProductList, inCallback, inCallbackData]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StoreProductsShow", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StoreProductsShow(IntPtr inContext, IntPtr inTagsList)
    {
        Log(nameof(UPC_StoreProductsShow), [inContext, inTagsList]);
        return 0;
    }
}
