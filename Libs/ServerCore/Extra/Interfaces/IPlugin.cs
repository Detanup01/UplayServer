using NetCoreServer;

namespace ServerCore.Extra.Interfaces
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
        /// Shutdowning the Plugin
        /// </summary>
        void ShutDown();
    }
}
