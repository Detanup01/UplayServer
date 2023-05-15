using Microsoft.Win32.SafeHandles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static upc_r2.Basics;

namespace upc_r2.Exports
{
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

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static unsafe int UPC_StorageFileListGet(IntPtr inContext, [Out] IntPtr outStorageFileList)
        {

            Basics.Log(nameof(UPC_StorageFileListGet), new object[] { inContext, outStorageFileList });
            List<UPC_StorageFile> storageFiles = new();
            var files = Directory.GetFiles(Main.GlobalContext.Config.savePath);
            foreach (var item in files)
            {
                if (item != string.Empty)
                {
                    FileInfo info = new(item);
                    UPC_StorageFile storageFile = new();
                    storageFile.fileNameUtf8 = info.Name;
                    storageFile.legacyNameUtf8 = info.Name;
                    storageFile.size = (uint)info.Length;
                    storageFile.lastModifiedMs = (ulong)(info.LastWriteTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                    storageFiles.Add(storageFile);
                }
            }
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

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_StorageFileListFree(IntPtr inContext, IntPtr inStorageFileList)
        {
            Basics.Log(nameof(UPC_StorageFileListFree), new object[] { inContext, inStorageFileList });
            if (inStorageFileList == IntPtr.Zero)
                return -0xd;
            var list = Basics.IntPtrToStruct<Basics.BasicList>(inStorageFileList);
            if (list.count > 0)
            {
                Basics.FreeListPtr(list.count, list.list);
            }
            Marshal.FreeHGlobal(inStorageFileList);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_StorageFileOpen(IntPtr inContext, IntPtr inFileNameUtf8, uint inFlags, IntPtr outHandle)
        {
            Basics.Log(nameof(UPC_StorageFileOpen), new object[] { inContext, inFileNameUtf8, inFlags, outHandle });

            FileMode oflag = (FileMode.Open | FileMode.Create | FileMode.OpenOrCreate);
            if (inFlags == 0x2) oflag |= FileMode.Truncate;
            var filename = Marshal.PtrToStringUTF8(inFileNameUtf8);
            var handler = File.OpenHandle(Path.Combine(Main.GlobalContext.Config.savePath, filename), oflag, FileAccess.ReadWrite, FileShare.ReadWrite);
            var inthandler = handler.DangerousGetHandle();
            Marshal.WriteIntPtr(outHandle, inthandler);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static unsafe int UPC_StorageFileRead(IntPtr inContext, IntPtr inHandle, int inBytesToRead, uint inBytesReadOffset, IntPtr outData, IntPtr outBytesRead, IntPtr inCallback, IntPtr inCallbackData)
        {
            Basics.Log(nameof(UPC_StorageFileRead), new object[] { inContext, inHandle, inBytesToRead, inBytesReadOffset, outData, outBytesRead, inCallback, inCallbackData });
            SafeFileHandle fileHandle = new(inHandle, false);
            var stream = new FileStream(fileHandle, FileAccess.ReadWrite);
            var buff = new byte[inBytesToRead];
            var readed = stream.Read(buff, (int)inBytesReadOffset, inBytesToRead);
            Marshal.WriteInt32(outBytesRead, readed);
            Marshal.Copy(buff, 0, outData, buff.Length);
            Main.GlobalContext.Callbacks.Append(new(inCallback, inCallbackData, 0));
            return 0x10000;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static unsafe int UPC_StorageFileWrite(IntPtr inContext, IntPtr inHandle, IntPtr inData, int inSize, IntPtr inCallback, IntPtr inCallbackData)
        {
            Basics.Log(nameof(UPC_StorageFileOpen), new object[] { inContext, inHandle, inData, inSize, inCallback, inCallbackData });
            SafeFileHandle fileHandle = new(inHandle, false);
            var stream = new FileStream(fileHandle, FileAccess.ReadWrite);
            var buff = new byte[inSize];
            Marshal.Copy(inData, buff, 0, inSize);
            stream.Write(buff, 0, inSize);
            Main.GlobalContext.Callbacks.Append(new(inCallback, inCallbackData, 0));
            return 0x10000;
        }
    }
}
