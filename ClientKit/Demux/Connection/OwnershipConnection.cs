using Google.Protobuf;
using Newtonsoft.Json;
using Uplay.Ownership;
using static ClientKit.Demux.Socket;

namespace ClientKit.Demux.Connection
{
    public class OwnershipConnection
    {
        #region Base
        private uint connectionId;
        private Socket socket;
        public bool isServiceSuccess = false;
        public bool isConnectionClosed = false;
        public static readonly string ServiceName = "ownership_service";
        public event EventHandler<Push> PushEvent;
        private uint ReqId { get; set; } = 1;
        public OwnershipConnection(Socket demuxSocket)
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
            socket.RequestId++;
            var rsp = socket.SendReq(openConnectionReq);
            if (rsp == null)
            {
                Console.WriteLine("Ownership Connection cancelled.");
                Close();
            }
            else
            {
                isServiceSuccess = rsp.OpenConnectionRsp.Success;
                connectionId = rsp.OpenConnectionRsp.ConnectionId;
                if (isServiceSuccess == true)
                {
                    Console.WriteLine("Ownership Connection successful");
                    socket.AddToObj(connectionId, this);
                    socket.AddToDict(connectionId, ServiceName);
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
                    Debug.WriteDebug(down.Push.ToString(), "ownership_push.txt");
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
            Debug.WriteDebug(ds.ToString(), "ownership.txt");

            if (ds != null || ds?.Response != null)
                return ds.Response;
            return null;
        }
        #endregion
        #region Functions
        public InitializeRsp? Initialize(bool GetAssociations = true)
        {
            Req req = new()
            {
                RequestId = ReqId,
                InitializeReq = new()
                {
                    GetAssociations = GetAssociations,
                    ProtoVersion = 7,
                    UseStaging = socket.TestConfig
                }
            };
            ReqId += 1;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.InitializeRsp.Success;
                return rsp.InitializeRsp;
            }
            else
            {
                isServiceSuccess = false;
                return null;
            }
        }




        /// <summary>
        /// Initializer
        /// </summary>
        /// <returns></returns>
        public List<OwnedGame>? GetOwnedGames(bool writeToFile = false)
        {
            Req req = new()
            {
                RequestId = ReqId,
                InitializeReq = new()
                {
                    GetAssociations = true,
                    ProtoVersion = 7,
                    UseStaging = socket.TestConfig
                }
            };
            ReqId += 1;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.InitializeRsp.Success;

                if (writeToFile)
                {
                    File.WriteAllText("Ownership.json", JsonConvert.SerializeObject(rsp.InitializeRsp, Formatting.Indented));
                    MemoryStream ms = new();
                    rsp.InitializeRsp.WriteTo(ms);
                    File.WriteAllBytes("Ownership", ms.ToArray());

                }

                return rsp.InitializeRsp.OwnedGames.OwnedGames_.ToList();
            }
            else
            {
                isServiceSuccess = false;
                return null;
            }
        }

