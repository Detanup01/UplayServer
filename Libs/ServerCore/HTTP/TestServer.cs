using ModdableWebServer.Servers;
using NetCoreServer;
using System.Net;
using System.Net.Security;

namespace ServerCore.HTTP;

internal class TestServer(SslContext context, IPAddress address, int port) : WSS_Server(context, address, port)
{
    public override void OnHandshaking(SslSession session)
    {
        Console.WriteLine("OnHandshaking");
    }

    public override void OnHandshaked(SslSession session)
    {
        Console.WriteLine("OnHandshaked");

    }
}
