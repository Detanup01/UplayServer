﻿using Google.Protobuf;
using Uplay.Party;

namespace ServerCore.DemuxResponders;

public class Party
{
    public const string Name = "party_service";
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
        public static void Requests(Guid ClientNumb, Req req)
        {
            File.AppendAllText($"logs/client_{ClientNumb}_party_req.log", req.ToString() + "\n");
            if (req?.StartSessionReq != null) { StartSession(req.StartSessionReq); }
            IsIdDone = true;
        }

        public static void StartSession(StartSessionReq req)
        {
            uint cookie = 25;
            if (req.HasCookie)
            {
                cookie = req.Cookie;
            }
            Downstream = new()
            {
                Response = new()
                {
                    StartSessionRsp = new()
                    {
                        Cookie = cookie
                    }
                }
            };
        }
    }
}