        public (string, ulong) GetOwnershipToken(uint productId)
        {
            Req req = new()
            {
                RequestId = ReqId,
                OwnershipTokenReq = new()
                {
                    ProductId = productId
                }
            };
            ReqId += 1;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.OwnershipTokenRsp.Success;
                return (rsp.OwnershipTokenRsp.Token, rsp.OwnershipTokenRsp.Expiration);
            }
            else
            {
                isServiceSuccess = false;
                return ("", 0);
            }
        }

        public RegisterTemporaryOwnershipRsp? RegisterTempOwnershipToken(string token)
        {
            Req req = new()
            {
                RequestId = ReqId,
                RegisterTemporaryOwnershipReq = new()
                {
                    Token = token
                }
            };
            ReqId += 1;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.RegisterTemporaryOwnershipRsp.Success;
                return rsp.RegisterTemporaryOwnershipRsp;
            }
            else
            {
                isServiceSuccess = false;
                return null;
            }
        }

        public ConsumeOwnershipRsp? ConsumeOwnership(uint productId, uint Quantity, string TransactionId, uint GameProductId, string Signature)
        {
            Req req = new()
            {
                RequestId = ReqId,
                ConsumeOwnershipReq = new()
                {
                    ProductId = productId,
                    Quantity = Quantity,
                    TransactionId = TransactionId,
                    GameProductId = GameProductId,
                    Signature = Signature
                }
            };
            ReqId += 1;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.ConsumeOwnershipRsp.Result == ConsumeOwnershipRsp.Types.Result.Ok;
                return rsp.ConsumeOwnershipRsp;
            }
            else
            {
                isServiceSuccess = false;
                return null;
            }
        }

        public UnlockProductBranchRsp? UnlockProductBranch(uint productId, string password)
        {
            Req req = new()
            {
                RequestId = ReqId,
                UnlockProductBranchReq = new()
                {
                    Branch = new()
                    {
                        ProductId = productId,
                        Password = password
                    }
                }
            };
            ReqId += 1;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.UnlockProductBranchRsp.Result == UnlockProductBranchRsp.Types.Result.Success;
                return rsp.UnlockProductBranchRsp;
            }
            else
            {
                isServiceSuccess = false;
                return null;
            }
        }

        public string GetUplayPCTicket(uint productId, GetUplayPCTicketReq.Types.Platform platform = GetUplayPCTicketReq.Types.Platform.Normal)
        {
            Req req = new()
            {
                RequestId = ReqId,
                GetUplayPcTicketReq = new()
                {
                    UplayId = productId,
                    Platform = platform
                }
            };
            ReqId += 1;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.GetUplayPcTicketRsp.Success;
                return rsp.GetUplayPcTicketRsp.UplayPcTicket;
            }
            else
            {
                isServiceSuccess = false;
                return "";
            }
        }

        public ClaimKeystorageKeyRsp? ClaimKeystorageKeys(List<uint> productIds)
        {
            Req req = new()
            {
                RequestId = ReqId,
                ClaimKeystorageKeyReq = new()
                {
                    ProductIds = { productIds }
                }
            };
            ReqId += 1;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.ClaimKeystorageKeyRsp.Result == ClaimKeystorageKeyRsp.Types.Result.Success;
                return rsp.ClaimKeystorageKeyRsp;
            }
            else
            {
                isServiceSuccess = false;
                return null;
            }
        }

        public RegisterOwnershipRsp? RegisterOwnership(uint productId, string cdkey)
        {
            Req req = new()
            {
                RequestId = ReqId,
                RegisterOwnershipReq = new()
                {
                    ProductId = productId,
                    CdKey = cdkey
                }
            };
            ReqId += 1;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.RegisterOwnershipRsp.Result == RegisterOwnershipRsp.Types.Result.Success;
                return rsp.RegisterOwnershipRsp;
            }
            else
            {
                isServiceSuccess = false;
                return null;
            }
        }

        public RegisterOwnershipRsp? RegisterOwnershipByCdKey(string cdkey)
        {
            Req req = new()
            {
                RequestId = ReqId,
                RegisterOwnershipByCdKeyReq = new()
                {
                    CdKey = cdkey
                }
            };
            ReqId += 1;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.RegisterOwnershipRsp.Result == RegisterOwnershipRsp.Types.Result.Success;
                return rsp.RegisterOwnershipRsp;
            }
            else
            {
                isServiceSuccess = false;
                return null;
            }
        }

        public DeprecatedGetProductFromCdKeyRsp? DeprecatedGetProductFromCdKey(string cdkey)
        {
            Req req = new()
            {
                RequestId = ReqId,
                DeprecatedGetProductFromCdKeyReq = new()
                {
                    CdKey = cdkey
                }
            };
            ReqId += 1;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.DeprecatedGetProductFromCdKeyRsp.Result == DeprecatedGetProductFromCdKeyRsp.Types.Result.Success;
                return rsp.DeprecatedGetProductFromCdKeyRsp;
            }
            else
            {
                isServiceSuccess = false;
                return null;
            }
        }

        public string GetProductConfig(uint productId)
        {
            Req req = new()
            {
                RequestId = ReqId,
                GetProductConfigReq = new()
                {
                    ProductId = productId,
                    DeprecatedTestConfig = socket.TestConfig
                }
            };
            ReqId += 1;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.GetProductConfigRsp.Result == GetProductConfigRsp.Types.Result.Success;
                return rsp.GetProductConfigRsp.Configuration;
            }
            else
            {
                isServiceSuccess = false;
                return "";
            }
        }
        #endregion
    }
}
