using System.Runtime.InteropServices;

namespace upc_r1;

public static class Basics
{
    public static void WriteOverlappedResult(IntPtr Overlapped, bool IsSuccess, UPLAY_OverlappedResult result)
    {
        var overlapped = Marshal.PtrToStructure<UPLAY_Overlapped>(Overlapped);
        overlapped.Completed = IsSuccess;
        overlapped.Result = result;
        Marshal.StructureToPtr(overlapped, Overlapped, false);
    }
}