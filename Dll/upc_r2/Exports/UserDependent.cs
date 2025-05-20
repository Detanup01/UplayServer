using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports;

public class UserDependent
{
    [UnmanagedCallersOnly(EntryPoint = "UPC_EmailGet", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPC_EmailGet(IntPtr inContext)
    {
        Log(nameof(UPC_EmailGet), [inContext]);
        return Marshal.StringToHGlobalAnsi(Main.GlobalContext.Config.Saved.account.Email);
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_IdGet", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPC_IdGet(IntPtr inContext)
    {
        Log(nameof(UPC_IdGet), [inContext]);
        return Marshal.StringToHGlobalAnsi(Main.GlobalContext.Config.Saved.account.AccountId);
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_IdGet_Extended", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_IdGet_Extended(IntPtr inContext, IntPtr idptr)
    {
        Log(nameof(UPC_IdGet_Extended), [inContext]);
        Marshal.WriteIntPtr(idptr, 0, Marshal.StringToHGlobalAnsi(Main.GlobalContext.Config.Saved.account.AccountId));
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_InstallLanguageGet", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPC_InstallLanguageGet(IntPtr inContext)
    {
        Log(nameof(UPC_InstallLanguageGet), [inContext]);
        return Marshal.StringToHGlobalAnsi(Main.GlobalContext.Config.Saved.account.Country);
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_InstallLanguageGet_Extended", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_InstallLanguageGet_Extended(IntPtr inContext, IntPtr langPtr)
    {
        Log(nameof(UPC_InstallLanguageGet_Extended), [inContext]);
        Marshal.WriteIntPtr(langPtr, 0, Marshal.StringToHGlobalAnsi(Main.GlobalContext.Config.Saved.account.Country));
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_NameGet", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPC_NameGet(IntPtr inContext)
    {
        Log(nameof(UPC_NameGet), [inContext]);
        return Marshal.StringToHGlobalAnsi(Main.GlobalContext.Config.Saved.account.NameOnPlatform);
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_NameGet_Extended", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_NameGet_Extended(IntPtr inContext, IntPtr nameptr)
    {
        Log(nameof(UPC_NameGet_Extended), [inContext]);
        Marshal.WriteIntPtr(nameptr, 0, Marshal.StringToHGlobalAnsi(Main.GlobalContext.Config.Saved.account.NameOnPlatform));
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_TicketGet", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPC_TicketGet(IntPtr inContext)
    {
        Log(nameof(UPC_TicketGet), [inContext]);
        if (UPC_Json.GetRoot().Account.UseTicket)
        {
            string? ticket = !string.IsNullOrEmpty(Main.GlobalContext.Config.Saved.ubiTicket) ? Main.GlobalContext.Config.Saved.ubiTicket : null;
            Log(nameof(UPC_TicketGet), [ticket == null]);
            return Marshal.StringToHGlobalAnsi(ticket);
        }
        return Marshal.StringToHGlobalAnsi("");
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_TicketGet_Extended", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_TicketGet_Extended(IntPtr inContext, IntPtr ticketPtr)
    {
        Log(nameof(UPC_TicketGet_Extended), [inContext]);
        if (UPC_Json.GetRoot().Account.UseTicket)
        {
            string? ticket = !string.IsNullOrEmpty(Main.GlobalContext.Config.Saved.ubiTicket) ? Main.GlobalContext.Config.Saved.ubiTicket : null;
            Log(nameof(UPC_TicketGet_Extended), [ticket == null]);
            Marshal.WriteIntPtr(ticketPtr, 0 , Marshal.StringToHGlobalAnsi(ticket));
        }
        else
            Marshal.WriteIntPtr(ticketPtr, 0, Marshal.StringToHGlobalAnsi(null));
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_UserAccountCountryGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_UserAccountCountryGet(IntPtr inContext, IntPtr outCountryCode)
    {
        Log(nameof(UPC_UserAccountCountryGet), [inContext]);
        Marshal.WriteIntPtr(outCountryCode, 0, Marshal.StringToHGlobalAnsi(Main.GlobalContext.Config.Saved.account.Country));
        return (int)UPC_Result.UPC_Result_Ok;
    }
}
