using Google.Protobuf;
using ServerCore.Controllers;
using ServerCore.DB;
using ServerCore.Models.User;
using Uplay.Friends;

namespace ServerCore.DMX.Connections;

public static class FriendsTask
{
    public const string Name = "friends_service";
    private readonly static List<DmxSession> SessionsLoggedIn = [];
    public static Task<ByteString> RunConnection(DmxSession dmxSession, ByteString data)
    {
        var req = Upstream.Parser.ParseFrom(data);
        if (req == null)
            return CoreTask.ReturnEmptyByteString();
        if (req.Request == null)
            return CoreTask.ReturnEmptyByteString();
        if (req.Request.AcceptFriendshipReq != null)
            return AcceptFriendship(dmxSession, req.Request.RequestId, req.Request.AcceptFriendshipReq);
        if (req.Request.InitializeReq != null)
            return Initialize(dmxSession, req.Request.RequestId, req.Request.InitializeReq);
        return CoreTask.ReturnEmptyByteString();
    }

    public static Task<ByteString> AcceptFriendship(DmxSession dmxSession, uint ReqId, AcceptFriendshipReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                AcceptFriendshipRsp = new()
                {
                    Ok = false
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        var user = DBUser.Get<UserCommon>(dmxSession.UserId);
        var friend = DBUser.Get<UserFriend>(dmxSession.UserId, x => x.UserId.ToString() == req.User.AccountId);
        var friend_friend = DBUser.Get<UserFriend>(req.User.AccountId, x => x.UserId == dmxSession.UserId);
        var friend_user = DBUser.Get<UserCommon>(req.User.AccountId);
        if (user != null && friend != null && friend_friend != null && friend_user != null)
        {
            user.Friends.Add(Guid.Parse(req.User.AccountId));
            DBUser.Edit(user);
            friend_user.Friends.Add(Guid.Parse(req.User.AccountId));
            DBUser.Edit(friend_user);
            friend.Relation = (int)Relationship.Types.Relation.Friends;
            DBUser.Edit(friend);
            friend_friend.Relation = (int)Relationship.Types.Relation.Friends;
            DBUser.Edit(friend_friend);
            downstream.Response.AcceptFriendshipRsp.Ok = true;
        }
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> Initialize(DmxSession dmxSession, uint ReqId, InitializeReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                InitializeRsp = new()
                {
                    Success = false,
                    Relationship = { }
                }
            }
        };

        if (!JWTController.Validate(req.UbiTicket))
            return Task.FromResult(downstream.ToByteString());
        var activity = DBUser.Get<UserActivity>(dmxSession.UserId);
        if (activity == null)
        {
            DBUser.Add(activity = new UserActivity()
            {
                Status = (int)req.ActivityStatus,
                UserId = dmxSession.UserId
            });
        }
        else
        {
            activity.Status = (int)req.ActivityStatus;
            DBUser.Edit(activity);
        }
        var friends = DBUser.GetList<UserFriend>(dmxSession.UserId);
        if (friends == null)
        {
            SessionsLoggedIn.Add(dmxSession);
            return Task.FromResult(downstream.ToByteString());
        }

        foreach (var friend in friends)
        {
            downstream.Response.InitializeRsp.Relationship.Add(new Relationship()
            {
                Blacklisted = friend.IsBlacklisted,
                ChangeDate = "",
                Relation = (Relationship.Types.Relation)friend.Relation,
                Friend = new()
                {
                    AccountId = friend.UserId.ToString(),
                    IsFavorite = friend.IsFavorite,
                    NameOnPlatform = friend.Name,
                    Nickname = friend.Nickname
                }
            });
            Push send_update = new()
            {
                PushUpdatedStatus = new()
                {
                    IsInitialStatus = true,
                    UpdatesStatus = new()
                    {
                        ActivityStatus = req.ActivityStatus,
                        OnlineStatus = (Status.Types.OnlineStatus)activity.OnlineStatus,
                        User = new()
                        {
                            AccountId = dmxSession.UserId.ToString()
                        }
                    }
                }
            };
            uint userCon = Auth.GetConIdByUserAndName(friend.UserId, Name);
            DemuxController.SendToClient(x => x.UserId == friend.UserId, new Uplay.Demux.Downstream()
            { 
                Push = new()
                {
                    Data = new()
                    {
                        ConnectionId = userCon,
                        Data = send_update.ToByteString()
                    }
                }
            });
        }
        return Task.FromResult(downstream.ToByteString());
    }
}
