using System.Runtime.CompilerServices;

namespace Storm.Echo;

public static class PeerDescriptor_Exports
{
    [UnmanagedCallersOnly(EntryPoint = "PeerDescriptor_Init", CallConvs = [typeof(CallConvCdecl)])]
    public static void PeerDescriptor_Init(IntPtr createTest, IntPtr releaseTest)
    {
        PeerDescriptor.Descriptors.Clear();
        PeerDescriptor.CreatePeer = Marshal.GetDelegateForFunctionPointer<CreatePeerDescriptor>(createTest);
        PeerDescriptor.ReleasePeer = Marshal.GetDelegateForFunctionPointer<ReleasePeerDescriptor>(releaseTest);
    }

    [UnmanagedCallersOnly(EntryPoint = "PeerDescriptor_Create", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr Create(IntPtr handle)
    {
        var native = new PeerDescriptor(handle);
        PeerDescriptor.Descriptors.Add(native);
        if (PeerDescriptor.CreatePeer == null)
            return native.ThisObjectHandle;
        native.OutsideObjectHandle = PeerDescriptor.CreatePeer(native.ThisObjectHandle);
        return native.ThisObjectHandle;
    }

    [UnmanagedCallersOnly(EntryPoint = "PeerDescriptor_Destroy", CallConvs = [typeof(CallConvCdecl)])]
    public static void Destroy(IntPtr handle)
    {
        if (!PeerDescriptor.Descriptors.Any(x => x.ThisObjectHandle == handle))
            return;
        var index = PeerDescriptor.Descriptors.FindIndex(x => x.ThisObjectHandle == handle);
        if (index == -1)
            return;
        var val = PeerDescriptor.Descriptors[index];
        PeerDescriptor.Descriptors.Remove(val);
        if (PeerDescriptor.ReleasePeer == null)
            return;
        PeerDescriptor.ReleasePeer(val.OutsideObjectHandle);
        val.Dispose();
    }
}
