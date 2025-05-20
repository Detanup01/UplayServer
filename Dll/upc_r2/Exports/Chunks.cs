using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports;

internal class Chunks
{

    [UnmanagedCallersOnly(EntryPoint = "UPC_InstallChunkListFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_InstallChunkListFree(IntPtr inContext, IntPtr inChunkList)
    {
        Log(nameof(UPC_InstallChunkListFree), [inContext, inChunkList]);
        if (inContext == IntPtr.Zero || inChunkList == IntPtr.Zero)
            return (int)UPC_Result.UPC_Result_FailedPrecondition;
        //Basics.FreeList(inChunkList);
        Marshal.FreeHGlobal(inChunkList);
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_InstallChunkListGet", CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe int UPC_InstallChunkListGet(IntPtr inContext, [Out] IntPtr inChunkList)
    {
        Log(nameof(UPC_InstallChunkListGet), [inContext, inChunkList]);
        if (inContext == IntPtr.Zero || inChunkList == IntPtr.Zero)
            return (int)UPC_Result.UPC_Result_FailedPrecondition;
        List<ChunkId> chunkIds = [];
        foreach (var item in UPC_Json.GetRoot().ChunkIds)
        {
            chunkIds.Add(new() { Id = item.ChunkId, IsInstalled = 1, Tag = item.ChunkTag });
        }
        WriteOutList(inChunkList, chunkIds);
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_InstallChunksOrderUpdate", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_InstallChunksOrderUpdate(IntPtr inContext, IntPtr inChunkIds, uint inChunkCount)
    {
        Log(nameof(UPC_InstallChunksOrderUpdate), [inContext, inChunkIds, inChunkCount]);
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_InstallChunksOrderUpdate_Extended", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_InstallChunksOrderUpdate_Extended(IntPtr inContext, IntPtr inChunkIds, uint inChunkCount, IntPtr unk1, IntPtr unk2)
    {
        Log(nameof(UPC_InstallChunksOrderUpdate_Extended), [inContext, inChunkIds, inChunkCount, unk1, unk2]);
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_InstallChunksPresenceCheck", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_InstallChunksPresenceCheck(IntPtr inContext, IntPtr inChunkIds, uint inChunkCount)
    {
        Log(nameof(UPC_InstallChunksPresenceCheck), [inContext, inChunkIds, inChunkCount]);
        return (int)UPC_Result.UPC_Result_Ok;
    }
}
