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
                        ReqRSP.Requests(ClientNumb, Upsteam.Request);
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
            public static void Requests(int ClientNumb, Req req)
            {
                ReqId = req.RequestId;
                if (req?.InitializeReq != null) { Init(req.InitializeReq); }
                if (req?.GetDataReq != null) { Init(req.InitializeReq); }
                if (req?.GetStoreReq != null) { Init(req.InitializeReq); }
                if (req?.IngameStoreCheckoutReq != null) { Init(req.InitializeReq); }
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

            public static void GetData(GetDataReq req)
            {
                Downstream = new()
                {
                    Response = new()
                    {
                        GetDataRsp = new()
                        { 
                            Result = StoreResult.StoreResponseFailure,
                            StoreDataType = req.StoreDataType,
                            Products = { }
                        }
                    }
                };
            }

            public static void GetStore(GetStoreReq req)
            {
                Downstream = new()
                {
                    Response = new()
                    {
                        GetStoreRsp = new()
                        {
                            Result = StoreResult.StoreResponseFailure,
                            StoreProducts = { }
                        }
                    }
                };
            }

            public static void IngameStoreCheckout(IngameStoreCheckoutReq req)
            {
                Downstream = new()
                {
                    Response = new()
                    {
                        IngameStoreCheckoutRsp = new()
                        {
                            Result = StoreResult.StoreResponseFailure,
                            CheckoutRsp = new()
                            { 
                                NewWindowFlag = false,
                                RedirectLocaleCode = "en-us",
                                Url = ""
                            }

                        }
                    }
                };
            }
        }
    }
}
