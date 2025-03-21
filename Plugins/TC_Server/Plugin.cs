using ServerCore.DMX;
using ServerCore.Extra.Interfaces;
using System.Composition;
using TC_Server;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Plugin;
#pragma warning restore IDE0130 // Namespace does not match folder structure

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
        GC.SuppressFinalize(this);
    }

    public void Initialize()
    {
        Servers?.Start();
        Console.WriteLine($"Initialize {Name} {Servers != null} !");
    }

    public bool DemuxDataReceived(DmxSession dmxSession, byte[] receivedData)
    {
        return false;
    }

    public bool DemuxDataReceivedCustom(DmxSession dmxSession, byte[] receivedData, string Protoname)
    {
        return false;
    }

    public void ShutDown()
    {
        Console.WriteLine("TCServer GoodBye!");
    }
}