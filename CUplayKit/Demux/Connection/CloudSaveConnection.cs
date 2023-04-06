using Google.Protobuf;
using Uplay.CloudsaveService;

namespace CUplayKit.Demux.Connection
{
    public class CloudSaveConnection
    {
        #region Base
        public uint connectionId;
        public Socket socket;
        public bool isServiceSuccess = false;
        public bool isConnectionClosed = false;
        public static readonly string ServiceName = "cloudsave_service";
        private uint ReqId { get; set; } = 1;
        public CloudSaveConnection(Socket demuxSocket)
        {
            socket = demuxSocket;

            Connect();
        }

        public void Reconnect()
        {
            if (isConnectionClosed)
                Connect();
        }
        internal void Connect()
        {
            var openConnectionReq = new Uplay.Demux.Req
            {
                OpenConnectionReq = new()
                {
                    ServiceName = ServiceName
                },
                RequestId = socket.RequestId
            };
            socket.RequestId++;
            var rsp = socket.SendReq(openConnectionReq);
            if (rsp == null)
            {
                Console.WriteLine("CloudSave Connection cancelled.");
                Close();
            }
            else
            {
                isServiceSuccess = rsp.OpenConnectionRsp.Success;
                connectionId = rsp.OpenConnectionRsp.ConnectionId;
                if (isServiceSuccess == true)
                {
                    Console.WriteLine("CloudSave Connection successful");
                    socket.AddToObj(connectionId, this);
                    socket.AddToDict(connectionId, ServiceName);
                    isConnectionClosed = false;
                }
            }
        }

        public void Close()
        {
            if (socket.TerminateConnectionId == connectionId)
            {
                Console.WriteLine($"Connection terminated via Socket {ServiceName}");
            }
            socket.RemoveConnection(connectionId);
            isServiceSuccess = false;
            connectionId = uint.MaxValue;
            isConnectionClosed = true;
        }
        #endregion
        #region Request
        public Rsp? SendRequest(Req req)
        {
            if (isConnectionClosed)
                return null;

            Upstream post = new() { Request = req };
            Uplay.Demux.Upstream up = new()
            {
                Push = new()
                {
                    Data = new()
                    {
                        ConnectionId = connectionId,
                        Data = ByteString.CopyFrom(Formatters.FormatUpstream(post.ToByteArray()))
                    }
                }
            };

            var down = socket.SendUpstream(up);
            if (isConnectionClosed || down == null || !down.Push.Data.HasData)
                return null;

            var ds = Formatters.FormatData<Downstream>(down.Push.Data.Data.ToByteArray());
            Debug.WriteDebug(ds.ToString(), "cloudsave.txt");

            if (ds != null || ds?.Response != null)
                return ds.Response;
            return null;
        }
        #endregion
        #region Functions
        public CloudsaveRsp List(uint uplayid, string Username, string ownershipToken)
        {
            Req getuserinfo = new()
            {
                RequestId = ReqId + 1,
                CloudsaveReq = new()
                {
                    UplayId = uplayid,
                    Username = Username,
                    OwnershipToken = ownershipToken,
                    ListItems = new()
                }
            };
            var userinforsp = SendRequest(getuserinfo);
            if (userinforsp != null)
            {
                return userinforsp.CloudsaveRsp;
            }
            else
            {
                return new() { Status = CloudsaveRsp.Types.Status.InternalError };
            }
        }

        public CloudsaveRsp GetItem(uint uplayid, string Username, string ownershipToken, uint ItemId)
        {
            Req getuserinfo = new()
            {
                RequestId = ReqId + 1,
                CloudsaveReq = new()
                {
                    UplayId = uplayid,
                    Username = Username,
                    OwnershipToken = ownershipToken,
                    GetItem = new()
                    {
                        ItemId = ItemId
                    }
                }
            };
            var userinforsp = SendRequest(getuserinfo);
            if (userinforsp != null)
            {
                return userinforsp.CloudsaveRsp;
            }
            else
            {
                return new() { Status = CloudsaveRsp.Types.Status.InternalError };
            }
        }

        public CloudsaveUrlRsp Send(uint uplayid, string ownershipToken, string ItemName, string md5B64, uint lenght)
        {
            Item item = new()
            {
                ItemName = ItemName,
                OptionalArgs = new()
                {
                    Md5Base64 = md5B64,
                    WriteLength = lenght
                }
            };
            Req getuserinfo = new()
            {
                RequestId = ReqId + 1,
                CloudsaveUrlReq = new()
                {
                    UplayId = uplayid,
                    OwnershipToken = ownershipToken,
                    Method = CloudsaveUrlReq.Types.Method.Put,
                    Items = { item }
                }
            };
            var userinforsp = SendRequest(getuserinfo);
            if (userinforsp != null)
            {
                return userinforsp.CloudsaveUrlRsp;
            }
            else
            {
                return new() { Status = CloudsaveUrlRsp.Types.Status.InternalError };
            }
        }

        public CloudsaveUrlRsp Get(uint uplayid, string ownershipToken, string ItemName)
        {
            Item item = new()
            {
                ItemName = ItemName
            };
            Req getuserinfo = new()
            {
                RequestId = ReqId + 1,
                CloudsaveUrlReq = new()
                {
                    UplayId = uplayid,
                    OwnershipToken = ownershipToken,
                    Method = CloudsaveUrlReq.Types.Method.Get,
                    Items = { item }
                }
            };
            var userinforsp = SendRequest(getuserinfo);
            if (userinforsp != null)
            {
                return userinforsp.CloudsaveUrlRsp;
            }
            else
            {
                return new() { Status = CloudsaveUrlRsp.Types.Status.InternalError };
            }
        }

        public CloudsaveRspV2 GetV2(uint uplayid, string ownershipToken, string ItemName, uint ItemId)
        {
            CloudsaveReqV2.Types.GetItems.Types.Item item = new()
            {
                ItemId = ItemId,
                ItemName = ItemName
            };

            CloudsaveReqV2.Types.GetItems Getitem = new();
            Getitem.Items.Add(item);
            Req getuserinfo = new()
            {
                RequestId = ReqId + 1,
                CloudsaveReqV2 = new()
                {
                    UplayId = uplayid,
                    OwnershipToken = ownershipToken,
                    GetItems = Getitem,
                }
            };
            var userinforsp = SendRequest(getuserinfo);
            if (userinforsp != null)
            {
                return userinforsp.CloudsaveRspV2;
            }
            else
            {
                return new() { Status = CloudsaveRspV2.Types.Status.InternalError };
            }
        }
        #endregion
    }
}
