using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports
{
    internal class Store
    {
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_StoreCheckout(IntPtr inContext, uint inId)
        {
            Basics.Log(nameof(UPC_StoreCheckout), new object[] { inContext, inId });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_StoreIsEnabled(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_StoreIsEnabled), new object[] { inContext });
            return 1;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_StoreLanguageSet(IntPtr inContext, IntPtr inLanguageCountryCode)
        {
            Basics.Log(nameof(UPC_StoreLanguageSet), new object[] { inContext , inLanguageCountryCode });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_StorePartnerGet(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_StorePartnerGet), new object[] { inContext });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_StoreProductDetailsShow(IntPtr inContext, uint inId)
        {
            Basics.Log(nameof(UPC_StoreProductDetailsShow), new object[] { inContext, inId });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_StoreProductListFree(IntPtr inContext, IntPtr inProductList)
        {
            Basics.Log(nameof(UPC_StoreProductListFree), new object[] { inContext, inProductList });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_StoreProductListGet(IntPtr inContext, IntPtr outProductList, IntPtr inCallback, IntPtr inCallbackData)
        {
            Basics.Log(nameof(UPC_StoreProductListGet), new object[] { inContext, outProductList, inCallback, inCallbackData });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_StoreProductsShow(IntPtr inContext, IntPtr inTagsList)
        {
            Basics.Log(nameof(UPC_StoreProductsShow), new object[] { inContext, inTagsList });
            return 0;
        }
    }
}
