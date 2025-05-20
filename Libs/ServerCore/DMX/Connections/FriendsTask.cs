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
        if (req.Request.AddToBlacklistReq != null)
            return AddToBlacklist(dmxSession, req.Request.RequestId, req.Request.AddToBlacklistReq);
        if (req.Request.ClearRelationshipReq != null)
            return ClearRelationship(dmxSession, req.Request.RequestId, req.Request.ClearRelationshipReq);
        if (req.Request.DeclineFriendshipReq != null)
            return DeclineFriendship(dmxSession, req.Request.RequestId, req.Request.DeclineFriendshipReq);
        if (req.Request.RemoveFromBlacklistReq != null)
            return RemoveFromBlacklist(dmxSession, req.Request.RequestId, req.Request.RemoveFromBlacklistReq);
        if (req.Request.FindFriendReq != null)
            return FindFriend(dmxSession, req.Request.RequestId, req.Request.FindFriendReq);
        if (req.Request.SetGameReq != null)
            return SetGame(dmxSession, req.Request.RequestId, req.Request.SetGameReq);
        if (req.Request.JoinGameInvitationReq != null)
            return JoinGameInvitation(dmxSession, req.Request.RequestId, req.Request.JoinGameInvitationReq);
        if (req.Request.RequestFriendshipsReq != null)
            return RequestFriendships(dmxSession, req.Request.RequestId, req.Request.RequestFriendshipsReq);
        if (req.Request.SetActivityStatusReq != null)
            return SetActivityStatus(dmxSession, req.Request.RequestId, req.Request.SetActivityStatusReq);
        if (req.Request.SetRichPresenceReq != null)
            return SetRichPresence(dmxSession, req.Request.RequestId, req.Request.SetRichPresenceReq);
        if (req.Request.DeclineGameInviteReq != null)
            return DeclineGameInvite(dmxSession, req.Request.RequestId, req.Request.DeclineGameInviteReq);
        if (req.Request.UbiTicketRefreshReq != null)
            return UbiTicketRefresh(dmxSession, req.Request.RequestId, req.Request.UbiTicketRefreshReq);
        if (req.Request.InitializeReq != null)
            return Initialize(dmxSession, req.Request.RequestId, req.Request.InitializeReq);
        if (req.Request.SetNicknameReq != null)
            return SetNickname(dmxSession, req.Request.RequestId, req.Request.SetNicknameReq);
        if (req.Request.GetBlacklistReq != null)
            return GetBlacklist(dmxSession, req.Request.RequestId, req.Request.GetBlacklistReq);
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
        Guid friend_id = Guid.Parse(req.User.AccountId);
        if (user != null && friend != null && friend_friend != null && friend_user != null)
        {
            user.Friends.Add(friend_id);
            DBUser.Edit(user);
            friend_user.Friends.Add(friend_id);
            DBUser.Edit(friend_user);
            friend.Relation = (int)Relationship.Types.Relation.Friends;
            DBUser.Edit(friend);
            friend_friend.Relation = (int)Relationship.Types.Relation.Friends;
            DBUser.Edit(friend_friend);
            PushToFriend(friend_id, new()
            {
                PushUpdatedRelationship = new()
                {
                    Relationship = new()
                    {
                        Friend = new()
                        {
                            AccountId = dmxSession.UserId.ToString()
                        },
                        Relation = Relationship.Types.Relation.Friends
                    }
                }
            });
            downstream.Response.AcceptFriendshipRsp.Ok = true;
        }
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> AddToBlacklist(DmxSession dmxSession, uint ReqId, AddToBlacklistReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                AddToBlacklistRsp = new()
                {
                    Ok = false
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        var user = DBUser.Get<UserCommon>(dmxSession.UserId);
        var friend = DBUser.Get<UserFriend>(dmxSession.UserId, x => x.UserId.ToString() == req.User.AccountId);
        if (user != null && friend != null)
        {
            friend.IsBlacklisted = true;
            DBUser.Edit(friend);
            downstream.Response.AddToBlacklistRsp.Ok = true;
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> ClearRelationship(DmxSession dmxSession, uint ReqId, ClearRelationshipReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                ClearRelationshipRsp = new()
                {
                    Ok = false
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        var friendId = Guid.Parse(req.User.AccountId);

        bool UserFromFriend = DBUserExt.RemoveFromFriends(dmxSession.UserId, friendId);
        bool FriendFromUser = DBUserExt.RemoveFromFriends(friendId, dmxSession.UserId);

        if (UserFromFriend && FriendFromUser)
        {
            PushToFriend(friendId, new()
            {
                PushUpdatedRelationship = new()
                {
                    Relationship = new()
                    {
                        Friend = new()
                        {
                            AccountId = dmxSession.UserId.ToString()
                        },
                        Relation = Relationship.Types.Relation.NoRelationship
                    }
                }
            });
            downstream.Response.ClearRelationshipRsp.Ok = true;
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> DeclineFriendship(DmxSession dmxSession, uint ReqId, DeclineFriendshipReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                DeclineFriendshipRsp = new()
                {
                    Ok = false
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        var friendId = Guid.Parse(req.User.AccountId);

        bool UserFromFriend = DBUserExt.RemoveFromFriends(dmxSession.UserId, friendId);
        bool FriendFromUser = DBUserExt.RemoveFromFriends(friendId, dmxSession.UserId);

        if (UserFromFriend && FriendFromUser)
        {
            PushToFriend(friendId, new()
            {
                PushUpdatedRelationship = new()
                {
                    Relationship = new()
                    {
                        Friend = new()
                        {
                            AccountId = dmxSession.UserId.ToString()
                        },
                        Relation = Relationship.Types.Relation.NoRelationship
                    }
                }
            });
            downstream.Response.DeclineFriendshipRsp.Ok = true;
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> RemoveFromBlacklist(DmxSession dmxSession, uint ReqId, RemoveFromBlacklistReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                RemoveFromBlacklistRsp = new()
                {
                    Ok = false
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        var user = DBUser.Get<UserCommon>(dmxSession.UserId);
        var friend = DBUser.Get<UserFriend>(dmxSession.UserId, x => x.UserId.ToString() == req.User.AccountId);
        if (user != null && friend != null)
        {
            friend.IsBlacklisted = false;
            DBUser.Edit(friend);
            downstream.Response.AddToBlacklistRsp.Ok = true;
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> FindFriend(DmxSession dmxSession, uint ReqId, FindFriendReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                FindFriendRsp = new()
                {
                    Ok = false,
                    Alternatives = { }
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        var user_friends = DBUser.GetList<UserFriend>(dmxSession.UserId);
        if (user_friends != null)
        {
            foreach (var friend in user_friends)
            {
                //  should not be done like friend.Nickname.Contains(FindFriend.SearchQueryUsername)?
                if (req.HasSearchQueryUsername && ((friend.Name != null && req.SearchQueryUsername.Contains(friend.Name)) || (friend.Nickname != null && req.SearchQueryUsername.Contains(friend.Nickname))))
                {
                    downstream.Response.FindFriendRsp.Alternatives.Add(new Friend()
                    {
                        AccountId = friend.UserId.ToString(),
                        IsFavorite = friend.IsFavorite,
                        NameOnPlatform = friend.Name,
                        Nickname = friend.Nickname
                    });
                }

                // TODO: do email search?
                /*
                if (req.HasEmail)
                {
                    downstream.Response.FindFriendRsp.Alternatives.Add(new Friend()
                    {
                        AccountId = friend.UserId.ToString(),
                        IsFavorite = friend.IsFavorite,
                        NameOnPlatform = friend.Name,
                        Nickname = friend.Nickname
                    });
                }
                */
            }
            downstream.Response.FindFriendRsp.Ok = true;
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> SetGame(DmxSession dmxSession, uint ReqId, SetGameReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                SetGameRsp = new()
                {
                    Success = false
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        var activity = DBUser.Get<UserActivity>(dmxSession.UserId);
        if (activity != null)
        {
            DBUserExt.UplayFriendsGameParseToUser(dmxSession.UserId, req.Game);
            PushToFriendsUser(dmxSession.UserId, new Push()
            {
                PushUpdatedStatus = new()
                {
                    IsInitialStatus = false,
                    UpdatesStatus = new()
                    {

                        ActivityStatus = (Status.Types.ActivityStatus)activity.Status,
                        OnlineStatus = (Status.Types.OnlineStatus)activity.OnlineStatus,
                        User = new()
                        {
                            AccountId = dmxSession.UserId.ToString()
                        },
                        Game = req.Game
                    }
                }
            });
            downstream.Response.SetGameRsp.Success = true;
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> JoinGameInvitation(DmxSession dmxSession, uint ReqId, JoinGameInvitationReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                JoinGameInvitationRsp = new()
                {
                    Ok = false,
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        DBUserExt.UplayFriendsGameParseToUser(dmxSession.UserId, req.Game);

        if (DBUserExt.IsUserExist(req.AccountIdTo))
        {
            PushToFriend(Guid.Parse(req.AccountIdTo), new()
            {
                PushJoinGameInvitation = new()
                {
                    Invite = new()
                    {
                        AccountIdFrom = dmxSession.UserId.ToString(),
                        Game = req.Game
                    }
                }
            });
            downstream.Response.JoinGameInvitationRsp.Ok = true;
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> RequestFriendships(DmxSession dmxSession, uint ReqId, RequestFriendshipsReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                RequestFriendshipsRsp = new()
                {
                    Ok = { },
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());
        foreach (var fuser in req.Users)
        {
            var fuser_guid = Guid.Parse(fuser.AccountId);
            var friend = DBUser.Get<UserFriend>(dmxSession.UserId, x => x.UserId == fuser_guid);
            if (friend == null)
            {
                downstream.Response.RequestFriendshipsRsp.Ok.Add(false);
                continue;
            }
            DBUser.Add(new UserFriend()
            {
                IdOfFriend = dmxSession.UserId,
                Relation = 1,
                UserId = fuser_guid
            });
            DBUser.Add(new UserFriend()
            {
                IdOfFriend = fuser_guid,
                Relation = 2,
                UserId = dmxSession.UserId
            });
            PushToFriend(fuser_guid, new()
            {
                PushUpdatedRelationship = new()
                {
                    Relationship = new()
                    {
                        Friend = new()
                        {
                            AccountId = dmxSession.UserId.ToString()
                        },
                        Relation = Relationship.Types.Relation.PendingReceivedInvite
                    }
                }
            });
            downstream.Response.RequestFriendshipsRsp.Ok.Add(true);
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> SetActivityStatus(DmxSession dmxSession, uint ReqId, SetActivityStatusReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                SetActivityStatusRsp = new()
                {
                    Success = false
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        var activity = DBUser.Get<UserActivity>(dmxSession.UserId);

        if (activity != null)
        {
            activity.Status = (int)req.ActivityStatus;
            DBUser.Edit(activity);
            PushToFriendsUser(dmxSession.UserId, new Push()
            {
                PushUpdatedStatus = new()
                {
                    IsInitialStatus = false,
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
            });
            downstream.Response.SetActivityStatusRsp.Success = true;
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> SetRichPresence(DmxSession dmxSession, uint ReqId, SetRichPresenceReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                SetRichPresenceRsp = new()
                {
                    Success = false
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        var activity = DBUser.Get<UserActivity>(dmxSession.UserId);
        var config = App.GetAppConfig(req.PresenceState.ProductId);
        if (activity != null && config != null && req.PresenceState.PresenceTokens.Count == 1 && config.RichPresence.Available_KeyValues.Contains(req.PresenceState.PresenceTokens[0].Key))
        {
            activity.IsPlaying = true;
            activity.GameId = req.PresenceState.ProductId;
            activity.Key = req.PresenceState.PresenceTokens[0].Key;
            activity.Value = req.PresenceState.PresenceTokens[0].Val;
            DBUser.Edit(activity);
            PushToFriendsUser(dmxSession.UserId, new Push()
            {
                PushUpdatedStatus = new()
                {
                    IsInitialStatus = false,
                    UpdatesStatus = new()
                    {
                        ActivityStatus = (Status.Types.ActivityStatus)activity.Status,
                        OnlineStatus = (Status.Types.OnlineStatus)activity.OnlineStatus,
                        User = new()
                        {
                            AccountId = dmxSession.UserId.ToString()
                        }
                    }
                }
            });
            downstream.Response.SetActivityStatusRsp.Success = true;
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> DeclineGameInvite(DmxSession dmxSession, uint ReqId, DeclineGameInviteReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                DeclineGameInviteRsp = new()
                {
                    Success = false,
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        if (DBUserExt.IsUserExist(req.AccountId))
        {
            PushToFriend(Guid.Parse(req.AccountId), new()
            {
                PushGameInviteDeclined = new()
                {
                    AccountId = req.AccountId
                }
            });
            downstream.Response.DeclineGameInviteRsp.Success = true;
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> UbiTicketRefresh(DmxSession dmxSession, uint ReqId, UbiTicketRefreshReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                UbiTicketRefreshRsp = new()
                {
                    Success = false,
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());
        if (JWTController.Validate(req.UbiTicket))
            downstream.Response.UbiTicketRefreshRsp.Success = true;

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
            Downstream send_update = new()
            {
                Push = new()
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

    public static Task<ByteString> SetNickname(DmxSession dmxSession, uint ReqId, SetNicknameReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                SetNicknameRsp = new()
                {
                    Success = false,
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        var friend = DBUser.Get<UserFriend>(dmxSession.UserId, x => x.UserId.ToString() == req.AccountId);
        if (friend != null)
        {
            friend.Nickname = req.Nickname;
            DBUser.Edit(friend);
            downstream.Response.SetNicknameRsp.Success = true;
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> GetBlacklist(DmxSession dmxSession, uint ReqId, GetBlacklistReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                GetBlacklistRsp = new()
                {
                    Success = false,
                    Blacklist = { }
                }
            }
        };
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());

        var friends = DBUser.GetList<UserFriend>(dmxSession.UserId);
        if (friends != null)
        {
            if (req.HasUser)
            {
                var friend = friends.Find(x => x.UserId.ToString() == req.User);
                if (friend != null && friend.IsBlacklisted)
                    downstream.Response.GetBlacklistRsp.Blacklist.Add(friend.UserId.ToString());
            }
            else
            {
                var frBlacklist = friends.Where(x => x.IsBlacklisted == true).ToList();
                foreach (var fr in frBlacklist)
                {
                    downstream.Response.GetBlacklistRsp.Blacklist.Add(fr.UserId.ToString());
                }
            }
            downstream.Response.GetBlacklistRsp.Success = true;
        }

        return Task.FromResult(downstream.ToByteString());
    }

    public static void PushToFriendsUser(Guid BaseUserId, Push push)
    {
        var user = DBUser.Get<UserCommon>(BaseUserId);
        if (user == null)
            return;
        foreach (var friend in user.Friends)
        {
            PushToFriend(friend, push);
        }
    }

    public static void PushToFriend(Guid friend, Push push)
    {
        Downstream send_update = new()
        {
            Push = push
        };
        uint userCon = Auth.GetConIdByUserAndName(friend, Name);
        DemuxController.SendToClient(x => x.UserId == friend, new Uplay.Demux.Downstream()
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
}
