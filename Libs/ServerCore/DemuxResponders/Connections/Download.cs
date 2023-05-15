using Google.Protobuf;
using SharedLib.Server.Json;
using Uplay.DownloadService;

namespace Core.DemuxResponders
{
    public class Download
    {
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
                File.AppendAllText($"logs/client_{ClientNumb}_download_req.log", req.ToString() + "\n");
                ReqId = req.RequestId;
                if (req?.InitializeReq != null) { Initialize(ClientNumb, req.InitializeReq); }
                if (req?.UrlReq != null) { Url(ClientNumb, req.UrlReq); }
                if (req?.UrlReqCovid != null) { Url(ClientNumb, req.UrlReqCovid); }
                IsIdDone = true;
            }

            public static void Initialize(Guid ClientNumb, InitializeReq initialize)
            {
                bool TokenValid = false;
                if (initialize.OwnershipToken != null)
                {
                    TokenValid = jwt.Validate(initialize.OwnershipToken);
                    // We could add another step here to verify each things exist
                    UserInits.TryAdd(ClientNumb, true);
                }
                else
                {
                    var userID = Globals.IdToUser[ClientNumb];
                    if (ServerConfig.DMX.GlobalOwnerShipCheck || (userID != null && initialize.Signature != null && Ownership.GetOwnerSignature(userID).ToBase64() != "T3duZXJTaWduYXR1cmVfSXNGYWlsZWQ="))
                    {
                        var gameconf = GameConfig.GetGameConfig(initialize.ProductId);
                        if (gameconf != null && (gameconf.branches.active_branch_id == initialize.BranchId || gameconf.branches.product_branches.Where(x => x.branch_id == initialize.BranchId).Any()))
                        {
                            UserInits.TryAdd(ClientNumb, true);
                            TokenValid = true;
                            //  We skip initialize.Expiration bc the files live on a server prob forever
                        }
                    }
                }

                Downstream = new()
                {
                    Response = new()
                    {
                        RequestId = ReqId,
                        InitializeRsp = new()
                        {
                            Ok = TokenValid
                        }
                    }
                };
            }

            public static void Url(Guid ClientNumb, UrlReq url)
            {
                List<UrlRsp.Types.UrlResponse> resp = new();
                UrlRsp.Types.UrlResponse urlresp = new()
                {
                    DownloadUrls = { }
                };

                foreach (var part in url.UrlRequests)
                {
                    Console.WriteLine(part.ToString());
                    foreach (var relative in part.RelativeFilePath)
                    {
                        Console.WriteLine(part.ProductId);
                        Console.WriteLine($"{ServerConfig.DMX.DownloadGamePath}{part.ProductId}/{relative}");
                        if (!File.Exists($"{ServerConfig.DMX.DownloadGamePath}{part.ProductId}/{relative}"))
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
                            urls.Urls.Add($"{ServerConfig.DMX.DownloadGameUrl}{part.ProductId}/{relative}");
                            urlresp.DownloadUrls.Add(urls);
                        }
                    }
                    resp.Add(urlresp);
                }

                /*
                if (Config.DMX.GlobalOwnerShipCheck)// || UserInits.TryGetValue(ClientNumb, out bool val) && val)
                {
                    foreach (var part in url.UrlRequests)
                    {
                        Console.WriteLine(part.ToString());
                        foreach (var relative in part.RelativeFilePath)
                        {
                            Console.WriteLine(part.ProductId);
                            Console.WriteLine($"{Config.DMX.DownloadGamePath}{part.ProductId}/{relative}");
                            if (!File.Exists($"{Config.DMX.DownloadGamePath}{part.ProductId}/{relative}"))
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
                                urls.Urls.Add($"{Config.DMX.DownloadGameUrl}{part.ProductId}/{relative}");
                                urlresp.DownloadUrls.Add(urls);
                            }
                        }
                        resp.Add(urlresp);
                    }
                }
                else
                {
                    foreach (var _ in url.UrlRequests)
                    {
                        urlresp.Result = UrlRsp.Types.Result.NotOwned;
                        resp.Add(urlresp);
                    }
                }
                */

                Downstream = new()
                {
                    Response = new()
                    {
                        RequestId = ReqId,
                        UrlRsp = new()
                        {
                            TtlSeconds = 300,
                            UrlResponses = { resp }
                        }
                    }
                };
            }
        }
    }
}
