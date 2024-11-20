﻿using Google.Protobuf;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace upc_r2;

public class Basics
{
    public static string GetCuPath()
    {
        return AppContext.BaseDirectory;
    }

    public static void Log(string actionName, object[] parameters)
    {
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

    public static unsafe IntPtr GetListPtr<T>(List<T> values) where T : struct
    {
        IntPtr main_ptr = Marshal.AllocHGlobal(sizeof(IntPtr) * values.Count);
        int indx = 0;
        foreach (var item in values)
        {
            IntPtr iptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr(item, iptr, false);
            Marshal.WriteIntPtr(main_ptr, indx * sizeof(IntPtr), iptr);
            indx++;
        }
        return main_ptr;
    }

    public static unsafe List<T> GetListFromPtr<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(BasicList list) where T : struct
    {
        List<T> returner = new List<T>();
        for (int i = 0; i < list.count; i++)
        {
            var ptr = Marshal.ReadIntPtr(list.list, i * Marshal.SizeOf<IntPtr>());
            returner.Add(IntPtrToStruct<T>(ptr));
        }
        return returner;
    }

    public static unsafe void FreeListPtr(int count, IntPtr listPointer)
    {
        for (int i = 0; i < count; i++)
        {
            var ptr = Marshal.ReadIntPtr(listPointer, i * Marshal.SizeOf<IntPtr>());
            Marshal.FreeHGlobal(ptr);
        }
        Marshal.FreeHGlobal(listPointer);
    }

    public static T IntPtrToStruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(IntPtr ptr) where T : struct
    {
        return Marshal.PtrToStructure<T>(ptr);
    }
}