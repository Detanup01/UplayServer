using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2
{
    internal class Structs
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct cb_data
        {
            public cb_data(IntPtr fn, IntPtr contextdata, int uarg)
            {
                context_data = contextdata;
                arg = uarg;
                fun = fn;
            }

            [MarshalAs(UnmanagedType.SysInt)]
            public IntPtr fun;
            [MarshalAs(UnmanagedType.I4)]
            public int arg;
            [MarshalAs(UnmanagedType.SysInt)]
            public IntPtr context_data;

        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct Emu
        {
            public string name;
        
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Context
        {
            [MarshalAs(UnmanagedType.U1)]
            public bool initialized;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStruct)]
            public cb_data[] cb;
            public Emu emu;
        }

        public static Context GlobalContext = new Context();
        public static IntPtr GlobalContextPTR = IntPtr.Zero;

        [UnmanagedCallersOnly(EntryPoint = "getcontext", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr getcontext()
        {
            GlobalContext = new Context();
            GlobalContext.initialized = false;
            GlobalContext.cb = new cb_data[1];
            GlobalContextPTR = Marshal.AllocHGlobal(Marshal.SizeOf(GlobalContext));
            Marshal.StructureToPtr(GlobalContext, GlobalContextPTR, false);
            return GlobalContextPTR;
        }

        [UnmanagedCallersOnly(EntryPoint = "freecontext", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int freecontext(IntPtr data)
        {
            Marshal.DestroyStructure<Context>(data);
            Marshal.FreeHGlobal(data);
            GlobalContext = new();
            GlobalContextPTR = IntPtr.Zero;
            return 0;
        }

        [UnmanagedCallersOnly(EntryPoint = "usecontext", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int usecontext(IntPtr data, IntPtr inCallback, IntPtr inCallbackData)
        {
            if (data != GlobalContextPTR)
            {
                return -1;
            }

            GlobalContext.initialized = true;
            var cl = GlobalContext.cb.ToList(); 
            cl.Add(new cb_data(inCallback, inCallbackData, 0));
            GlobalContext.cb = cl.ToArray();
            return 0;
        }

        [UnmanagedCallersOnly(EntryPoint = "updatecontext", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static unsafe int updatecontext(IntPtr data)
        {
            if (data != GlobalContextPTR)
            {
                return -1;
            }
            var cblist = GlobalContext.cb.ToList();
            foreach (var cb in cblist)
            {
                if (cb.fun != IntPtr.Zero)
                {
                    delegate* unmanaged<int, IntPtr, void> @delegate;
                    @delegate = (delegate* unmanaged<int, IntPtr, void>)cb.fun;
                    @delegate(cb.arg, cb.context_data);
                }
            }
            GlobalContext.cb = cblist.ToArray();
            return 0;
        }

    }
}
