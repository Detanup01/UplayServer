using Google.Protobuf;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace upc_r2
{
    public class Basics
    {
        public static string GetCuPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static void Log(string actionName, object[] parameters)
        {
            File.AppendAllText(GetCuPath() + "\\upc_r2.log", $"{Process.GetCurrentProcess().Id} | {actionName} {string.Join(", ", parameters)}\n");
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
            if (IfReqLog())
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
            if (IfRspLog())
            {
                LogRsp(rsp);
            }
            //Log("SendReq", new object[] { "Done!" });
        }

        public static bool IfReqLog()
        {
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<json.Root>(File.ReadAllText(GetCuPath() + "\\upc.json"));
            if (data != null && data.Base.ReqLog)
            {
                return data.Base.ReqLog;
            }
            else return false;
        }

        public static bool IfRspLog()
        {
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<json.Root>(File.ReadAllText(GetCuPath() + "\\upc.json"));
            if (data != null && data.Base.RspLog)
            {
                return data.Base.RspLog;
            }
            else return false;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BasicList
        {
            [MarshalAs(UnmanagedType.I4)]
            public int count;
            [MarshalAs(UnmanagedType.SysInt)]
            public IntPtr list;
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

        public static unsafe List<T> GetListFromPtr<T>(BasicList list) where T : struct
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

        public static T IntPtrToStruct<T>(IntPtr ptr) where T : struct
        {
            return (T)((object)Marshal.PtrToStructure(ptr, typeof(T)));
        }
    }
}