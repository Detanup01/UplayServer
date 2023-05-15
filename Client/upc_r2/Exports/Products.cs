using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static upc_r2.Basics;

namespace upc_r2.Exports
{
    internal class Products
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct UPC_Product
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint id;
            [MarshalAs(UnmanagedType.U4)]
            public Uplay.Uplaydll.ProductType type;
            [MarshalAs(UnmanagedType.U4)]
            public Uplay.Uplaydll.ProductOwnership ownership;
            [MarshalAs(UnmanagedType.U4)]
            public Uplay.Uplaydll.ProductState state;
            [MarshalAs(UnmanagedType.U4)]
            public uint balance;
            [MarshalAs(UnmanagedType.U4)]
            public Uplay.Uplaydll.ProductActivation activation;

            public UPC_Product(uint a, uint b)
            {
                id = a;
                type = (Uplay.Uplaydll.ProductType)b;
                balance = 0;
                ownership = Uplay.Uplaydll.ProductOwnership.Owned;
                state = Uplay.Uplaydll.ProductState.Playable;
                activation = Uplay.Uplaydll.ProductActivation.Purchased;
            }
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static unsafe int UPC_ProductListGet(IntPtr inContext, IntPtr inOptUserIdUtf8, uint inFilter, [Out] IntPtr outProductList, IntPtr inCallback, IntPtr inOptCallbackData)
        {
            Basics.Log(nameof(UPC_ProductListGet), new object[] { inContext, inOptUserIdUtf8, inFilter, outProductList, inCallback, inOptCallbackData });

            Main.GlobalContext.Callbacks.Append(new(inCallback, inOptCallbackData, 0));

            List<UPC_Product> products = new()
            {
                new(Main.GlobalContext.Config.AppId, 1)
            };

            var listptr = Basics.GetListPtr(products);
            BasicList productList = new()
            {
                count = products.Count,
                list = listptr
            };
            IntPtr ptr = Marshal.AllocHGlobal(sizeof(BasicList));
            Marshal.StructureToPtr(productList, ptr, false);
            Marshal.WriteIntPtr(outProductList, ptr);
            return 0x10000;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static unsafe int UPC_ProductListFree(IntPtr inContext, IntPtr inProductList)
        {
            Basics.Log(nameof(UPC_ProductListFree), new object[] { inContext, inProductList });
            BasicList upc_ProductList = Basics.IntPtrToStruct<BasicList>(inProductList);
            Basics.FreeListPtr(upc_ProductList.count, upc_ProductList.list);
            Marshal.FreeHGlobal(inProductList);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_ProductConsume(IntPtr inContext, uint inProductId, uint inQuantity, IntPtr inTransactionIdUtf8, IntPtr inSignatureUtf8, IntPtr outResponseSignatureUtf8, IntPtr inCallback, IntPtr inOptCallbackData)
        {
            Basics.Log(nameof(UPC_ProductConsume), new object[] { inContext, inProductId, inQuantity, inTransactionIdUtf8, inSignatureUtf8, outResponseSignatureUtf8, inCallback, inOptCallbackData });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_ProductConsumeSignatureFree(IntPtr inContext, IntPtr inResponseSignature)
        {
            Basics.Log(nameof(UPC_ProductConsumeSignatureFree), new object[] { inContext, inResponseSignature });
            return 0;
        }
    }
}
