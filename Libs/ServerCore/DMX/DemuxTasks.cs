using Newtonsoft.Json.Linq;
using Serilog;
using ServerCore.DB;
using ServerCore.DemuxResponders;
using ServerCore.Models;
using System.Runtime.CompilerServices;
using Uplay.Demux;

namespace ServerCore.DMX;

public static class DemuxTasks
{
    public static Task<Downstream?> Empty(uint reqId, Guid id, Req req)
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
    public static Task<Downstream?> GetPatchInfo(uint reqId, Guid id, GetPatchInfoReq req)
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
                    LatestVersion = Globals.AcceptVersions.Last(),
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

    public static Task<Downstream?> Authenticate(uint reqId, Guid id, AuthenticateReq authenticateReq)
    {
        bool IsSuccess = false;
        bool IsBanned = false;
        bool IsExpired = false;
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
                return ReturnFailedAuth(reqId, IsBanned, IsExpired);
            UserId = Auth.GetUserIdBySessionId(Guid.Parse(sid.ToString()));
            if (UserId == Guid.Empty)
            {
                UserId = Auth.GetUserIdByToken(authenticateReq.Token.UbiTicket, TokenType.AuthToken);
            }
        }

        if (DBUserExt.IsUserBanned(UserId))
        {
            return ReturnFailedAuth(reqId, true, IsExpired);
        }
        else
        {
            Globals.IdToUser.Add(id, UserId);
            Globals.UserToId.Add(UserId, id);
            IsSuccess = true;
        }

        return Task.FromResult<Downstream?>(new()
        {
            Response = new()
            {
                RequestId = reqId,
                AuthenticateRsp = new()
                {
                    Expired = IsExpired,
                    Banned = IsBanned,
                    Success = IsSuccess,
                }
            }
        });
    }

    public static Task<Downstream?> OpenConnection(uint reqId, Guid demux_id, OpenConnectionReq openConnection)
    {
        uint Id = 0;
        bool IsSucces = false;

        if (Globals.Connections.Contains(openConnection.ServiceName) && Globals.IdToUser.TryGetValue(demux_id, out var user_id))
        {
            IsSucces = true;
            uint LatestCon = Auth.GetLatestConIdByUser(user_id);
            uint con = Auth.GetConIdByUserAndName(user_id, openConnection.ServiceName);

            if (LatestCon == uint.MaxValue && con == uint.MaxValue)
            {
                Auth.AddDMX(user_id, Id, openConnection.ServiceName);
            }
            if (LatestCon != 0)
            {
                Auth.AddDMX(user_id, LatestCon + 1, openConnection.ServiceName);
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
    #endregion
    #region Push
    public static Task<Downstream?> ClientVersion(Guid id, ClientVersionPush versionPush)
    {
        Log.Information("{Id} has been using version: {Version}", id, versionPush.Version);
        if (!Globals.AcceptVersions.Contains(versionPush.Version))
        {
            return Task.FromResult<Downstream?>(new()
            {
                Push = new()
                {

                    ClientOutdated = new()
                }
            });
        }
        return Task.FromResult<Downstream?>(null);
    }
    #endregion
}
