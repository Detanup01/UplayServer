using Google.Protobuf;
using SharedLib.Shared;

namespace ClientKit.Demux.Services
{
    public class CustomService
    {
        #region Base
        public string Service;
        private Socket socket;
        public CustomService(string service, Socket demuxSocket)
        {
            Service = service;
            socket = demuxSocket;
            Console.WriteLine("CustomService is Ready");
        }
        #endregion
        #region Request        
        public V? SendRequest<T, V>(T post)
            where V : IMessage<V>, new()
            where T : IMessage<T>, new()
        {
            var ServiceRequest = new Uplay.Demux.Req
            {
                ServiceRequest = new()
                {
                    Service = Service,
                    Data = ByteString.CopyFrom(post.ToByteArray())
                },
                RequestId = socket.RequestId
            };
            socket.RequestId++;

            var rsp = socket.SendReq(ServiceRequest);

            if (rsp == null || !rsp.ServiceRsp.Success)
            {
                return default;
            }
            return Formatters.FormatDataNoLength<V>(rsp.ServiceRsp.Data.ToByteArray());
        }
        #endregion
    }
}
