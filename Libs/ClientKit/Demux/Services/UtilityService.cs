using Google.Protobuf;
using SharedLib.Shared;
using Uplay.Utility;

namespace ClientKit.Demux.Services
{
    public class UtilityService
    {
        #region Base
        private Socket socket;
        public UtilityService(Socket demuxSocket)
        {
            socket = demuxSocket;
            Console.WriteLine("UtilityService is Ready");
        }
        #endregion
        #region Request
        public Rsp? SendRequest(Req req)
        {
            Upstream post = new() { Request = req };
            var ServiceRequest = new Uplay.Demux.Req
            {
                ServiceRequest = new()
                {
                    Service = "utility_service",
                    Data = ByteString.CopyFrom(post.ToByteArray())
                },
                RequestId = socket.RequestId
            };
            socket.RequestId++;

            var rsp = socket.SendReq(ServiceRequest);

            if (rsp == null || !rsp.ServiceRsp.Success)
            {
                return null;
            }
            return Formatters.FormatDataNoLength<Downstream>(rsp.ServiceRsp.Data.ToByteArray()).Response;
        }
        #endregion
        #region Function
        public GeoIpRsp GetGeoIp()
        {
            Req req = new()
            {
                GeoipReq = new() { }
            };
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                Debug.PrintDebug(rsp);
                return rsp.GeoipRsp;
            }
            else
            {
                return new() { ContinentCode = "", CountryCode = "" };
            }
        }
        #endregion
    }
}
