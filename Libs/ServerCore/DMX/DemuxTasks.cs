using Google.Protobuf;
using Newtonsoft.Json.Linq;
using Serilog;
using ServerCore.DB;
using ServerCore.DemuxResponders;
using ServerCore.DMX.Connections;
using ServerCore.DMX.Services;
using ServerCore.Models;
using Uplay.Demux;

namespace ServerCore.DMX;

public static class DemuxTasks
{
    public static List<uint> AcceptVersions =
    [
        uint.MinValue, 11194, 11646
    ];

    private readonly static Dictionary<string /* ServiceName */, Func<DmxSession /* dmxSession */, ByteString /* RequestData */,Task<ByteString> /* Result */ > /* ServiceRunner */ > ServiceToRunner = new()
    {
        { "utility_service", UtilityServiceTask.RunService },
        { "client_configuration_service", ClientConfigurationServiceTask.RunService },
        // steam?
        // uplaydll?
        // uplayauxdll?
    };

    private readonly static Dictionary<string /* ConnectionName */, Func<DmxSession /* dmxSession */, ByteString /* RequestData */, Task<ByteString> /* Result */ > /* ConnectionRunner */ > ConnectionRunner = new()
    {
        { "playtime_service", PlaytimeTask.RunConnection },
        { "ownership_service", OwnershipTask.RunConnection },
        { "download_service", DownloadTask.RunConnection },
        { "ach_frontend", AchievementTask.RunConnection }, // waiting for actual implementation
        { "cloudsave_service", CloudSaveTask.RunConnection },
        { "friends_service", FriendsTask.RunConnection }, // waiting for actual implementation
        { "store_service", StoreTask.RunConnection },
        { "party_service", PartyTask.RunConnection }, // waiting for actual implementation
        /*
        { "denuvo_service", DownloadTask.RunConnection }, // idk how to yet. probably just return a stored token, so no generation.
        */
    };


    public static Task<Downstream?> Empty(uint reqId, DmxSession dmxSession, Req req)
    {
        return Task.FromResult<Downstream?>(new()
        {
            Response = new()
            {
                RequestId = reqId,
            }
        });
    }
    #region Requests
    public static Task<Downstream?> GetPatchInfo(uint reqId, DmxSession dmxSession, GetPatchInfoReq req)
    {
        return Task.FromResult<Downstream?>(new()
        {
            Response = new()
            {
                RequestId = reqId,
                GetPatchInfoRsp = new()
                {
                    TrackType = req.TrackType,
                    TestConfig = req.TestConfig,
                    Success = true,
                    LatestVersion = AcceptVersions.Last(),
                    PatchTrackId = req.PatchTrackId,
                    PatchBaseUrl = ServerConfig.Instance.HTTPS_Url + "/patch/"
                }
            }
        });
    }

    private static Task<Downstream?> ReturnFailedAuth(uint reqId, bool IsBanned, bool IsExpired)
    {
        return Task.FromResult<Downstream?>(new()
        {
            Response = new()
            {
                RequestId = reqId,
                AuthenticateRsp = new()
                {
                    Expired = IsExpired,
                    Banned = IsBanned,
                    Success = false,
                }
            }
        });
    }

    public static Task<Downstream?> Authenticate(uint reqId, DmxSession dmxSession, AuthenticateReq authenticateReq)
    {
        Guid UserId = Guid.Empty;

        // Our Custom Client should using UbiToken For easier
        if (authenticateReq.Token.HasUbiToken)
        {
            //Todo Check Token Expire
            UserId = Auth.GetUserIdByToken(authenticateReq.Token.UbiToken.ToStringUtf8(), TokenType.Ticket);
        }

        if (authenticateReq.Token.HasOrbitToken)
        {
            UserId = Auth.GetUserIdByToken(authenticateReq.Token.OrbitToken, TokenType.Orbit);
        }

        if (authenticateReq.Token.HasUbiTicket)
        {
            var job = JObject.Parse(JWTController.GetUnkownJWTJson(authenticateReq.Token.UbiTicket));
            var sid = job["sid"];
            Console.WriteLine(sid);
            if (sid == null)
                return ReturnFailedAuth(reqId, false, false);
            UserId = Auth.GetUserIdBySessionId(Guid.Parse(sid.ToString()));
            if (UserId == Guid.Empty)
            {
                UserId = Auth.GetUserIdByToken(authenticateReq.Token.UbiTicket, TokenType.AuthToken);
            }
        }

        if (DBUserExt.IsUserBanned(UserId))
        {
            return ReturnFailedAuth(reqId, true, false);
        }
        else
        {
            dmxSession.UserId = UserId;
        }

        return Task.FromResult<Downstream?>(new()
        {
            Response = new()
            {
                RequestId = reqId,
                AuthenticateRsp = new()
                {
                    Expired = false,
                    Banned = false,
                    Success = true,
                }
            }
        });
    }

