using Google.Protobuf;
using Uplay.Friends;
using static ClientKit.Demux.Socket;

namespace ClientKit.Demux.Connection
{
    public class FriendsConnection
    {
        #region Base
        public uint connectionId;
        public Socket socket;
        public bool isServiceSuccess = false;
        public bool isConnectionClosed = false;
        public List<Friend> Friends;
        public List<Friend> Friends_Send;
        public List<Friend> Friends_Received;
        public static readonly string ServiceName = "friends_service";
        public event EventHandler<Push> PushEvent;
        private uint ReqId { get; set; } = 1;
        public FriendsConnection(Socket demuxSocket)
        {
            socket = demuxSocket;
            Friends = new();
            Friends_Send = new();
            Friends_Received = new();
            Connect();
        }

        public void Reconnect()
        {
            if (isConnectionClosed)
                Connect();
        }
        internal void Connect()
        {
            Friends = new();
            Friends_Send = new();
            Friends_Received = new();
            var openConnectionReq = new Uplay.Demux.Req
            {
                OpenConnectionReq = new()
                {
                    ServiceName = ServiceName
                },
                RequestId = socket.RequestId,
            };
            socket.RequestId++;
            var rsp = socket.SendReq(openConnectionReq);
            if (rsp == null)
            {
                Console.WriteLine("Friends Connection cancelled.");
                Close();
            }
            else
            {
                isServiceSuccess = rsp.OpenConnectionRsp.Success;
                connectionId = rsp.OpenConnectionRsp.ConnectionId;
                if (isServiceSuccess == true)
                {
                    Console.WriteLine("Friends Connection successful");
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
            Debug.WriteDebug(ds.ToString(), "friends.txt");

            if (ds != null || ds?.Response != null)
                return ds.Response;
            return null;
        }

        private void Socket_NewMessage(object? sender, DMXEventArgs e)
        {
            if (e.Data.ConnectionId == connectionId)
            {
                var down = Formatters.FormatData<Downstream>(e.Data.Data.ToArray());
                if (down != null && down.Push != null)
                {
                    Debug.WriteDebug(down.Push.ToString(), "friends_push.txt");
                    PushEvent?.Invoke(this, down.Push);
                }
            }
        }
        #endregion
        #region Functions
        public bool Auth(string Token, string SessionId, Status.Types.ActivityStatus status = Status.Types.ActivityStatus.Invisible)
        {
            Req req = new()
            {
                RequestId = ReqId,
                InitializeReq = new()
                {
                    ProtoVersion = 1,
                    IsStaging = false,
                    Localization = "en-US",
                    ActivityStatus = status,
                    UbiTicket = Token,
                    SessiondId = SessionId
                }
            };
            ReqId++;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                Console.WriteLine("Init rsp: " + rsp.InitializeRsp.Success);
                foreach (var rel in rsp.InitializeRsp.Relationship)
                {
                    if (rel.Blacklisted)
                        continue;

                    switch (rel.Relation)
                    {
                        case Relationship.Types.Relation.Friends:
                            Friends.Add(rel.Friend);
                            break;
                        case Relationship.Types.Relation.PendingReceivedInvite:
                            Friends_Received.Add(rel.Friend);
                            break;
                        case Relationship.Types.Relation.PendingSentInvite:
                            Friends_Send.Add(rel.Friend);
                            break;
                        case Relationship.Types.Relation.NoRelationship:
                        default:
                            break;
                    }
                }
                isServiceSuccess = rsp.InitializeRsp.Success;
                return rsp.InitializeRsp.Success;
            }
            else
            {
                isServiceSuccess = false;
                return false;
            }

        }

        public void AcceptAll()
        {
            foreach (var friend in Friends_Received)
            {
                Req req = new()
                {
                    RequestId = ReqId,
                    AcceptFriendshipReq = new()
                    {
                        User = friend
                    }
                };
                ReqId++;
                var rsp = SendRequest(req);
                if (rsp != null)
                {
                    isServiceSuccess = rsp.AcceptFriendshipRsp.Ok;
                    Console.WriteLine(friend.NameOnPlatform + " Is Accepted? " + rsp.AcceptFriendshipRsp.Ok);
                }
                else
                {
                    isServiceSuccess = false;
                }
            }
        }

        public bool SetActivity(Status.Types.ActivityStatus status)
        {
            Req req = new()
            {
                RequestId = ReqId,
                SetActivityStatusReq = new()
                {
                    ActivityStatus = status
                }
            };
            ReqId++;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.SetActivityStatusRsp.Success;
                return rsp.SetActivityStatusRsp.Success;
            }
            else
            {
                isServiceSuccess = false;
                return false;
            }
        }

        public bool SetGame(uint productId, string productname)
        {
            Req req = new()
            {
                RequestId = ReqId,
                SetGameReq = new()
                {
                    Game = new()
                    {
                        UplayId = productId,
                        ProductName = productname
                    }
                }
            };
            ReqId++;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.SetGameRsp.Success;
                return rsp.SetGameRsp.Success;
            }
            else
            {
                isServiceSuccess = false;
                return false;
            }
        }
        public bool SetEmptyGame()
        {
            Req req = new()
            {
                RequestId = ReqId,
                SetGameReq = new()
                {
                }
            };
            ReqId++;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.SetGameRsp.Success;
                return rsp.SetGameRsp.Success;
            }
            else
            {
                isServiceSuccess = false;
                return false;
            }
        }
        public void SetRichPresence(uint productId, string key, string val)
        {

            RichPresenceTokenPair richPresenceToken = new()
            {
                Key = key,
                Val = val
            };
            Req req = new()
            {
                RequestId = ReqId,
                SetRichPresenceReq = new()
                {
                    PresenceState = new()
                    {
                        PresenceId = 1,
                        ProductId = productId,
                        PresenceTokens = { richPresenceToken }
                    }
                }
            };
            ReqId++;

            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = rsp.SetRichPresenceRsp.Success;
            }
            else
            {
                isServiceSuccess = false;
            }
        }
        #endregion
    }
}
