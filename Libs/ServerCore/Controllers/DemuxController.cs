using Google.Protobuf;
using ServerCore.DMX;
using ServerCore.ServerAndSession;
using SharedLib;
using Uplay.Demux;

namespace ServerCore.Controllers;

public static class DemuxController
{
    private static List<DmxSession> DmxSessions = [];

    public static void Start()
    {
        UplaySession.OnSSLReceived += UplaySession_OnSSLReceived;
    }

    public static void Stop()
    {
        UplaySession.OnSSLReceived += UplaySession_OnSSLReceived;
    }

    private static void UplaySession_OnSSLReceived(object? sender, byte[] buf)
    {
        UplaySession session = (UplaySession)sender!;
        var buffer = buf.Skip(4).ToArray();
        DmxSession? dmxSession = null;
        if (DmxSessions.Any(x => x.Session == session))
            dmxSession = DmxSessions.First(x => x.Session == session);
        else
        {
            dmxSession = new DmxSession(session);
            DmxSessions.Add(dmxSession);
        }
        var result = DMX.CoreTask.RunTask(session.Id, buffer).Result;
        if (result == null)
            return;
        session.Send(Formatters.FormatUpstream(result.ToByteArray()));
    }

    #region SendtoClients
    /// <summary>
    /// Demux Sent To Client
    /// </summary>
    /// <param name="ClientNumber">SSL Client Number</param>
    /// <param name="down">Uplay.Demux Downstream data</param>
    public static void SendToClient(Guid ClientNumber, Downstream down)
    {
        if (UplayServer.UplaySessions.TryGetValue(ClientNumber, out var session) && !session.IsClosed && !session.IsSSL)
        {
            session.Send(Formatters.FormatUpstream(down.ToByteArray()));
        }
    }

    /// <summary>
    /// Demux Sent To Client
    /// </summary>
    /// <param name="ClientNumber">SSL Client Number</param>
    /// <param name="message">Push DataMessage</param>
    public static void SendToClient(Guid ClientNumber, DataMessage message)
    {
        if (UplayServer.UplaySessions.TryGetValue(ClientNumber, out var session) && !session.IsClosed && !session.IsSSL)
        {
            Downstream downstream = new()
            {
                Push = new()
                {
                    Data = message
                }
            };
            session.Send(Formatters.FormatUpstream(downstream.ToByteArray()));
        }
    }

    /// <summary>
    /// Demux Sent To Client
    /// </summary>
    /// <param name="ClientNumber">SSL Client Number</param>
    /// <param name="bstr">ByteString data</param>
    /// <param name="conId">ConnectionId</param>
    public static void SendToClient(Guid ClientNumber, ByteString bstr, uint conId)
    {
        if (UplayServer.UplaySessions.TryGetValue(ClientNumber, out var session) && !session.IsClosed && !session.IsSSL)
        {
            Downstream downstream = new()
            {
                Push = new()
                {
                    Data = new()
                    {
                        ConnectionId = conId,
                        Data = bstr
                    }
                }
            };
            session.Send(Formatters.FormatUpstream(downstream.ToByteArray()));
        }
    }
    #endregion
    #region Multicast
    public static void SendToAllClient(Downstream down)
    {
        foreach (var item in UplayServer.UplaySessions.Values)
        {
            if (item.IsClosed)
                continue;
            if (!item.IsSSL)
                continue;
            item.SendAsync(Formatters.FormatUpstream(down.ToByteArray()));
        }
    }
    public static void SendToAllClient(DataMessage message)
    {
        Downstream down = new()
        {
            Push = new()
            {
                Data = message
            }
        };
        SendToAllClient(down);
    }
    public static void SendToAllClient(ByteString bstr, uint conId)
    {
        Downstream down = new()
        {
            Push = new()
            {
                Data = new()
                {
                    ConnectionId = conId,
                    Data = bstr
                }
            }
        };
        SendToAllClient(down);
    }
    #endregion

}
