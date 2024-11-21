using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Avatar
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_AVATAR_GetBitmap", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_AVATAR_GetBitmap(IntPtr aAvatarId, int aAvatarSize, IntPtr aOutRGBA, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_AVATAR_GetBitmap), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_AVATAR_GetBitmap", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_AVATAR_GetAvatarIdForCurrentUser(IntPtr aOutAvatarId, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_AVATAR_GetAvatarIdForCurrentUser), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_AVATAR_GetBitmap", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_AVATAR_Get(IntPtr aAccountIdUtf8, int aAvatarSize, IntPtr aOutRGBA, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_AVATAR_Get), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_AVATAR_GetBitmap", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_AVATAR_Release(IntPtr aRGBA)
    {
        Basics.Log(nameof(UPLAY_AVATAR_Release), []);
        return true;
    }
}
