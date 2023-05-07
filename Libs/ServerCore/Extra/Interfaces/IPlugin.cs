using NetCoreServer;

namespace Core.Extra.Interfaces
{
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// Priority of the Plugin
        /// </summary>
        uint Priority { get; }

        /// <summary>
        /// Name of the plugin
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Initalize the Plugin
        /// </summary>
        void Initialize();

        /// <summary>
        /// Receiving Data from Demux connection
        /// </summary>
        /// <param name="ClientNumb">Client Number</param>
        /// <param name="receivedData">Recieved Data</param>
        /// <returns>If data sent to client</returns>
        bool DemuxDataReceived(Guid ClientNumb, byte[] receivedData);

        /// <summary>
        /// Receiving Custom Proto from Demux connection
        /// </summary>
        /// <param name="ClientNumb">Client Number</param>
        /// <param name="receivedData">Recieved Data</param>
        /// <param name="Protoname">Proto Name</param>
        /// <returns>If data sent to client</returns>
        bool DemuxDataReceivedCustom(Guid ClientNumb, byte[] receivedData, string Protoname);

        /// <summary>
        /// Receiving the HTTPS Request from Ubiservice
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="session">Session</param>
        /// <returns>If data sent to client</returns>
        bool HttpRequest(HttpRequest request, HttpsSession session);

        /// <summary>
        /// Shutdowning the Plugin
        /// </summary>
        void ShutDown();
    }
}
