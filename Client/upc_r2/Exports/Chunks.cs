using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports
{
    internal class Chunks
    {
        public struct chunk_list
        {
            public uint count;
            public IntPtr list;
        };

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_InstallChunkListFree(IntPtr inContext, IntPtr inChunkList)
        {
            Basics.Log(nameof(UPC_InstallChunkListFree), new object[] { inContext, inChunkList });
            if (inContext == IntPtr.Zero || inChunkList == IntPtr.Zero)
                return -0xd;
            var chunk = Marshal.PtrToStructure<chunk_list>(inChunkList);
            if (chunk.list != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(chunk.list);
            }
            Marshal.DestroyStructure<chunk_list>(inChunkList);
            Marshal.FreeHGlobal(inChunkList);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static unsafe int UPC_InstallChunkListGet(IntPtr inContext, [Out] IntPtr inChunkList)
        {
            Basics.Log(nameof(UPC_InstallChunkListGet), new object[] { inContext, inChunkList });
            if (inContext == IntPtr.Zero || inChunkList == IntPtr.Zero)
                return -0xd;
            var chunkptr = Marshal.AllocHGlobal(sizeof(chunk_list));
            chunk_list chunk = new();
            chunk.count = 0;
            chunk.list = IntPtr.Zero;
            Marshal.StructureToPtr<chunk_list>(chunk, chunkptr, false);
            Marshal.WriteIntPtr(inChunkList, chunkptr);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_InstallChunksOrderUpdate(IntPtr inContext, IntPtr inChunkIds, uint inChunkCount)
        {
            Basics.Log(nameof(UPC_InstallChunksOrderUpdate), new object[] { inContext, inChunkIds, inChunkCount });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_InstallChunksPresenceCheck(IntPtr inContext, [Out] IntPtr inChunkIds, uint inChunkCount)
        {
            Basics.Log(nameof(UPC_InstallChunksPresenceCheck), new object[] { inContext, inChunkIds, inChunkCount });
            return 0;
        }
    }
}
