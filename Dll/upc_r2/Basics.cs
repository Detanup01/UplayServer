using Google.Protobuf;
using System.Diagnostics;
using System.Runtime.InteropServices;
using upc_r2.Exports;

namespace upc_r2;

public class Basics
{
    public static string GetCuPath()
    {
        return AppContext.BaseDirectory;
    }

    public static void Log(string actionName, object[] parameters)
    {
        if (actionName == "UPC_Update" && !UPC_Json.GetRoot().BasicLog.LogUpdate)
            return;
        File.AppendAllText(GetCuPath() + "\\upc_r2.log", $"{Process.GetCurrentProcess().Id} | {actionName} {string.Join(", ", parameters)}\n");
    }

    public static void Log(string actionName)
    {
        File.AppendAllText(GetCuPath() + "\\upc_r2.log", $"{Process.GetCurrentProcess().Id} | {actionName}\n");
    }

    public static void LogReq(Uplay.Uplaydll.Req req)
    {
        File.AppendAllText(GetCuPath() + "\\upc_r2_req.log", $"{req.ToString()}\n");
    }

    public static void LogRsp(Uplay.Uplaydll.Rsp rsp)
    {
        File.AppendAllText(GetCuPath() + "\\upc_r2_rsp.log", $"{rsp.ToString()}\n");
    }

    public static void SendReq(Uplay.Uplaydll.Req req, out Uplay.Uplaydll.Rsp rsp)
    {
        if (UPC_Json.GetRoot().BasicLog.ReqLog)
        {
            LogReq(req);
        }

        Uplay.Demux.Upstream upstream = new()
        {
            Request = new()
            {
                RequestId = 0,
                ServiceRequest = new()
                {
                    Service = "uplaydll",
                    Data = ByteString.CopyFrom(req.ToByteArray())
                }
            }
        };
        rsp = new();
        NamePipe.NamePipeReqRsp(upstream, out rsp);
        if (UPC_Json.GetRoot().BasicLog.RspLog)
        {
            LogRsp(rsp);
        }
        //Log("SendReq", new object[] { "Done!" });
    }

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

}