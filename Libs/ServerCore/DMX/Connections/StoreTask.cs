using Google.Protobuf;
using ServerCore.DB;
using Uplay.Store;

namespace ServerCore.DMX.Connections;

public static class StoreTask
{
    public static Task<ByteString> RunConnection(DmxSession dmxSession, ByteString data)
    {
        var upstream = Upstream.Parser.ParseFrom(data);
        if (upstream == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request.InitializeReq != null)
            return Initialize(dmxSession, upstream.Request.RequestId, upstream.Request.InitializeReq);
        if (upstream.Request.GetDataReq != null)
            return GetData(dmxSession, upstream.Request.RequestId, upstream.Request.GetDataReq);
        if (upstream.Request.GetStoreReq != null)
            return GetStore(dmxSession, upstream.Request.RequestId, upstream.Request.GetStoreReq);
        if (upstream.Request.IngameStoreCheckoutReq != null)
            return IngameStoreCheckout(dmxSession, upstream.Request.RequestId, upstream.Request.IngameStoreCheckoutReq);
        return CoreTask.ReturnEmptyByteString();
    }
    public static Task<ByteString> Initialize(DmxSession dmxSession, uint reqid, InitializeReq req)
    {
        if (!dmxSession.IsLoggedIn)
            return CoreTask.ReturnEmptyByteString();
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = reqid,
                InitializeRsp = new()
                {
                    Success = true,
                    Storefront = new()
                    {
                        Configuration = "custom"
                    }
                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> GetData(DmxSession dmxSession, uint reqid, GetDataReq req)
    {
        if (!dmxSession.IsLoggedIn)
            return CoreTask.ReturnEmptyByteString();
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = reqid,
                GetDataRsp = new()
                {
                    Result = StoreResult.StoreResponseSuccess,
                    StoreDataType = req.StoreDataType,
                    Products = { }
                }
            }
        };
        var rpid = req.ProductId.ToList();
        foreach (var item in rpid)
        {
            var storedata = Store.GetStoreByProdId(item);
            if (storedata == null)
                continue;
            if (storedata.Type != (int)req.StoreDataType)
                continue;
            downstream.Response.GetDataRsp.Products.Add(new StoreProduct()
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
            });
        }
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> GetStore(DmxSession dmxSession, uint reqid, GetStoreReq req)
    {
        if (!dmxSession.IsLoggedIn)
            return CoreTask.ReturnEmptyByteString();
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = reqid,
                GetStoreRsp = new()
                {
                    Result = StoreResult.StoreResponseSuccess,
                    StoreProducts = { }
                }
            }
        };
        var stores = Store.GetAllStore();
        if (stores == null)
            return Task.FromResult(downstream.ToByteString());
        foreach (var storedata in stores)
        {
            downstream.Response.GetStoreRsp.StoreProducts.Add(new StoreProduct()
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
            });
        }
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> IngameStoreCheckout(DmxSession dmxSession, uint reqid, IngameStoreCheckoutReq req)
    {
        if (!dmxSession.IsLoggedIn)
            return CoreTask.ReturnEmptyByteString();
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = reqid,
                IngameStoreCheckoutRsp = new()
                {
                    Result = StoreResult.StoreResponseFailure,
                    CheckoutRsp = new()
                    {

                    }
                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }
}
