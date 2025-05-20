using DllLib;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Uplay.Uplaydll;

namespace upc_r2;

public class Main
{
    internal static Stopwatch Stopwatch = new();
    internal static Context GlobalContext = new();
    internal static IntPtr FakeContextPTR = IntPtr.Zero;
    private readonly static Lock lockObject = new();

    [UnmanagedCallersOnly(EntryPoint = "UPC_ContextCreate", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr UPC_ContextCreate(uint inVersion, IntPtr inOptSetting)
    {
        Log(nameof(UPC_ContextCreate), [inVersion, inOptSetting]);

        if (FakeContextPTR != IntPtr.Zero)
            return FakeContextPTR;

        UPC_ContextSettings contextSettings = new()
        {
            subsystems = UPC_ContextSubsystem.UPC_ContextSubsystem_None
        };
        if (inOptSetting != IntPtr.Zero)
        {
            contextSettings = Marshal.PtrToStructure<UPC_ContextSettings>(inOptSetting);
        }
        Log(nameof(UPC_ContextCreate), ["Subsystems: ", contextSettings.subsystems]);
        Rsp Response = new()
        {
            InitRsp = new()
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
            }
        };
        /*
        if (UPC_Json.GetRoot().BasicLog.UseNamePipeClient)
        {
            Basics.SendReq(new Req()
            {
                InitReq = new()
                {
                    ApiVersion = inVersion,
                    UplayId = GlobalContext.Config.ProductId,
                    ProcessId = (uint)Environment.ProcessId,
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
            
        }
        */
        Log(nameof(UPC_ContextCreate), [Response]);
        var initRsp = Response.InitRsp;
        GlobalContext.Config.Saved.account = initRsp.Account;
        GlobalContext.Config.Saved.savePath = initRsp.Storage.SavegameStoragePath;
        if (initRsp.HasUpcTicket)
            GlobalContext.Config.Saved.ubiTicket = initRsp.UpcTicket;
        GlobalContext.Config.Saved.ApplicationId = initRsp.UbiServices.AppId;
        FakeContext fc = new();
        FakeContextPTR = Marshal.AllocHGlobal(Marshal.SizeOf(fc));
        Marshal.StructureToPtr(fc, FakeContextPTR, false);
        Log(nameof(UPC_ContextCreate), ["Context ", FakeContextPTR]);
        return FakeContextPTR;

    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_ContextFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_ContextFree(IntPtr inContext)
    {
        Log(nameof(UPC_ContextFree), ["Freeing context", inContext]);
        Marshal.DestroyStructure<FakeContext>(inContext);
        Marshal.FreeHGlobal(inContext);
        GlobalContext = new();
        FakeContextPTR = IntPtr.Zero;
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_Update", CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe int UPC_Update(IntPtr inContext)
    {
        //Internal waiting update for reason
        Stopwatch.Stop();
        if (Stopwatch.ElapsedTicks <= UPC_Json.GetRoot().BasicLog.WaitBetweebUpdate)
        {
            Stopwatch.Start();
            return 0;
        }
        Log(nameof(UPC_Update), [Stopwatch.ElapsedTicks, UPC_Json.GetRoot().BasicLog.WaitBetweebUpdate]);
        Stopwatch.Restart();
        Log(nameof(UPC_Update), [GlobalContext.Callbacks.Count]);
        foreach (var cb in GlobalContext.Callbacks)
        {
            if (cb.fun != IntPtr.Zero)
            {
                Log(nameof(UPC_Update), ["Callback run with: ", cb.fun, cb.Result, cb.context_data]);
                delegate* unmanaged<int, void*, void> @Callback = (delegate* unmanaged<int, void*, void>)cb.fun;
                Log(nameof(UPC_Update), [cb.fun, "Is Calling!"]);
                @Callback(cb.Result, (void*)cb.context_data);
            }
        }
        Log(nameof(UPC_Update), ["Cleared Callbacks"]);
        GlobalContext.Callbacks.Clear();
        /*
         * Rsp Response = new();
        foreach (var ev in GlobalContext.Events)
        {
            if (ev.Handler != IntPtr.Zero)
            {
                delegate* unmanaged<void*, void*, void> @delegate = (delegate* unmanaged<void*, void*, void>)ev.Handler;
                if (Response.OverlayRsp != null && Response.OverlayRsp.OverlayStateChangedPush != null)
                {
                    if (Response.OverlayRsp.OverlayStateChangedPush.State == OverlayState.Showing && ev.EventType == UPC_EventType.UPC_Event_OverlayShown)
                    {
                        var ptr = Marshal.AllocHGlobal(sizeof(IntPtr));
                        Marshal.StructureToPtr(UPC_EventType.UPC_Event_OverlayShown, ptr, false);
                        @delegate((void*)ptr, (void*)ev.OptData);
                    }
                    if (Response.OverlayRsp.OverlayStateChangedPush.State == OverlayState.Hidden && ev.EventType == UPC_EventType.UPC_Event_OverlayHidden)
                    {
                        var ptr = Marshal.AllocHGlobal(sizeof(IntPtr));
                        Marshal.StructureToPtr(UPC_EventType.UPC_Event_OverlayHidden, ptr, false);
                        @delegate((void*)ptr, (void*)ev.OptData);
                    }
                }
            }
        }
        GlobalContext.Events.Clear();
        */
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_Cancel", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_Cancel(IntPtr inContext, IntPtr inHandler)
    {
        Log(nameof(UPC_Cancel), [inContext, inHandler]);
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_EventNextPeek", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_EventNextPeek(IntPtr inContext, IntPtr outEvent)
    {
        //Log(nameof(UPC_EventNextPeek), new object[] { inContext, outEvent });
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_EventNextPoll", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_EventNextPoll(IntPtr inContext, IntPtr outEvent)
    {
        //Log(nameof(UPC_EventNextPoll), new object[] { inContext, outEvent });
        return (int)UPC_Result.UPC_Result_NotFound;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_EventRegisterHandler", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_EventRegisterHandler(IntPtr inContext, uint inType, IntPtr inHandler, IntPtr inOptData)
    {
        Log(nameof(UPC_EventRegisterHandler), [inContext, inType, inHandler, inOptData]);
        var eventType = (UPC_EventType)inType;
        Log(nameof(UPC_EventRegisterHandler), ["EventType: ", eventType]);
        GlobalContext.Events.Add(new Event(eventType, inHandler, inOptData));
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_EventUnregisterHandler", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_EventUnregisterHandler(IntPtr inContext, uint inType)
    {
        Log(nameof(UPC_EventUnregisterHandler), [inContext, inType]);
        var eventRemove = GlobalContext.Events.Where(x => x.EventType == (UPC_EventType)inType);
        if (eventRemove.Any())
        {
            GlobalContext.Events.Remove(eventRemove.First());
        }
        return (int)UPC_Result.UPC_Result_Ok;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_Init", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_Init(uint inVersion, int productId)
    {
        GlobalContext = new()
        {
            Config = new()
            {
                Saved = new(),
                ProductId = (uint)productId
            },
            Callbacks = [],
            Events = []
        };
        Log(nameof(UPC_Init), [inVersion, productId]);
        try
        {
            Rsp Response = new();
            Response = new()
            {
                InitProcessRsp = new()
                {
                    OverlayEnabled = false,
                    Devmode = true,
                    UplayPID = (uint)Environment.ProcessId,
                    OverlayInjectionMethod = OverlayInjectionMethod.None,
                    Result = InitResult.Success,
                    SdkMonitoringConfig = new()
                    {
                        SdkMonitoringEnabled = false
                    }
                }
            };
            LoadDll.LoadPlugins();
            Log(nameof(UPC_Init), [Response.InitProcessRsp.Result]);
            return Response.InitProcessRsp.Result switch
            {
                InitResult.Success => (int)UPC_InitResult.UPC_InitResult_Ok,
                InitResult.Failure => (int)UPC_InitResult.UPC_InitResult_Failed,
                InitResult.ReconnectRequired => (int)UPC_InitResult.UPC_InitResult_DesktopInteractionRequired,
                InitResult.RestartWithGameLauncherRequired => (int)UPC_InitResult.UPC_InitResult_ExitProcessRequired,
                _ => (int)UPC_InitResult.UPC_InitResult_Failed,
            };
        }
        catch (Exception ex)
        {
            Log(nameof(UPC_Init), [ex.ToString()]);
        }
        return 1;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_Uninit", CallConvs = [typeof(CallConvCdecl)])]
    public static void UPC_Uninit()
    {
        Log(nameof(UPC_Uninit));
        LoadDll.FreePlugins();
    }
}
