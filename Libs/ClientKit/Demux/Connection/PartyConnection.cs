using Google.Protobuf;
using SharedLib.Shared;
using Uplay.Party;
using static ClientKit.Demux.Socket;

namespace ClientKit.Demux.Connection
{
    public class PartyConnection
    {
        #region Base
        public uint connectionId;
        public Socket socket;
        public bool isServiceSuccess = false;
        public bool isConnectionClosed = false;
        public static readonly string ServiceName = "party_service";
        private uint Cookie = 0;
        public event EventHandler<Push> PushEvent;
        private uint ReqId { get; set; } = 1;
        public PartyConnection(Socket demuxSocket)
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
                Console.WriteLine("Party Connection cancelled.");
                Close();
            }
            else
            {
                isServiceSuccess = rsp.OpenConnectionRsp.Success;
                connectionId = rsp.OpenConnectionRsp.ConnectionId;
                if (isServiceSuccess == true)
                {
                    Console.WriteLine("Party Connection successful");
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
            socket.NewMessage -= Socket_NewMessage;
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
            Debug.WriteDebug(ds.ToString(), "playtime.txt");

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
                    Debug.WriteDebug(down.Push.ToString(), "party_push.txt");
                    PushEvent?.Invoke(this, down.Push);
                }
            }
        }
        #endregion
        #region Functions
        public void StartSession(uint cookie = 0)
        {
            StartSessionReq session = new();

            if (cookie != 0)
            {
                session.Cookie = cookie;
            }

            Req req = new()
            {
                RequestId = ReqId,
                StartSessionReq = session
            };
            ReqId++;
            var rsp = SendRequest(req);
            if (rsp != null)
            {
                isServiceSuccess = true;
                Cookie = rsp.StartSessionRsp.Cookie;
                Debug.PWDebug(rsp.StartSessionRsp);
            }
            else
            {
                isServiceSuccess = false;
            }

        }
        #endregion
    }
}
