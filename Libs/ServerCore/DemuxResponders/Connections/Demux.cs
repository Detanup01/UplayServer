using Google.Protobuf;
using Newtonsoft.Json.Linq;
using ServerCore.DB;
using SharedLib.Shared;
using Uplay.Demux;
using ServerCore.Models;

namespace ServerCore.DemuxResponders;

public static class Demux
{
    public static void DeleteClient(Guid ClientNumb)
    {
        if (Globals.IdToUser.TryGetValue(ClientNumb, out var Id))
        {
            Globals.IdToUser.Remove(ClientNumb);
            Globals.UserToId.Remove(Id);
            Auth.DeleteCurrentWithUserId(Id);
            Auth.DeleteDMXWithUserId(Id);
            Download.ReqRSP.UserInits.Remove(ClientNumb);
            Friends.ReqRSP.UserInits.Remove(ClientNumb);
            Ownership.ReqRSP.UserInits.Remove(ClientNumb);
        }
    }


    public static class PushRSP
    {
        public static Downstream? Downstream = null;
        public static void Push(Guid ClientNumb, Push push)
        {
            File.AppendAllText($"logs/client_{ClientNumb}_push.log", push.ToString() + "\n");
            if (push?.Data != null) { PushData(ClientNumb, push.Data); }
            if (push?.ProductEnded != null) { ProductEnd(ClientNumb, push.ProductEnded); }
            if (push?.ProductStarted != null) { ProductStart(ClientNumb, push.ProductStarted); }
            if (push?.KeepAlive != null) { KeepAlive(ClientNumb); }
            if (push?.ClientVersion != null) { ClientVersion(ClientNumb, push.ClientVersion); }
        }

        public static void PushData(Guid ClientNumb, DataMessage data)
        {
            bool SendBackEndPush = false;
            ByteString rspBytes = ByteString.Empty;
            if (Globals.IdToUser.TryGetValue(ClientNumb, out var id))
            {
                var ConnectName = Auth.GetConNameByUserAndId(id, data.ConnectionId);
                if (Globals.Connections.Contains(ConnectName))
                {
                    switch (ConnectName)
                    {
                        case Download.Name:
                            Download.Up.UpstreamConverter(ClientNumb, data.Data);
                            if (Download.Up.Downstream != null)
                            {
                                File.AppendAllText($"logs/client_{ClientNumb}_download_rsp.log", Download.Up.Downstream.ToString() + "\n");
                                rspBytes = Download.Up.Downstream.ToByteString();
                            }
                            break;
                        case Denuvo.Name:
                            Denuvo.Up.UpstreamConverter(ClientNumb, data.Data);
                            if (Denuvo.Up.Downstream != null)
                            {
                                File.AppendAllText($"logs/client_{ClientNumb}_denuvo_rsp.log", Denuvo.Up.Downstream.ToString() + "\n");
                                rspBytes = Denuvo.Up.Downstream.ToByteString();
                            }
                            break;
                        case Friends.Name:
                            Friends.Up.UpstreamConverter(ClientNumb, data.Data);
                            if (Friends.Up.Downstream != null)
                            {
                                File.AppendAllText($"logs/client_{ClientNumb}_friends_rsp.log", Friends.Up.Downstream.ToString() + "\n");
                                rspBytes = Friends.Up.Downstream.ToByteString();
                            }
                            break;
                        case Ownership.Name:
                            Ownership.Up.UpstreamConverter(ClientNumb, data.Data);
                            if (Ownership.Up.Downstream != null)
                            {
                                File.AppendAllText($"logs/client_{ClientNumb}_ownership_rsp.log", Ownership.Up.Downstream.ToString() + "\n");
                                rspBytes = Ownership.Up.Downstream.ToByteString();
                            }
                            break;
                        case Party.Name:
                            Party.Up.UpstreamConverter(ClientNumb, data.Data);
                            if (Party.Up.Downstream != null)
                            {
                                File.AppendAllText($"logs/client_{ClientNumb}_party_rsp.log", Party.Up.Downstream.ToString() + "\n");
                                rspBytes = Party.Up.Downstream.ToByteString();
                            }
                            break;
                        case Store.Name:
                            Store.Up.UpstreamConverter(ClientNumb, data.Data);
                            if (Store.Up.Downstream != null)
                            {
                                File.AppendAllText($"logs/client_{ClientNumb}_store_rsp.log", Store.Up.Downstream.ToString() + "\n");
                                rspBytes = Store.Up.Downstream.ToByteString();
                            }
                            break;
                        case CloudSave.Name:
                            CloudSave.Up.UpstreamConverter(ClientNumb, data.Data);
                            if (CloudSave.Up.Downstream != null)
                            {
                                File.AppendAllText($"logs/client_{ClientNumb}_cloudsave_rsp.log", CloudSave.Up.Downstream.ToString() + "\n");
                                rspBytes = CloudSave.Up.Downstream.ToByteString();
                            }
                            break;
                        case Achievement.Name:
                            Achievement.Up.ReqConverter(ClientNumb, data.Data);
                            if (Achievement.Up.Rsp != null)
                            {
                                File.AppendAllText($"logs/client_{ClientNumb}_ach_rsp.log", Achievement.Up.Rsp.ToString() + "\n");
                                rspBytes = Achievement.Up.Rsp.ToByteString();
                            }
                            break;
                    }
                }
                else { SendBackEndPush = true; }
            }
            else { SendBackEndPush = true; }

            if (SendBackEndPush)
            {
                Downstream = new()
                {
                    Push = new()
                    {
                        ConnectionClosed = new()
                        {
                            ConnectionId = data.ConnectionId,
                            ErrorCode = ConnectionClosedPush.Types.Connection_ErrorCode.ConnectionForceQuit
                        }
                    }
                };
            }
            else
            {
                Downstream = new()
                {
                    Push = new()
                    {
                        Data = new()
                        {
                            ConnectionId = data.ConnectionId,
                            Data = ByteString.CopyFrom(Formatters.FormatUpstream(rspBytes.ToArray()))
                        }
                    }
                };
            }


        }

