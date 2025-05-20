using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Save
{
    static readonly Dictionary<int, string> PtrToFilePath = [];

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Close", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Close(IntPtr aOutSaveHandle)
    {
        Log(nameof(UPLAY_SAVE_Close), [aOutSaveHandle]);
        PtrToFilePath.Remove(aOutSaveHandle.ToInt32());
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_GetSavegames", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_GetSavegames(IntPtr aOutGamesList, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_SAVE_GetSavegames), [aOutGamesList, aOverlapped]);
        if (aOutGamesList == IntPtr.Zero)
            return false;
        string savepath = UPC_Json.GetRoot().Save.Path;
        if (!Directory.Exists(savepath))
            Directory.CreateDirectory(savepath);
        List<UPLAY_SAVE_Game> saves = [];
        uint i = 1;
        foreach (var item in Directory.GetFiles(savepath))
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
        WriteOutList(aOutGamesList, saves);
        Basics.WriteOverlappedResult(aOverlapped, true, UPLAY_OverlappedResult.UPLAY_OverlappedResult_Ok);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Open", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Open(uint aSlotId, uint aMode, IntPtr aOutSaveHandle, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_SAVE_Open), [aSlotId, aMode, aOutSaveHandle, aOverlapped]);
        string jsonSavePath = UPC_Json.GetRoot().Save.Path;
        string savePath = string.Empty;
        if (UPC_Json.GetRoot().Save.UseAppIdInName)
            savePath = Path.Combine(jsonSavePath, Main.ProductId.ToString(), $"{aSlotId}.save");
        else
            savePath = Path.Combine(jsonSavePath, $"{aSlotId}.save");
        Log(nameof(UPLAY_SAVE_Open), ["savePath: ", savePath]);
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
        if (!File.Exists(savePath))
            File.Create(savePath).Close();
        int ptr = Random.Shared.Next();
        Log(nameof(UPLAY_SAVE_Open), ["Handle open: ", ptr]);
        PtrToFilePath.Add(ptr, savePath);
        Marshal.WriteInt32(aOutSaveHandle, 0, ptr);
        Basics.WriteOverlappedResult(aOverlapped, true, UPLAY_OverlappedResult.UPLAY_OverlappedResult_Ok);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Read", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Read(IntPtr aSaveHandle, uint aNumOfBytesToRead, uint aOffset, IntPtr aOutBuffer, IntPtr aOutNumOfBytesRead, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_SAVE_Read), [aSaveHandle, aNumOfBytesToRead, aOffset, aOutBuffer, aOutNumOfBytesRead, aOverlapped]);
        if (aSaveHandle == 0)
        {
            Basics.WriteOverlappedResult(aOverlapped, false, UPLAY_OverlappedResult.UPLAY_OverlappedResult_InvalidArgument);
            return false;
        }
        if (!PtrToFilePath.TryGetValue(aSaveHandle.ToInt32(), out string? path))
        {
            Basics.WriteOverlappedResult(aOverlapped, false, UPLAY_OverlappedResult.UPLAY_OverlappedResult_InvalidArgument);
            return false;
        }
        if (path == null)
        {
            Basics.WriteOverlappedResult(aOverlapped, false, UPLAY_OverlappedResult.UPLAY_OverlappedResult_InvalidArgument);
            return false;
        }
        if (aNumOfBytesToRead <= 0)
        {
            Basics.WriteOverlappedResult(aOverlapped, false, UPLAY_OverlappedResult.UPLAY_OverlappedResult_InvalidArgument);
            return false;
        }
        FileStream filestream = File.OpenRead(path);
        var buff = new byte[aNumOfBytesToRead];
        var readed = filestream.Read(buff, (int)aOffset, (int)aNumOfBytesToRead);
        filestream.Close();
        if (readed < 0)
        {
            Basics.WriteOverlappedResult(aOverlapped, false, UPLAY_OverlappedResult.UPLAY_OverlappedResult_InvalidArgument);
            return false;
        }
        Marshal.WriteInt32(aOutNumOfBytesRead, readed);
        Marshal.Copy(buff, 0, aOutBuffer, buff.Length);
        Basics.WriteOverlappedResult(aOverlapped, true, UPLAY_OverlappedResult.UPLAY_OverlappedResult_Ok);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_ReleaseGameList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_ReleaseGameList(IntPtr aGamesList)
    {
        Log(nameof(UPLAY_SAVE_ReleaseGameList), [aGamesList]);
        if (aGamesList == IntPtr.Zero)
            return false;
        FreeList(aGamesList);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Remove", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Remove(uint aSlotId, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_SAVE_Remove), [aSlotId, aOverlapped]);
        string savepath = UPC_Json.GetRoot().Save.Path;
        if (!Directory.Exists(savepath))
            Directory.CreateDirectory(savepath);
        if (UPC_Json.GetRoot().Save.UseAppIdInName)
            savepath = Path.Combine(savepath, Main.ProductId.ToString(), $"{aSlotId}.save");
        var files = Directory.GetFiles(savepath);
        if (files.Length > aSlotId - 1)
        {
            Basics.WriteOverlappedResult(aOverlapped, true, UPLAY_OverlappedResult.UPLAY_OverlappedResult_Failed);
            return false;
        }
        var file = files.ElementAt((int)aSlotId - 1);
        File.Delete(file);
        Basics.WriteOverlappedResult(aOverlapped, true, UPLAY_OverlappedResult.UPLAY_OverlappedResult_Ok);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_SetName", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_SetName(IntPtr aSaveHandle, IntPtr aNameUtf8)
    {
        Log(nameof(UPLAY_SAVE_SetName), [aSaveHandle, aNameUtf8]);
        string? nameUtf = Marshal.PtrToStringAnsi(aNameUtf8);
        if (string.IsNullOrEmpty(nameUtf))
            return false;
        if (!PtrToFilePath.TryGetValue(aSaveHandle.ToInt32(), out string? path))
            return false;
        if (path == null)
            return false;
        string newFileName = path.Replace(Path.GetFileNameWithoutExtension(path), nameUtf);
        File.Copy(path, newFileName);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SAVE_Write", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SAVE_Write(IntPtr aSaveHandle, uint aNumOfBytesToWrite, IntPtr aBuffer, IntPtr aOverlapped)
    {
        Log(nameof(UPLAY_SAVE_Write), [aSaveHandle, aNumOfBytesToWrite, aBuffer, aOverlapped]);
        if (!PtrToFilePath.TryGetValue(aSaveHandle.ToInt32(), out string? path))
        {
            Basics.WriteOverlappedResult(aOverlapped, true, UPLAY_OverlappedResult.UPLAY_OverlappedResult_Failed);
            return false;
        }
        if (path == null)
        {
            Basics.WriteOverlappedResult(aOverlapped, true, UPLAY_OverlappedResult.UPLAY_OverlappedResult_Failed);
            return false;
        }
        var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        var buff = new byte[aNumOfBytesToWrite];
        Marshal.Copy(aBuffer, buff, 0, (int)aNumOfBytesToWrite);
        stream.Write(buff);
        stream.Flush(true);
        stream.Close();
        return true;
    }
}
