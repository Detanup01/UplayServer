using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Overlay
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_SetShopUrl", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_SetShopUrl(IntPtr aUrl, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_OVERLAY_SetShopUrl), [aUrl, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_Show", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_Show(IntPtr aOverlaySection, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_OVERLAY_Show), [aOverlaySection, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_ShowBrowserUrl", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_ShowBrowserUrl(IntPtr aUrlUtf8)
    {
        Log(nameof(UPLAY_OVERLAY_ShowBrowserUrl), [aUrlUtf8]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_ShowFacebookAuthentication", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_ShowFacebookAuthentication(IntPtr aFacebookAppId, IntPtr aRedirectUri, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_OVERLAY_ShowFacebookAuthentication), [aFacebookAppId, aRedirectUri, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_ShowMicroApp", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_ShowMicroApp(IntPtr aAppName, IntPtr aMicroAppParamList, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_OVERLAY_ShowMicroApp), [aAppName, aMicroAppParamList, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_ShowNotification", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_ShowNotification(uint aNotificationId)
    {
        Log(nameof(UPLAY_OVERLAY_ShowNotification), [aNotificationId]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_OVERLAY_ShowShopUrl", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_OVERLAY_ShowShopUrl(IntPtr aUrlUtf8)
    {
        Log(nameof(UPLAY_OVERLAY_ShowShopUrl), [aUrlUtf8]);
        return false;
    }
}
