using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace dummydll
{
    public unsafe class dummy
    {

        [UnmanagedCallersOnly(EntryPoint = "_yeet", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int _()
        {
            return 1337;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct test
        {
            [MarshalAs(UnmanagedType.LPUTF8Str)]
            public string nameUtf8;
        }

        public static T IntPtrToStruct<T>(IntPtr ptr) where T : struct
        {
            return (T)((object)Marshal.PtrToStructure(ptr, typeof(T)));
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int ProductListGet([Out] IntPtr outProductList)
        {
            test test = new();
            test.nameUtf8 = "yeeeet";
            var ptr = Marshal.AllocHGlobal(sizeof(test));
            Marshal.StructureToPtr(test, ptr, false);
            Marshal.WriteIntPtr(outProductList, ptr);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int ProductListFree(IntPtr inProductList)
        {
            Marshal.FreeHGlobal(inProductList);
            return 0;
        }
    }
}