using Google.Protobuf;
using SharedLib.Server.Json;
using Uplay.DenuvoService;

namespace Core.DemuxResponders
{
    public class Denuvo
    {
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
                ReqId = req.RequestId;
                if (req?.GetGameTokenReq != null) { GameToken(ClientNumb, req.GetGameTokenReq); }
                if (req?.GetGameTimeTokenReq != null) { GameTimeToken(ClientNumb, req.GetGameTimeTokenReq); }
                IsIdDone = true;
            }

            public static void GameToken(Guid ClientNumb, GetGameTokenReq gameTokenReq)
            {
                if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || jwt.Validate(gameTokenReq.OwnershipToken))
                {
                    Downstream = new()
                    {
                        Response = new()
                        {
                            RequestId = ReqId,
                            Result = Rsp.Types.Result.Success,
                            GetGameTokenRsp = new()
                            {
                                GameToken = gameTokenReq.RequestToken
                            }
                        }
                    };
                }
                else
                {
                    Downstream = new()
                    {
                        Response = new()
                        {
                            RequestId = ReqId,
                            Result = Rsp.Types.Result.NotOwned
                        }
                    };
                }
            }

            public static void GameTimeToken(Guid ClientNumb, GetGameTimeTokenReq gameTimeTokenReq)
            {
                if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || jwt.Validate(gameTimeTokenReq.OwnershipToken))
                {
                    Downstream = new()
                    {
                        Response = new()
                        {
                            RequestId = ReqId,
                            Result = Rsp.Types.Result.Success,
                            GetGameTimeTokenRsp = new()
                            {
                                TimeToken = gameTimeTokenReq.RequestToken,
                                TimeTokenTtlSec = 300
                            }
                        }
                    };
                }
                else
                {
                    Downstream = new()
                    {
                        Response = new()
                        {
                            RequestId = ReqId,
                            Result = Rsp.Types.Result.NotOwned
                        }
                    };
                }
            }
        }
    }
}
