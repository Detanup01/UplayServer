using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class User
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_ClearGameSession", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_ClearGameSession()
    {
        Log(nameof(UPLAY_USER_ClearGameSession));
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_ConsumeItem", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_ConsumeItem(IntPtr aTransactionIdUtf8, uint aUplayId, uint aQuantity, IntPtr aSignatureUtf8, IntPtr aOverlapped, IntPtr aOutResult)
    {
        Log(nameof(UPLAY_USER_ConsumeItem), [aTransactionIdUtf8, aUplayId, aQuantity, aSignatureUtf8, aOverlapped, aOutResult]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetAccountId", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPLAY_USER_GetAccountId(IntPtr aOutAccountId)
    {
        Log(nameof(UPLAY_USER_GetAccountId), [aOutAccountId]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetAccountIdUtf8", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPLAY_USER_GetAccountIdUtf8()
    {
        Log(nameof(UPLAY_USER_GetAccountIdUtf8));
        return Marshal.StringToHGlobalAnsi(UPC_Json.GetRoot().Account.AccountId);
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetCPUScore", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_GetCPUScore(IntPtr aOutCpuScore)
    {
        Log(nameof(UPLAY_USER_GetCPUScore), [aOutCpuScore]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetCdKeyUtf8", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPLAY_USER_GetCdKeyUtf8(uint aUplayId)
    {
        Log(nameof(UPLAY_USER_GetCdKeyUtf8), [aUplayId]);
        string defaultKey = "1111-2222 -3333-4444";
        var list = UPC_Json.GetRoot().CDKey.Where(x=>x.ProductId == aUplayId);
        if (list.Count() == 1)
        {
            defaultKey = list.ToList()[0].Key;
        }
        return Marshal.StringToHGlobalAnsi(defaultKey);
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetCdKeys", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_GetCdKeys(IntPtr aOutCdKeyList, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_USER_GetCdKeys), [aOutCdKeyList, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetConsumableItems", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_GetConsumableItems(IntPtr aOutConsumableItemsList)
    {
        Log(nameof(UPLAY_USER_GetConsumableItems), [aOutConsumableItemsList]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetCredentials", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_GetCredentials(IntPtr aOutUserCredentials, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_USER_GetCredentials), [aOutUserCredentials, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetEmail", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPLAY_USER_GetEmail(IntPtr aOutEmail)
    {
        Log(nameof(UPLAY_USER_GetEmail), [aOutEmail]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetEmailUtf8", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPLAY_USER_GetEmailUtf8()
    {
        Log(nameof(UPLAY_USER_GetEmailUtf8), []);
        return Marshal.StringToHGlobalAnsi(UPC_Json.GetRoot().Account.Email);
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetGPUScore", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_GetGPUScore(IntPtr aOutGpuScore)
    {
        Log(nameof(UPLAY_USER_GetGPUScore), [aOutGpuScore]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetGPUScoreConfidenceLevel", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_GetGPUScoreConfidenceLevel(IntPtr aOutConfidenceLevel)
    {
        Log(nameof(UPLAY_USER_GetGPUScoreConfidenceLevel), [aOutConfidenceLevel]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetNameUtf8", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPLAY_USER_GetNameUtf8()
    {
        Log(nameof(UPLAY_USER_GetNameUtf8), []);
        return Marshal.StringToHGlobalAnsi(UPC_Json.GetRoot().Account.Name);
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetPassword", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPLAY_USER_GetPassword(IntPtr aOutPassword)
    {
        Log(nameof(UPLAY_USER_GetPassword), [aOutPassword]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetPasswordUtf8", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPLAY_USER_GetPasswordUtf8()
    {
        Log(nameof(UPLAY_USER_GetPasswordUtf8), []);
        return Marshal.StringToHGlobalAnsi(UPC_Json.GetRoot().Account.Password);
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetProfile", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_GetProfile(IntPtr aAccountIdUtf8, IntPtr aOverlapped, IntPtr aOutProfile)
    {
        Log(nameof(UPLAY_USER_GetProfile), [aAccountIdUtf8, aOverlapped, aOutProfile]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetTicketUtf8", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPLAY_USER_GetTicketUtf8()
    {
        Log(nameof(UPLAY_USER_GetTicketUtf8), []);
        return Marshal.StringToHGlobalAnsi(UPC_Json.GetRoot().Account.Ticket);
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetUsername", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPLAY_USER_GetUsername(IntPtr aOutUsername)
    {
        Log(nameof(UPLAY_USER_GetUsername), [aOutUsername]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_GetUsernameUtf8", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPLAY_USER_GetUsernameUtf8()
    {
        Log(nameof(UPLAY_USER_GetUsernameUtf8), []);
        return Marshal.StringToHGlobalAnsi(UPC_Json.GetRoot().Account.Name);
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_IsConnected", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_IsConnected()
    {
        Log(nameof(UPLAY_USER_IsConnected));
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_IsInOfflineMode", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_IsInOfflineMode()
    {
        Log(nameof(UPLAY_USER_IsInOfflineMode), []);
        return UPC_Json.GetRoot().Others.OfflineMode;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_IsOwned", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_IsOwned(uint aUplayId)
    {
        Log(nameof(UPLAY_USER_IsOwned), [aUplayId]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_ReleaseCdKeyList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_ReleaseCdKeyList(IntPtr aCdKeyList)
    {
        Log(nameof(UPLAY_USER_ReleaseCdKeyList), [aCdKeyList]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_ReleaseConsumeItemResult", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_ReleaseConsumeItemResult(IntPtr aConsumeItemResult)
    {
        Log(nameof(UPLAY_USER_ReleaseConsumeItemResult), [aConsumeItemResult]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_ReleaseProfile", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_ReleaseProfile(IntPtr aOutProfile)
    {
        Log(nameof(UPLAY_USER_ReleaseProfile), [aOutProfile]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_USER_SetGameSession", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_USER_SetGameSession(IntPtr aGameSessionIdentifier, IntPtr aSessionData, uint aFlags)
    {
        Log(nameof(UPLAY_USER_SetGameSession), [aGameSessionIdentifier, aSessionData, aFlags]);
        return false;
    }
}
