using Google.Protobuf;
using ServerCore.Models;
using Uplay.Utility;

namespace ServerCore.DMX.Services;

public static class UtilityServiceTask
{

    public static Task<ByteString> RunService(DmxSession dmxSession, ByteString data)
    {
        var upstream = Upstream.Parser.ParseFrom(data);
        if (upstream == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request == null)
            return CoreTask.ReturnEmptyByteString();
        if (upstream.Request.GeoipReq != null)
            return RequestGeoIp(dmxSession, upstream.Request.GeoipReq);
        return CoreTask.ReturnEmptyByteString();
    }

    public static Task<ByteString> RequestGeoIp(DmxSession dmxSession, GeoIpReq data)
    {
        Downstream downstream = new()
        {
            Response = new()
            {
                GeoipRsp = new()
                {
                    ContinentCode = ServerConfig.Instance.Demux.DefaultContinentCode,
                    CountryCode = ServerConfig.Instance.Demux.DefaultCountryCode
                }
            }
        };
        return Task.FromResult(downstream.ToByteString());
    }
}
