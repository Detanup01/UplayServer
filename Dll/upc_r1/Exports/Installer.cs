using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

public class Installer
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_INSTALLER_AreChunksInstalled", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_INSTALLER_AreChunksInstalled(IntPtr aChunkIds, uint aChunkCount)
    {
        Log(nameof(UPLAY_INSTALLER_AreChunksInstalled), [aChunkIds, aChunkCount]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_INSTALLER_GetChunkIdsFromTag", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_INSTALLER_GetChunkIdsFromTag(IntPtr aTagUtf8, IntPtr aOutChunkIdList)
    {
        Log(nameof(UPLAY_INSTALLER_GetChunkIdsFromTag), [aTagUtf8, aOutChunkIdList]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_INSTALLER_GetChunks", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_INSTALLER_GetChunks(IntPtr aOutChunkIdList)
    {
        Log(nameof(UPLAY_INSTALLER_GetChunks), [aOutChunkIdList]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_INSTALLER_GetLanguageUtf8", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPLAY_INSTALLER_GetLanguageUtf8()
    {
        Log(nameof(UPLAY_INSTALLER_GetLanguageUtf8), []);
        return Marshal.StringToHGlobalAnsi(UPC_Json.GetRoot().Account.Country);
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_INSTALLER_Init", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_INSTALLER_Init(uint aFlags)
    {
        Log(nameof(UPLAY_INSTALLER_Init), [aFlags]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_INSTALLER_ReleaseChunkIdList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_INSTALLER_ReleaseChunkIdList(IntPtr aChunkIdList)
    {
        Log(nameof(UPLAY_INSTALLER_ReleaseChunkIdList), [aChunkIdList]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_INSTALLER_UpdateInstallOrder", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_INSTALLER_UpdateInstallOrder(IntPtr aChunkIds, uint aChunkCount)
    {
        Log(nameof(UPLAY_INSTALLER_UpdateInstallOrder), [aChunkIds, aChunkCount]);
        return true;
    }
}