    public static Task<Downstream?> OpenConnection(uint reqId, DmxSession dmxSession, OpenConnectionReq openConnection)
    {
        uint Id = 0;
        bool IsSucces = false;

        if (ConnectionRunner.ContainsKey(openConnection.ServiceName) && dmxSession.IsLoggedIn)
        {
            IsSucces = true;
            uint LatestCon = Auth.GetLatestConIdByUser(dmxSession.UserId);
            uint con = Auth.GetConIdByUserAndName(dmxSession.UserId, openConnection.ServiceName);

            if (LatestCon == uint.MaxValue && con == uint.MaxValue)
            {
                Auth.AddDMX(dmxSession.UserId, Id, openConnection.ServiceName);
            }
            if (LatestCon != 0)
            {
                Auth.AddDMX(dmxSession.UserId, LatestCon + 1, openConnection.ServiceName);
                Id = LatestCon + 1;
            }
        }

        return Task.FromResult<Downstream?>(new()
        {
            Response = new()
            {
                RequestId = reqId,
                OpenConnectionRsp = new()
                {
                    ConnectionId = Id,
                    Success = IsSucces
                }
            }
        });
    }

    public static Task<Downstream?> Service(uint reqId, DmxSession dmxSession, ServiceReq req)
    {
        bool IsSucces = false;
        ByteString returnData = ByteString.Empty;
        if (ServiceToRunner.TryGetValue(req.Service, out var func) && dmxSession.IsLoggedIn)
        {
            returnData = func(dmxSession, req.Data).Result;
            if (returnData != ByteString.Empty)
                IsSucces = true;
        }
        return Task.FromResult<Downstream?>(new()
        {
            Response = new()
            {
                RequestId = reqId,
                ServiceRsp = new()
                {
                    Success = IsSucces,
                    Data = returnData
                }
            }
        });
    }
    #endregion
    #region Push
    public static Task<Downstream?> ClientVersion(DmxSession dmxSession, ClientVersionPush versionPush)
    {
        Log.Information("{Session} has been using version: {Version}", dmxSession, versionPush.Version);
        if (!AcceptVersions.Contains(versionPush.Version))
        {
            return Task.FromResult<Downstream?>(new()
            {
                Push = new()
                {
                    ClientOutdated = new()
                }
            });
        }
        return CoreTask.ReturnDownstream();
    }

    public static Task<Downstream?> SendClose(DmxSession dmxSession, uint connectionId, ConnectionClosedPush.Types.Connection_ErrorCode errorCode = ConnectionClosedPush.Types.Connection_ErrorCode.ConnectionForceQuit)
    {
        return Task.FromResult<Downstream?>(new()
        {
            Push = new()
            {
                ConnectionClosed = new()
                {
                    ConnectionId = connectionId,
                    ErrorCode = errorCode
                }
            }
        });
    }

    public static Task<Downstream?> PushMessage(DmxSession dmxSession, DataMessage dataMessage)
    {
        ByteString returnData = ByteString.Empty;
        if (dmxSession.IsLoggedIn)
        {
            var ConnectName = Auth.GetConNameByUserAndId(dmxSession.UserId, dataMessage.ConnectionId);
            if (!ConnectionRunner.TryGetValue(ConnectName, out var connectionRunner))
                return SendClose(dmxSession, dataMessage.ConnectionId);
            returnData = connectionRunner(dmxSession, dataMessage.Data).Result;
        }
        return Task.FromResult<Downstream?>(new()
        {
            Push = new()
            {
                Data = new()
                {
                    ConnectionId = dataMessage.ConnectionId,
                    Data = returnData
                }
            }
        });
    }
    #endregion
}
