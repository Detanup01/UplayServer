using Microsoft.Win32.SafeHandles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static upc_r2.Basics;

namespace upc_r2.Exports;

internal class Storage
{
    [StructLayout(LayoutKind.Sequential)]
    struct UPC_StorageFile
    {
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string fileNameUtf8;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string legacyNameUtf8;
        [MarshalAs(UnmanagedType.U4)]
        public uint size;
        [MarshalAs(UnmanagedType.U8)]
        public ulong lastModifiedMs;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileListGet", CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe int UPC_StorageFileListGet(IntPtr inContext, [Out] IntPtr outStorageFileList)
    {
        Log(nameof(UPC_StorageFileListGet), [inContext, outStorageFileList]);
        List<UPC_StorageFile> storageFiles = new();
        Log(nameof(UPC_StorageFileListGet), [Main.GlobalContext.Config.Saved.savePath]);
        if (!Directory.Exists(Main.GlobalContext.Config.Saved.savePath))
            Directory.CreateDirectory(Main.GlobalContext.Config.Saved.savePath);
        var files = Directory.GetFiles(Main.GlobalContext.Config.Saved.savePath);
        foreach (var item in files)
        {
            if (item != string.Empty)
            {
                FileInfo info = new(item);
                UPC_StorageFile storageFile = new();
                storageFile.fileNameUtf8 = info.Name;
                storageFile.legacyNameUtf8 = info.Name;
                storageFile.size = (uint)((uint)info.Length);
                storageFile.lastModifiedMs = (ulong)(info.LastWriteTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                storageFiles.Add(storageFile);
            }
        }
        Log(nameof(UPC_StorageFileListGet), ["storageFiles Count: ", storageFiles.Count]);
        if (storageFiles.Count == 0)
            return -6;
        var listptr = Basics.GetListPtr(storageFiles);
        BasicList list = new()
        {
            count = storageFiles.Count,
            list = listptr
        };
        IntPtr ptr = Marshal.AllocHGlobal(sizeof(BasicList));
        Marshal.StructureToPtr(list, ptr, false);
        Marshal.WriteIntPtr(outStorageFileList, ptr);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileListFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StorageFileListFree(IntPtr inContext, IntPtr inStorageFileList)
    {
        Log(nameof(UPC_StorageFileListFree), [inContext, inStorageFileList]);
        if (inStorageFileList == IntPtr.Zero)
            return -0xd;
        var list = IntPtrToStruct<BasicList>(inStorageFileList);
        if (list.count > 0)
        {
            FreeListPtr(list.count, list.list);
        }
        Marshal.FreeHGlobal(inStorageFileList);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileOpen", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StorageFileOpen(IntPtr inContext, IntPtr inFileNameUtf8, uint inFlags, IntPtr outHandle)
    {
        Log(nameof(UPC_StorageFileOpen), [inContext, inFileNameUtf8, inFlags, outHandle]);

        FileMode oflag = FileMode.OpenOrCreate;
        if (inFlags == 0x2) oflag |= FileMode.Truncate;
        string? filename = Marshal.PtrToStringUTF8(inFileNameUtf8);
        Log(nameof(UPC_StorageFileOpen), ["Filename is null?: ", filename == null]);
        if (filename == null)
            return (int)UPC_Result.UPC_Result_CommunicationError;
        Log(nameof(UPC_StorageFileOpen), ["Filename: ", filename]);
        try 
        {
            var opened = File.Open(Path.Combine(Main.GlobalContext.Config.Saved.savePath, filename), oflag, FileAccess.ReadWrite, FileShare.ReadWrite);
            var sfh = opened.SafeFileHandle;
            var handler = sfh.DangerousGetHandle();;
            Marshal.WriteIntPtr(outHandle, 0, handler);
        }
        catch (Exception ex)
        {
            Log(nameof(UPC_StorageFileOpen), ["Exception!", ex]);
        }

        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileRead", CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe int UPC_StorageFileRead(IntPtr inContext, IntPtr inHandle, int inBytesToRead, uint inBytesReadOffset, IntPtr outData, IntPtr outBytesRead, IntPtr inCallback, IntPtr inCallbackData)
    {
        Basics.Log(nameof(UPC_StorageFileRead), [inContext, inHandle, inBytesToRead, inBytesReadOffset, outData, outBytesRead, inCallback, inCallbackData]);

        if (inBytesToRead != 0)
        {
            SafeFileHandle fileHandle = new(inHandle, false);
            var stream = new FileStream(fileHandle, FileAccess.ReadWrite);
            var buff = new byte[inBytesToRead];
            var readed = stream.Read(buff, (int)inBytesReadOffset, inBytesToRead);
            Marshal.WriteInt32(outBytesRead, readed);
            Marshal.Copy(buff, 0, outData, buff.Length);
        }
        var cbList = Main.GlobalContext.Callbacks.ToList();
        cbList.Add(new(inCallback, inCallbackData, 0));
        Main.GlobalContext.Callbacks = cbList.ToArray();
        return 0x10000;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileWrite", CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe int UPC_StorageFileWrite(IntPtr inContext, IntPtr inHandle, IntPtr inData, int inSize, IntPtr inCallback, IntPtr inCallbackData)
    {
        Basics.Log(nameof(UPC_StorageFileWrite), [inContext, inHandle, inData, inSize, inCallback, inCallbackData]);
        SafeFileHandle fileHandle = new(inHandle, false);
        var stream = new FileStream(fileHandle, FileAccess.ReadWrite);
        var buff = new byte[inSize];
        Marshal.Copy(inData, buff, 0, inSize);
        stream.Write(buff);
        stream.Flush(true); 
        var cbList = Main.GlobalContext.Callbacks.ToList();
        cbList.Add(new(inCallback, inCallbackData, 0));
        Main.GlobalContext.Callbacks = cbList.ToArray();
        return 0x10000;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileClose", CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe int UPC_StorageFileClose(IntPtr inContext, IntPtr inHandle)
    {
        Basics.Log(nameof(UPC_StorageFileClose), [inContext, inHandle]);
        SafeFileHandle fileHandle = new(inHandle, false);
        fileHandle.Close();
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_StorageFileDelete", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_StorageFileDelete(IntPtr inContext, IntPtr inFileNameUtf8)
    {
        Basics.Log(nameof(UPC_StorageFileDelete), [inContext, inFileNameUtf8]);
        return 0;
    }
}
