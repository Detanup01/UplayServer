using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static upc_r2.Enums;

namespace upc_r2
{
    public class Main
    {
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
            public string name;
            public uint AppId;
            public string savePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Context
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStruct)]
            public Callback[] Callbacks;
            public Config Config;
        }

        public static Context GlobalContext = new Context();
        public static IntPtr GlobalContextPTR = IntPtr.Zero;

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr UPC_ContextCreate(uint inVersion, IntPtr inOptSetting)
        {
            Basics.Log(nameof(UPC_ContextCreate), new object[] { inVersion, inOptSetting });
            GlobalContext = new Context();
            GlobalContext.Config = new();
            GlobalContext.Callbacks = new Callback[1];
            GlobalContextPTR = Marshal.AllocHGlobal(Marshal.SizeOf(GlobalContext));
            Marshal.StructureToPtr(GlobalContext, GlobalContextPTR, false);
            GlobalContext.Config.savePath = Path.Combine(Environment.CurrentDirectory, "SAVE");
            return GlobalContextPTR;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_ContextFree(IntPtr data)
        {
            Basics.Log(nameof(UPC_ContextFree), new object[] { data });
            Marshal.DestroyStructure<Context>(data);
            Marshal.FreeHGlobal(data);
            GlobalContext = new();
            GlobalContextPTR = IntPtr.Zero;
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static unsafe int UPC_Update(IntPtr data)
        {
            Basics.Log(nameof(UPC_Update), new object[] { data });

            var cblist = GlobalContext.Callbacks.ToList();
            foreach (var cb in cblist)
            {
                if (cb.fun != IntPtr.Zero)
                {
                    delegate* unmanaged<int, IntPtr, void> @delegate;
                    @delegate = (delegate* unmanaged<int, IntPtr, void>)cb.fun;
                    @delegate(cb.arg, cb.context_data);
                }
            }
            GlobalContext.Callbacks = new Callback[1];
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_Cancel(IntPtr inContext, IntPtr inHandler)
        {
            Basics.Log(nameof(UPC_Cancel), new object[] { inContext, inHandler });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_EventNextPeek(IntPtr inContext, IntPtr outEvent)
        {
            Basics.Log(nameof(UPC_EventNextPeek), new object[] { inContext, outEvent });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_EventNextPoll(IntPtr inContext, IntPtr outEvent)
        {
            Basics.Log(nameof(UPC_EventNextPoll), new object[] { inContext, outEvent });
            return -6;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_EventRegisterHandler(IntPtr inContext, uint inType, IntPtr inHandler, IntPtr inOptData)
        {
            Basics.Log(nameof(UPC_EventRegisterHandler), new object[] { inContext, inType, inHandler, inOptData });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_EventUnregisterHandler(IntPtr inContext, uint inType)
        {
            Basics.Log(nameof(UPC_EventUnregisterHandler), new object[] { inContext, inType });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_Init(uint inVersion, int appid)
        {
            GlobalContext.Config.AppId = (uint)appid;
            Basics.Log(nameof(UPC_Init), new object[] { inVersion, appid });
            Basics.SendReq(new Uplay.Uplaydll.Req()
            { 
                InitProcessReq = new()
                { 
                    ApiVersion = inVersion,
                    UplayEnvIsSet = false,
                    UplayId = (uint)appid,
                    ProcessId = (uint)Process.GetCurrentProcess().Id
                }
            }, out var rsp);
            Console.WriteLine(rsp);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void UPC_Uninit()
        {
            Basics.Log(nameof(UPC_Uninit), new object[] { });
        }
    }
}
