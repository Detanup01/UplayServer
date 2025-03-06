namespace Storm;

[UnmanagedFunctionPointer(CallingConvention.StdCall)]
public delegate void LocalPeerDescriptorUpdatedHandler(IntPtr peerDescriptorHandle);