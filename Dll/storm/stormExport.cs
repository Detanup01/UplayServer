using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Storm;

public class stormExport
{
    [UnmanagedCallersOnly(EntryPoint = "StormSdkInitialize", CallConvs = [typeof(CallConvCdecl)])]
    public static void StormSdkInitialize()
    {

    }

    [UnmanagedCallersOnly(EntryPoint = "StormSdkUninitialize", CallConvs = [typeof(CallConvCdecl)])]
    public static void StormSdkUninitialize()
    {

    }

    [UnmanagedCallersOnly(EntryPoint = "StormVersion", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr StormVersion()
    {
        return Marshal.StringToHGlobalAnsi("2.5.6");
    }

    [UnmanagedCallersOnly(EntryPoint = "StormInitialize", CallConvs = [typeof(CallConvCdecl)])]
    public static bool StormInitialize(IntPtr onLocalPeerDescriptorUpdated, IntPtr applicationId, IntPtr compatibilityId, IntPtr persistentDataPath, IntPtr argumentsKeys, IntPtr argumentsValues, int argumentsSize)
    {
        LocalPeerDescriptorUpdatedHandler v = Marshal.GetDelegateForFunctionPointer<LocalPeerDescriptorUpdatedHandler>(onLocalPeerDescriptorUpdated);
        return false;
    }
}
