using Google.Protobuf;
using ServerCore.DB;
using ServerCore.Models.User;
using Uplay.Friends;

namespace ServerCore.DemuxResponders;

public class Friends
{
    public const string Name = "friends_service";
    public class Up
    {
        public static Downstream? Downstream = null;
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
        public static Downstream? Downstream = null;
        public static uint ReqId = 0;
        public static bool IsIdDone = false;
        public static Dictionary<Guid, bool> UserInits = new();

        public static void Requests(Guid ClientNumb, Req req)
        {
            File.AppendAllText($"logs/client_{ClientNumb}_friends_req.log", req.ToString() + "\n");
            //All Deprecated func should return false or emtpy data
            ReqId = req.RequestId;
            //Deprecated func
            if (req?.DeprecatedInitializeReq != null) { Deprecated_Init(req.DeprecatedInitializeReq); }
            if (req?.DeprecatedGetRelationshipsListReq != null) { Deprecated_FriendShipList(req.DeprecatedGetRelationshipsListReq); }
            if (req?.DeprecatedRequestFriendshipReq != null) { Deprecated_RequestFriendships(req.DeprecatedRequestFriendshipReq); }
            if (req?.SetRichPresenceReqDeprecated != null) { Deprecated_SetRichPresence(req.SetRichPresenceReqDeprecated); }
            if (req?.DeprecatedGetStatusReq != null) { Deprecated_GetStatus(req.DeprecatedGetStatusReq); }
            if (req?.GetNickReq != null) { Deprecated_GetNick(req.GetNickReq); }
            //normal func
            if (req?.AcceptFriendshipReq != null) { AcceptFriendship(ClientNumb, req.AcceptFriendshipReq); }
            if (req?.AddToBlacklistReq != null) { AddToBlacklist(ClientNumb, req.AddToBlacklistReq); }
            if (req?.ClearRelationshipReq != null) { ClearRelationship(ClientNumb, req.ClearRelationshipReq); }
            if (req?.DeclineFriendshipReq != null) { DeclineFriendship(ClientNumb, req.DeclineFriendshipReq); }
            if (req?.RemoveFromBlacklistReq != null) { RemoveFromBlacklist(ClientNumb, req.RemoveFromBlacklistReq); }
            if (req?.FindFriendReq != null) { FindFriend(ClientNumb, req.FindFriendReq); }
            if (req?.SetGameReq != null) { SetGame(ClientNumb, req.SetGameReq); }
            if (req?.JoinGameInvitationReq != null) { JoinGameInvitation(ClientNumb, req.JoinGameInvitationReq); }
            if (req?.RequestFriendshipsReq != null) { RequestFriendships(ClientNumb, req.RequestFriendshipsReq); }
            if (req?.SetActivityStatusReq != null) { SetActivityStatus(ClientNumb, req.SetActivityStatusReq); }
            if (req?.SetRichPresenceReq != null) { SetRichPresence(ClientNumb, req.SetRichPresenceReq); }
            if (req?.DeclineGameInviteReq != null) { DeclineGameInvite(ClientNumb, req.DeclineGameInviteReq); }
            if (req?.UbiTicketRefreshReq != null) { UbiTicketRefresh(ClientNumb, req.UbiTicketRefreshReq); }
            if (req?.InitializeReq != null) { Initialize(ClientNumb, req.InitializeReq); }
            if (req?.SetNicknameReq != null) { SetNickname(ClientNumb, req.SetNicknameReq); }
            if (req?.GetBlacklistReq != null) { GetBlacklist(ClientNumb, req.GetBlacklistReq); }
            IsIdDone = true;
        }
        #region Depr
        public static void Deprecated_Init(DeprecatedInitializeReq deprecatedInitialize)
        {
            Utils.WriteFile(deprecatedInitialize.ToString(), "Deprecated.txt");
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    DeprecatedInitializeRsp = new()
                    {
                        Success = false
                    }
                }
            };
        }

        public static void Deprecated_FriendShipList(GetRelationshipsListReq getRelationshipsList)
        {
            Utils.WriteFile(getRelationshipsList.ToString(), "Deprecated.txt");
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    DeprecatedGetRelationshipsListRsp = new()
                    {
                        Ok = false
                    }
                }
            };
        }

        public static void Deprecated_GetNick(DeprecatedGetNickReq GetNick)
        {
            Utils.WriteFile(GetNick.ToString(), "Deprecated.txt");
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    GetNickRsp = new()
                    {
                        Ok = false
                    }
                }
            };
        }

        public static void Deprecated_RequestFriendships(RequestFriendshipReq RequestFriendhips)
        {
            Utils.WriteFile(RequestFriendhips.ToString(), "Deprecated.txt");
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    DeprecatedRequestFriendshipRsp = new()
                    {
                        Ok = false
                    }
                }
            };
        }

        public static void Deprecated_SetRichPresence(SetRichPresenceReqDeprecated setRichPresenceReqDeprecated)
        {
            Utils.WriteFile(setRichPresenceReqDeprecated.ToString(), "Deprecated.txt");
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    SetRichPresenceRspDeprected = new()
                    {
                        Success = false
                    }
                }
            };
        }

        public static void Deprecated_GetStatus(GetStatusReq DeprecatedGetStatusReq)
        {
            Utils.WriteFile(DeprecatedGetStatusReq.ToString(), "Deprecated.txt");
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    DeprecatedGetStatusRsp = new()
                    {
                        Ok = false
                    }
                }
            };
        }
        #endregion

        #region Normal Functions
        public static void AcceptFriendship(Guid ClientNumb, AcceptFriendshipReq acceptFriendship)
        {
            bool IsSuccess = false;
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var user = DBUser.Get<UserCommon>(userID);
                var friend = DBUser.Get<UserFriend>(userID, x=> x.UserId.ToString() == acceptFriendship.User.AccountId);
                var friend_friend = DBUser.Get<UserFriend>(acceptFriendship.User.AccountId, x => x.UserId == userID);
                var friend_user = DBUser.Get<UserCommon>(acceptFriendship.User.AccountId);
                if (user != null && friend != null && friend_friend != null && friend_user != null)
                {
                    user.Friends.Add(Guid.Parse(acceptFriendship.User.AccountId));
                    DBUser.Edit(user);
                    friend_user.Friends.Add(Guid.Parse(acceptFriendship.User.AccountId));
                    DBUser.Edit(friend_user);
                    friend.Relation = (int)Relationship.Types.Relation.Friends;
                    DBUser.Edit(friend);
                    friend_friend.Relation = (int)Relationship.Types.Relation.Friends;
                    DBUser.Edit(friend_friend);
                    IsSuccess = true;
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    AcceptFriendshipRsp = new()
                    {
                        Ok = IsSuccess
                    }
                }
            };
        }

        public static void AddToBlacklist(Guid ClientNumb, AddToBlacklistReq AddToBlacklist)
        {
            bool IsSuccess = false;
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var user = DBUser.Get<UserCommon>(userID);
                var friend = DBUser.Get<UserFriend>(userID, x => x.UserId.ToString() == AddToBlacklist.User.AccountId);
                if (user != null && friend != null)
                {
                    friend.IsBlacklisted = true;
                    DBUser.Edit(friend);
                    IsSuccess = true;
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    AddToBlacklistRsp = new()
                    {
                        Ok = IsSuccess
                    }
                }
            };
        }

        public static void RemoveFromBlacklist(Guid ClientNumb, RemoveFromBlacklistReq RemoveFromBlacklist)
        {
            bool IsSuccess = false;
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var user = DBUser.Get<UserCommon>(userID);
                var friend = DBUser.Get<UserFriend>(userID, x => x.UserId.ToString() == RemoveFromBlacklist.User.AccountId);
                if (user != null && friend != null)
                {
                    friend.IsBlacklisted = false;
                    DBUser.Edit(friend);
                    IsSuccess = true;
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    RemoveFromBlacklistRsp = new()
                    {
                        Ok = IsSuccess
                    }
                }
            };
        }

        public static void Initialize(Guid ClientNumb, InitializeReq initialize)
        {
            bool IsSuccess = false;
            List<Relationship> Relationships = new();
            if (JWTController.Validate(initialize.UbiTicket))
            {
                var userID = Globals.IdToUser[ClientNumb];

                var activity = DBUser.Get<UserActivity>(userID);
                if (activity == null)
                {
                    DBUser.Add(activity = new UserActivity()
                    {
                        Status = (int)initialize.ActivityStatus,
                        UserId = userID
                    });
                }
                else
                {
                    activity.Status = (int)initialize.ActivityStatus;
                    DBUser.Edit(activity);
                }
                var friends = DBUser.GetList<UserFriend>(userID);
                if (friends != null)
                {
                    foreach (var friend in friends)
                    {
                        Relationships.Add(new Relationship()
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
                        Pusher.PushToFriend(friend.UserId, new Push()
                        {
                            PushUpdatedStatus = new()
                            {
                                IsInitialStatus = true,
                                UpdatesStatus = new()
                                {
                                    ActivityStatus = initialize.ActivityStatus,
                                    OnlineStatus = (Status.Types.OnlineStatus)activity.OnlineStatus,
                                    User = new()
                                    {
                                        AccountId = userID.ToString()
                                    }
                                }
                            }
                        });
                    }
                
                }
                IsSuccess = true;
                UserInits[ClientNumb] = true;
            }
            else
            {
                IsSuccess = false;
                UserInits[ClientNumb] = false;
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    InitializeRsp = new()
                    {
                        Success = IsSuccess,
                        Relationship = { Relationships }
                    }
                }
            };
        }

        public static void FindFriend(Guid ClientNumb, FindFriendReq FindFriend)
        {
            bool IsSuccess = false;
            List<Friend> friends = new();
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var user_friends = DBUser.GetList<UserFriend>(userID);
                if (user_friends != null)
                {
                    foreach (var friend in user_friends)
                    {
                        //  should not be done like friend.Nickname.Contains(FindFriend.SearchQueryUsername)?
                        if (FindFriend.HasSearchQueryUsername && (FindFriend.SearchQueryUsername == friend.Name || FindFriend.SearchQueryUsername == friend.Nickname))
                        {
                            friends.Add(new Friend()
                            {
                                AccountId = friend.UserId.ToString(),
                                IsFavorite = friend.IsFavorite,
                                NameOnPlatform = friend.Name,
                                Nickname = friend.Nickname
                            });
                        }
                    }
                    IsSuccess = true;
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    FindFriendRsp = new()
                    {
                        Ok = IsSuccess,
                        Alternatives = { friends }
                    }
                }
            };
        }

        public static void SetActivityStatus(Guid ClientNumb, SetActivityStatusReq SetActivityStatus)
        {
            bool IsSuccess = false;
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var activity = DBUser.Get<UserActivity>(userID);

                if (activity != null)
                {
                    activity.Status = (int)SetActivityStatus.ActivityStatus;
                    DBUser.Edit(activity);
                    Pusher.PushToFriendsUser(userID, new Push()
                    {
                        PushUpdatedStatus = new()
                        {
                            IsInitialStatus = false,
                            UpdatesStatus = new()
                            {
                                ActivityStatus = SetActivityStatus.ActivityStatus,
                                OnlineStatus = (Status.Types.OnlineStatus)activity.OnlineStatus,
                                User = new()
                                {
                                    AccountId = userID.ToString()
                                }
                            }
                        }
                    });
                    IsSuccess = true;
                }
            }
            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    SetActivityStatusRsp = new()
                    {
                        Success = IsSuccess
                    }
                }
            };
        }

        public static void SetNickname(Guid ClientNumb, SetNicknameReq SetNickname)
        {
            bool IsSuccess = false;
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var friend = DBUser.Get<UserFriend>(userID, x=>x.UserId.ToString() == SetNickname.AccountId);
                if (friend != null)
                {
                    friend.Nickname = SetNickname.Nickname;
                    DBUser.Edit(friend);
                    IsSuccess = true;
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    SetNicknameRsp = new()
                    {
                        Success = IsSuccess
                    }
                }
            };
        }

        public static void SetRichPresence(Guid ClientNumb, SetRichPresenceReq SetRichPresence)
        {
            bool IsSuccess = false;
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var activity = DBUser.Get<UserActivity>(userID);

                if (activity != null)
                {
                    var config = App.GetAppConfig(SetRichPresence.PresenceState.ProductId);
                    if (config != null)
                    {
                        if (config.RichPresence.Available_KeyValues.Contains(SetRichPresence.PresenceState.PresenceTokens[0].Key))
                        {
                            activity.IsPlaying = true;
                            activity.GameId = SetRichPresence.PresenceState.ProductId;
                            activity.Key = SetRichPresence.PresenceState.PresenceTokens[0].Key;
                            activity.Value = SetRichPresence.PresenceState.PresenceTokens[0].Val;
                            DBUser.Edit(activity);
                            Pusher.PushToFriendsUser(userID, new Push()
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
                                            AccountId = userID.ToString()
                                        }
                                    }
                                }
                            });
                        }
                    }
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    SetRichPresenceRsp = new()
                    {
                        Success = IsSuccess
                    }
                }
            };
        }

        public static void SetGame(Guid ClientNumb, SetGameReq SetGame)
        {
            bool IsSuccess = false;
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var activity = DBUser.Get<UserActivity>(userID);

                if (activity != null)
                {
                    DBUserExt.UplayFriendsGameParseToUser(userID, SetGame.Game);
                    Pusher.PushToFriendsUser(userID, new Push()
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
                                    AccountId = userID.ToString()
                                },
                                Game = SetGame.Game
                            }
                        }
                    });
                    IsSuccess = true;
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    SetGameRsp = new()
                    {
                        Success = IsSuccess
                    }
                }
            };
        }

        public static void UbiTicketRefresh(Guid ClientNumb, UbiTicketRefreshReq UbiTicketRefresh)
        {
            bool IsSuccess = false;

            //  We are not storing tickets!
            if (JWTController.Validate(UbiTicketRefresh.UbiTicket))
                IsSuccess = true;

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    UbiTicketRefreshRsp = new()
                    {
                        Success = IsSuccess
                    }
                }
            };
        }

        public static void DeclineFriendship(Guid ClientNumb, DeclineFriendshipReq DeclineFriendship)
        {
            bool IsSuccess = false;
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];
                var friendId = Guid.Parse(DeclineFriendship.User.AccountId);

                bool UserFromFriend = DBUserExt.RemoveFromFriends(userID, friendId);
                bool FriendFromUser = DBUserExt.RemoveFromFriends(friendId, userID);

                if (UserFromFriend && FriendFromUser)
                {
                    Pusher.PushToFriend(friendId, new()
                    {
                        PushUpdatedRelationship = new()
                        {
                            Relationship = new()
                            {
                                Friend = new()
                                {
                                    AccountId = userID.ToString()
                                },
                                Relation = Relationship.Types.Relation.NoRelationship
                            }
                        }
                    });
                    IsSuccess = true;
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    DeclineFriendshipRsp = new()
                    {
                        Ok = IsSuccess
                    }
                }
            };
        }

        /// <summary>
        /// Sending a friend request for the accounts
        /// </summary>
        /// <param name="ClientNumb"></param>
        /// <param name="RequestFriendships"></param>
        public static void RequestFriendships(Guid ClientNumb, RequestFriendshipsReq RequestFriendships)
        {
            // This for be all false if something isnt go right 
            List<bool> successes = new();
            int userscount = RequestFriendships.Users.Count();
            for (int i = 0; i < userscount; i++)
            {
                successes[i] = false;
            }

            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];

                for (int i = 0; i < userscount; i++)
                {
                    var fuser = RequestFriendships.Users[i];
                    var fuser_guid = Guid.Parse(fuser.AccountId);
                    var friend = DBUser.Get<UserFriend>(userID, x=>x.UserId == fuser_guid);
                    if (friend == null)
                    {
                        DBUser.Add(new UserFriend()
                        { 
                            IdOfFriend = userID,
                            Relation = 1,
                            UserId = fuser_guid
                        });
                        DBUser.Add(new UserFriend()
                        {
                            IdOfFriend = fuser_guid,
                            Relation = 2,
                            UserId = userID
                        });
                        Pusher.PushToFriend(fuser_guid, new()
                        {
                            PushUpdatedRelationship = new()
                            {
                                Relationship = new()
                                {
                                    Friend = new()
                                    {
                                        AccountId = userID.ToString()
                                    },
                                    Relation = Relationship.Types.Relation.PendingReceivedInvite
                                }
                            }
                        });
                        successes[i] = true;
                    }
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    RequestFriendshipsRsp = new()
                    {
                        Ok = { successes }
                    }
                }
            };
        }

        public static void ClearRelationship(Guid ClientNumb, ClearRelationshipReq ClearRelationship)
        {
            bool IsSuccess = false;
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];

                var friendId = Guid.Parse(ClearRelationship.User.AccountId);

                bool UserFromFriend = DBUserExt.RemoveFromFriends(userID, friendId);
                bool FriendFromUser = DBUserExt.RemoveFromFriends(friendId, userID);

                if (UserFromFriend && FriendFromUser)
                {
                    Pusher.PushToFriend(friendId, new()
                    {
                        PushUpdatedRelationship = new()
                        {
                            Relationship = new()
                            {
                                Friend = new()
                                {
                                    AccountId = userID.ToString()
                                },
                                Relation = Relationship.Types.Relation.NoRelationship
                            }
                        }
                    });
                    IsSuccess = true;
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    ClearRelationshipRsp = new()
                    {
                        Ok = IsSuccess
                    }
                }
            };
        }

        public static void GetBlacklist(Guid ClientNumb, GetBlacklistReq GetBlacklist)
        {
            bool IsSuccess = false;
            List<string> blacklist = new();
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];

                var friends = DBUser.GetList<UserFriend>(userID);
                if (friends != null)
                {
                    if (GetBlacklist.HasUser)
                    {
                        var friend = friends.Find(x => x.UserId.ToString() == GetBlacklist.User);
                        if (friend != null && friend.IsBlacklisted)
                            blacklist.Add(friend.UserId.ToString());
                    }
                    else
                    {

                        var frBlacklist = friends.Where(x => x.IsBlacklisted == true).ToList();

                        foreach (var fr in frBlacklist)
                        {
                            blacklist.Add(fr.UserId.ToString());
                        }
                    }
                    IsSuccess = true;
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    RequestId = ReqId,
                    GetBlacklistRsp = new()
                    {
                        Success = IsSuccess,
                        Blacklist = { blacklist }
                    }
                }
            };
        }

        //TODO: Make this better because currently IDK what it does and why
        public static void JoinGameInvitation(Guid ClientNumb, JoinGameInvitationReq JoinGameInvitation)
        {
            bool IsSuccess = false;
            if (UserInits[ClientNumb])
            {
                var userID = Globals.IdToUser[ClientNumb];

                DBUserExt.UplayFriendsGameParseToUser(userID, JoinGameInvitation.Game);

                if (DBUserExt.IsUserExist(JoinGameInvitation.AccountIdTo))
                {
                    Pusher.PushToFriend(Guid.Parse(JoinGameInvitation.AccountIdTo), new()
                    {
                        PushJoinGameInvitation = new()
                        {
                            Invite = new()
                            {
                                AccountIdFrom = userID.ToString(),
                                Game = JoinGameInvitation.Game
                            }
                        }
                    });
                    IsSuccess = true;
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    JoinGameInvitationRsp = new()
                    {
                        Ok = IsSuccess
                    }
                }
            };
        }

        //TODO: Make this better because currently IDK what it does and why
        public static void DeclineGameInvite(Guid ClientNumb, DeclineGameInviteReq DeclineGameInvite)
        {
            bool IsSuccess = false;
            if (UserInits[ClientNumb])
            {
                if (DBUserExt.IsUserExist(DeclineGameInvite.AccountId))
                {
                    Pusher.PushToFriend(Guid.Parse(DeclineGameInvite.AccountId), new()
                    {
                        PushGameInviteDeclined = new()
                        {
                            AccountId = DeclineGameInvite.AccountId
                        }
                    });
                    IsSuccess = true;
                }
            }

            Downstream = new()
            {
                Response = new()
                {
                    DeclineGameInviteRsp = new()
                    {
                        Success = IsSuccess
                    }
                }
            };
        }
        #endregion
    }

    public class Pusher
    {
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
            if (Globals.UserToId.TryGetValue(friend, out var ClientNumb))
            {
                Pushes(ClientNumb, push);
            }
        }

        public static void Pushes(Guid ClientNumb, Push push)
        {
            if (push?.PushUpdatedRelationship != null) { PushUpdatedRelationship(ClientNumb, push.PushUpdatedRelationship); }
            if (push?.PushUpdatedStatus != null) { PushUpdatedStatus(ClientNumb, push.PushUpdatedStatus); }
            if (push?.PushJoinGameInvitation != null) { PushJoinGameInvitation(ClientNumb, push.PushJoinGameInvitation); }
            if (push?.PushRecentlyMetPlayers != null) { PushRecentlyMetPlayers(ClientNumb, push.PushRecentlyMetPlayers); }
            if (push?.PushGameInviteDeclined != null) { PushGameInviteDeclined(ClientNumb, push.PushGameInviteDeclined); }
            if (push?.PushNicknameUpdate != null) { PushNicknameUpdate(ClientNumb, push.PushNicknameUpdate); }
            if (push?.PushIsFavoriteUpdate != null) { PushIsFavoriteUpdate(ClientNumb, push.PushIsFavoriteUpdate); }
        }

        public static void PushUpdatedRelationship(Guid ClientNumb, PushUpdatedRelationship pushUpdatedRelationship)
        {
            if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
            {
                uint userCon = Auth.GetConIdByUserAndName(username, Name);
                var bstr = pushUpdatedRelationship.ToByteString();

                DemuxServer.SendToClient(ClientNumb, bstr, userCon);
            }
        }

        public static void PushUpdatedStatus(Guid ClientNumb, PushUpdatedStatus pushUpdatedStatus)
        {
            if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
            {
                uint userCon = Auth.GetConIdByUserAndName(username, Name);
                var bstr = pushUpdatedStatus.ToByteString();

                DemuxServer.SendToClient(ClientNumb, bstr, userCon);
            }
        }

        public static void PushJoinGameInvitation(Guid ClientNumb, PushJoinGameInvitation pushJoinGameInvitation)
        {
            if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
            {
                uint userCon = Auth.GetConIdByUserAndName(username, Name);
                var bstr = pushJoinGameInvitation.ToByteString();

                DemuxServer.SendToClient(ClientNumb, bstr, userCon);
            }
        }

        public static void PushRecentlyMetPlayers(Guid ClientNumb, PushRecentlyMetPlayers pushRecentlyMetPlayers)
        {
            if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
            {
                uint userCon = Auth.GetConIdByUserAndName(username, Name);
                var bstr = pushRecentlyMetPlayers.ToByteString();

                DemuxServer.SendToClient(ClientNumb, bstr, userCon);
            }
        }

        public static void PushGameInviteDeclined(Guid ClientNumb, PushGameInviteDeclined pushGameInviteDeclined)
        {
            if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
            {
                uint userCon = Auth.GetConIdByUserAndName(username, Name);
                var bstr = pushGameInviteDeclined.ToByteString();

                DemuxServer.SendToClient(ClientNumb, bstr, userCon);
            }
        }

        public static void PushNicknameUpdate(Guid ClientNumb, PushNicknameUpdate pushNicknameUpdate)
        {
            if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
            {
                uint userCon = Auth.GetConIdByUserAndName(username, Name);
                var bstr = pushNicknameUpdate.ToByteString();

                DemuxServer.SendToClient(ClientNumb, bstr, userCon);
            }
        }

        public static void PushIsFavoriteUpdate(Guid ClientNumb, PushIsFavoriteUpdate pushIsFavoriteUpdate)
        {
            if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
            {
                uint userCon = Auth.GetConIdByUserAndName(username, Name);
                var bstr = pushIsFavoriteUpdate.ToByteString();

                DemuxServer.SendToClient(ClientNumb, bstr, userCon);
            }
        }
    }
}
