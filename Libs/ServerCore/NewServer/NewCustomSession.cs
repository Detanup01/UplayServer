namespace ServerCore;

/// <summary>
/// This SSL Session is handling webserver and also demux (all demux request will be sent into demuxcontroller or smth)
/// </summary>
internal class NewCustomSession : NewCustomWssSession
{
    public NewCustomSession(NewServer server) : base(server)
    {

    }

    protected override void OnSSLReceived(byte[] bytes)
    {
        base.OnSSLReceived(bytes);
    }
}
