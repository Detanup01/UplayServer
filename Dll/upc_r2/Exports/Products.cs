using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports;

internal class Products
{

    [UnmanagedCallersOnly(EntryPoint = "UPC_ProductListGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_ProductListGet(IntPtr inContext, IntPtr inOptUserIdUtf8, uint inFilter, [Out] IntPtr outProductList, IntPtr inCallback, IntPtr inOptCallbackData)
    {
        Log(nameof(UPC_ProductListGet), [inContext, inOptUserIdUtf8, inFilter, outProductList, inCallback, inOptCallbackData]);
        // Zero Means ourself
        if (inOptUserIdUtf8 != IntPtr.Zero)
        {
            string? userId = Marshal.PtrToStringAnsi(inOptUserIdUtf8);
            // Seems like no user requested. Should we use or own?
            if (userId == null)
                return -1;
            Log(nameof(UPC_ProductListGet), [userId]);
        }

        // We adding or own product (So the productId as App [Required]) then DLC/Items/Others.
        List<UPC_Product> products =
        [
            new(Main.GlobalContext.Config.ProductId, 1)
        ];
        foreach (var item in UPC_Json.GetRoot().Products)
        {
            products.Add(new(item.ProductId, item.Type));
        }
        foreach (var item in UPC_Json.GetRoot().AutoProductIds)
        {
            products.Add(new(item, 2));
        }

        Log(nameof(UPC_ProductListGet), ["Products: ", string.Join("\n", products)]);
        WriteOutList(outProductList, products);
        Main.GlobalContext.Callbacks.Add(new(inCallback, inOptCallbackData, (int)UPC_Result.UPC_Result_Ok));
        return 10000;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_ProductListFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_ProductListFree(IntPtr inContext, IntPtr inProductList)
    {
        Log(nameof(UPC_ProductListFree), [inContext, inProductList]);
        FreeList(inProductList);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_ProductConsume", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_ProductConsume(IntPtr inContext, uint inProductId, uint inQuantity, IntPtr inTransactionIdUtf8, IntPtr inSignatureUtf8, IntPtr outResponseSignatureUtf8, IntPtr inCallback, IntPtr inOptCallbackData)
    {
        Log(nameof(UPC_ProductConsume), [inContext, inProductId, inQuantity, inTransactionIdUtf8, inSignatureUtf8, outResponseSignatureUtf8, inCallback, inOptCallbackData]);
        Marshal.WriteIntPtr(outResponseSignatureUtf8, 0, Marshal.StringToHGlobalAnsi($"FunnySignature_{inProductId}_{Random.Shared.Next()}"));
        Main.GlobalContext.Callbacks.Add(new(inCallback, inOptCallbackData, (int)UPC_Result.UPC_Result_Ok));
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_ProductConsumeSignatureFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_ProductConsumeSignatureFree(IntPtr inContext, IntPtr inResponseSignature)
    {
        Log(nameof(UPC_ProductConsumeSignatureFree), [inContext, inResponseSignature]);
        Marshal.FreeHGlobal(inResponseSignature);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_ProductAddonTrack", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_ProductAddonTrack(IntPtr inContext, uint inAddonId, IntPtr inOptCallback, IntPtr inOptCallbackData)
    {
        Log(nameof(UPC_ProductAddonTrack), [inContext, inAddonId, inOptCallback, inOptCallbackData]);
        Main.GlobalContext.Callbacks.Add(new(inOptCallback, inOptCallbackData, (int)UPC_Result.UPC_Result_Ok));
        return 0;
    }
}
