using NetCoreServer;
using System.Net;

namespace StormDetect;

public class DetectServer : UdpServer
{
    public readonly string DetectServerName;
    public DetectServer(string address, int port, string detectServerName) : base(address, port)
    {

        DetectServerName = detectServerName;
    }

    protected override void OnStarted()
    {
        ReceiveAsync();
        Console.WriteLine($"[{DetectServerName}] Started receiving");
    }
    protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
    {
        byte[] recvBytes = buffer.Skip((int)offset).Take((int)size).ToArray();
        Console.WriteLine($"[{DetectServerName}] Detect Received packet from {endpoint}. Packet saved!");
        if (!Directory.Exists("DetectPackets"))
            Directory.CreateDirectory("DetectPackets");
        File.WriteAllBytes($"DetectPackets/{endpoint.ToString()!.Replace(":", "_").Replace(".", "_")}_{DateTime.Now:yyy-MM-dd_HH-mm-ss}", recvBytes);
        ReceiveAsync();
    }
}
