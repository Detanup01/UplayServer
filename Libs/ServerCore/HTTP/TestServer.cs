using ModdableWebServer.Servers;
using NetCoreServer;
using System.Net;
using System.Net.Security;

namespace ServerCore.HTTP;

internal class TestServer(SslContext context, IPAddress address, int port) : WSS_Server(context, address, port)
{
    protected override void OnHandshaking(SslSession session)
    {
        Console.WriteLine("OnHandshaking");
    }

    protected override void OnHandshaked(SslSession session)
    {
        Console.WriteLine("OnHandshaked");

    }
}
