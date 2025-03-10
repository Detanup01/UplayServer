﻿using Google.Protobuf;
using ServerCore.DB;
using ServerCore.Models.User;
using Uplay.Playtime;

namespace ServerCore.DemuxResponders;

public class Playtime
{
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
            File.AppendAllText($"logs/client_{ClientNumb}_playtime_req.log", req.ToString() + "\n");
            if (req?.GetPlaytimeReq != null) { GetPlaytime(ClientNumb, req.GetPlaytimeReq); }
            if (req?.UpdatePlaytimeReq != null) { UpdatePlaytime(ClientNumb, req.UpdatePlaytimeReq); }
            IsIdDone = true;
        }

        public static void GetPlaytime(Guid ClientNumb, GetPlaytimeReq req)
        {
            Downstream = new()
            {
                Response = new()
                {
                    GetPlaytimeRsp = new()
                    {
                        Result = Result.ServerError
                    }
                }
            };
        }

        public static void UpdatePlaytime(Guid ClientNumb, UpdatePlaytimeReq req)
        {
            var UserId = Globals.IdToUser[ClientNumb];
            var playtime = DBUser.Get<UserPlaytime>(UserId, x=>x.UplayId == req.GameId);
            if (playtime != null)
                playtime.PlayTime += req.SecondsToAdd;

            Downstream = new()
            {
                Response = new()
                {
                    UpdatePlaytimeRsp = new()
                    {
                        Result = Result.Success
                    }
                }
            };
        }
    }
}
