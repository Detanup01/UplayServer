using Google.Protobuf;
using Uplay.Utility;

namespace Core.DemuxResponsers
{
    public class Utility
    {
        public static readonly string Name = "utility_service";
        public class Up
        {
            public static Downstream Downstream = null;
            public static void UpstreamConverter(int ClientNumb, ByteString bytes)
            {
                var UpstreamBytes = bytes.ToArray();
                var Upsteam = Upstream.Parser.ParseFrom(UpstreamBytes);
                Utils.WriteFile(Upsteam.ToString(), $"client_{ClientNumb}_utility_req.log");
                if (Upsteam != null)
                {
                    if (Upsteam.Request != null)
                    {
                        ReqRSP.Requests(Upsteam.Request);
                        while (ReqRSP.IsIdDone == false)
                        {

                        }
                        Utils.WriteFile(ReqRSP.Downstream.ToString(), $"client_{ClientNumb}_utility_rsp.log");
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
                            ContinentCode = Config.DMX.DefaultContinentCode,
                            CountryCode = Config.DMX.DefaultCountryCode
                        }
                    }
                };
            }
        }
    }
}
