using Google.Protobuf;
using Uplay.Uplaydll;

namespace Core.DemuxResponsers
{
    public class UplayDll_old
    {
        public class ReqRsp
        {
            public static readonly string Name = "uplaydll";
            public static void Requester(int ClientNumb, Req req)
            {
                if (req.LaunchAppReq != null) { LaunchApp(ClientNumb,req.LaunchAppReq); }
            }

            public static void LaunchApp(int ClientNumb, LaunchAppReq LaunchApp)
            {
                Rsp rsp = new()
                {
                    LaunchAppRsp = new()
                    { 
                        Result = GameLaunchResult.Success
                    }
                };

                if (DemuxServer.SslClients.TryGetValue(ClientNumb, out var sslStream))
                {
                    sslStream.Write(Utils.FormatUpstream(rsp.ToByteArray()));
                    sslStream.Flush();
                }
            }
        }
    }
}
