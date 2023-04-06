using Core.SQLite;
using Google.Protobuf;
using Newtonsoft.Json.Linq;
using Uplay.Demux;
using static Core.SQLite.CurrentLogged;

namespace Core.DemuxResponders
{
    public static class Demux
    {
        public static void DeleteClient(int ClientNumb)
        {
            if (Globals.IdToUser.TryGetValue(ClientNumb, out var Id))
            {
                Globals.IdToUser.Remove(ClientNumb);
                Globals.UserToId.Remove(Id);
                Delete(Id);
                UserDMX.Delete(Id);
            }
        }


        public static class PushRSP
        {
            public static Downstream Downstream = null;
            public static void Push(int ClientNumb, Push push)
            {
                File.AppendAllText($"logs/client_{ClientNumb}_push.log", push.ToString() + "\n");
                if (push?.Data != null) { PushData(ClientNumb, push.Data); }
                if (push?.ProductEnded != null) { ProductEnd(ClientNumb, push.ProductEnded); }
                if (push?.ProductStarted != null) { ProductStart(ClientNumb, push.ProductStarted); }
                if (push?.KeepAlive != null) { KeepAlive(ClientNumb); }
                if (push?.ClientVersion != null) { ClientVersion(ClientNumb, push.ClientVersion); }
            }

            public static void PushData(int ClientNumb, DataMessage data)
            {
                bool SendBackEndPush = false;
                ByteString rspBytes = ByteString.Empty;
                if (Globals.IdToUser.TryGetValue(ClientNumb, out string id))
                {
                    var ConnectName = UserDMX.GetConNameByUserAndId(id, data.ConnectionId);
                    if (Globals.Connections.Contains(ConnectName))
                    {
                        switch (ConnectName)
                        {
                            case "download_service":
                                Download.Up.UpstreamConverter(ClientNumb, data.Data);
                                File.AppendAllText($"logs/client_{ClientNumb}_download_rsp.log", Download.Up.Downstream.ToString() + "\n");
                                rspBytes = Download.Up.Downstream.ToByteString();
                                break;
                            case "denuvo_service":
                                Denuvo.Up.UpstreamConverter(ClientNumb, data.Data);
                                File.AppendAllText($"logs/client_{ClientNumb}_denuvo_rsp.log", Denuvo.Up.Downstream.ToString() + "\n");
                                rspBytes = Denuvo.Up.Downstream.ToByteString();
                                break;
                            case "friends_service":
                                Friends.Up.UpstreamConverter(ClientNumb, data.Data);
                                File.AppendAllText($"logs/client_{ClientNumb}_friends_rsp.log", Friends.Up.Downstream.ToString() + "\n");
                                rspBytes = Friends.Up.Downstream.ToByteString();
                                break;
                            case "ownership_service":
                                Ownership.Up.UpstreamConverter(ClientNumb, data.Data);
                                File.AppendAllText($"logs/client_{ClientNumb}_ownership_rsp.log", Ownership.Up.Downstream.ToString() + "\n");
                                rspBytes = Ownership.Up.Downstream.ToByteString();
                                break;
                            case "party_service":
                                Party.Up.UpstreamConverter(ClientNumb, data.Data);
                                File.AppendAllText($"logs/client_{ClientNumb}_party_rsp.log", Party.Up.Downstream.ToString() + "\n");
                                rspBytes = Party.Up.Downstream.ToByteString();
                                break;
                            case "store_service":
                                Store.Up.UpstreamConverter(ClientNumb, data.Data);
                                File.AppendAllText($"logs/client_{ClientNumb}_store_rsp.log", Store.Up.Downstream.ToString() + "\n");
                                rspBytes = Store.Up.Downstream.ToByteString();
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
                                Data = ByteString.CopyFrom(Utils.FormatUpstream(rspBytes.ToArray()))
                            }
                        }
                    };
                }


            }

            public static void ProductEnd(int ClientNumb, ProductEndedPush productEnded)
            {
                Console.WriteLine(productEnded.ProductId);
            }

            public static void ProductStart(int ClientNumb, ProductStartedPush productStarted)
            {
                Console.WriteLine(productStarted.ProductId);
            }

