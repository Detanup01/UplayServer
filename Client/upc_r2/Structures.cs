using System.Runtime.InteropServices;

namespace upc_r2
{
    public class Structures
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Context
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStruct)]
            public Callback[] Callbacks;
            public Config Config;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Callback
        {
            public Callback(IntPtr fn, IntPtr contextdata, int uarg)
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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct Config
        {
            public InitSaved Saved;
            public uint ProductId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct InitSaved
        {
            public Uplay.Uplaydll.Account account;
            public string savePath;
        }

    }
}
