using Google.Protobuf;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace upc_r1
{
    public class Basics
    {
        public static string GetCuPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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
                IntPtr iptr = Marshal.AllocHGlobal(sizeof(T));
                Marshal.StructureToPtr(item, iptr, false);
                Marshal.WriteIntPtr(main_ptr, indx * sizeof(IntPtr), iptr);
                indx++;
            }
            return main_ptr;
        }

        public static unsafe void FreeListPtr(int count, IntPtr listPointer)
        {
            for (int i = 0; i < count; i++)
            {
                var ptr = Marshal.ReadIntPtr(listPointer, i * sizeof(IntPtr));
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