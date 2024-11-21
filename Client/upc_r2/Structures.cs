using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

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
public struct Event
{
    public Event(UPC_EventType eventType, IntPtr handler, IntPtr optdata)
    {
        EventType = eventType;
        Handler = handler;
        OptData = optdata;
    }

    public UPC_EventType EventType;
    public IntPtr Handler;
    public IntPtr OptData;
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
}

[StructLayout(LayoutKind.Sequential)]
public struct UPC_Product
{
    [MarshalAs(UnmanagedType.U4)]
    public uint id;
    [MarshalAs(UnmanagedType.U4)]
    public Uplay.Uplaydll.ProductType type;
    [MarshalAs(UnmanagedType.U4)]
    public Uplay.Uplaydll.ProductOwnership ownership;
    [MarshalAs(UnmanagedType.U4)]
    public Uplay.Uplaydll.ProductState state;
    [MarshalAs(UnmanagedType.U4)]
    public uint balance;
    [MarshalAs(UnmanagedType.U4)]
    public Uplay.Uplaydll.ProductActivation activation;

    public UPC_Product(uint _id, uint _type)
    {
        id = _id;
        type = (Uplay.Uplaydll.ProductType)_type;
        balance = 0;
        ownership = Uplay.Uplaydll.ProductOwnership.Owned;
        state = Uplay.Uplaydll.ProductState.Playable;
        activation = Uplay.Uplaydll.ProductActivation.Purchased;
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

    public override string ToString()
    {
        return $"id: {idUtf8}, name: {nameUtf8}, rel: {relationship}, presence: {presence}";
    }
}