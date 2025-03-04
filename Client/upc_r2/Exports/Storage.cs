using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static upc_r2.Basics;

namespace upc_r2.Exports;

internal class Storage
{
    static readonly Dictionary<int, string> PtrToFilePath = [];

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileListGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StorageFileListGet(IntPtr inContext, IntPtr outStorageFileList)
    {
        Log(nameof(UPC_StorageFileListGet), [inContext, outStorageFileList]);
        List<UPC_StorageFile> storageFiles = [];
        Log(nameof(UPC_StorageFileListGet), [Main.GlobalContext.Config.Saved.savePath]);
        if (!Directory.Exists(Main.GlobalContext.Config.Saved.savePath))
            Directory.CreateDirectory(Main.GlobalContext.Config.Saved.savePath);
        var files = Directory.GetFiles(Main.GlobalContext.Config.Saved.savePath);
        foreach (var item in files)
        {
            if (string.IsNullOrEmpty(item))
                continue;

            FileInfo info = new(item);
            UPC_StorageFile storageFile = new()
            {
                fileNameUtf8 = info.Name,
                legacyNameUtf8 = info.Name,
                size = (uint)info.Length,
                lastModifiedMs = (ulong)(info.LastWriteTime - new DateTime(1970, 1, 1)).TotalMilliseconds
            };
            storageFiles.Add(storageFile);
        }
        Log(nameof(UPC_StorageFileListGet), ["storageFiles Count: ", storageFiles.Count]);
        if (storageFiles.Count == 0)
            return -6;
        WriteOutList(outStorageFileList, storageFiles);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileListFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StorageFileListFree(IntPtr inContext, IntPtr inStorageFileList)
    {
        Log(nameof(UPC_StorageFileListFree), [inContext, inStorageFileList]);
        if (inStorageFileList == IntPtr.Zero)
            return -0xd;
        var list = Marshal.PtrToStructure<BasicList>(inStorageFileList);
        if (list.count > 0)
            FreeListPtr(list.count, list.list);
        Marshal.FreeHGlobal(inStorageFileList);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileOpen", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StorageFileOpen(IntPtr inContext, IntPtr inFileNameUtf8, uint inFlags, IntPtr outHandle)
    {
        Log(nameof(UPC_StorageFileOpen), [inContext, inFileNameUtf8, inFlags, outHandle]);
        UPC_FileOpenMode mode = (UPC_FileOpenMode)inFlags;
        FileMode oflag = FileMode.OpenOrCreate;
        if (mode == UPC_FileOpenMode.UPC_FileOpenMode_Write)
            oflag |= FileMode.Truncate;
        string? filename = Marshal.PtrToStringUTF8(inFileNameUtf8);
        if (filename == null)
            return (int)UPC_Result.UPC_Result_CommunicationError;
        string file = string.Empty;
        if (UPC_Json.GetRoot().Save.UseProductIdInName)
            file = Path.Combine(Main.GlobalContext.Config.Saved.savePath, Main.GlobalContext.Config.ProductId.ToString(), filename);
        else
            file = Path.Combine(Main.GlobalContext.Config.Saved.savePath, filename);
        Log(nameof(UPC_StorageFileOpen), ["filename", filename,"file_withpath", file, "dirname is null?", Path.GetDirectoryName(file) == null]);
        Log(nameof(UPC_StorageFileOpen), ["open mode", mode]);
        if (!Directory.Exists(Path.GetDirectoryName(file)))
            Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        if (!File.Exists(file))
            File.Create(file).Close();
        int ptr = Random.Shared.Next();
        PtrToFilePath.Add(ptr, file);
        Log(nameof(UPC_StorageFileOpen), ["File handler", ptr]);
        Marshal.WriteInt32(outHandle, 0, ptr);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileRead", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StorageFileRead(IntPtr inContext, int inHandle, int inBytesToRead, uint inBytesReadOffset, IntPtr outData, IntPtr outBytesRead, IntPtr inCallback, IntPtr inCallbackData)
    {
        Log(nameof(UPC_StorageFileRead), [inContext, inHandle, inBytesToRead, inBytesReadOffset, outData, outBytesRead, inCallback, inCallbackData]);
        if (inHandle == 0)
        {
            Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, (int)UPC_Result.UPC_Result_FailedPrecondition));
            return -13;
        }
        if (!PtrToFilePath.TryGetValue(inHandle, out string? path))
        {
            Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, (int)UPC_Result.UPC_Result_FailedPrecondition));
            return -13;
        }
        if (path == null)
        {
            Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, (int)UPC_Result.UPC_Result_FailedPrecondition));
            return -13;
        }
        if (inBytesToRead <= 0)
        {
            Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, (int)UPC_Result.UPC_Result_FailedPrecondition));
            return -13;
        }

        var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        stream.Seek(0, SeekOrigin.Begin);
        if (stream.Length < inBytesReadOffset)
        {
            Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, (int)UPC_Result.UPC_Result_EOF));
            return -13;
        }
        var buff = new byte[inBytesToRead];
        var readed = stream.Read(buff, (int)inBytesReadOffset, inBytesToRead);
        stream.Close();
        Log(nameof(UPC_StorageFileRead), ["bytes readed:", readed, "must read:", inBytesToRead]);
        if (readed < 0)
        {
            Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, (int)UPC_Result.UPC_Result_EOF));
            return -13;
        }
        Marshal.WriteInt32(outBytesRead, readed);
        Marshal.Copy(buff, 0, outData, buff.Length);
        Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, (int)UPC_Result.UPC_Result_Ok));
        Log(nameof(UPC_StorageFileRead), ["Read Done!"]);
        return 0x10000;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileWrite", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StorageFileWrite(IntPtr inContext, int inHandle, IntPtr inData, int inSize, IntPtr inCallback, IntPtr inCallbackData)
    {
        Log(nameof(UPC_StorageFileWrite), [inContext, inHandle, inData, inSize, inCallback, inCallbackData]);
        if (!PtrToFilePath.TryGetValue(inHandle, out string? path))
        {
            Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, (int)UPC_Result.UPC_Result_FailedPrecondition));
            return -13;
        }
        if (path == null)
        {
            Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, (int)UPC_Result.UPC_Result_FailedPrecondition));
            return -13;
        }
        var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        var buff = new byte[inSize];
        Marshal.Copy(inData, buff, 0, inSize);
        stream.Write(buff);
        stream.Flush(true);
        stream.Close();
        Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, (int)UPC_Result.UPC_Result_Ok));
        Log(nameof(UPC_StorageFileWrite), ["Write Done!"]);
        return 0x10000;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileClose", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StorageFileClose(IntPtr inContext, int inHandle)
    {
        Log(nameof(UPC_StorageFileClose), [inContext, inHandle]);
        PtrToFilePath.Remove(inHandle);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileDelete", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StorageFileDelete(IntPtr inContext, IntPtr inFileNameUtf8)
    {
        Log(nameof(UPC_StorageFileDelete), [inContext, inFileNameUtf8]);
        if (UPC_Json.GetRoot().Save.EnableFileDelete)
        {
            string? fileName = Marshal.PtrToStringUTF8(inFileNameUtf8);
            if (string.IsNullOrEmpty(fileName))
                return 0;
            string file = string.Empty;
            if (UPC_Json.GetRoot().Save.UseProductIdInName)
                file = Path.Combine(Main.GlobalContext.Config.Saved.savePath, Main.GlobalContext.Config.ProductId.ToString(), fileName);
            else
                file = Path.Combine(Main.GlobalContext.Config.Saved.savePath, fileName);
            if (string.IsNullOrEmpty(file))
                return 0;
            File.Delete(file);
        }
        return 0;
    }
}