        public static void ProductEnd(Guid ClientNumb, ProductEndedPush productEnded)
        {
            Console.WriteLine(productEnded.ProductId);
        }

        public static void ProductStart(Guid ClientNumb, ProductStartedPush productStarted)
        {
            Console.WriteLine(productStarted.ProductId);
        }

        public static void KeepAlive(Guid ClientNumb)
        {
            Downstream = new()
            {
                Push = new()
                {
                    KeepAlive = new()
                    { }
                }
            };
        }

        public static void ClientVersion(Guid ClientNumb, ClientVersionPush clientVersion)
        {
            if (!Globals.AcceptVersions.Contains(clientVersion.Version))
            {
                Downstream = new()
                {
                    Push = new()
                    {

                        ClientOutdated = new()
                        {

                        }
                    }
                };
            }
        }


    }

    public static class ReqRSP
    {
        public static Downstream? Downstream = null;
        public static uint ReqId = 0;
        public static void Requests(Guid ClientNumb, Req req)
        {
            File.AppendAllText($"logs/client_{ClientNumb}_req.log", req.ToString() + "\n");
            ReqId = req.RequestId;
            //if (req?.ClientIpOverride != null) { Console.WriteLine(req.ClientIpOverride); }
            if (req?.AuthenticateReq != null) { Authenticate(ClientNumb, req.AuthenticateReq); }
            if (req?.GetPatchInfoReq != null) { GetPatchInfo(req.GetPatchInfoReq); }
            if (req?.OpenConnectionReq != null) { OpenConnection(ClientNumb, req.OpenConnectionReq); }
            if (req?.ServiceRequest != null) { ServiceRequest(ClientNumb, req.ServiceRequest); }
        }

