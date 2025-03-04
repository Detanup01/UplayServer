using ServerCore.Extra.Interfaces;
using System.Composition;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Plugin;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[Export(typeof(IPlugin))]
public class Plugin : IPlugin, IDisposable
{
    public uint Priority => uint.MaxValue;

    public string Name => "Plugin Example";

    public Plugin()
    {
        Console.WriteLine("Welcome from " + Name + " !");
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public void Initialize()
    {

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