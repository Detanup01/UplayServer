using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Win
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_WIN_GetActions", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_WIN_GetActions(IntPtr aOutActionList, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_WIN_GetActions), [aOutActionList, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_WIN_GetRewards", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_WIN_GetRewards(IntPtr aOutRewardList, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_WIN_GetRewards), [aOutRewardList, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_WIN_GetUnitBalance", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_WIN_GetUnitBalance(int aOutBalance, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_WIN_GetUnitBalance), [aOutBalance, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_WIN_RefreshActions", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_WIN_RefreshActions()
    {
        Basics.Log(nameof(UPLAY_WIN_RefreshActions), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_WIN_ReleaseActionList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_WIN_ReleaseActionList(IntPtr aActionList)
    {
        Basics.Log(nameof(UPLAY_WIN_ReleaseActionList), [aActionList]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_WIN_ReleaseRewardList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_WIN_ReleaseRewardList(IntPtr aRewardList)
    {
        Basics.Log(nameof(UPLAY_WIN_ReleaseRewardList), [aRewardList]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_WIN_SetActionsCompleted", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_WIN_SetActionsCompleted(IntPtr aActionIdsUtf8, uint aActionIdsCount, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_WIN_SetActionsCompleted), [aActionIdsUtf8, aActionIdsCount, aOverlapped]);
        return true;
    }
}
