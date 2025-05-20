using Google.Protobuf;
using ServerCore.Controllers;
using ServerCore.DB;
using ServerCore.Models;
using ServerCore.Models.User;
using Uplay.Ownership;

namespace ServerCore.DMX.Connections;

public static class OwnershipTask
{
    private readonly static List<DmxSession> SessionsLoggedIn = [];

    public static Task<ByteString> RunConnection(DmxSession dmxSession, ByteString data)
    {
        var upstream = Upstream.Parser.ParseFrom(data);
        if (upstream == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request.InitializeReq != null)
            return Initialize(dmxSession, upstream.Request.RequestId, upstream.Request.InitializeReq);
        if (upstream.Request.OwnershipTokenReq != null)
            return OwnershipToken(dmxSession, upstream.Request.RequestId, upstream.Request.OwnershipTokenReq);
        if (upstream.Request.SignOwnershipReq != null)
            return SignOwnership(dmxSession, upstream.Request.RequestId, upstream.Request.SignOwnershipReq);
        if (upstream.Request.GetBatchDownloadUrlsReq != null)
            return GetBatchDownloadUrls(dmxSession, upstream.Request.RequestId, upstream.Request.GetBatchDownloadUrlsReq);
        if (upstream.Request.GetUplayPcTicketReq != null)
            return GetUplayPCTicket(dmxSession, upstream.Request.RequestId, upstream.Request.GetUplayPcTicketReq);
        if (upstream.Request.UnlockProductBranchReq != null)
            return UnlockProductBranch(dmxSession, upstream.Request.RequestId, upstream.Request.UnlockProductBranchReq);
        if (upstream.Request.SwitchProductBranchReq != null)
            return SwitchProductBranch(dmxSession, upstream.Request.RequestId, upstream.Request.SwitchProductBranchReq);
#pragma warning disable CS0612 // Type or member is obsolete
        if (upstream.Request.RegisterTemporaryOwnershipReq != null)
            return RegisterTemporaryOwnership(dmxSession, upstream.Request.RequestId, upstream.Request.RegisterTemporaryOwnershipReq);
#pragma warning restore CS0612 // Type or member is obsolete
        return CoreTask.ReturnEmptyByteString();
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
                    KeySpamBanSeconds = 10,
                    SubscriptionState = SubscriptionState.NoState,
                    SubscriptionType = 0,
                    OwnedGames = new()
                    {
                        OwnedGames_ = { }
                    },
                    OwnedGamesContainer = new()
                    {
                        ProductIds = { },
                        VisibleOrInstallableProductIds = { }
                    },
                }
            }
        };
        if (!dmxSession.IsLoggedIn)
            return Task.FromResult(downstream.ToByteString());
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic == null)
            return Task.FromResult(downstream.ToByteString());

        SessionsLoggedIn.Add(dmxSession);
        downstream.Response.InitializeRsp.Success = true;
        downstream.Response.InitializeRsp.SubscriptionType = owbasic.UbiPlus;
        downstream.Response.InitializeRsp.SubscriptionState = (SubscriptionState)owbasic.UbiPlus;

        downstream.Response.InitializeRsp.OwnedGamesContainer.Signature = OwnershipController.GetOwnerSignature(dmxSession.UserId);
        downstream.Response.InitializeRsp.OwnedGamesContainer.ProductIds.AddRange(owbasic.OwnedGamesIds);
        downstream.Response.InitializeRsp.OwnedGamesContainer.VisibleOrInstallableProductIds.AddRange(owbasic.OwnedGamesIds);
        Dictionary<uint, uint> branches = [];
        if (req.Branches != null)
        {
            foreach (var item in req.Branches)
            {
                var branch = App.GetAppBranch(item.ProductId, item.ActiveBranchId);
                if (branch != null && item.Passwords.Contains(branch.BranchPassword))
                    branches.Add(item.ProductId, item.ActiveBranchId);
            }
        }
        downstream.Response.InitializeRsp.OwnedGames = OwnershipController.GetOwnershipGames(dmxSession.UserId, branches);
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> OwnershipToken(DmxSession dmxSession, uint ReqId, OwnershipTokenReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                OwnershipTokenRsp = new()
                {
                    Success = false,
                }
            }
        };
        if (!dmxSession.IsLoggedIn)
            return Task.FromResult(downstream.ToByteString());
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic == null)
            return Task.FromResult(downstream.ToByteString());
        if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || owbasic.OwnedGamesIds.Contains(req.ProductId))
        {
            var ownership = DBUser.Get<UserOwnership>(dmxSession.UserId, x => x.ProductId == req.ProductId);
            var appconfig = App.GetAppConfig(req.ProductId);
            if (appconfig != null && ownership != null)
            {
                if (ownership.AppFlags.Count == 0)
                {
                    ownership.AppFlags = appconfig.AppFlags;
                }

                downstream.Response.OwnershipTokenRsp.Token = JWTController.CreateOwnershipToken(dmxSession.UserId, req.ProductId, req.ProductId, ownership.CurrentBranchId, ownership.AppFlags);
                downstream.Response.OwnershipTokenRsp.Expiration = (ulong)JWTController.GetExp(downstream.Response.OwnershipTokenRsp.Token);
                downstream.Response.OwnershipTokenRsp.Success = true;
            }
        }
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> SignOwnership(DmxSession dmxSession, uint ReqId, SignOwnershipReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                SignOwnershipRsp = new()
                {
                    Success = false,
                }
            }
        };
        if (!dmxSession.IsLoggedIn)
            return Task.FromResult(downstream.ToByteString());
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic == null)
            return Task.FromResult(downstream.ToByteString());
        var ownership = DBUser.Get<UserOwnership>(dmxSession.UserId, x => x.ProductId == req.ProductId);
        if (ownership == null)
            return Task.FromResult(downstream.ToByteString());
        if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || owbasic.OwnedGamesIds.Contains(req.ProductId))
        {
            var gameconfig = App.GetAppConfig(req.ProductId);

            if (gameconfig == null)
                return Task.FromResult(downstream.ToByteString());
            downstream.Response.SignOwnershipRsp.BranchId = ownership.CurrentBranchId;
            downstream.Response.SignOwnershipRsp.Signature = OwnershipController.GetSignofOwnership(dmxSession.UserId, req.ProductId);
            downstream.Response.SignOwnershipRsp.Success = true;
        }
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> GetBatchDownloadUrls(DmxSession dmxSession, uint ReqId, GetBatchDownloadUrlsReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                GetBatchDownloadUrlsRsp = new()
                {
                    UrlResponses = { },
                }
            }
        };
        if (!dmxSession.IsLoggedIn)
            return Task.FromResult(downstream.ToByteString());
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic == null)
            return Task.FromResult(downstream.ToByteString());
        for (int i = 0; i < req.UrlRequests.Count; i++)
        {
            var reg = req.UrlRequests[i];

            for (int j = 0; j < reg.RelativeFilePaths.Count; j++)
            {
                var path = reg.RelativeFilePaths[j];
                GetBatchDownloadUrlsRsp.Types.UrlResponse url = new();
                var gameproduct_Path = Path.Combine(ServerConfig.Instance.Demux.DownloadGamePath, reg.ProductId.ToString(), path);
                if (!File.Exists(gameproduct_Path))
                {
                    url.Result = GetBatchDownloadUrlsRsp.Types.Result.ProductNotOwned;
                }
                else
                {
                    url.Result = GetBatchDownloadUrlsRsp.Types.Result.Success;
                    url.DownloadUrls.Add(new GetBatchDownloadUrlsRsp.Types.DownloadUrls()
                    {
                        Urls = { $"{ServerConfig.Instance.HTTPS_Url}/download/{reg.ProductId}/{path}" }
                    });
                }
                downstream.Response.GetBatchDownloadUrlsRsp.UrlResponses.Add(url);
            }
        }
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> GetUplayPCTicket(DmxSession dmxSession, uint ReqId, GetUplayPCTicketReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                GetUplayPcTicketRsp = new()
                {
                    Success = false,
                    ErrorCode = GetUplayPCTicketRsp.Types.ErrorCode.Unknown,
                }
            }
        };
        if (!dmxSession.IsLoggedIn)
            return Task.FromResult(downstream.ToByteString());
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic == null)
            return Task.FromResult(downstream.ToByteString());
        if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || owbasic.OwnedGamesIds.Contains(req.UplayId))
        {
            downstream.Response.GetUplayPcTicketRsp.UplayPcTicket = JWTController.CreateUplayTicket(dmxSession.UserId, req.UplayId, (int)req.Platform);
            downstream.Response.GetUplayPcTicketRsp.Success = true;
        }
        else
        {
            downstream.Response.GetUplayPcTicketRsp.ErrorCode = GetUplayPCTicketRsp.Types.ErrorCode.ProductNotOwned;
        }
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> UnlockProductBranch(DmxSession dmxSession, uint ReqId, UnlockProductBranchReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                UnlockProductBranchRsp = new()
                {
                    Result = UnlockProductBranchRsp.Types.Result.Denied,
                    Branch = { }
                }
            }
        };
        if (!dmxSession.IsLoggedIn)
            return Task.FromResult(downstream.ToByteString());
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic == null)
            return Task.FromResult(downstream.ToByteString());
        var branches = App.GetAppBranches(req.Branch.ProductId);
        if (branches == null)
            return Task.FromResult(downstream.ToByteString());
        foreach (var branch in branches)
        {
            if (branch.BranchPassword != req.Branch.Password)
                continue;
            UnlockProductBranchRsp.Types.ProductBranch productBranch = new()
            {
                BranchId = branch.BranchId,
                BranchName = branch.BranchName
            };
            downstream.Response.UnlockProductBranchRsp.Result = UnlockProductBranchRsp.Types.Result.Success;
            downstream.Response.UnlockProductBranchRsp.Branch.Add(productBranch);
        }
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> SwitchProductBranch(DmxSession dmxSession, uint ReqId, SwitchProductBranchReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                SwitchProductBranchRsp = new()
                {
                    Result = SwitchProductBranchRsp.Types.Result.Denied,
                    Products = { OwnedGames_ = { } }
                }
            }
        };
        if (!dmxSession.IsLoggedIn)
            return Task.FromResult(downstream.ToByteString());
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic == null)
            return Task.FromResult(downstream.ToByteString());
        if (req.DefaultBranch != null)
        {
            downstream.Response.SwitchProductBranchRsp.Products.OwnedGames_.Add(OwnershipController.GetOwnershipGame(dmxSession.UserId, req.DefaultBranch.ProductId, uint.MaxValue));
            downstream.Response.SwitchProductBranchRsp.Result = SwitchProductBranchRsp.Types.Result.Success;
        }
        if (req.SpecifiedBranch != null)
        {
            var bId = req.SpecifiedBranch.BranchId;
            var pass = req.SpecifiedBranch.Password;
            var pid = req.SpecifiedBranch.ProductId;

            var branch = App.GetAppBranch(pid, bId);
            if (branch == null)
                return Task.FromResult(downstream.ToByteString());
            if (branch.BranchPassword == pass)
            {
                downstream.Response.SwitchProductBranchRsp.Products.OwnedGames_.Add(OwnershipController.GetOwnershipGame(dmxSession.UserId, pid, bId));
                downstream.Response.SwitchProductBranchRsp.Result = SwitchProductBranchRsp.Types.Result.Success;
            }
        }
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> GetProductConfig(DmxSession dmxSession, uint ReqId, GetProductConfigReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                GetProductConfigRsp = new()
                {
                    Result = GetProductConfigRsp.Types.Result.InternalServerError,
                }
            }
        };
        if (!dmxSession.IsLoggedIn)
            return Task.FromResult(downstream.ToByteString());
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic == null)
            return Task.FromResult(downstream.ToByteString());
        if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || owbasic.OwnedGamesIds.Contains(req.ProductId))
        {
            var gameconfig = App.GetAppConfig(req.ProductId);
            if (gameconfig == null)
                return Task.FromResult(downstream.ToByteString());

            var conf_path = Path.Combine("ServerFiles", "ProductConfigs", gameconfig.Configuration);
            if (!File.Exists(conf_path))
                return Task.FromResult(downstream.ToByteString());

            downstream.Response.GetProductConfigRsp.Configuration = File.ReadAllText(conf_path);
            downstream.Response.GetProductConfigRsp.Result = GetProductConfigRsp.Types.Result.Success;
        }
        return Task.FromResult(downstream.ToByteString());
    }

    [Obsolete]
    public static Task<ByteString> RegisterTemporaryOwnership(DmxSession dmxSession, uint ReqId, RegisterTemporaryOwnershipReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                RegisterTemporaryOwnershipRsp = new()
                {
                    Success = false,
                    ProductIds = { }
                }
            }
        };
        if (!dmxSession.IsLoggedIn)
            return Task.FromResult(downstream.ToByteString());
        if (!SessionsLoggedIn.Contains(dmxSession))
            return Task.FromResult(downstream.ToByteString());
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic == null)
            return Task.FromResult(downstream.ToByteString());
        downstream.Response.RegisterTemporaryOwnershipRsp.ProductIds.AddRange(OwnershipController.FromOwnerSignature(req.Token));
        downstream.Response.RegisterTemporaryOwnershipRsp.Success = true;
        return Task.FromResult(downstream.ToByteString());
    }
}
