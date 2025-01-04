using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

public class Main
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_GetLastError", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_GetLastError(IntPtr aOutErrorString)
    {
        Basics.Log(nameof(UPLAY_GetLastError), [aOutErrorString]);
        Marshal.WriteIntPtr(aOutErrorString, Marshal.StringToHGlobalAnsi(string.Empty));
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_HasOverlappedOperationCompleted", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_HasOverlappedOperationCompleted(IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_HasOverlappedOperationCompleted), [aOverlapped]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_GetOverlappedOperationResult", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_GetOverlappedOperationResult(IntPtr aOverlapped, IntPtr aOutResult)
    {
        Basics.Log(nameof(UPLAY_GetOverlappedOperationResult), [aOverlapped, aOutResult]);
        Marshal.WriteInt32(aOutResult, (int)UPLAY_OverlappedResult.UPLAY_OverlappedResult_Ok);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PeekNextEvent", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PeekNextEvent()
    {
        Basics.Log(nameof(UPLAY_PeekNextEvent), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_GetNextEvent", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_GetNextEvent(IntPtr aEvent)
    {
        Basics.Log(nameof(UPLAY_GetNextEvent), [aEvent]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_Init", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_Init()
    {
        Basics.Log(nameof(UPLAY_Init), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_Start", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_Start(uint aUplayId, uint aFlags)
    {
        Basics.Log(nameof(UPLAY_Start), [aUplayId, aFlags]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_Startup", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_Startup(uint aUplayId, uint aGameVersion, IntPtr aLanguageCountryCodeUtf8)
    {
        Basics.Log(nameof(UPLAY_Startup), [aUplayId, aGameVersion, aLanguageCountryCodeUtf8]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_Update", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_Update()
    {
        Basics.Log(nameof(UPLAY_Update), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_Quit", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_Quit()
    {
        Basics.Log(nameof(UPLAY_Quit), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SetLanguage", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SetLanguage(IntPtr aLanguageCountryCode)
    {
        Basics.Log(nameof(UPLAY_SetLanguage), [aLanguageCountryCode]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SetGameSession", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SetGameSession(IntPtr aGameSessionIdentifier, IntPtr aSessionData, uint aFlags)
    {
        Basics.Log(nameof(UPLAY_SetGameSession), [aGameSessionIdentifier, aSessionData, aFlags]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ClearGameSession", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ClearGameSession()
    {
        Basics.Log(nameof(UPLAY_ClearGameSession), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PRESENCE_SetPresence", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PRESENCE_SetPresence(uint presenceId, IntPtr tokens)
    {
        Basics.Log(nameof(UPLAY_PRESENCE_SetPresence), [presenceId, tokens]);
        return true;
    }
}
