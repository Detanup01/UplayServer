using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Storm;

internal static class MarshalHelper
{
    public static IntPtr GetListPtr<T>(this List<T> values) where T : struct
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

    public static void FreeListPtr(IntPtr listPointer, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var ptr = Marshal.ReadIntPtr(listPointer, i * Marshal.SizeOf<IntPtr>());
            Marshal.FreeHGlobal(ptr);
        }
        Marshal.FreeHGlobal(listPointer);
    }

    /// <summary>
    /// Making a String to IntPtr
    /// <br>Also Allocating for Null Bytes and encoding as UTF-8</br>
    /// </summary>
    /// <param name="str">The String</param>
    /// <returns>Allocated Pointer</returns>
    public static IntPtr FromString(this string str)
    {
        var bytes = Encoding.ASCII.GetBytes(str);
        var result_bytes = bytes.Concat(new byte[] { 0x00 }).ToArray();
        var PTR = Marshal.AllocHGlobal(bytes.Length + 1);
        Marshal.Copy(result_bytes, 0, PTR, result_bytes.Length);
        return PTR;
    }

    /// <summary>
    /// Making a String to IntPtr
    /// <br>Also Allocating for Null Bytes and encoding as UTF-8</br>
    /// </summary>
    /// <param name="str">The String</param>
    /// <returns>Allocated Pointer</returns>
    public static void StringWriteNoAlloc(this string str, IntPtr str_ptr, int ptr_len)
    {
        var bytes = Encoding.ASCII.GetBytes(str);
        if (bytes.Length + 1 > ptr_len)
            return;
        var result_bytes = bytes.Concat(new byte[] { 0x00 }).ToArray();
        Marshal.Copy(result_bytes, 0, str_ptr, result_bytes.Length);
    }

    /// <summary>
    /// Making the Allocated Pointer back to String
    /// </summary>
    /// <param name="source">The Allocated Pointer</param>
    /// <returns>The String if exist of Empty</returns>
    public static string ToASCIIString(this IntPtr source)
    {
        if (source == IntPtr.Zero)
        {
            return string.Empty;
        }

        // C style strlen
        int length = source.GetStringLength();

        // +1 byte for the null terminator.
        byte[] bytes = new byte[length + 1];
        Marshal.Copy(source, bytes, 0, length + 1);
        return Encoding.ASCII.GetString(bytes);
    }

    /// <summary>
    /// Getting String Length
    /// </summary>
    /// <param name="address">Address of the String</param>
    /// <returns>Length of the String</returns>
    public static int GetStringLength(this IntPtr address)
    {
        if (address == IntPtr.Zero)
            return 0;
        int length = 0;
        while (Marshal.ReadByte(address, length) != 0)
        {
            ++length;
        }

        return length;
    }

    /// <summary>
    /// Destroying the Strucutre and Freeing the Pointer
    /// </summary>
    /// <typeparam name="T">The Type</typeparam>
    /// <param name="itemAddress">The Allocated Address</param>
    public static void Destroy<T>(IntPtr itemAddress)
    {
        Marshal.DestroyStructure<T>(itemAddress);
        Marshal.FreeHGlobal(itemAddress);
    }

    /// <summary>
    /// Making a Structure to a Pointer
    /// </summary>
    /// <typeparam name="T">The Structure Type</typeparam>
    /// <param name="_struct">The Structure Itself</param>
    /// <returns>Allocated Pointer</returns>
    public static IntPtr StructToPtr<T>([DisallowNull] this T _struct)
    {
        var itemSize = Marshal.SizeOf<T>();
        return _struct.StructToPtr(itemSize);
    }

    /// <summary>
    /// Making a Structure to a Pointer with the Size
    /// </summary>
    /// <typeparam name="T">The Structure Type</typeparam>
    /// <param name="_struct">The Structure Itself</param>
    /// <param name="Size">The Structure Size</param>
    /// <returns>Allocated Pointer</returns>
    public static IntPtr StructToPtr<T>([DisallowNull] this T _struct, int Size)
    {
        IntPtr address = Marshal.AllocHGlobal(Size);
        Marshal.StructureToPtr(_struct, address, false);
        return address;
    }

    /// <summary>
    /// Writing the Structure to Output Pointer
    /// </summary>
    /// <typeparam name="T">The Structure Type</typeparam>
    /// <param name="_struct">The Structure Itself</param>
    /// <param name="outPtr">The Pointer to write to</param>
    public static void StructWriteOut<T>([DisallowNull] T _struct, IntPtr outPtr)
    {
        Marshal.WriteIntPtr(outPtr, _struct.StructToPtr());
    }

    /// <summary>
    /// Writing the String into the Output Pointer
    /// </summary>
    /// <param name="str">The String</param>
    /// <param name="outPtr">The Pointer to write to</param>
    public static void StringWriteOut(string str, IntPtr outPtr)
    {
        Marshal.WriteIntPtr(outPtr, str.FromString());
    }
}
