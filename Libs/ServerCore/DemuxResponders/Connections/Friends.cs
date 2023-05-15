using Google.Protobuf;
using SharedLib.Server.DB;
using SharedLib.Server.Json;
using SharedLib.Server.Json.Ext;
using SharedLib.Shared;
using Uplay.Friends;

namespace Core.DemuxResponders
{
    public class Friends
    {
        public static readonly string Name = "friends_service";
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
            public static Dictionary<Guid, bool> UserInits = new();

            public static void Requests(Guid ClientNumb, Req req)
            {
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
                    var user = DBUser.GetUser(userID);
                    var friend = DBUser.GetFriend(userID, acceptFriendship.User.AccountId);
                    var friend_friend = DBUser.GetFriend(acceptFriendship.User.AccountId, userID);
                    var friend_user = DBUser.GetUser(acceptFriendship.User.AccountId);
                    if (user != null && friend != null && friend_friend != null && friend_user != null)
                    {
                        user.Friends.Add(acceptFriendship.User.AccountId);
                        DBUser.Edit(user);
                        friend_user.Friends.Add(acceptFriendship.User.AccountId);
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
                    var user = DBUser.GetUser(userID);
                    var friend = DBUser.GetFriend(userID, AddToBlacklist.User.AccountId);
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
                    var user = DBUser.GetUser(userID);
                    var friend = DBUser.GetFriend(userID, RemoveFromBlacklist.User.AccountId);
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
                if (jwt.Validate(initialize.UbiTicket))
                {
                    var userID = Globals.IdToUser[ClientNumb];

                    var activity = DBUser.GetActivity(userID);
                    if (activity == null)
                    {
                        DBUser.Add(new SharedLib.Server.Json.DB.JActivity()
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
                    var friends = DBUser.GetFriends(userID);
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
                                    AccountId = friend.UserId,
                                    IsFavorite = friend.IsFavorite,
                                    NameOnPlatform = friend.Name,
                                    Nickname = friend.Nickname
                                }
                            });
                            Pusher.PushToFriend(friend.UserId, new Push()
                            {
                                PushUpdatedStatus = new()
                                {
                                    UpdatesStatus = new()
                                    {
                                        ActivityStatus = initialize.ActivityStatus,
                                        OnlineStatus = Status.Types.OnlineStatus.Online,
                                        User = new()
                                        {
                                            AccountId = userID
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
                    var user_friends = DBUser.GetFriends(userID);
                    if (user_friends != null)
                    {
                        foreach (var friend in user_friends)
                        {
                            //  should not be done like friend.Nickname.Contains(FindFriend.SearchQueryUsername)?
                            if (FindFriend.HasSearchQueryUsername && (FindFriend.SearchQueryUsername == friend.Name || FindFriend.SearchQueryUsername == friend.Nickname))
                            {
                                friends.Add(new Friend()
                                {
                                    AccountId = friend.UserId,
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
                    var activity = DBUser.GetActivity(userID);

                    if (activity != null)
                    {
                        activity.Status = (int)SetActivityStatus.ActivityStatus;
                        DBUser.Edit(activity);
                        Pusher.PushToFriends(userID, new Push()
                        {
                            PushUpdatedStatus = new()
                            {
                                UpdatesStatus = new()
                                {
                                    ActivityStatus = SetActivityStatus.ActivityStatus,
                                    OnlineStatus = Status.Types.OnlineStatus.Online,
                                    User = new()
                                    {
                                        AccountId = userID
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
                    var user = User.GetUser(userID);

                    if (user != null)
                    {
                        var user2 = user.Friends.Where(x => x.UserId == SetNickname.AccountId).FirstOrDefault();
                        if (user2 != null)
                        {
                            user2.Nickname = SetNickname.Nickname;
                            IsSuccess = true;
                            User.SaveUser(userID, user);
                        }
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
                    var activity = DBUser.GetActivity(userID);

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
                                //  PushUpdatedStatus to all friend

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
                    var user = User.GetUser(userID);

                    if (user != null)
                    {
                        DBUserExt.UplayFriendsGameParseToUser(userID, SetGame.Game);

                        //  PushUpdatedStatus to all friend

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
                if (jwt.Validate(UbiTicketRefresh.UbiTicket))
                {
                    //  We are not storing tickets with refresh thingy.
                    IsSuccess = true;
                }
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
                    var friendId = DeclineFriendship.User.AccountId;

                    bool UserFromFriend = DBUserExt.RemoveFromFriends(userID, friendId);
                    bool FriendFromUser = DBUserExt.RemoveFromFriends(friendId, userID);

                    if (UserFromFriend && FriendFromUser)
                    {
                        IsSuccess = true;
                        if (Globals.UserToId.TryGetValue(friendId, out var id))
                        {
                            Pusher.Pushes(id, new()
                            {
                                PushUpdatedRelationship = new()
                                {
                                    Relationship = new()
                                    {
                                        Friend = new()
                                        {
                                            AccountId = userID
                                        },
                                        Relation = Relationship.Types.Relation.NoRelationship
                                    }
                                }
                            });
                        }
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
                    var user = User.GetUser(userID);

                    if (user != null)
                    {
                        int i = 0;
                        foreach (var ruser in RequestFriendships.Users)
                        {
                            // We checking If user has already sent/recieved as Friend if not then we add it
                            if (user.Friends.Where(x => x.UserId == ruser.AccountId).Count() == 0)
                            {
                                var fuser = User.GetUser(ruser.AccountId);

                                if (fuser != null)
                                {
                                    // Rel 1 = Sent, Rel 2 = Rec
                                    user.Friends.Add(new() { UserId = ruser.AccountId, Relation = 1 });
                                    fuser.Friends.Add(new() { UserId = userID, Relation = 2 });

                                    User.SaveUser(userID, user);
                                    User.SaveUser(ruser.AccountId, fuser);

                                    if (Globals.UserToId.TryGetValue(ruser.AccountId, out var id))
                                    {
                                        Pusher.Pushes(id, new()
                                        {
                                            PushUpdatedRelationship = new()
                                            {
                                                Relationship = new()
                                                {
                                                    Friend = new()
                                                    {
                                                        AccountId = userID
                                                    },
                                                    Relation = Relationship.Types.Relation.NoRelationship
                                                }
                                            }
                                        });
                                    }

                                    successes[i] = true;
                                }
                            }
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

                    var friendId = ClearRelationship.User.AccountId;

                    bool UserFromFriend = DBUserExt.RemoveFromFriends(userID, friendId);
                    bool FriendFromUser = DBUserExt.RemoveFromFriends(friendId, userID);

                    if (UserFromFriend && FriendFromUser)
                    {
                        IsSuccess = true;
                        if (Globals.UserToId.TryGetValue(friendId, out var id))
                        {
                            Pusher.Pushes(id, new()
                            {
                                PushUpdatedRelationship = new()
                                {
                                    Relationship = new()
                                    {
                                        Friend = new()
                                        {
                                            AccountId = userID
                                        },
                                        Relation = Relationship.Types.Relation.NoRelationship
                                    }
                                }
                            });
                        }
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

                    var user = User.GetUser(userID);

                    if (user != null)
                    {
                        if (GetBlacklist.HasUser)
                        {
                            // IDK what it should be doing if we have a userId
                        }

                        var frBlacklist = user.Friends.Where(x => x.IsBlacklisted == true).ToList();

                        foreach (var fr in frBlacklist)
                        {
                            blacklist.Add(fr.UserId);
                        }
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

            public static void JoinGameInvitation(Guid ClientNumb, JoinGameInvitationReq JoinGameInvitation)
            {
                bool IsSuccess = false;
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];

                    DBUserExt.UplayFriendsGameParseToUser(userID, JoinGameInvitation.Game);

                    if (DBUserExt.IsUserExist(JoinGameInvitation.AccountIdTo))
                    {
                        Pusher.PushToFriend(JoinGameInvitation.AccountIdTo, new()
                        {
                            PushJoinGameInvitation = new()
                            {
                                Invite = new()
                                {
                                    AccountIdFrom = userID,
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

            public static void DeclineGameInvite(Guid ClientNumb, DeclineGameInviteReq DeclineGameInvite)
            {
                bool IsSuccess = false;
                if (UserInits[ClientNumb])
                {
                    if (DBUserExt.IsUserExist(DeclineGameInvite.AccountId))
                    {
                        Pusher.PushToFriend(DeclineGameInvite.AccountId, new()
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
            public static void PushToFriends(Guid ClientNumb, Push push)
            {
                var userId = Globals.IdToUser[ClientNumb];
                PushToFriends(userId, push);
            }

            public static void PushToFriends(string BaseUserId, Push push)
            {
                var user = DBUser.GetUser(BaseUserId);
                foreach (var friend in user.Friends)
                {
                    PushToFriend(friend, push);
                }
            }

            public static void PushToFriend(string friend, Push push)
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
}
