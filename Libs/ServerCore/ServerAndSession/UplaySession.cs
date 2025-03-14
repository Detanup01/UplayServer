using ModdableWebServer.Servers;
using NetCoreServer;

namespace ServerCore.ServerAndSession;

public class UplaySession(UplayServer server) : WSS_Server.Session(server)
{
    public UplayServer UServer => server;
    public bool IsSSL { get; protected set; }
    public bool IsClosed { get; internal set; }

    public static event EventHandler<byte[]>? OnSSLReceived;
    public static event EventHandler<Guid>? OnConnectedEvent;
    public static event EventHandler<Guid>? OnDisconnectedEvent;

    public override void OnConnected()
    {
        OnConnectedEvent?.Invoke(this, Id);
        IsClosed = false;
    }

    public override void OnDisconnected()
    {
        OnDisconnectedEvent?.Invoke(this, Id);
        IsClosed = true;
    }

    public override void OnReceived(byte[] buffer, long offset, long size)
    {
        var buf = buffer.Take((int)size).Skip((int)offset).ToArray();
        var is_ascii = char.IsAsciiLetterUpper((char)buf[0]);
        if (is_ascii || WebSocket.WsHandshaked)
            base.OnReceived(buffer, offset, size);
        else
        {
            IsSSL = true;
            OnSSLReceived?.Invoke(this, buf);
        }
    }
}