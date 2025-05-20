using Storm.Enums;
using System.Runtime.CompilerServices;

namespace Storm;

internal class Error
{
    // ref int errorCode, IntPtr feedString, ref int feedStringLength, IntPtr debugFile, ref int debugFileLength, ref uint debugLine
    [UnmanagedCallersOnly(EntryPoint = "GetLastResult", CallConvs = [typeof(CallConvCdecl)])]
    public static byte GetLastResult(IntPtr errorCode, IntPtr feedString, IntPtr feedStringLength, IntPtr debugFile, IntPtr debugFileLength, IntPtr debugLine)
    {
        // 0 is failed to get error (or no error?)
        // is is success.
        int feedStringLen = Marshal.ReadInt32(feedStringLength);
        int debugFileLen = Marshal.ReadInt32(debugFileLength);
        Marshal.WriteInt32(errorCode, (int)ECode.OK);
        MarshalHelper.StringWriteNoAlloc("Nothing", feedString, feedStringLen);
        MarshalHelper.StringWriteNoAlloc("Nothing", debugFile, debugFileLen);
        return 1;
    }
}
