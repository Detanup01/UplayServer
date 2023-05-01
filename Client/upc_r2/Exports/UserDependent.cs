using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports
{
    internal class UserDependent
    {
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr UPC_EmailGet(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_EmailGet), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("uplay@user");
            return ret;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr UPC_IdGet(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_EmailGet), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("80f33a39-e682-4d1f-b693-39267e890df2");
            return ret;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr UPC_InstallLanguageGet(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_InstallLanguageGet), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("en-US");
            return ret;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr UPC_NameGet(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_NameGet), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("user");
            return ret;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr UPC_TicketGet(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_TicketGet), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("TICKET");
            return ret;
        }
    }
}
