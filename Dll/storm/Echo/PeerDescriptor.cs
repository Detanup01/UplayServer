using System.Net;

namespace Storm.Echo;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public class PeerDescriptor : IDisposable
{
    public string? StationUrlUserData;
    public bool ReadyForMatchMaking;
    public ushort DedicatedRouterId;
    public ushort DedicatedRouterSubId;
    public string? GatewayBehavior;
    public byte[]? GUID;
    public byte NatType;
    public byte PlatformType;
    public string? PunchGUID;
    public List<IPAddress> AdvertisableAddresses = [];

    public IntPtr ThisObjectHandle { get; protected set; }
    public IntPtr OutsideObjectHandle { get; set; }
    private GCHandle ThisGCHandle;

    public PeerDescriptor(IntPtr _handle)
    {
        OutsideObjectHandle = _handle;
        ThisGCHandle = GCHandle.Alloc(this, GCHandleType.Pinned);
        ThisObjectHandle = GCHandle.ToIntPtr(ThisGCHandle);
    }

    public void Dispose()
    {
        ThisGCHandle.Free();
        GC.SuppressFinalize(this);
    }

    public static List<PeerDescriptor> Descriptors = [];

    public static CreatePeerDescriptor? CreatePeer;
    public static ReleasePeerDescriptor? ReleasePeer;
}

[UnmanagedFunctionPointer(CallingConvention.StdCall)]
public delegate IntPtr CreatePeerDescriptor(IntPtr peerDescriptor);

[UnmanagedFunctionPointer(CallingConvention.StdCall)]
public delegate void ReleasePeerDescriptor(IntPtr handle);
