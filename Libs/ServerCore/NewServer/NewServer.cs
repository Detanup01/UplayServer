using ModdableWebServer.Servers;
using NetCoreServer;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore;

public class NewServer : WSS_Server
{
    public NewServer(SslContext context, IPAddress address, int port) : base(context, address, port)
    {
    }

    protected override SslSession CreateSession()
    {
        return new NewWssSession(this);
    }
}