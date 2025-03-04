using System.Runtime.InteropServices;

namespace upc_r2;

[StructLayout(LayoutKind.Sequential, Size = 4)]
public struct FakeContext
{
    public int FakeContextInt;
}


public class Context
{
    public List<Callback> Callbacks = [];
    public Config Config;
    public List<Event> Events = [];
}

[StructLayout(LayoutKind.Sequential)]
public struct Callback(IntPtr fn, IntPtr contextdata, int result)
{
    [MarshalAs(UnmanagedType.SysInt)]
    public IntPtr fun = fn;
    [MarshalAs(UnmanagedType.I4)]
    public int Result = result;
    [MarshalAs(UnmanagedType.SysInt)]
    public IntPtr context_data = contextdata;

    public override readonly string ToString()
    {
        return $"FunctionPtr: {fun} Result: {Result} Data: {context_data}";
    }
}

public struct Config
{
    public InitSaved Saved;
    public uint ProductId;
}

public struct InitSaved
{
    public Uplay.Uplaydll.Account account;
    public string savePath;
    public string ubiTicket;
    public string ApplicationId;
}

[StructLayout(LayoutKind.Sequential)]
public struct Event(UPC_EventType eventType, IntPtr handler, IntPtr optdata)
{
    public UPC_EventType EventType = eventType;
    public IntPtr Handler = handler;
    public IntPtr OptData = optdata;
}


public struct UPC_ContextSettings
{
    public UPC_ContextSubsystem subsystems;
}

[StructLayout(LayoutKind.Sequential)]
public struct BasicList
{
    [MarshalAs(UnmanagedType.I4)]
    public int count;
    [MarshalAs(UnmanagedType.SysInt)]
    public IntPtr list;

    public BasicList()
    { 

    }

    public BasicList(int _count, IntPtr _list)
    {
        count = _count;
        list = _list;
    }

    public override readonly string ToString()
    {
        return $"ListPtr: {list} Count: {count}";
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct UPC_Product(uint _id, uint _type)
{
    [MarshalAs(UnmanagedType.U4)]
    public uint id = _id;
    [MarshalAs(UnmanagedType.U4)]
    public Uplay.Uplaydll.ProductType type = (Uplay.Uplaydll.ProductType)_type;
    [MarshalAs(UnmanagedType.U4)]
    public Uplay.Uplaydll.ProductOwnership ownership = Uplay.Uplaydll.ProductOwnership.Owned;
    [MarshalAs(UnmanagedType.U4)]
    public Uplay.Uplaydll.ProductState state = Uplay.Uplaydll.ProductState.Playable;
    [MarshalAs(UnmanagedType.U4)]
    public uint balance = 0;
    [MarshalAs(UnmanagedType.U4)]
    public Uplay.Uplaydll.ProductActivation activation = Uplay.Uplaydll.ProductActivation.Purchased;

    public override readonly string ToString()
    {
        return $"ProductId: {id} ProductType: {type} ProductOwnership: {ownership} ProductState: {state} Balance: {balance} ProductActivation: {activation}";
    }
}

[StructLayout(LayoutKind.Sequential, Size = 72, Pack = 8)]
public struct UPC_PresenceImpl
{
    public uint onlineStatus;
    public IntPtr detailsUtf8;
    public uint titleId;
    public IntPtr titleNameUtf8;
    public IntPtr multiplayerId;
    public int multiplayerJoinable;
    public uint multiplayerSize;
    public uint multiplayerMaxSize;
    public IntPtr multiplayerInternalData;
    public uint multiplayerInternalDataSize;
}

[StructLayout(LayoutKind.Sequential, Size = 32, Pack = 8)]
public struct UPC_UserImpl
{
    public IntPtr idUtf8;
    public IntPtr nameUtf8;
    public uint relationship;
    public IntPtr presence;

    public override readonly string ToString()
    {
        return $"id: {idUtf8}, name: {nameUtf8}, rel: {relationship}, presence: {presence}";
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct UPC_StorageFile
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string fileNameUtf8;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string legacyNameUtf8;
    [MarshalAs(UnmanagedType.U4)]
    public uint size;
    [MarshalAs(UnmanagedType.U8)]
    public ulong lastModifiedMs;
}

[StructLayout(LayoutKind.Sequential)]
public struct UPC_RichPresenceToken
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string idUtf8 = string.Empty;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string valueIdUtf8 = string.Empty;

    public UPC_RichPresenceToken()
    {

    }

    public override readonly string ToString()
    {
        return $"Id: {idUtf8} Value: {valueIdUtf8}";
    }
}