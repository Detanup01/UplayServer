using Google.Protobuf;
using ServerCore.DB;
using ServerCore.Json;
using Uplay.CloudsaveService;

namespace Core.DemuxResponders
{
    public class CloudSave
    {
        public const string Name = "cloudsave_service";
        public class Up
        {
            public static Downstream Downstream = null;
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
            public static Downstream Downstream = null;
            public static uint ReqId = 0;
            public static bool IsIdDone = false;
            public static void Requests(Guid ClientNumb, Req req)
            {
                File.AppendAllText($"logs/client_{ClientNumb}_cloudsave_req.log", req.ToString() + "\n");
                ReqId = req.RequestId;
                if (req?.CloudsaveReq != null) { Cloudsave(ClientNumb, req.CloudsaveReq); }
                if (req?.CloudsaveReqV2 != null) { CloudsaveV2(ClientNumb, req.CloudsaveReqV2); }
                if (req?.CloudsaveUrlReq != null) { CloudsaveURL(ClientNumb, req.CloudsaveUrlReq); }
                IsIdDone = true;
            }


            public static void Cloudsave(Guid ClientNumb, CloudsaveReq req)
            {
                CloudsaveRsp.Types.Status status = CloudsaveRsp.Types.Status.Denied;
                var userID = Globals.IdToUser[ClientNumb];
                var owbasic = DBUser.GetOwnershipBasic(userID);
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

                Downstream = new()
                { 
                    Response = new()
                    { 
                        RequestId = ReqId,
                        CloudsaveRsp = new()
                        { 
                            Status = status,
                            MethodString = Method,
                            HeaderString = $"UserId: {userID}",
                            HostString = ServerConfig.Instance.HTTPS_Url,
                            PathString = urlPath
                        }
                    }
                };
            }

            public static void CloudsaveV2(Guid ClientNumb, CloudsaveReqV2 req)
            {
                CloudsaveRspV2.Types.Status status = CloudsaveRspV2.Types.Status.Denied;
                List<CloudsaveRspV2.Types.HttpReq> httpreq = new();
                string method = "GET";
                string urlPath = $"/cloudsave/{req.UplayId}/";
                var userID = Globals.IdToUser[ClientNumb];

                var template = new CloudsaveRspV2.Types.HttpReq()
                {
                    Method = method,
                    Header = $"UserId: {userID}",
                    Host = ServerConfig.Instance.HTTPS_Url,
                    Path = urlPath
                };
                var owbasic = DBUser.GetOwnershipBasic(userID);
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


                Downstream = new()
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
            }

            public static void CloudsaveURL(Guid ClientNumb, CloudsaveUrlReq req)
            {
                Downstream = new()
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
            }
        }
    }
}
