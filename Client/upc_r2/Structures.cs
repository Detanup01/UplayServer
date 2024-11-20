using System.Runtime.InteropServices;

namespace upc_r2;

[StructLayout(LayoutKind.Sequential, Size = 4)]
public struct FakeContext
{
    public int FakeContextInt;
}


[StructLayout(LayoutKind.Sequential)]
public struct Context
{
    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStruct, SizeConst = 1)]
    public Callback[] Callbacks;
    public Config Config;
    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStruct, SizeConst = 1)]
    public Event[] Events;
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