using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Ach
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ACH_EarnAchievement", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ACH_EarnAchievement(uint aAchivementId, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_ACH_EarnAchievement), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ACH_GetAchievementImage", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ACH_GetAchievementImage(uint aId, IntPtr aOutImage, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_ACH_GetAchievementImage), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ACH_GetAchievements", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ACH_GetAchievements(uint aFilter, IntPtr aAccountIdUtf8OrNULLIfCurrentUser, IntPtr aOutAchievementList, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_ACH_GetAchievements), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ACH_ReleaseAchievementImage", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ACH_ReleaseAchievementImage(IntPtr aImage)
    {
        Basics.Log(nameof(UPLAY_ACH_ReleaseAchievementImage), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ACH_ReleaseAchievementList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ACH_ReleaseAchievementList(IntPtr aList)
    {
        Basics.Log(nameof(UPLAY_ACH_ReleaseAchievementList), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ACH_Write", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ACH_Write(IntPtr aAchievement)
    {
        Basics.Log(nameof(UPLAY_ACH_Write), []);
        return true;
    }
}
