﻿using Google.Protobuf;
using Uplay.Store;

namespace ServerCore.DemuxResponders;

public class Store
{
    public const string Name = "store_service";
    public class Up
    {
        public static Downstream? Downstream = null;
        public static void UpstreamConverter(Guid ClientNumb, ByteString bytes)
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
        public static Downstream? Downstream = null;
        public static uint ReqId = 0;
        public static bool IsIdDone = false;
        public static void Requests(Guid ClientNumb, Req req)
        {
            File.AppendAllText($"logs/client_{ClientNumb}_store_req.log", req.ToString() + "\n");
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
            List<StoreProduct> storelist = new();
            var rpid = req.ProductId.ToList();
            var datatype = (uint)req.StoreDataType;
            foreach (var item in rpid)
            {
                var storedata = ServerCore.DB.Store.GetStoreByProdId(item);
                if (storedata != null)
                {
                    StoreProduct product = new StoreProduct()
                    {
                        Staging = false,
                        StorePartner = (StorePartner)storedata.Partner,
                        StoreReference = storedata.Reference,
                        Associations = { storedata.Associations },
                        PromotionScore = 0,
                        Configuration = ByteString.CopyFromUtf8(storedata.Configuration),
                        Credentials = "",
                        OwnershipAssociations = { storedata.OwnershipAssociations },
                        ProductId = item,
                        Revision = 0,
                        UserBlob = storedata.UserBlob
                    };
                    storelist.Add(product);
                }
            }


            Downstream = new()
            {
                Response = new()
                {
                    GetDataRsp = new()
                    { 
                        Result = StoreResult.StoreResponseSuccess,
                        StoreDataType = req.StoreDataType,
                        Products = { storelist }
                    }
                }
            };
        }

        public static void GetStore(GetStoreReq req)
        {
            var stores = ServerCore.DB.Store.GetAllStore();
            List<StoreProduct> storelist = new();
            if (stores != null)
            {
                foreach (var storedata in stores)
                {
                    StoreProduct product = new StoreProduct()
                    {
                        Staging = false,
                        StorePartner = (StorePartner)storedata.Partner,
                        StoreReference = storedata.Reference,
                        Associations = { storedata.Associations },
                        PromotionScore = 0,
                        Configuration = ByteString.CopyFromUtf8(storedata.Configuration),
                        Credentials = "",
                        OwnershipAssociations = { storedata.OwnershipAssociations },
                        ProductId = storedata.ProductId,
                        Revision = 0,
                        UserBlob = storedata.UserBlob
                    };
                    storelist.Add(product);
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    GetStoreRsp = new()
                    {
                        Result = StoreResult.StoreResponseSuccess,
                        StoreProducts = { storelist }
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
