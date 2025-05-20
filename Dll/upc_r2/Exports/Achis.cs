using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports;

internal class Achis
{
    [UnmanagedCallersOnly(EntryPoint = "UPC_AchievementImageFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_AchievementImageFree(IntPtr inContext, IntPtr inImageRGBA)
    {
        Log(nameof(UPC_AchievementImageFree), [inContext, inImageRGBA]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_AchievementImageGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_AchievementImageGet(IntPtr inContext, uint inId, IntPtr outImageRGBA, IntPtr inCallback, IntPtr inCallbackData)
    {
        Log(nameof(UPC_AchievementImageGet), [inContext, inId, outImageRGBA, inCallback, inCallbackData]);
        Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, (int)UPC_Result.UPC_Result_CommunicationError));
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_AchievementListFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_AchievementListFree(IntPtr inContext, IntPtr inAchievementList)
    {
        Log(nameof(UPC_AchievementListFree), [inContext, inAchievementList]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_AchievementListGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_AchievementListGet(IntPtr inContext, IntPtr inOptUserIdUtf8, uint inFilter, IntPtr outAchievementList, IntPtr inCallback, IntPtr inCallbackData)
    {
        Log(nameof(UPC_AchievementListGet), [inContext, inOptUserIdUtf8, outAchievementList, inCallback, inCallbackData]);
        Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, (int)UPC_Result.UPC_Result_CommunicationError));
        var chunkptr = Marshal.AllocHGlobal(Marshal.SizeOf<BasicList>());
        BasicList list = new();
        list.count = 0;
        list.list = IntPtr.Zero;
        Marshal.StructureToPtr(list, chunkptr, false);
        Marshal.WriteIntPtr(outAchievementList, chunkptr);
        return 2000;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_AchievementUnlock", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_AchievementUnlock(IntPtr inContext, uint inId, IntPtr inOptCallback, IntPtr inOptCallbackData)
    {
        Log(nameof(UPC_AchievementUnlock), [inContext, inId, inOptCallback, inOptCallbackData]);
        Main.GlobalContext.Callbacks.Add(new(inOptCallback, inOptCallbackData, (int)UPC_Result.UPC_Result_CommunicationError));
        return 0;
    }
}
