﻿using DllLib;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

public static class Main
{
    public static uint ProductId = 0;

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_GetLastError", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_GetLastError(IntPtr aOutErrorString)
    {
        Log(nameof(UPLAY_GetLastError), [aOutErrorString]);
        Marshal.WriteIntPtr(aOutErrorString, Marshal.StringToHGlobalAnsi(string.Empty));
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_HasOverlappedOperationCompleted", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_HasOverlappedOperationCompleted(IntPtr aOverlapped)
    {
        //Log(nameof(UPLAY_HasOverlappedOperationCompleted), [aOverlapped]);
        var lapped = Marshal.PtrToStructure<UPLAY_Overlapped>(aOverlapped);
        //Log(nameof(UPLAY_HasOverlappedOperationCompleted), [lapped.Completed, lapped.Result]);
        return lapped.Completed;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_GetOverlappedOperationResult", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_GetOverlappedOperationResult(IntPtr aOverlapped, IntPtr aOutResult)
    {
        //Log(nameof(UPLAY_GetOverlappedOperationResult), [aOverlapped, aOutResult]);
        //var lapped = Marshal.PtrToStructure<UPLAY_Overlapped>(aOverlapped);
        //Log(nameof(UPLAY_HasOverlappedOperationCompleted), [lapped.Completed, lapped.Result]);
        Marshal.WriteInt32(aOutResult, (int)UPLAY_OverlappedResult.UPLAY_OverlappedResult_Ok);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PeekNextEvent", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PeekNextEvent()
    {
        //Log(nameof(UPLAY_PeekNextEvent), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_GetNextEvent", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_GetNextEvent(IntPtr aEvent)
    {
        //Log(nameof(UPLAY_GetNextEvent), [aEvent]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_Init", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_Init()
    {
        Log(nameof(UPLAY_Init));
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_Start", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_Start(uint aUplayId, uint aFlags)
    {
        Log(nameof(UPLAY_Start), [aUplayId, aFlags]);
        ProductId = aUplayId;
        LoadDll.PluginPath = "r1";
        LoadDll.LoadPlugins();
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_Startup", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_Startup(uint aUplayId, uint aGameVersion, IntPtr aLanguageCountryCodeUtf8)
    {
        Log(nameof(UPLAY_Startup), [aUplayId, aGameVersion, aLanguageCountryCodeUtf8]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_Update", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_Update()
    {
        //Log(nameof(UPLAY_Update), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_Quit", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_Quit()
    {
        Log(nameof(UPLAY_Quit));
        LoadDll.FreePlugins();
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SetLanguage", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SetLanguage(IntPtr aLanguageCountryCode)
    {
        Log(nameof(UPLAY_SetLanguage), [aLanguageCountryCode]);
        string? langCode = Marshal.PtrToStringUTF8(aLanguageCountryCode);
        Log(nameof(UPLAY_SetLanguage), [langCode == null]);
        if (!string.IsNullOrEmpty(langCode))
            UPC_Json.GetRoot().Account.Country = langCode;
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_SetGameSession", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_SetGameSession(IntPtr aGameSessionIdentifier, IntPtr aSessionData, uint aFlags)
    {
        Log(nameof(UPLAY_SetGameSession), [aGameSessionIdentifier, aSessionData, aFlags]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_ClearGameSession", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_ClearGameSession()
    {
        Log(nameof(UPLAY_ClearGameSession));
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PRESENCE_SetPresence", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PRESENCE_SetPresence(uint presenceId, IntPtr tokens)
    {
        Log(nameof(UPLAY_PRESENCE_SetPresence), [presenceId, tokens]);
        return true;
    }
}