            public static void KeepAlive(int ClientNumb)
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

            public static void ClientVersion(int ClientNumb, ClientVersionPush clientVersion)
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
            public static Downstream Downstream = null;
            public static uint ReqId = 0;
            public static void Requests(int ClientNumb, Req req)
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
                            PatchBaseUrl = Config.DMX.PatchBaseUrl
                        }
                    }
                };
            }

            public static void Authenticate(int ClientNumb, AuthenticateReq authenticateReq)
            {
                bool IsSuccess = false;
                bool IsBanned = false;
                bool IsExpired = false;

                // Our Custom Client should using UbiToken For easier
                if (authenticateReq.Token.HasUbiToken)
                {
                    //Todo Check Token Expire
                    string userId = GetUserIdByToken(authenticateReq.Token.UbiToken.ToStringUtf8(), (int)TokenType.Ticket);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        if (!Preparing.BannedUsers.Contains(userId))
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
                }

                if (authenticateReq.Token.HasOrbitToken)
                {
                    string userId = GetUserIdByToken(authenticateReq.Token.OrbitToken, (int)TokenType.Orbit);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        if (!Preparing.BannedUsers.Contains(userId))
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
                }

                if (authenticateReq.Token.HasUbiTicket)
                {
                    var job = JObject.Parse(jwt.GetUnkownJWTJson(authenticateReq.Token.UbiTicket));
                    var sid = job["sid"];
                    Console.WriteLine(sid);
                    var user_Id = UserToSession.GetUserIdBySessionId(sid.ToString());
                    if (!string.IsNullOrEmpty(user_Id))
                    {
                        IsSuccess = true;
                        Globals.IdToUser.Add(ClientNumb, user_Id);
                        Globals.UserToId.Add(user_Id, ClientNumb);
                    }
                    string userId = GetUserIdByToken(authenticateReq.Token.UbiTicket, (int)TokenType.Ticket);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        if (!Preparing.BannedUsers.Contains(userId))
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
                }

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

            public static void OpenConnection(int ClientNumb, OpenConnectionReq OpenConnection)
            {
                uint Id = 1;
                bool IsSucces = false;

                if (Globals.Connections.Contains(OpenConnection.ServiceName))
                {
                    if (Globals.IdToUser.TryGetValue(ClientNumb, out string id))
                    {

                        IsSucces = true;
                        uint LatestCon = UserDMX.GetLatestConIdByUser(id);
                        uint con = UserDMX.GetConIdByUserAndName(id, OpenConnection.ServiceName);

                        if (LatestCon == 0 && con == 0)
                        {
                            UserDMX.Add(id, Id, OpenConnection.ServiceName);
                        }
                        if (LatestCon != 0)
                        {
                            UserDMX.Add(id, LatestCon + 1, OpenConnection.ServiceName);
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

            public static void ServiceRequest(int ClientNumb, ServiceReq service)
            {
                bool IsSucces = false;
                ByteString returnData = ByteString.Empty;
                if (Globals.Services.Contains(service.Service))
                {
                    IsSucces = true;
                    Console.WriteLine(service.Service);
                    if (service.Service == Utility.Name)
                    {
                        Utility.Up.UpstreamConverter(ClientNumb, service.Data);
                        if (Utility.Up.Downstream != null)
                        {
                            returnData = ByteString.CopyFrom(Utility.Up.Downstream.ToByteArray());
                        }
                    }
                    if (service.Service == UplayDll.Name)
                    {
                        UplayDll.Up.UpstreamConverter(ClientNumb, service.Data);
                        if (UplayDll.Up.Rsp != null)
                        {
                            returnData = ByteString.CopyFrom(UplayDll.Up.Rsp.ToByteArray());
                        }
                    }
                    if (service.Service == ClientConfig.Name)
                    {
                        ClientConfig.Up.UpstreamConverter(service.Data);
                        if (ClientConfig.Up.Downstream != null)
                        {
                            returnData = ByteString.CopyFrom(ClientConfig.Up.Downstream.ToByteArray());
                        }
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
}
