﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Overlay
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_SetShopUrl", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_SetShopUrl(IntPtr aUrl, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_OVERLAY_SetShopUrl), [aUrl, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_Show", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_Show(IntPtr aOverlaySection, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_OVERLAY_Show), [aOverlaySection, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_ShowBrowserUrl", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_ShowBrowserUrl(IntPtr aUrlUtf8)
    {
        Basics.Log(nameof(UPLAY_OVERLAY_ShowBrowserUrl), [aUrlUtf8]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_ShowFacebookAuthentication", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_ShowFacebookAuthentication(IntPtr aFacebookAppId, IntPtr aRedirectUri, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_OVERLAY_ShowFacebookAuthentication), [aFacebookAppId, aRedirectUri, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_ShowMicroApp", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_ShowMicroApp(IntPtr aAppName, IntPtr aMicroAppParamList, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_OVERLAY_ShowMicroApp), [aAppName, aMicroAppParamList, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_ShowNotification", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_ShowNotification(uint aNotificationId)
    {
        Basics.Log(nameof(UPLAY_OVERLAY_ShowNotification), [aNotificationId]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_ShowShopUrl", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_ShowShopUrl(IntPtr aUrlUtf8)
    {
        Basics.Log(nameof(UPLAY_OVERLAY_ShowShopUrl), [aUrlUtf8]);
        return true;
    }
}
