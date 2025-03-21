using Google.Protobuf;
using ServerCore.DB;
using ServerCore.Models.User;
using ServerCore.Models;
using Uplay.CloudsaveService;

namespace ServerCore.DMX.Connections;

public static class CloudSaveTask
{
    public static Task<ByteString> RunConnection(DmxSession dmxSession, ByteString data)
    {
        var upstream = Upstream.Parser.ParseFrom(data);
        if (upstream == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request.CloudsaveReq != null)
            return Cloudsave(dmxSession, upstream.Request.RequestId, upstream.Request.CloudsaveReq);
        if (upstream.Request.CloudsaveReqV2 != null)
            return CloudsaveV2(dmxSession, upstream.Request.RequestId, upstream.Request.CloudsaveReqV2);
        if (upstream.Request.CloudsaveUrlReq != null)
            return CloudsaveUrl(dmxSession, upstream.Request.RequestId, upstream.Request.CloudsaveUrlReq);
        return CoreTask.ReturnEmptyByteString();
    }

    public static Task<ByteString> Cloudsave(DmxSession dmxSession, uint ReqId, CloudsaveReq req)
    {
        CloudsaveRsp.Types.Status status = CloudsaveRsp.Types.Status.Denied;
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic != null)
        {
            if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || owbasic.OwnedGamesIds.Contains(req.UplayId))
            {
                status = CloudsaveRsp.Types.Status.Ok;
            }
        }

        string urlPath = $"/cloudsave/{req.UplayId}/";
        string Method = "GET";

        //  List items
        if (req.ListItems != null)
        {
            urlPath += "all";
        }

        //  Delete Item
        if (req.DeleteItem != null)
        {
            Method = "DELETE";
            urlPath += req.DeleteItem.ItemId;
        }

        //  Get item
        if (req.GetItem != null)
        {
            urlPath += req.GetItem.ItemId;
        }

        //  Put item
        if (req.PutItem != null)
        {
            Method = "PUT";
            urlPath += req.PutItem.ItemId;
        }

        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                CloudsaveRsp = new()
                {
                    Status = status,
                    MethodString = Method,
                    HeaderString = $"UserId: {dmxSession.UserId}",
                    HostString = ServerConfig.Instance.HTTPS_Url,
                    PathString = urlPath
                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> CloudsaveV2(DmxSession dmxSession, uint ReqId, CloudsaveReqV2 req)
    {
        CloudsaveRspV2.Types.Status status = CloudsaveRspV2.Types.Status.Denied;
        List<CloudsaveRspV2.Types.HttpReq> httpreq = [];
        string method = "GET";
        string urlPath = $"/cloudsave/{req.UplayId}/";

        var template = new CloudsaveRspV2.Types.HttpReq()
        {
            Method = method,
            Header = $"UserId: {dmxSession.UserId}",
            Host = ServerConfig.Instance.HTTPS_Url,
            Path = urlPath
        };
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic != null)
        {
            if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || owbasic.OwnedGamesIds.Contains(req.UplayId))
            {
                status = CloudsaveRspV2.Types.Status.Ok;
            }
        }
        if (req.DeleteItems != null)
        {
            foreach (var delete in req.DeleteItems.Items)
            {
                var newDelete = template;
                newDelete.Path += delete.ItemId;
                newDelete.Method = "DELETE";
                httpreq.Add(newDelete);
            }
        }

        if (req.GetItems != null)
        {
            foreach (var item in req.GetItems.Items)
            {
                var newDelete = template;
                newDelete.Path += item.ItemId;
                httpreq.Add(newDelete);
            }
        }

        if (req.PutItems != null)
        {
            foreach (var item in req.PutItems.Items)
            {
                var newDelete = template;
                newDelete.Path += item.ItemId;
                newDelete.Method = "PUT";
                httpreq.Add(newDelete);
            }
        }


        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                CloudsaveRspV2 = new()
                {
                    Status = status,
                    HttpReqs =
                        {
                            httpreq
                        }
                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> CloudsaveUrl(DmxSession dmxSession, uint ReqId, CloudsaveUrlReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                CloudsaveUrlRsp = new()
                {
                    Status = CloudsaveUrlRsp.Types.Status.InternalError
                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }
}
