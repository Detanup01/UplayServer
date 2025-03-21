using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Save
{
    static readonly Dictionary<int, string> PtrToFilePath = [];

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Close", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Close(IntPtr aOutSaveHandle)
    {
        Basics.Log(nameof(UPLAY_SAVE_Close), [aOutSaveHandle]);
        PtrToFilePath.Remove(aOutSaveHandle.ToInt32());
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_GetSavegames", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_GetSavegames(IntPtr aOutGamesList, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_SAVE_GetSavegames), [aOutGamesList, aOverlapped]);
        if (aOutGamesList == IntPtr.Zero)
            return false;
        string savepath = UPC_Json.GetRoot().Save.Path;
        if (!Directory.Exists(savepath))
            Directory.CreateDirectory(savepath);
        var files = Directory.GetFiles(savepath);
        List<UPLAY_SAVE_Game> saves = [];
        uint i = 1;
        foreach (var item in files)
        {
            if (string.IsNullOrEmpty(item))
                continue;

            FileInfo info = new(item);
            UPLAY_SAVE_Game saveGame = new()
            {
                nameUtf8 = info.Name,
                id = i,
                size = (uint)info.Length
            };
            saves.Add(saveGame);
            i++;
        }
        Basics.WriteOutList(aOutGamesList, saves);
        Basics.WriteOverlappedResult(aOverlapped, true, UPLAY_OverlappedResult.UPLAY_OverlappedResult_Ok);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Open", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Open(uint aSlotId, uint aMode, IntPtr aOutSaveHandle, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_SAVE_Open), [aSlotId, aMode, aOutSaveHandle, aOverlapped]);
        UPLAY_SAVE_Mode saveMode = (UPLAY_SAVE_Mode)aMode;

        var savePath = UPC_Json.GetRoot().Save.Path;
        var spath = Path.Combine(savePath, $"{aSlotId}.save");
        Basics.Log(nameof(UPLAY_SAVE_Open), ["savePath: ", spath]);
        if (!Directory.Exists(Path.GetDirectoryName(spath)))
            Directory.CreateDirectory(Path.GetDirectoryName(spath)!);
        if (!File.Exists(spath))
            File.Create(spath).Close();
        int ptr = Random.Shared.Next();
        Basics.Log(nameof(UPLAY_SAVE_Open), ["Handle open: ", ptr]);
        PtrToFilePath.Add(ptr, spath);
        Marshal.WriteInt32(aOutSaveHandle, 0, ptr);
        Basics.WriteOverlappedResult(aOverlapped, true, UPLAY_OverlappedResult.UPLAY_OverlappedResult_Ok);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Read", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Read(IntPtr aSaveHandle, uint aNumOfBytesToRead, uint aOffset, IntPtr aOutBuffer, uint aOutNumOfBytesRead, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_SAVE_Read), [aSaveHandle, aNumOfBytesToRead, aOffset, aOutBuffer, aOutNumOfBytesRead, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_ReleaseGameList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_ReleaseGameList(IntPtr aGamesList)
    {
        Basics.Log(nameof(UPLAY_SAVE_ReleaseGameList), [aGamesList]);
        Basics.FreeList(aGamesList);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Remove", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Remove(uint aSlotId, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_SAVE_Remove), [aSlotId, aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_SetName", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_SetName(IntPtr aSaveHandle, IntPtr aNameUtf8)
    {
        Basics.Log(nameof(UPLAY_SAVE_SetName), [aSaveHandle, aNameUtf8]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Write", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Write(IntPtr aSaveHandle, uint aNumOfBytesToWrite, IntPtr aBuffer, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_SAVE_Write), [aSaveHandle, aNumOfBytesToWrite, aBuffer, aOverlapped]);
        return true;
    }
}
