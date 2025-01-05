using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace upc_r1;

public class Basics
{
    public static string GetCuPath()
    {
        return AppContext.BaseDirectory;
    }

    public static void Log(string actionName, object[] parameters)
    {
        File.AppendAllText(GetCuPath() + "\\upc_r1.log", $"{Process.GetCurrentProcess().Id} | {actionName} {string.Join(", ", parameters)}\n");
    }

    public static void LogReq(Uplay.Uplaydll.Req req)
    {
        File.AppendAllText(GetCuPath() + "\\upc_r1_req.log", $"{req.ToString()}\n");
    }

    public static void LogRsp(Uplay.Uplaydll.Rsp rsp)
    {
        File.AppendAllText(GetCuPath() + "\\upc_r1_rsp.log", $"{rsp.ToString()}\n");
    }

    public static uint ReqId = uint.MinValue;

    public static IntPtr GetListPtr<T>(List<T> values) where T : struct
    {
        IntPtr main_ptr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>() * values.Count);
        int indx = 0;
        foreach (var item in values)
        {
            IntPtr iptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr(item, iptr, false);
            Marshal.WriteIntPtr(main_ptr, indx * Marshal.SizeOf<IntPtr>(), iptr);
            indx++;
        }
        return main_ptr;
    }

    public static void FreeList(IntPtr listPointer)
    {
        BasicList upcList = Marshal.PtrToStructure<BasicList>(listPointer);
        FreeListPtr(upcList.count, upcList.list);
        Marshal.FreeHGlobal(listPointer);
    }

    public static void FreeListPtr(int count, IntPtr listPointer)
    {
        for (int i = 0; i < count; i++)
        {
            var ptr = Marshal.ReadIntPtr(listPointer, i * Marshal.SizeOf<IntPtr>());
            Marshal.FreeHGlobal(ptr);
        }
        Marshal.FreeHGlobal(listPointer);
    }

    public static void WriteOutList(IntPtr outList, int Count, IntPtr ptrToList)
    {
        BasicList list = new(Count, ptrToList);
        IntPtr iptr = Marshal.AllocHGlobal(Marshal.SizeOf<BasicList>());
        Marshal.StructureToPtr(list, iptr, false);
        Marshal.WriteIntPtr(outList, 0, iptr);
    }

    public static void WriteOutList<T>(IntPtr outList, List<T> values) where T : struct
    {
        BasicList list = new(values.Count, GetListPtr(values));
        IntPtr iptr = Marshal.AllocHGlobal(Marshal.SizeOf<BasicList>());
        Marshal.StructureToPtr(list, iptr, false);
        Marshal.WriteIntPtr(outList, 0, iptr);
    }

    public static void WriteOverlappedResult(IntPtr Overlapped, bool IsSuccess, UPLAY_OverlappedResult result)
    {
        var overlapped = Marshal.PtrToStructure<UPLAY_Overlapped>(Overlapped);
        overlapped.Completed = IsSuccess;
        overlapped.Result = UPLAY_OverlappedResult.UPLAY_OverlappedResult_Ok;
        Marshal.StructureToPtr(overlapped, Overlapped, false);
    }
}