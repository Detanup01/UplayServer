using Core.Extra.Interfaces;
using NetCoreServer;
using System.Composition;

namespace Plugin;

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

    public bool HttpRequest(HttpRequest request, HttpsSession session)
    {
        if (request.Url.Contains("ok"))
        {
            session.SendResponseAsync(session.Response.MakeGetResponse("Success!"));
            return true;
        }
        return false;
    }

    public void ShutDown()
    {
        Console.WriteLine("Goodbye!");
    }


}