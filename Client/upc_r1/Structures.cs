using System.Runtime.InteropServices;

namespace upc_r1;

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
}

[StructLayout(LayoutKind.Sequential)]
public struct UPLAY_Overlapped
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public ulong[] Internal = new ulong[2];

    public bool Completed
    { 
        get
        {
            return Internal[0] == 1;
        }
        set
        {
            Internal[0] = Convert.ToUInt64(value);
        }
    }

    public UPLAY_OverlappedResult Result
    {
        get
        {
            return (UPLAY_OverlappedResult)Convert.ToInt32(Internal[1]);
        }
        set
        {
            Internal[1] = Convert.ToUInt64(value);
        }
    }

    public UPLAY_Overlapped()
    {

    }

}

[StructLayout(LayoutKind.Sequential)]
public struct UPLAY_DataBlob
{
    [MarshalAs(UnmanagedType.SysInt)]
    public IntPtr data;
    [MarshalAs(UnmanagedType.U4)]
    public uint numBytes;
}

[StructLayout(LayoutKind.Sequential)]
public struct UPLAY_Event
{
    [MarshalAs(UnmanagedType.I4)]
    public UPLAY_EventType type;
    [MarshalAs(UnmanagedType.SysInt)]
    public IntPtr @event;
}

[StructLayout(LayoutKind.Sequential)]
public struct UPLAY_SAVE_Game
{
    [MarshalAs(UnmanagedType.U4)]
    public uint id;
    [MarshalAs(UnmanagedType.LPStr)]
    public string nameUtf8;
    [MarshalAs(UnmanagedType.U4)]
    public uint size;
}

[StructLayout(LayoutKind.Sequential)]
public struct UPLAY_USER_CdKey
{
    [MarshalAs(UnmanagedType.U4)]
    public uint uplayId;
    [MarshalAs(UnmanagedType.LPStr)]
    public string keyUtf8;
}