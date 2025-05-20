using ClientTester.DMX_Test;
using UbiServices;
using UbiServices.Records;
using UplayKit;

namespace ClientTester;

internal class DemuxTest
{
    DemuxSocket socket;
    LoginJson Login;
    List<Action> actions = new();
    public DemuxTest(LoginJson login)
    {
        socket = new();
        socket.NewMessage += Socket_NewMessage;
        Login = login;
        actions.Add(SendVersion);
        actions.Add(VersionCheck);
        actions.Add(Auth);
        actions.Add(DoOwnership);
        actions.Add(VersionCheck);
        actions.ForEach(x => x());
        socket.NewMessage -= Socket_NewMessage;
        socket.Disconnect();
        Console.WriteLine("DemuxTest Done!");
    }

    void SendVersion()
    {
        socket.PushVersion();
        if (!socket.IsConnected)
        {
            Console.WriteLine("Socket is closed!");
        }
    }

    void VersionCheck()
    {
        bool version = socket.VersionCheck();
        if (!version)
        {
            Console.WriteLine("Version is Not same!");
        }
    }

    void Auth()
    {
        bool authed = socket.Authenticate(Login.Ticket);
        if (!authed)
        {
            Console.WriteLine("User is NOT authed!");
        }
    }

    void DoOwnership()
    {
        new OwnershipTest(socket, new UplayKit.Connection.OwnershipConnection(socket, Login.Ticket, Login.SessionId));
    }
    private void Socket_NewMessage(object? sender, DemuxEventArgs e)
    {
        Console.WriteLine("Socket_NewMessage fired!");
        Debug.WriteDebug(e.ToString()!, "[Socket_NewMessage]");
    }
}
