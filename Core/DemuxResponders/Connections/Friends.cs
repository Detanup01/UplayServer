using Core.JSON;
using Core.SQLite;
using Google.Protobuf;
using Uplay.Friends;

namespace Core.DemuxResponders
{
    public class Friends
    {
        public static readonly string Name = "friends_service";
        public class Up
        {
            public static Downstream Downstream = null;
            public static void UpstreamConverter(int ClientNumb, ByteString bytes)
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
            public static Dictionary<int, bool> UserInits = new();

            public static void Requests(int ClientNumb, Req req)
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
            public static void AcceptFriendship(int ClientNumb, AcceptFriendshipReq acceptFriendship)
            {
                bool IsSuccess = false;
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var user = User.GetUser(userID);
                    if (user != null)
                    {
                        var friend = User.GetUser(acceptFriendship.User.AccountId);
                        if (friend != null)
                        {
                            foreach (var userfriend in user.Friends)
                            {
                                if (userfriend.UserId == acceptFriendship.User.AccountId)
                                {
                                    userfriend.Relation = (int)Relationship.Types.Relation.Friends;
                                }
                            }
                            foreach (var userfriend in friend.Friends)
                            {
                                if (userfriend.UserId == userID)
                                {
                                    userfriend.Relation = (int)Relationship.Types.Relation.Friends;
                                }
                            }
                            User.SaveUser(userID, user);
                            User.SaveUser(acceptFriendship.User.AccountId, friend);
                            IsSuccess = true;
                        }
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

            public static void AddToBlacklist(int ClientNumb, AddToBlacklistReq AddToBlacklist)
            {
                bool IsSuccess = false;
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var user = User.GetUser(userID);
                    if (user != null)
                    {
                        foreach (var friend in user.Friends)
                        {
                            if (friend.UserId == AddToBlacklist.User.AccountId)
                            {
                                friend.IsBlacklisted = true;
                            }
                        }

                        User.SaveUser(userID, user);
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

            public static void RemoveFromBlacklist(int ClientNumb, RemoveFromBlacklistReq RemoveFromBlacklist)
            {
                bool IsSuccess = false;
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var user = User.GetUser(userID);
                    if (user != null)
                    {
                        foreach (var friend in user.Friends)
                        {
                            if (friend.UserId == RemoveFromBlacklist.User.AccountId)
                            {
                                friend.IsBlacklisted = false;
                            }
                        }

                        User.SaveUser(userID, user);
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

            public static void Initialize(int ClientNumb, InitializeReq initialize)
            {
                bool IsSuccess = false;
                List<Relationship> Relationships = new();
                if (jwt.Validate(initialize.UbiTicket))
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var user = User.GetUser(userID);
                    if (user != null)
                    {
                        Friend curUser_friend = new();
                        curUser_friend.AccountId = user.UserId;
                        curUser_friend.Nickname = user.Name;
                        curUser_friend.NameOnPlatform = user.Name;

                        user.Activity.Status = (int)initialize.ActivityStatus;
                        User.SaveUser(userID, user);
                        JSON.Ext.UserExt.SetAllUserActivity(userID);
                        foreach (var userfriend in user.Friends)
                        {
                            if (userfriend.Activity.Status == 1)
                            {
                                // This should work
                                Push push = new()
                                {
                                    PushUpdatedStatus = new()
                                    {
                                        UpdatesStatus = new()
                                        {
                                            ActivityStatus = initialize.ActivityStatus,
                                            OnlineStatus = Status.Types.OnlineStatus.Online,
                                            User = curUser_friend
                                        }
                                    }
                                };

                                var usertoid = Globals.UserToId[userfriend.UserId];
                                Pusher.Pushes(usertoid, push);
                            }
                        }
                        IsSuccess = true;
                        UserInits[ClientNumb] = true;

                    }
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

            public static void FindFriend(int ClientNumb, FindFriendReq FindFriend)
            {
                bool IsSuccess = false;
                List<Friend> friends = new();
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var user = User.GetUser(userID);
                    if (user != null)
                    {
                        foreach (var friend in user.Friends)
                        {
                            if (FindFriend.HasSearchQueryUsername && (FindFriend.SearchQueryUsername == friend.Name || FindFriend.SearchQueryUsername == friend.Nickname))
                            {
                                Friend _friend = new();
                                _friend.AccountId = friend.UserId;
                                _friend.IsFavorite = friend.IsFavorite;
                                _friend.NameOnPlatform = friend.Name;
                                _friend.Nickname = friend.Nickname;
                                friends.Add(_friend);
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

            public static void SetActivityStatus(int ClientNumb, SetActivityStatusReq SetActivityStatus)
            {
                bool IsSuccess = false;
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var user = User.GetUser(userID);

                    if (user != null)
                    {
                        user.Activity.Status = (int)SetActivityStatus.ActivityStatus;
                        User.SaveUser(userID, user);
                        JSON.Ext.UserExt.SetAllUserActivity(userID);
                        //  PushUpdatedStatus to all friend
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

            public static void SetNickname(int ClientNumb, SetNicknameReq SetNickname)
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

            public static void SetRichPresence(int ClientNumb, SetRichPresenceReq SetRichPresence)
            {
                bool IsSuccess = false;
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var user = User.GetUser(userID);

                    if (user != null)
                    {
                        var config = GameConfig.GetGameConfig(SetRichPresence.PresenceState.ProductId);
                        if (config != null)
                        {
                            if (config.richpresence.Available_KeyValues.Contains(SetRichPresence.PresenceState.PresenceTokens[0].Key))
                            {
                                user.Activity.IsPlaying = true;
                                user.Activity.GameId = SetRichPresence.PresenceState.ProductId;
                                user.Activity.Key = SetRichPresence.PresenceState.PresenceTokens[0].Key;
                                user.Activity.Value = SetRichPresence.PresenceState.PresenceTokens[0].Val;
                                User.SaveUser(userID, user);
                                JSON.Ext.UserExt.SetAllUserActivity(userID);
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

            public static void SetGame(int ClientNumb, SetGameReq SetGame)
            {
                bool IsSuccess = false;
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var user = User.GetUser(userID);

                    if (user != null)
                    {
                        JSON.Ext.UserExt.UplayFriendsGameParseToUser(userID,SetGame.Game);

                        JSON.Ext.UserExt.SetAllUserActivity(userID);
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

            public static void UbiTicketRefresh(int ClientNumb, UbiTicketRefreshReq UbiTicketRefresh)
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

            public static void DeclineFriendship(int ClientNumb, DeclineFriendshipReq DeclineFriendship)
            {
                bool IsSuccess = false;
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    var friendId = DeclineFriendship.User.AccountId;

                    bool UserFromFriend = JSON.Ext.UserExt.RemoveFromFriends(userID, friendId);
                    bool FriendFromUser = JSON.Ext.UserExt.RemoveFromFriends(friendId, userID);

                    if (UserFromFriend && FriendFromUser)
                    {
                        IsSuccess = true;
                        if (Globals.UserToId.TryGetValue(friendId, out int id))
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

            public static void RequestFriendships(int ClientNumb, RequestFriendshipsReq RequestFriendships)
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

                                    if (Globals.UserToId.TryGetValue(ruser.AccountId, out int id))
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

            public static void ClearRelationship(int ClientNumb, ClearRelationshipReq ClearRelationship)
            {
                bool IsSuccess = false;
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];

                    var friendId = ClearRelationship.User.AccountId;

                    bool UserFromFriend = JSON.Ext.UserExt.RemoveFromFriends(userID, friendId);
                    bool FriendFromUser = JSON.Ext.UserExt.RemoveFromFriends(friendId, userID);

                    if (UserFromFriend && FriendFromUser)
                    {
                        IsSuccess = true;
                        if (Globals.UserToId.TryGetValue(friendId, out int id))
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

            public static void GetBlacklist(int ClientNumb, GetBlacklistReq GetBlacklist)
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

            public static void JoinGameInvitation(int ClientNumb, JoinGameInvitationReq JoinGameInvitation)
            {
                bool IsSuccess = false;
                if (UserInits[ClientNumb])
                {
                    var userID = Globals.IdToUser[ClientNumb];

                    JSON.Ext.UserExt.UplayFriendsGameParseToUser(userID, JoinGameInvitation.Game);

                    if (JSON.Ext.UserExt.IsUserExist(JoinGameInvitation.AccountIdTo))
                    {
                        Pusher.Pushes(ClientNumb, new()
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

            public static void DeclineGameInvite(int ClientNumb, DeclineGameInviteReq DeclineGameInvite)
            {
                bool IsSuccess = false;
                if (UserInits[ClientNumb])
                {
                    if (JSON.Ext.UserExt.IsUserExist(DeclineGameInvite.AccountId))
                    {
                        Pusher.Pushes(ClientNumb, new()
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
                        JoinGameInvitationRsp = new()
                        {
                            Ok = IsSuccess
                        }
                    }
                };
            }
            #endregion
        }

        public class Pusher
        {
            public static void Pushes(int ClientNumb, Push push)
            {
                if (push?.PushUpdatedRelationship != null) { PushUpdatedRelationship(ClientNumb, push.PushUpdatedRelationship); }
                if (push?.PushUpdatedStatus != null) { PushUpdatedStatus(ClientNumb, push.PushUpdatedStatus); }
                if (push?.PushJoinGameInvitation != null) { PushJoinGameInvitation(ClientNumb, push.PushJoinGameInvitation); }
                if (push?.PushRecentlyMetPlayers != null) { PushRecentlyMetPlayers(ClientNumb, push.PushRecentlyMetPlayers); }
                if (push?.PushGameInviteDeclined != null) { PushGameInviteDeclined(ClientNumb, push.PushGameInviteDeclined); }
                if (push?.PushNicknameUpdate != null) { PushNicknameUpdate(ClientNumb, push.PushNicknameUpdate); }
                if (push?.PushIsFavoriteUpdate != null) { PushIsFavoriteUpdate(ClientNumb, push.PushIsFavoriteUpdate); }
            }

            public static void PushUpdatedRelationship(int ClientNumb, PushUpdatedRelationship pushUpdatedRelationship)
            {
                if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
                {
                    uint userCon = UserDMX.GetConIdByUserAndName(username, Name);
                    var bstr = pushUpdatedRelationship.ToByteString();

                    DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
                }
            }

            public static void PushUpdatedStatus(int ClientNumb, PushUpdatedStatus pushUpdatedStatus)
            {
                if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
                {
                    uint userCon = UserDMX.GetConIdByUserAndName(username, Name);
                    var bstr = pushUpdatedStatus.ToByteString();

                    DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
                }
            }

            public static void PushJoinGameInvitation(int ClientNumb, PushJoinGameInvitation pushJoinGameInvitation)
            {
                if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
                {
                    uint userCon = UserDMX.GetConIdByUserAndName(username, Name);
                    var bstr = pushJoinGameInvitation.ToByteString();

                    DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
                }
            }

            public static void PushRecentlyMetPlayers(int ClientNumb, PushRecentlyMetPlayers pushRecentlyMetPlayers)
            {
                if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
                {
                    uint userCon = UserDMX.GetConIdByUserAndName(username, Name);
                    var bstr = pushRecentlyMetPlayers.ToByteString();

                    DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
                }
            }

            public static void PushGameInviteDeclined(int ClientNumb, PushGameInviteDeclined pushGameInviteDeclined)
            {
                if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
                {
                    uint userCon = UserDMX.GetConIdByUserAndName(username, Name);
                    var bstr = pushGameInviteDeclined.ToByteString();

                    DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
                }
            }

            public static void PushNicknameUpdate(int ClientNumb, PushNicknameUpdate pushNicknameUpdate)
            {
                if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
                {
                    uint userCon = UserDMX.GetConIdByUserAndName(username, Name);
                    var bstr = pushNicknameUpdate.ToByteString();

                    DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
                }
            }

            public static void PushIsFavoriteUpdate(int ClientNumb, PushIsFavoriteUpdate pushIsFavoriteUpdate)
            {
                if (Globals.IdToUser.TryGetValue(ClientNumb, out var username))
                {
                    uint userCon = UserDMX.GetConIdByUserAndName(username, Name);
                    var bstr = pushIsFavoriteUpdate.ToByteString();

                    DemuxServer.SendToClientBSTR(ClientNumb, bstr, userCon);
                }
            }
        }
    }
}
