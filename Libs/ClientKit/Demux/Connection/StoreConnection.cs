using Google.Protobuf;
using SharedLib.Shared;
using Uplay.Store;
using static ClientKit.Demux.Socket;

namespace ClientKit.Demux.Connection
{
    public class StoreConnection
    {
        #region Base
        private uint connectionId;
        private Socket socket;
        public bool isServiceSuccess = false;
        public bool isConnectionClosed = false;
        public Storefront storefront;
        public static readonly string ServiceName = "store_service";
        public event EventHandler<Push> PushEvent;
        private uint ReqId { get; set; } = 1;
        public StoreConnection(Socket demuxSocket)
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
                RequestId = socket.RequestId,
                OpenConnectionReq = new()
                {
                    ServiceName = ServiceName
                }
            };
            var rsp = socket.SendReq(openConnectionReq);
            storefront = new() { Configuration = "" };
            if (rsp == null)
            {
                Console.WriteLine("Store Connection cancelled.");
                Close();
            }
            else
            {
                isServiceSuccess = rsp.OpenConnectionRsp.Success;
                connectionId = rsp.OpenConnectionRsp.ConnectionId;
                if (isServiceSuccess == true)
                {
                    Console.WriteLine("Store Connection successful.");
                    socket.AddToObj(connectionId, this);
                    socket.AddToDict(connectionId, ServiceName);
                    socket.RequestId++;
                    socket.NewMessage += Socket_NewMessage;
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
            socket.NewMessage -= Socket_NewMessage;
        }
        #endregion
        #region Request/Message
        private void Socket_NewMessage(object? sender, DMXEventArgs e)
        {
            if (e.Data.ConnectionId == connectionId)
            {
                var down = Formatters.FormatData<Downstream>(e.Data.Data.ToArray());
                if (down != null && down.Push != null)
                {
                    Debug.WriteDebug(down.Push.ToString(), "store_push.txt");
                    PushEvent?.Invoke(this, down.Push);
                }
            }
        }

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
            Debug.WriteDebug(ds.ToString(), "store.txt");
            if (ds != null || ds?.Response != null)
                return ds.Response;
            return null;
        }
        #endregion
        #region Functions
        public void Init()
        {
            Req initializeReqDownload = new()
            {
                InitializeReq = new()
                {
                    UseStaging = socket.TestConfig,
                    ProtoVersion = 7
                },
                RequestId = ReqId
            };
            ReqId++;
            var initializeRspDownload = SendRequest(initializeReqDownload);
            if (initializeRspDownload != null)
            {
                isServiceSuccess = initializeRspDownload.InitializeRsp.Success;
                Console.WriteLine("Store Servive Initialized: " + initializeRspDownload.InitializeRsp.Success);
                storefront = initializeRspDownload.InitializeRsp.Storefront;
            }
            else
            {
                isServiceSuccess = false;
                storefront = new() { Configuration = "" };
            }
        }

        public GetStoreRsp GetStore()
        {
            Req getstorereq = new()
            {
                RequestId = ReqId,
                GetStoreReq = new()
                {
                }
            };
            ReqId++;
            var getstorersp = SendRequest(getstorereq);
            if (getstorersp != null)
            {
                isServiceSuccess = StoreResult.StoreResponseSuccess == getstorersp.GetStoreRsp.Result;
                return getstorersp.GetStoreRsp;
            }
            else
            {
                isServiceSuccess = false;
                return new() { StoreProducts = { }, Result = StoreResult.StoreResponseFailure };
            }
        }

        public GetDataRsp GetData(StoreType storeType, List<uint> prodIds)
        {
            Req getdatareq = new()
            {
                RequestId = ReqId,
                GetDataReq = new()
                {
                    StoreDataType = storeType,
                    ProductId =
                    {
                        prodIds
                    }
                }
            };
            ReqId++;
            var getdatarsp = SendRequest(getdatareq);
            if (getdatarsp != null)
            {
                isServiceSuccess = StoreResult.StoreResponseSuccess == getdatarsp.GetDataRsp.Result;
                return getdatarsp.GetDataRsp;
            }
            else
            {
                isServiceSuccess = false;
                return new() { Products = { }, Result = StoreResult.StoreResponseFailure };
            }
        }
        #endregion
    }
}
