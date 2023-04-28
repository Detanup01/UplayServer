using NetCoreServer;
using SharedLib.Server.Json;

namespace Core.Extra.Interfaces
{
    public interface IPlugin : IDisposable
    {
        uint Priority { get; }
        string Name { get; }
        Plugin PluginExtra { get; }
        void Initialize();
        void DemuxDataReceived(Guid ClientNumb, byte[] receivedData);
        void DemuxDataReceivedCustom(Guid ClientNumb, byte[] receivedData, string Protoname);
        void HttpRequest(HttpRequest request, HttpsSession session);
        void ShutDown();
    }
}
