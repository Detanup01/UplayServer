using System.Runtime.InteropServices;

namespace DllLib;

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