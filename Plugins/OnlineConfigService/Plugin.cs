using OnlineConfigService;
using ServerCore.Controllers;
using ServerCore.DMX;
using ServerCore.Extra.Interfaces;
using System.Composition;

namespace Plugin;

[Export(typeof(IPlugin))]
public class Plugin : IPlugin, IDisposable
{
    public uint Priority => uint.MaxValue;

    public string Name => "OnlineConfigService";

    public Plugin()
    {
        Console.WriteLine($"Welcome from {Name} !");
    }

    public void Dispose()
    {

    }

    public void Initialize()
    {
        ServerController.AddRoutes(typeof(OnlineService_SVC).Assembly);
    }


    public void ShutDown()
    {
        ServerController.RemoveRoutes(typeof(OnlineService_SVC).Assembly);
    }

    public bool DemuxDataReceivedCustom(DmxSession dmxSession, byte[] receivedData, string Protoname)
    {
        return false;
    }
}