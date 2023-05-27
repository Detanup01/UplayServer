using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static upc_r2.Enums;
using static upc_r2.Structures;

namespace upc_r2
{
    public class Main
    {
        public static Stopwatch Stopwatch = new Stopwatch();
        public static Context GlobalContext = new Context();
        public static IntPtr GlobalContextPTR = IntPtr.Zero;
        public static long TimeBettweenUpdate = 2000000;

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr UPC_ContextCreate(uint inVersion, IntPtr inOptSetting)
        {
            Basics.Log(nameof(UPC_ContextCreate), new object[] { inVersion, inOptSetting });

            var contextSettings =  Basics.IntPtrToStruct<UPC_ContextSettings>(inOptSetting);
            Basics.Log(nameof(UPC_ContextCreate), new object[] { contextSettings.subsystems });
            Basics.SendReq(new Uplay.Uplaydll.Req()
            {
                InitReq = new()
                {
                    ApiVersion = inVersion,
                    UplayId = GlobalContext.Config.ProductId,
                    ProcessId = (uint)Process.GetCurrentProcess().Id,
                    SubSystemFriend = contextSettings.subsystems.HasFlag(UPC_ContextSubsystem.UPC_ContextSubsystem_Friend),
                    SubSystemInstall = contextSettings.subsystems.HasFlag(UPC_ContextSubsystem.UPC_ContextSubsystem_Install),
                    SubSystemMultiplayer = contextSettings.subsystems.HasFlag(UPC_ContextSubsystem.UPC_ContextSubsystem_Multiplayer),
                    SubSystemOverlay = contextSettings.subsystems.HasFlag(UPC_ContextSubsystem.UPC_ContextSubsystem_Overlay),
                    SubSystemProduct = contextSettings.subsystems.HasFlag(UPC_ContextSubsystem.UPC_ContextSubsystem_Product),
                    SubSystemStorage = contextSettings.subsystems.HasFlag(UPC_ContextSubsystem.UPC_ContextSubsystem_Storage),
                    SubSystemStore = contextSettings.subsystems.HasFlag(UPC_ContextSubsystem.UPC_ContextSubsystem_Store),
                    SubSystemStreaming = contextSettings.subsystems.HasFlag(UPC_ContextSubsystem.UPC_ContextSubsystem_Streaming)
                }
            }, out var rsp);
            Basics.Log(nameof(UPC_ContextCreate), new object[] { rsp });
            try
            {
                GlobalContextPTR = Marshal.AllocHGlobal(Marshal.SizeOf(GlobalContext));
                Marshal.StructureToPtr(GlobalContext, GlobalContextPTR, false);
                return GlobalContextPTR;
            }
            catch (Exception ex)
            {
                Basics.Log(nameof(UPC_ContextCreate), new object[] { ex });
                return IntPtr.MinValue;
            }

        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_ContextFree(IntPtr inContext)
        {
            Basics.Log(nameof(UPC_ContextFree), new object[] { inContext });
            Marshal.DestroyStructure<Context>(inContext);
            Marshal.FreeHGlobal(inContext);
            GlobalContext = new();
            GlobalContextPTR = IntPtr.Zero;
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static unsafe int UPC_Update(IntPtr inContext)
        {
            Stopwatch.Stop();
            if (Stopwatch.ElapsedTicks <= TimeBettweenUpdate)
            {
                Stopwatch.Start();
                return 0;
            }
            Basics.Log(nameof(UPC_Update), new object[] { Stopwatch.ElapsedTicks });
            Stopwatch.Restart();
            //Basics.Log(nameof(UPC_Update), new object[] { inContext });
            Basics.SendReq(new Uplay.Uplaydll.Req(), out var rsp);
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
            GlobalContext.Callbacks = new Callback[0];
            var events = GlobalContext.Events.ToList();
            foreach (var ev in events)
            {
                if (ev.Handler != IntPtr.Zero)
                {
                    delegate* unmanaged<IntPtr, IntPtr, void> @delegate;
                    @delegate = (delegate* unmanaged<IntPtr, IntPtr, void>)ev.Handler;
                    if (rsp.OverlayRsp != null && rsp.OverlayRsp.OverlayStateChangedPush != null)
                    {
                        if (rsp.OverlayRsp.OverlayStateChangedPush.State == Uplay.Uplaydll.OverlayState.Showing && ev.EventType == UPC_EventType.UPC_Event_OverlayShown)
                        {
                            var ptr = Marshal.AllocHGlobal(sizeof(IntPtr));
                            Marshal.StructureToPtr(UPC_EventType.UPC_Event_OverlayShown,ptr,false);
                            @delegate(ptr, ev.OptData);
                        }
                        if (rsp.OverlayRsp.OverlayStateChangedPush.State == Uplay.Uplaydll.OverlayState.Hidden && ev.EventType == UPC_EventType.UPC_Event_OverlayHidden)
                        {
                            var ptr = Marshal.AllocHGlobal(sizeof(IntPtr));
                            Marshal.StructureToPtr(UPC_EventType.UPC_Event_OverlayHidden, ptr, false);
                            @delegate(ptr, ev.OptData);
                        }
                    }
                }               
            }
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
            //Basics.Log(nameof(UPC_EventNextPeek), new object[] { inContext, outEvent });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_EventNextPoll(IntPtr inContext, IntPtr outEvent)
        {
            //Basics.Log(nameof(UPC_EventNextPoll), new object[] { inContext, outEvent });
            return -6;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_EventRegisterHandler(IntPtr inContext, uint inType, IntPtr inHandler, IntPtr inOptData)
        {
            Basics.Log(nameof(UPC_EventRegisterHandler), new object[] { inContext, inType, inHandler, inOptData });
            var eventType = (UPC_EventType)inType;
            GlobalContext.Events.Append(new Event(eventType, inHandler, inOptData));
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_EventUnregisterHandler(IntPtr inContext, uint inType)
        {
            Basics.Log(nameof(UPC_EventUnregisterHandler), new object[] { inContext, inType });
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_Init(uint inVersion, int productId)
        {
            GlobalContext = new Context();
            GlobalContext.Config = new();
            GlobalContext.Config.Saved = new();
            GlobalContext.Callbacks = new Callback[1];
            GlobalContext.Events = new Event[1];
            GlobalContext.Config.ProductId = (uint)productId;
            GlobalContext.Config.Saved.savePath = Path.Combine(Environment.CurrentDirectory, "SAVE_GAMES", GlobalContext.Config.ProductId.ToString());
            Basics.Log(nameof(UPC_Init), new object[] { inVersion, productId });
            Basics.SendReq(new Uplay.Uplaydll.Req()
            { 
                InitProcessReq = new()
                { 
                    ApiVersion = inVersion,
                    UplayEnvIsSet = false,
                    UplayId = (uint)productId,
                    ProcessId = (uint)Process.GetCurrentProcess().Id
                }
            }, out var rsp);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void UPC_Uninit()
        {
            Basics.Log(nameof(UPC_Uninit), new object[] { });

            //  We dont need to send any req or wait for response, this just to make sure we dont update our stuff!
            Basics.SendReq(new Uplay.Uplaydll.Req()
            {
                InitProcessReq = new()
                {
                    ApiVersion = uint.MaxValue,
                    ProcessId = (uint)Process.GetCurrentProcess().Id
                }
            }, out var rsp);
        }
    }
}
