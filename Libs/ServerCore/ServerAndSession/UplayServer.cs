using ModdableWebServer.Servers;
using NetCoreServer;
using ServerCore.ServerAndSession;
using System.Collections.Concurrent;
using System.Net;

namespace ServerCore;

public class UplayServer(SslContext context, IPAddress address, int port) : WSS_Server(context, address, port)
{
    public static ConcurrentDictionary<Guid, UplaySession> UplaySessions = [];
    public override bool Start()
    {
        UplaySession.OnConnectedEvent += Session_OnConnected;
        UplaySession.OnDisconnectedEvent += Session_OnDisconnected;
        return base.Start();
    }


    public override bool Stop()
    {
        UplaySession.OnConnectedEvent -= Session_OnConnected;
        UplaySession.OnDisconnectedEvent -= Session_OnDisconnected;
        return base.Stop();
    }

    protected override SslSession CreateSession()
    {
        return new UplaySession(this);
    }


    private void Session_OnConnected(object? sender, Guid e)
    {
        UplaySessions.TryAdd(e, (UplaySession)sender!);
    }

    private void Session_OnDisconnected(object? sender, Guid e)
    {
        UplaySessions.Remove(e, out _);
    }
}
