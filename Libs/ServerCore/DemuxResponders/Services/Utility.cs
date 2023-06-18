using Google.Protobuf;
using SharedLib.Server.Json;
using Uplay.Utility;

namespace Core.DemuxResponders
{
    public class Utility
    {
        public static readonly string Name = "utility_service";
        public class Up
        {
            public static Downstream Downstream = null;
            public static void UpstreamConverter(Guid ClientNumb, ByteString bytes)
            {
                var UpstreamBytes = bytes.ToArray();
                var Upsteam = Upstream.Parser.ParseFrom(UpstreamBytes);
                Utils.WriteFile(Upsteam.ToString(), $"logs/client_{ClientNumb}_utility_req.log");
                if (Upsteam != null)
                {
                    if (Upsteam.Request != null)
                    {
                        ReqRSP.Requests(Upsteam.Request);
                        while (ReqRSP.IsIdDone == false)
                        {

                        }
                        Utils.WriteFile(ReqRSP.Downstream.ToString(), $"logs/client_{ClientNumb}_utility_rsp.log");
                        Downstream = ReqRSP.Downstream;
                    }
                }
            }
        }


        public class ReqRSP
        {
            public static Downstream Downstream = null;
            public static uint ReqId = 0;
            public static bool IsIdDone = false;
            public static void Requests(Req req)
            {
                if (req?.GeoipReq != null) { GeoIP(req.GeoipReq); }
                IsIdDone = true;
            }

            public static void GeoIP(GeoIpReq geoIp)
            {
                Downstream = new()
                {
                    Response = new()
                    {
                        GeoipRsp = new()
                        {
                            ContinentCode = ServerConfig.Instance.Demux.DefaultContinentCode,
                            CountryCode = ServerConfig.Instance.Demux.DefaultCountryCode
                        }
                    }
                };
            }
        }
    }
}
