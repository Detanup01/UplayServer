using ModdableWebServer.Helper;
using ServerCore.Extra.Interfaces;
using ServerCore.Models;
using System.Composition;
using TD1_Server;

namespace Plugin;

[Export(typeof(IPlugin))]
public class Plugin : IPlugin, IDisposable
{
    KeyStoreServer? KeyStore;
    ProxyCoreServer? ProxyCore;

    public uint Priority => uint.MaxValue;

    public string Name => "TD1_Server";

    public Plugin()
    {
        Console.WriteLine($"Welcome from {Name} !");
        var context = CertHelper.GetContextNoValidate(System.Security.Authentication.SslProtocols.Tls12, $"cert/services.pfx", ServerConfig.Instance.CERT.ServicesCertPassword);
        KeyStore = new(context, "0.0.0.0");
        ProxyCore = new(context, "0.0.0.0");
    }

    public void Dispose()
    {
        KeyStore?.Stop();
        KeyStore = null;
        ProxyCore?.Stop();
        ProxyCore = null;
    }

    public void Initialize()
    {
        KeyStore?.Start();
        ProxyCore?.Start();
    }

    public bool DemuxDataReceived(Guid ClientNumb, byte[] receivedData)
    {
        return false;
    }

    public bool DemuxDataReceivedCustom(Guid ClientNumb, byte[] receivedData, string Protoname)
    {
        return false;
    }

    public void ShutDown()
    {
        Console.WriteLine("Goodbye!");
    }
}