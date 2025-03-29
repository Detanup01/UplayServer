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

    public static UPC_PresenceImpl BuildFrom(UPC_Presence presence)
    {
        UPC_PresenceImpl impl = new()
        {
            onlineStatus = (uint)presence.onlineStatus,
            detailsUtf8 = Marshal.StringToHGlobalAnsi(presence.detailsUtf8),
            titleId = presence.titleId,
            titleNameUtf8 = Marshal.StringToHGlobalAnsi(presence.titleNameUtf8),
            multiplayerId = Marshal.StringToHGlobalAnsi(presence.multiplayerId),
            multiplayerJoinable = presence.multiplayerJoinable,
            multiplayerSize = presence.multiplayerSize,
            multiplayerMaxSize = presence.multiplayerMaxSize,
            multiplayerInternalDataSize = (uint)presence.multiplayerInternalData.Length
        };
        var ptr = Marshal.AllocHGlobal(sizeof(byte) * presence.multiplayerInternalData.Length);
        Marshal.Copy(presence.multiplayerInternalData, 0, ptr, presence.multiplayerInternalData.Length);
        impl.multiplayerInternalData = ptr;
        return impl;
    }

    public static void Free(UPC_PresenceImpl impl)
    {
        Marshal.FreeHGlobal(impl.detailsUtf8);
        Marshal.FreeHGlobal(impl.titleNameUtf8);
        Marshal.FreeHGlobal(impl.multiplayerId);
        Marshal.FreeHGlobal(impl.multiplayerInternalData);
    }
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

    public static UPC_UserImpl BuildFrom(UPC_User upc_User)
    {
        UPC_UserImpl impl = new()
        {
            idUtf8 = Marshal.StringToHGlobalAnsi(upc_User.idUtf8),
            nameUtf8 = Marshal.StringToHGlobalAnsi(upc_User.nameUtf8),
            relationship = (uint)upc_User.relationship
        };
        var presetimpl = UPC_PresenceImpl.BuildFrom(upc_User.presence);
        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(impl));
        Marshal.StructureToPtr(presetimpl, ptr, false);
        impl.presence = ptr;
        return impl;
    }

    public static void Free(UPC_UserImpl impl)
    {
        Marshal.FreeHGlobal(impl.idUtf8);
        Marshal.FreeHGlobal(impl.nameUtf8);
        var presence = Marshal.PtrToStructure<UPC_PresenceImpl>(impl.presence);
        UPC_PresenceImpl.Free(presence);
        Marshal.DestroyStructure<UPC_PresenceImpl>(impl.presence);
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

public class UPC_Presence
{
    public Uplay.Uplaydll.OnlineStatusV2 onlineStatus = Uplay.Uplaydll.OnlineStatusV2.OnlineStatusOnline;
    public string detailsUtf8 = string.Empty;
    public uint titleId;
    public string titleNameUtf8 = string.Empty;
    public string multiplayerId = string.Empty;
    public int multiplayerJoinable;
    public uint multiplayerSize;
    public uint multiplayerMaxSize;
    public byte[] multiplayerInternalData = [];
}

public class UPC_User
{
    public string idUtf8 = string.Empty;
    public string nameUtf8 = string.Empty;
    public Uplay.Uplaydll.Relationship relationship;
    public UPC_Presence presence = new();
}

[StructLayout(LayoutKind.Sequential)]
public struct ChunkId
{
    public uint Id;
    public int IsInstalled;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string Tag;

    public ChunkId()
    {
        IsInstalled = 1;
        Tag = string.Empty;
    }
}

[StructLayout(LayoutKind.Sequential)]
public class UPC_StoreProduct
{
    public uint id;
    public string titleUtf8;
    public string descriptionUtf8;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string imageUrlUtf8;
    public byte isOwned;
    public float price;
    public float priceOriginal;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string currencyUtf8;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string userBlobUtf8 = string.Empty;
    public UPC_StoreTag[] tags ;
}