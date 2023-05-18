using System;
using System.Diagnostics;
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
            if (Main.GlobalContext.Config.Saved.account == null)
            {
                Basics.SendReq(new Uplay.Uplaydll.Req()
                {
                    UserReq = new()
                    {
                        GetCredentialsReq = new()
                        {
                            RequestId = 0
                        }

                    }
                }, out var rsp);
            }
            var ret = Marshal.StringToHGlobalAnsi("uplay@user");
            return ret;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr UPC_IdGet(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_IdGet), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("80f33a39-e682-4d1f-b693-39267e890df2");
            return ret;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_IdGet_Extended(IntPtr inContext, IntPtr idptr)
        {
            Basics.Log(nameof(UPC_IdGet_Extended), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("80f33a39-e682-4d1f-b693-39267e890df2");
            Marshal.WriteIntPtr(idptr, 0, ret);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr UPC_InstallLanguageGet(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_InstallLanguageGet), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("en-US");
            return ret;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_InstallLanguageGet_Extended(IntPtr inContext, IntPtr langPtr)
        {
            Basics.Log(nameof(UPC_InstallLanguageGet_Extended), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("en-US");
            Marshal.WriteIntPtr(langPtr, 0, ret);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr UPC_NameGet(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_NameGet), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("user");
            return ret;
        }


        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_NameGet_Extended(IntPtr inContext, IntPtr nameptr)
        {
            Basics.Log(nameof(UPC_NameGet_Extended), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("user");
            Marshal.WriteIntPtr(nameptr, 0, ret);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr UPC_TicketGet(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_TicketGet), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("TICKET");
            return ret;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_TicketGet_Extended(IntPtr inContext, IntPtr ticketPtr)
        {
            Basics.Log(nameof(UPC_TicketGet_Extended), new object[] { inContext });
            var ret = Marshal.StringToHGlobalAnsi("TICKET");
            Marshal.WriteIntPtr(ticketPtr, 0 , ret);
            return 0;
        }
    }
}
