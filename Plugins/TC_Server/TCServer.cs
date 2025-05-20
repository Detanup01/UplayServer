using NetCoreServer;
using System.Net;
using System.Net.Sockets;

namespace TC_Server;

public class TCServer : UdpServer
{
    public TCServer(string address, int port) : base(address, port)
    {

    }

    protected override void OnStarted()
    {
        ReceiveAsync();
        Console.WriteLine("[TCServer] Started receiving");
    }

    protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
    {
        byte[] recvBytes = buffer.Skip((int)offset).Take((int)size).ToArray();
        Console.WriteLine($"[TCServer] Detect Received packet from {endpoint}. Packet saved!");
        if (!Directory.Exists("DetectPackets"))
            Directory.CreateDirectory("DetectPackets");
        File.WriteAllBytes($"DetectPackets/{endpoint.ToString()!.Replace(":", "_").Replace(".", "_")}_{DateTime.Now:yyy-MM-dd_HH-mm-ss}", recvBytes);
        ReceiveAsync();
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine("TestUDP: " + error);
    }
}
