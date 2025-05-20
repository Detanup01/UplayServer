using Google.Protobuf;
using ServerCore.Controllers;
using ServerCore.DB;
using ServerCore.Models;
using ServerCore.Models.User;
using Uplay.DownloadService;

namespace ServerCore.DMX.Connections;

public static class DownloadTask
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
        if (upstream.Request.UrlReq != null)
            return Url(dmxSession, upstream.Request.RequestId, upstream.Request.UrlReq);
        if (upstream.Request.UrlReqCovid != null)
            return Url(dmxSession, upstream.Request.RequestId, upstream.Request.UrlReqCovid);
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
                    Ok = false
                }
            }
        };
        if (!dmxSession.IsLoggedIn)
            return Task.FromResult(downstream.ToByteString());
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic == null)
            return Task.FromResult(downstream.ToByteString());

        if (req.OwnershipToken != null)
            downstream.Response.InitializeRsp.Ok = JWTController.Validate(req.OwnershipToken);
        else if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || (req.Signature != null && OwnershipController.GetOwnerSignature(dmxSession.UserId).ToBase64() != "T3duZXJTaWduYXR1cmVfSXNGYWlsZWQ="))
        {
            var branch = App.GetAppBranch(req.ProductId, req.BranchId);
            if (branch == null)
                return Task.FromResult(downstream.ToByteString());
            downstream.Response.InitializeRsp.Ok = true;
        }
        if (downstream.Response.InitializeRsp.Ok)
            SessionsLoggedIn.Add(dmxSession);
        return Task.FromResult(downstream.ToByteString());
    }

    public static Task<ByteString> Url(DmxSession dmxSession, uint ReqId, UrlReq req)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                RequestId = ReqId,
                UrlRsp = new()
                {
                    TtlSeconds = 300,
                    UrlResponses = { }
                }
            }
        };
        if (!dmxSession.IsLoggedIn)
            return Task.FromResult(downstream.ToByteString());
        var owbasic = DBUser.Get<UserOwnershipBasic>(dmxSession.UserId);
        if (owbasic == null)
            return Task.FromResult(downstream.ToByteString());

        UrlRsp.Types.UrlResponse urlresp = new()
        {
            DownloadUrls = { }
        };
        if (ServerConfig.Instance.Demux.GlobalOwnerShipCheck || (SessionsLoggedIn.Contains(dmxSession)))
        {
            foreach (var part in req.UrlRequests)
            {
                urlresp = new()
                {
                    DownloadUrls = { }
                };
                foreach (var relative in part.RelativeFilePath)
                {
                    if (!File.Exists($"{ServerConfig.Instance.Demux.DownloadGamePath}{part.ProductId}/{relative}"))
                    {
                        urlresp.Result = UrlRsp.Types.Result.NotOwned;
                    }
                    else
                    {
                        urlresp.Result = UrlRsp.Types.Result.Success;
                        UrlRsp.Types.DownloadUrls urls = new()
                        {
                            Urls = { }
                        };
                        urls.Urls.Add($"{ServerConfig.Instance.HTTPS_Url}/download/{part.ProductId}/{relative}");
                        urlresp.DownloadUrls.Add(urls);
                    }
                }
                downstream.Response.UrlRsp.UrlResponses.Add(urlresp);
            }
        }
        else
        {
            foreach (var _ in req.UrlRequests)
            {
                urlresp.Result = UrlRsp.Types.Result.NotOwned;
                downstream.Response.UrlRsp.UrlResponses.Add(urlresp);
                urlresp = new()
                {
                    DownloadUrls = { }
                };
            }
        }
        return Task.FromResult(downstream.ToByteString());
    }
}