        public static void GetPatchInfo(GetPatchInfoReq req)
        {
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    GetPatchInfoRsp = new()
                    {
                        TrackType = req.TrackType,
                        TestConfig = req.TestConfig,
                        Success = true,
                        LatestVersion = Globals.AcceptVersions.Last(),
                        PatchTrackId = req.PatchTrackId,
                        PatchBaseUrl = ServerConfig.Instance.HTTPS_Url + "/patch/"
                    }
                }
            };
        }

        public static void Authenticate(Guid ClientNumb, AuthenticateReq authenticateReq)
        {
            bool IsSuccess = false;
            bool IsBanned = false;
            bool IsExpired = false;

            // Our Custom Client should using UbiToken For easier
            if (authenticateReq.Token.HasUbiToken)
            {
                //Todo Check Token Expire
                var userId = Auth.GetUserIdByToken(authenticateReq.Token.UbiToken.ToStringUtf8(), TokenType.Ticket);
                if (!DBUserExt.IsUserBanned(userId))
                {
                    Globals.IdToUser.Add(ClientNumb, userId);
                    Globals.UserToId.Add(userId, ClientNumb);
                    IsSuccess = true;
                }
                else
                {
                    IsBanned = true;
                }
            }

            if (authenticateReq.Token.HasOrbitToken)
            {
                var userId = Auth.GetUserIdByToken(authenticateReq.Token.OrbitToken, TokenType.Orbit);
                if (!DBUserExt.IsUserBanned(userId))
                {
                    Globals.IdToUser.Add(ClientNumb, userId);
                    Globals.UserToId.Add(userId, ClientNumb);
                    IsSuccess = true;
                }
                else
                {
                    IsBanned = true;
                }
            }

            if (authenticateReq.Token.HasUbiTicket)
            {
                var job = JObject.Parse(JWTController.GetUnkownJWTJson(authenticateReq.Token.UbiTicket));
                var sid = job["sid"];
                Console.WriteLine(sid);
                if (sid == null)
                    goto END;
                var user_Id = Auth.GetUserIdBySessionId(Guid.Parse(sid.ToString()));
                if (user_Id != Guid.Empty)
                {
                    IsSuccess = true;
                    Globals.IdToUser.Add(ClientNumb, user_Id);
                    Globals.UserToId.Add(user_Id, ClientNumb);
                }
                var userId = Auth.GetUserIdByToken(authenticateReq.Token.UbiTicket, TokenType.Ticket);
                if (!DBUserExt.IsUserBanned(userId))
                {
                    Globals.IdToUser.Add(ClientNumb, userId);
                    Globals.UserToId.Add(userId, ClientNumb);
                    IsSuccess = true;
                }
                else
                {
                    IsBanned = true;
                }
            }
            END:
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    AuthenticateRsp = new()
                    {
                        Banned = IsBanned,
                        Expired = IsExpired,
                        Success = IsSuccess
                    }
                }
            };

        }

        public static void OpenConnection(Guid ClientNumb, OpenConnectionReq OpenConnection)
        {
            uint Id = 1;
            bool IsSucces = false;

            if (Globals.Connections.Contains(OpenConnection.ServiceName))
            {
                if (Globals.IdToUser.TryGetValue(ClientNumb, out var id))
                {

                    IsSucces = true;
                    uint LatestCon = Auth.GetLatestConIdByUser(id);
                    uint con = Auth.GetConIdByUserAndName(id, OpenConnection.ServiceName);

                    if (LatestCon == 0 && con == 0)
                    {
                        Auth.AddDMX(id, Id, OpenConnection.ServiceName);
                    }
                    if (LatestCon != 0)
                    {
                        Auth.AddDMX(id, LatestCon + 1, OpenConnection.ServiceName);
                        Id = LatestCon + 1;
                    }
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    OpenConnectionRsp = new()
                    {
                        ConnectionId = Id,
                        Success = IsSucces
                    }
                }
            };
        }

        public static void ServiceRequest(Guid ClientNumb, ServiceReq service)
        {
            bool IsSucces = false;
            ByteString returnData = ByteString.Empty;
            if (Globals.Services.Contains(service.Service))
            {
                IsSucces = true;
                //Console.WriteLine(service.Service);
                if (service.Service == Utility.Name)
                {
                    Utility.Up.UpstreamConverter(ClientNumb, service.Data);
                    if (Utility.Up.Downstream != null)
                    {
                        returnData = ByteString.CopyFrom(Utility.Up.Downstream.ToByteArray());
                    }
                }
                else if (service.Service == UplayDll.Name)
                {
                    UplayDll.Up.UpstreamConverter(ClientNumb, service.Data);
                    if (UplayDll.Up.Rsp != null)
                    {
                        returnData = ByteString.CopyFrom(UplayDll.Up.Rsp.ToByteArray());
                    }
                }
                else if (service.Service == ClientConfig.Name)
                {
                    ClientConfig.Up.UpstreamConverter(service.Data);
                    if (ClientConfig.Up.Downstream != null)
                    {
                        returnData = ByteString.CopyFrom(ClientConfig.Up.Downstream.ToByteArray());
                    }
                }
                else
                {
                    Console.WriteLine(service.Service);
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    ServiceRsp = new()
                    {
                        Data = returnData,
                        Success = IsSucces
                    }
                }
            };
        }
    }
}
