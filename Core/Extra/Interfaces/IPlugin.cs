using NetCoreServer;
using System.Net.Security;

namespace Core.Extra.Interfaces
{
    public interface IPlugin : IDisposable
    {
        uint Priority { get; }
        string Name { get; }
        JSON.Plugin PluginExtra { get; }
        void Initialize();
        void DemuxParse(DemuxResponsers.DemuxServer demux);
        void DemuxParseInitFinish(DemuxResponsers.DemuxServer demux);
        void DemuxDataReceived(int ClientNumb, SslStream sslStream, byte[] receivedData);
        void DemuxDataReceivedCustom(int ClientNumb, byte[] receivedData, string Protoname);
        void HttpRequest(HttpRequest request, HttpsSession session);
        void ShutDown();
    }
}
