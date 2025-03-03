using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace dbdata;

public class dbdataExport
{
    static uint AppId;
    static string Token = string.Empty;
    static IntPtr IGameTokenInterfaceVTable_ptr;
    static IntPtr IGameTokenInterface_ptr;
    public delegate uint IsTokenLoadedDelegate(IntPtr ptr);
    public delegate int Return0Delegate();
    public delegate bool GetCachedOrFreshTokenDelegate(IntPtr IGameTokenInterface, IntPtr tokenBuffer, int unUssd);
    public delegate void InvalidateCacheTokenDelegate(IntPtr IGameTokenInterface);
    public delegate IntPtr RegularDelegate(IntPtr IGameTokenInterface, IntPtr a2);

    [UnmanagedCallersOnly(EntryPoint = "?getGameTokenInterface@@YAPEAVIGameTokenInterface@@PEAX_K@Z", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr getGameTokenInterface(IntPtr appid_ptr, int always8)
    {
        AppId = (uint)Marshal.ReadInt32(appid_ptr);
        Log(nameof(getGameTokenInterface), ["Appid ",AppId]);
        IGameTokenInterfaceVTable vTable = new()
        {
            IsTokenLoaded = Marshal.GetFunctionPointerForDelegate(new IsTokenLoadedDelegate(IsTokenLoaded)),
            Return0 = Marshal.GetFunctionPointerForDelegate(new Return0Delegate(Return0)),
            GetCachedOrFreshToken = Marshal.GetFunctionPointerForDelegate(new GetCachedOrFreshTokenDelegate(GetCachedOrFreshToken)),
            InvalidateCacheToken = Marshal.GetFunctionPointerForDelegate(new InvalidateCacheTokenDelegate(InvalidateCacheToken)),
            GetBuffer = Marshal.GetFunctionPointerForDelegate(new RegularDelegate(GetBuffer)),
            GetThread = Marshal.GetFunctionPointerForDelegate(new RegularDelegate(GetThread)),
            NewThreadGetBurnTicketResponse = Marshal.GetFunctionPointerForDelegate(new RegularDelegate(NewThreadGetBurnTicketResponse)),
        }; 
        IGameTokenInterfaceVTable_ptr = Marshal.AllocHGlobal(Marshal.SizeOf<IGameTokenInterfaceVTable>());
        Marshal.StructureToPtr(vTable, IGameTokenInterfaceVTable_ptr, false);
        IGameTokenInterface_ptr = Marshal.AllocHGlobal(Marshal.SizeOf<IGameTokenInterface>());
        IGameTokenInterface gameTokenInterface = new()
        { 
            IGameTokenInterfaceVTable = IGameTokenInterfaceVTable_ptr
        };
        Marshal.StructureToPtr(gameTokenInterface, IGameTokenInterface_ptr, false);
        var token_path = Path.Combine(GetCuPath(), "token.txt");
        if (File.Exists(token_path))
            Token = File.ReadAllText(token_path);
        else
            Log(nameof(getGameTokenInterface), ["You dont have a token! Wait for the GetCachedOrFreshToken and use that token to generate a denuvo ticket!"]);
        return IGameTokenInterface_ptr;
    }
    public static string GetCuPath()
    {
        return AppContext.BaseDirectory;
    }

    public static void Log(string actionName, object[] parameters)
    {
        File.AppendAllText(Path.Combine(GetCuPath(), "dbdata.log"), $"{Environment.ProcessId} | {actionName} {string.Join(", ", parameters)}\n");
    }

    public static uint IsTokenLoaded(IntPtr IGameTokenInterface)
    {
        Log(nameof(IsTokenLoaded), [IGameTokenInterface]);
        return Convert.ToUInt32(!string.IsNullOrEmpty(Token));
    }

    public static int Return0()
    {
        Log(nameof(Return0), []);
        return 0;
    }

    public static bool GetCachedOrFreshToken(IntPtr IGameTokenInterface, IntPtr tokenBuffer, int len)
    {
        var b64Request = Marshal.PtrToStringAnsi(tokenBuffer);
        Log(nameof(GetCachedOrFreshToken), [b64Request == null ? "" : b64Request]);
        if (b64Request != null)
        {
            File.WriteAllText(Path.Combine(GetCuPath(), "token_req.txt"), b64Request);
            File.WriteAllText(Path.Combine(GetCuPath(), $"token_req_{AppId}.txt"), b64Request);

            // Should we return false if we dont have the token?
        }  
        return true;
    }

    public static void InvalidateCacheToken(IntPtr IGameTokenInterface)
    {
        Log(nameof(InvalidateCacheToken), ["Denuvo tried to delete this token!"]);
    }

    public static IntPtr GetBuffer(IntPtr IGameTokenInterface, IntPtr out_length)
    {
        //Log(nameof(GetBuffer), [IGameTokenInterface, out_length]);
        Marshal.WriteInt32(out_length, Token.Length);
        return Marshal.StringToHGlobalAnsi(Token);
    }

    public static IntPtr NewThreadGetBurnTicketResponse(IntPtr IGameTokenInterface, IntPtr a2)
    {
        Log(nameof(NewThreadGetBurnTicketResponse), [IGameTokenInterface, a2]);
        return 0;
    }

    public static IntPtr GetThread(IntPtr IGameTokenInterface, IntPtr a2)
    {
        Log(nameof(GetThread), [IGameTokenInterface, a2]);
        return 0;
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 32)]
public struct IGameTokenInterfaceVTable
{
    public IntPtr IsTokenLoaded;
    public IntPtr Return0;
    public IntPtr GetCachedOrFreshToken;
    public IntPtr InvalidateCacheToken;
    public IntPtr GetBuffer;
    public IntPtr NewThreadGetBurnTicketResponse;
    public IntPtr GetThread;
}

[StructLayout(LayoutKind.Sequential)]
public struct IGameTokenInterface
{
    public IntPtr IGameTokenInterfaceVTable;
}