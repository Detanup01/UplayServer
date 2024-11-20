using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using static upc_r2.Basics;

namespace upc_r2.Exports;

internal class Products
{

    [UnmanagedCallersOnly(EntryPoint = "UPC_ProductListGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_ProductListGet(IntPtr inContext, IntPtr inOptUserIdUtf8, uint inFilter, [Out] IntPtr outProductList, IntPtr inCallback, IntPtr inOptCallbackData)
    {
        Log(nameof(UPC_ProductListGet), [inContext, inOptUserIdUtf8, inFilter, outProductList, inCallback, inOptCallbackData]);
        string? userId = Marshal.PtrToStringUTF8(inOptUserIdUtf8);
        if (userId == null)
            return -1;
        Log(nameof(UPC_ProductListGet), [userId]);
        var cbList = Main.GlobalContext.Callbacks.ToList();
        cbList.Add(new(inCallback, inOptCallbackData, 0));
        Main.GlobalContext.Callbacks = cbList.ToArray();

        List<UPC_Product> products = new()
            {
                new(Main.GlobalContext.Config.ProductId, 1)
            };
        Log(nameof(UPC_ProductListGet), ["Products: ", JsonSerializer.Serialize(products, JsonSourceGen.Default.ListUPC_Product)]);
        var listptr = GetListPtr(products);
        BasicList productList = new()
        {
            count = products.Count,
            list = listptr
        };
        IntPtr iptr = Marshal.AllocHGlobal(Marshal.SizeOf<BasicList>());
        Marshal.StructureToPtr(productList, iptr, false);
        Marshal.WriteIntPtr(outProductList, 0, iptr);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_ProductListFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_ProductListFree(IntPtr inContext, IntPtr inProductList)
    {
        Log(nameof(UPC_ProductListFree), [inContext, inProductList]);
        BasicList upc_ProductList = IntPtrToStruct<BasicList>(inProductList);
        FreeListPtr(upc_ProductList.count, upc_ProductList.list);
        Marshal.FreeHGlobal(inProductList);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_ProductConsume", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_ProductConsume(IntPtr inContext, uint inProductId, uint inQuantity, IntPtr inTransactionIdUtf8, IntPtr inSignatureUtf8, IntPtr outResponseSignatureUtf8, IntPtr inCallback, IntPtr inOptCallbackData)
    {
        Log(nameof(UPC_ProductConsume), [inContext, inProductId, inQuantity, inTransactionIdUtf8, inSignatureUtf8, outResponseSignatureUtf8, inCallback, inOptCallbackData]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_ProductConsumeSignatureFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_ProductConsumeSignatureFree(IntPtr inContext, IntPtr inResponseSignature)
    {
        Log(nameof(UPC_ProductConsumeSignatureFree), [inContext, inResponseSignature]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_ProductAddonTrack", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_ProductAddonTrack(IntPtr inContext, uint inAddonId, IntPtr inOptCallback, IntPtr inOptCallbackData)
    {
        Log(nameof(UPC_ProductAddonTrack), [inContext, inAddonId, inOptCallback, inOptCallbackData]);
        return 0;
    }
}
