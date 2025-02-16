using NetCoreServer;
using System.Net.Sockets;

namespace TD1_Server;

public class ProxyCoreServer : SslServer
{
    public ProxyCoreServer(SslContext context, string address) : base(context, address, 51000)
    {
    }

    protected override SslSession CreateSession()
    {
        return new ProxyCoreServerSession(this);
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"[TD1_Server.ProxyCoreServer] OnError {Id}. {error}!");
    }
}

public class ProxyCoreServerSession : SslSession
{
    public ProxyCoreServerSession(SslServer server) : base(server)
    {

    }

    protected override void OnConnected()
    {
        Console.WriteLine($"[TD1_Server.ProxyCoreServerSession] {Id} Connected!");
    }

    protected override void OnDisconnected()
    {
        Console.WriteLine($"[TD1_Server.ProxyCoreServerSession] {Id} Disconnected!");
    }

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        byte[] recvBytes = buffer.Skip((int)offset).Take((int)size).ToArray();
        Console.WriteLine($"[TD1_Server.ProxyCoreServerSession] Detect Received packet from {Id}. Packet saved!");
        if (!Directory.Exists("ProxyCoreServerSession"))
            Directory.CreateDirectory("ProxyCoreServerSession");
        File.WriteAllBytes($"ProxyCoreServerSession/{Id.ToString()!.Replace("-", "_")}_{DateTime.Now:yyy-MM-dd_HH-mm-ss}", recvBytes);
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"[TD1_Server.ProxyCoreServerSession] OnError {Id}. {error}!");
    }
}