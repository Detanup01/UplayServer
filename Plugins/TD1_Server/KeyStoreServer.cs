using NetCoreServer;
using System.Net.Sockets;

namespace TD1_Server;

public class KeyStoreServer : SslServer
{
    public KeyStoreServer(SslContext context, string address) : base(context, address, 27015)
    {
    }

    protected override SslSession CreateSession()
    {
        return new KeyStoreSession(this);
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"[TD1_Server.KeyStoreServer] OnError {Id}. {error}!");
    }
}

public class KeyStoreSession : SslSession
{
    public KeyStoreSession(SslServer server) : base(server)
    {

    }

    protected override void OnConnected()
    {
        Console.WriteLine($"[TD1_Server.KeyStoreSession] {Id} Connected!");
    }

    protected override void OnDisconnected()
    {
        Console.WriteLine($"[TD1_Server.KeyStoreSession] {Id} Disconnected!");
    }

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        byte[] recvBytes = buffer.Skip((int)offset).Take((int)size).ToArray();
        Console.WriteLine($"[TD1_Server.KeyStoreSession] Detect Received packet from {Id}. Packet saved!");
        if (!Directory.Exists("KeyStoreSession"))
            Directory.CreateDirectory("KeyStoreSession");
        File.WriteAllBytes($"KeyStoreSession/{Id.ToString()!.Replace("-", "_")}_{DateTime.Now:yyy-MM-dd_HH-mm-ss}", recvBytes);
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"[TD1_Server.KeyStoreSession] OnError {Id}. {error}!");
    }
}