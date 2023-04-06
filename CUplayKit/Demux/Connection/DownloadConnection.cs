using Google.Protobuf;
using Uplay.DownloadService;

namespace CUplayKit.Demux.Connection
{
    public class DownloadConnection
    {
        #region Base
        private uint connectionId;
        private Socket socket;
        public bool isServiceSuccess = false;
        public bool isConnectionClosed = false;
        public bool initDone = false;
        public static readonly string ServiceName = "download_service";
        private uint ReqId { get; set; } = 1;
        public DownloadConnection(Socket demuxSocket)
        {
            socket = demuxSocket;

            Connect();
        }
        /// <summary>
        /// Reconnect the DownloadConnection
        /// </summary>
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
            socket.RequestId++;
            var rsp = socket.SendReq(openConnectionReq);
            if (rsp == null)
            {
                Console.WriteLine("Download Connection cancelled.");
                Close();
            }
            else
            {
                isServiceSuccess = rsp.OpenConnectionRsp.Success;
                connectionId = rsp.OpenConnectionRsp.ConnectionId;
                if (isServiceSuccess == true)
                {
                    Console.WriteLine("Download Connection successful.");
                    socket.AddToObj(connectionId, this);
                    socket.AddToDict(connectionId, ServiceName);
                    isConnectionClosed = false;
                }
            }
        }
        /// <summary>
        /// Closing DownloadConnection
        /// </summary>
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
            Debug.WriteDebug(ds.ToString(), "download.txt");

            if (ds != null || ds?.Response != null)
                return ds.Response;
            return null;
        }
        #endregion
        #region Functions
        public bool InitDownloadToken(string ownershipToken)
        {
            Req initializeReqDownload = new()
            {
                InitializeReq = new()
                {
                    OwnershipToken = ownershipToken
                },
                RequestId = ReqId
            };
            ReqId++;
            var initializeRspDownload = SendRequest(initializeReqDownload);
            if (initializeRspDownload != null)
            {
                isServiceSuccess = initializeRspDownload.InitializeRsp.Ok;
                initDone = true;
                return initializeRspDownload.InitializeRsp.Ok;
            }
            else
            {
                isServiceSuccess = false;
                initDone = false;
                return false;
            }
        }

        /// <summary>
        /// Getting url for the ManifestType (Dont use on fileChunks!)
        /// </summary>
        /// <param name="manifest">Manifest Id</param>
        /// <param name="productId">Product Id</param>
        /// <param name="manifestType">manifest,metadata,licenses</param>
        /// <returns>Urls or ""</returns>
        public string GetUrl(string manifest, uint productId, string manifestType = "manifest")
        {
            if (!initDone)
            {
                return "";
            }

            Req urlReq = new()
            {
                RequestId = ReqId,
                UrlReq = new()
                {
                    UrlRequests =
                    {
                        new UrlReq.Types.Request()
                        {
                            ProductId = productId,
                            RelativeFilePath =
                            {
                                    $"manifests/{manifest}.{manifestType}"
                            }
                        }
                    }
                }
            };
            ReqId++;
            var urlRsp = SendRequest(urlReq);
            if (urlRsp != null)
            {
                isServiceSuccess = UrlRsp.Types.Result.Success == urlRsp.UrlRsp.UrlResponses[0].Result;
                return urlRsp.UrlRsp.UrlResponses[0].DownloadUrls[0].Urls[0].ToString();
            }
            else
            {
                isServiceSuccess = false;
                return "";
            }
        }

        public List<string> GetUrlList(uint productId, List<string> ToRelPath)
        {
            Req urlReq = new()
            {
                RequestId = ReqId,
                UrlReq = new()
                {
                    UrlRequests =
                    {
                        new UrlReq.Types.Request()
                        {
                            ProductId = productId,
                            RelativeFilePath = { }
                        }
                    }
                }
            };
            ReqId++;

            urlReq.UrlReq.UrlRequests[0].RelativeFilePath.Add(ToRelPath);
            var downloadUrls = SendRequest(urlReq);
            if (downloadUrls != null)
            {
                isServiceSuccess = UrlRsp.Types.Result.Success == downloadUrls.UrlRsp.UrlResponses[0].Result;
                return downloadUrls.UrlRsp.UrlResponses[0].DownloadUrls.ToList().Select(a => a.Urls[0]).ToList();
            }
            else
            {
                isServiceSuccess = false;
                return new List<string>();
            }
        }
        #endregion
    }
}
