using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Uplay.Uplaydll;

namespace upc_r2;

public class Main
{
    public delegate void UPC_CallbackImpl(int inResult, IntPtr inData);

    public static Stopwatch Stopwatch = new Stopwatch();
    public static Context GlobalContext = new Context();
    public static IntPtr FakeContextPTR = IntPtr.Zero;
    public static long TimeBetweenUpdate = 20000000;
    private static Lock lockObject = new Lock();

    [UnmanagedCallersOnly(EntryPoint = "UPC_ContextCreate", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPC_ContextCreate(uint inVersion, IntPtr inOptSetting)
    {
        Basics.Log(nameof(UPC_ContextCreate), [inVersion, inOptSetting]);

        if (FakeContextPTR != IntPtr.Zero)
            return FakeContextPTR;

        UPC_ContextSettings contextSettings = new()
        {
            subsystems = UPC_ContextSubsystem.UPC_ContextSubsystem_None
        };
        if (inOptSetting != IntPtr.Zero) 
        {
            contextSettings = Basics.IntPtrToStruct<UPC_ContextSettings>(inOptSetting);
        }
        Basics.Log(nameof(UPC_ContextCreate), ["Subsystems: ", contextSettings.subsystems]);
        Rsp Response = new();
        if (UPC_Json.GetRoot().BasicLog.UseNamePipeClient)
        {
            Basics.SendReq(new Req()
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
            }, out Response);
        }
        else
        {
            Response.InitRsp = new()
            { 
                Account = new()
                { 
                    AccountId = UPC_Json.GetRoot().Account.AccountId,
                    Country = UPC_Json.GetRoot().Account.Country,
                    Email = UPC_Json.GetRoot().Account.Email,
                    NameOnPlatform = UPC_Json.GetRoot().Account.Name,
                    Username = UPC_Json.GetRoot().Account.Name,
                    Password = UPC_Json.GetRoot().Account.Password
                },
                UpcTicket = UPC_Json.GetRoot().Account.Ticket,
                Storage = new()
                { 
                    SavegameStoragePath = UPC_Json.GetRoot().Save.Path
                },
                UbiServices = new()
                {
                    AppId = UPC_Json.GetRoot().Others.ApplicationId,
                }
            };
        }
        Basics.Log(nameof(UPC_ContextCreate), [Response]);
        var initRsp = Response.InitRsp;
        GlobalContext.Config.Saved.account = initRsp.Account;
        //GlobalContext.Config.Saved.savePath = initRsp.Storage.SavegameStoragePath;
        if (initRsp.HasUpcTicket)
            GlobalContext.Config.Saved.ubiTicket = initRsp.UpcTicket;
        GlobalContext.Config.Saved.ApplicationId = initRsp.UbiServices.AppId;
        FakeContext fc = new();
        FakeContextPTR = Marshal.AllocHGlobal(Marshal.SizeOf(fc));
        Marshal.StructureToPtr(fc, FakeContextPTR, false);
        return FakeContextPTR;

    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_ContextFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_ContextFree(IntPtr inContext)
    {
        Basics.Log(nameof(UPC_ContextFree), [inContext]);
        Marshal.DestroyStructure<FakeContext>(inContext);
        Marshal.FreeHGlobal(inContext);
        GlobalContext = new();
        FakeContextPTR = IntPtr.Zero;
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_Update", CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe int UPC_Update(IntPtr inContext)
    {
        lock (lockObject)
        {
            //Internal waiting update for reason
            Stopwatch.Stop();
            if (Stopwatch.ElapsedTicks <= TimeBetweenUpdate)
            {
                Stopwatch.Start();
                return 0;
            }
            //Basics.Log(nameof(UPC_Update), new object[] { Stopwatch.ElapsedTicks });
            Stopwatch.Restart();
            //Basics.Log(nameof(UPC_Update), new object[] { inContext });
            Rsp Response = new();
            if (UPC_Json.GetRoot().BasicLog.UseNamePipeClient)
                Basics.SendReq(new Req(), out Response);
            else
            {
                Response = new();
            }
            Basics.Log(nameof(UPC_Update), [GlobalContext.Callbacks.Count]);
            foreach (var cb in GlobalContext.Callbacks)
            {
                if (cb.fun != IntPtr.Zero)
                {
                    try
                    {
                        Basics.Log(nameof(UPC_Update), ["Delegate run with: ", cb.fun, cb.arg, cb.context_data]);
                        /*
                        delegate* unmanaged<int, IntPtr, void> @delegate;
                        Basics.Log(nameof(UPC_Update), [cb.fun, "is now @delegate!"]);
                        @delegate = (delegate* unmanaged<int, IntPtr, void>)cb.fun;
                        Basics.Log(nameof(UPC_Update), [cb.fun, "aaaaaaa!"]);
                        @delegate(cb.arg, cb.context_data);         // CRASHING HERE!   !!!!!!!!! [ SHOULD NOT CRASH BRO]
                        Basics.Log(nameof(UPC_Update), ["@delegate is delegated!"]);
                        */
                        /*
                         *  c# way, also crashes if not c# app?
                        var callback = Marshal.GetDelegateForFunctionPointer<UPC_CallbackImpl>(cb.fun);
                        Basics.Log(nameof(UPC_Update), [cb.fun, "is now callback!"]);
                        callback.Invoke(cb.arg, cb.context_data);
                        Basics.Log(nameof(UPC_Update), ["callback is called!"]);*/
                        
                    }
                    catch (Exception ex)
                    {

                        Basics.Log(nameof(UPC_Update), [ex.ToString()]);
                    }

                }

            }
            Basics.Log(nameof(UPC_Update), ["aaaaaaaaaaaaaaaaaaaaaaa"]);
            GlobalContext.Callbacks.Clear();
            /*
            foreach (var ev in GlobalContext.Events)
            {
                if (ev.Handler != IntPtr.Zero)
                {
                    delegate* unmanaged<IntPtr, IntPtr, void> @delegate;
                    @delegate = (delegate* unmanaged<IntPtr, IntPtr, void>)ev.Handler;
                    if (Response.OverlayRsp != null && Response.OverlayRsp.OverlayStateChangedPush != null)
                    {
                        if (Response.OverlayRsp.OverlayStateChangedPush.State == OverlayState.Showing && ev.EventType == UPC_EventType.UPC_Event_OverlayShown)
                        {
                            var ptr = Marshal.AllocHGlobal(sizeof(IntPtr));
                            Marshal.StructureToPtr(UPC_EventType.UPC_Event_OverlayShown, ptr, false);
                            @delegate(ptr, ev.OptData);
                        }
                        if (Response.OverlayRsp.OverlayStateChangedPush.State == OverlayState.Hidden && ev.EventType == UPC_EventType.UPC_Event_OverlayHidden)
                        {
                            var ptr = Marshal.AllocHGlobal(sizeof(IntPtr));
                            Marshal.StructureToPtr(UPC_EventType.UPC_Event_OverlayHidden, ptr, false);
                            @delegate(ptr, ev.OptData);
                        }
                    }
                }
            }*/
        }       
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_Cancel", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_Cancel(IntPtr inContext, IntPtr inHandler)
    {
        Basics.Log(nameof(UPC_Cancel), [inContext, inHandler]);
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_EventNextPeek", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_EventNextPeek(IntPtr inContext, IntPtr outEvent)
    {
        //Basics.Log(nameof(UPC_EventNextPeek), new object[] { inContext, outEvent });
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_EventNextPoll", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_EventNextPoll(IntPtr inContext, IntPtr outEvent)
    {
        //Basics.Log(nameof(UPC_EventNextPoll), new object[] { inContext, outEvent });
        return (int)UPC_Result.UPC_Result_NotFound;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_EventRegisterHandler", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_EventRegisterHandler(IntPtr inContext, uint inType, IntPtr inHandler, IntPtr inOptData)
    {
        Basics.Log(nameof(UPC_EventRegisterHandler), [inContext, inType, inHandler, inOptData]);
        var eventType = (UPC_EventType)inType;
        Basics.Log(nameof(UPC_EventRegisterHandler), ["EventType: ", eventType]);
        GlobalContext.Events.Append(new Event(eventType, inHandler, inOptData));
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_EventUnregisterHandler", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_EventUnregisterHandler(IntPtr inContext, uint inType)
    {
        Basics.Log(nameof(UPC_EventUnregisterHandler), [inContext, inType]);
        var eventRemove = GlobalContext.Events.Where(x=>x.EventType == (UPC_EventType)inType);
        if (eventRemove.Any())
        {
            GlobalContext.Events.Remove(eventRemove.First());
        }
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_Init", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_Init(uint inVersion, int productId)
    {
        GlobalContext = new Context();
        GlobalContext.Config = new();
        GlobalContext.Config.Saved = new();
        GlobalContext.Callbacks = new();
        GlobalContext.Events = new();
        GlobalContext.Config.ProductId = (uint)productId;
        Basics.Log(nameof(UPC_Init), [inVersion, productId]);
        try
        {
            Rsp Response = new();
            if (UPC_Json.GetRoot().BasicLog.UseNamePipeClient)
                Basics.SendReq(new Req()
                {
                    InitProcessReq = new()
                    {
                        ApiVersion = inVersion,
                        UplayEnvIsSet = false,
                        UplayId = (uint)productId,
                        ProcessId = (uint)Process.GetCurrentProcess().Id
                    }
                }, out Response);
            else
            {
                Response = new()
                {
                    InitProcessRsp = new()
                    {
                        OverlayEnabled = false,
                        Devmode = true,
                        UplayPID = (uint)Process.GetCurrentProcess().Id,
                        OverlayInjectionMethod = OverlayInjectionMethod.None,
                        Result = InitResult.Success,
                        SdkMonitoringConfig = new()
                        {
                            SdkMonitoringEnabled = false
                        }
                    }
                };
            }
            Basics.Log(nameof(UPC_Init), [Response.InitProcessRsp.Result]);
            switch (Response.InitProcessRsp.Result)
            {
                case InitResult.Success:
                    return (int)UPC_InitResult.UPC_InitResult_Ok;
                case InitResult.Failure:
                    return (int)UPC_InitResult.UPC_InitResult_Failed;
                case InitResult.ReconnectRequired:
                    return (int)UPC_InitResult.UPC_InitResult_DesktopInteractionRequired;
                case InitResult.RestartWithGameLauncherRequired:
                    return (int)UPC_InitResult.UPC_InitResult_ExitProcessRequired;
                default:
                    return (int)UPC_InitResult.UPC_InitResult_Failed;
            }
        }
        catch (Exception ex)
        {
            Basics.Log(nameof(UPC_Init), [ex.ToString()]);
        }
        return 1;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_Uninit", CallConvs = [typeof(CallConvCdecl)])]
    public static void UPC_Uninit()
    {
        Basics.Log(nameof(UPC_Uninit));

        //  We dont need to send any req or wait for response, this just to make sure we dont update our stuff!
        if (UPC_Json.GetRoot().BasicLog.UseNamePipeClient)
            Basics.SendReq(new Req()
            {
                InitProcessReq = new()
                {
                    ApiVersion = uint.MaxValue,
                    ProcessId = (uint)Process.GetCurrentProcess().Id
                }
            }, out _);
    }
}
