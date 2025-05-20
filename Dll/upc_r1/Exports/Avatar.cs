using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Avatar
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_AVATAR_GetBitmap", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_AVATAR_GetBitmap(IntPtr aAvatarId, int aAvatarSize, IntPtr aOutRGBA, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_AVATAR_GetBitmap), [aAvatarId, aAvatarSize, aOutRGBA, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_AVATAR_GetAvatarIdForCurrentUser", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_AVATAR_GetAvatarIdForCurrentUser(IntPtr aOutAvatarId, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_AVATAR_GetAvatarIdForCurrentUser), [aOutAvatarId, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_AVATAR_Get", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_AVATAR_Get(IntPtr aAccountIdUtf8, int aAvatarSize, IntPtr aOutRGBA, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_AVATAR_Get), [aAccountIdUtf8, aAvatarSize, aOutRGBA, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_AVATAR_Release", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_AVATAR_Release(IntPtr aRGBA)
    {
        Log(nameof(UPLAY_AVATAR_Release), [aRGBA]);
        return false;
    }
}
