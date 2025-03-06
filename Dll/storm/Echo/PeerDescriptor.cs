using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Echo;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct PeerDescriptor
{
    public IntPtr StationUrlUserData; // string
    public bool ReadyForMatchMaking;
    public ushort DedicatedRouterId;
    public ushort DedicatedRouterSubId;
    public IntPtr GatewayBehavior; // string
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
    public byte[] GUID;
    public byte NatType;
    public byte PlatformType;
    public IntPtr PunchGUID; // string
    public int AdvertisableAddressesCount;
    public IntPtr AdvertisableAddresses;
}
