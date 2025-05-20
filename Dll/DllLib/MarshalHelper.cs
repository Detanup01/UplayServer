using System.Runtime.InteropServices;

namespace DllLib;

public static class MarshalHelper
{
    public static IntPtr GetListPtr<T>(List<T> values) where T : struct
    {
        IntPtr main_ptr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>() * values.Count);
        int indx = 0;
        foreach (var item in values)
        {
            IntPtr iptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr(item, iptr, false);
            Marshal.WriteIntPtr(main_ptr, indx * Marshal.SizeOf<IntPtr>(), iptr);
            indx++;
        }
        return main_ptr;
    }

    public static void FreeList(IntPtr listPointer)
    {
        BasicList upcList = Marshal.PtrToStructure<BasicList>(listPointer);
        FreeListPtr(upcList.count, upcList.list);
        Marshal.FreeHGlobal(listPointer);
    }

    public static void FreeListPtr(int count, IntPtr listPointer)
    {
        for (int i = 0; i < count; i++)
        {
            var ptr = Marshal.ReadIntPtr(listPointer, i * Marshal.SizeOf<IntPtr>());
            Marshal.FreeHGlobal(ptr);
        }
        Marshal.FreeHGlobal(listPointer);
    }

    public static void WriteOutList(IntPtr outList, int Count, IntPtr ptrToList)
    {
        BasicList list = new(Count, ptrToList);
        IntPtr iptr = Marshal.AllocHGlobal(Marshal.SizeOf<BasicList>());
        Marshal.StructureToPtr(list, iptr, false);
        Marshal.WriteIntPtr(outList, 0, iptr);
    }

    public static void WriteOutList<T>(IntPtr outList, List<T> values) where T : struct
    {
        BasicList list = new(values.Count, GetListPtr(values));
        IntPtr iptr = Marshal.AllocHGlobal(Marshal.SizeOf<BasicList>());
        Marshal.StructureToPtr(list, iptr, false);
        Marshal.WriteIntPtr(outList, 0, iptr);
    }

}