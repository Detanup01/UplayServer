using ServerCore.DMX;
using ServerCore.Extra.Interfaces;
using StormDetect;
using System.Composition;

namespace Plugin;

[Export(typeof(IPlugin))]
public class Plugin : IPlugin, IDisposable
{
    List<DetectServer> Servers = [];

    public uint Priority => uint.MaxValue;

    public string Name => "StormDetect";

    public Plugin()
    {
        Console.WriteLine($"Welcome from {Name} !");
        Servers.Add(new DetectServer("127.0.0.1", 11000, "Detect1"));
        Servers.Add(new DetectServer("127.0.0.1", 11001, "Detect2"));
        Servers.Add(new DetectServer("127.0.0.1", 11005, "traversal"));
    }

    public void Dispose()
    {
        foreach (var item in Servers)
        {
            item.Stop();
        }
        Servers.Clear();
    }

    public void Initialize()
    {
        foreach (var item in Servers)
        {
            item.Start();
        }
    }


    public void ShutDown()
    {
        Console.WriteLine("Goodbye!");
    }

    public bool DemuxDataReceived(DmxSession dmxSession, byte[] receivedData)
    {
        return false;
    }

    public bool DemuxDataReceivedCustom(DmxSession dmxSession, byte[] receivedData, string Protoname)
    {
        return false;
    }
}