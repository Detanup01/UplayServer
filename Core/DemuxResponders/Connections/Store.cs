using Google.Protobuf;
using Uplay.Store;

namespace Core.DemuxResponders
{
    public class Store
    {
        public class Up
        {
            public static Downstream Downstream = null;
            public static void UpstreamConverter(int ClientNumb, ByteString bytes)
            {
                var UpstreamBytes = bytes.Skip(4).ToArray();
                var Upsteam = Upstream.Parser.ParseFrom(UpstreamBytes);
                if (Upsteam != null)
                {
                    if (Upsteam.Request != null)
                    {
                        ReqRSP.Requests(Upsteam.Request);
                        while (ReqRSP.IsIdDone == false)
                        {

                        }
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
                if (req?.InitializeReq != null) { Init(req.InitializeReq); }
                IsIdDone = true;
            }

            public static void Init(InitializeReq req)
            {
                Downstream = new()
                {
                    Response = new()
                    {
                        InitializeRsp = new()
                        {
                            Storefront = new() { Configuration = "custom" },
                            Success = true
                               
                        }
                    }
                };
            }
        }
    }
}
