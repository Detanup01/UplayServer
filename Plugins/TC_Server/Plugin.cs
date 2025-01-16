using ServerCore.Extra.Interfaces;
using System.Composition;
using TC_Server;

namespace Plugin;

[Export(typeof(IPlugin))]
public class Plugin : IPlugin, IDisposable
{
    TCServer? Servers;

    public uint Priority => uint.MaxValue;

    public string Name => "TCServer";

    public Plugin()
    {
        Console.WriteLine($"Welcome from {Name} !");
        Servers = new("0.0.0.0", 4000);
    }

    public void Dispose()
    {
        Servers?.Stop();
        Servers = null;
    }

    public void Initialize()
    {
        Servers?.Start();
        Console.WriteLine($"Initialize {Name} {Servers != null} !");
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
        Console.WriteLine("TCServer GoodBye!");
    }
}