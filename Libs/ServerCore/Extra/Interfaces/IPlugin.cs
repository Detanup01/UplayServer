using NetCoreServer;
using SharedLib.Server.Json;
using System.Net.Security;

namespace Core.Extra.Interfaces
{
    public interface IPlugin : IDisposable
    {
        uint Priority { get; }
        string Name { get; }
        Plugin PluginExtra { get; }
        void Initialize();
        void DemuxParseInitFinish(DemuxResponders.DemuxServer demux);
        void DemuxDataReceived(int ClientNumb, SslStream sslStream, byte[] receivedData);
        void DemuxDataReceivedCustom(int ClientNumb, byte[] receivedData, string Protoname);
        void HttpRequest(HttpRequest request, HttpsSession session);
        void ShutDown();
    }
}
