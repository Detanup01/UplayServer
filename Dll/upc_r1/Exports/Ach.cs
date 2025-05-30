﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Ach
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ACH_EarnAchievement", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ACH_EarnAchievement(uint aAchivementId, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_ACH_EarnAchievement), [aAchivementId, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ACH_GetAchievementImage", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ACH_GetAchievementImage(uint aId, IntPtr aOutImage, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_ACH_GetAchievementImage), [aId, aOutImage, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ACH_GetAchievements", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ACH_GetAchievements(uint aFilter, IntPtr aAccountIdUtf8OrNULLIfCurrentUser, IntPtr aOutAchievementList, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_ACH_GetAchievements), [aFilter, aAccountIdUtf8OrNULLIfCurrentUser, aOutAchievementList, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ACH_ReleaseAchievementImage", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ACH_ReleaseAchievementImage(IntPtr aImage)
    {
        Log(nameof(UPLAY_ACH_ReleaseAchievementImage), [aImage]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ACH_ReleaseAchievementList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ACH_ReleaseAchievementList(IntPtr aList)
    {
        Log(nameof(UPLAY_ACH_ReleaseAchievementList), [aList]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ACH_Write", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ACH_Write(IntPtr aAchievement)
    {
        Log(nameof(UPLAY_ACH_Write), [aAchievement]);
        return false;
    }
}
