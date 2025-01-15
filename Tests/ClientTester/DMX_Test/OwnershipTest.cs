using UplayKit.Connection;
using SharedLib.Shared;
using UplayKit;

namespace ClientTester.DMX_Test;

internal class OwnershipTest
{
    DemuxSocket Socket;
    OwnershipConnection OwnershipConnection;
    List<Action> actions = new();
    public OwnershipTest(DemuxSocket socket, OwnershipConnection ownershipConnection)
    {
        Socket = socket; 
        OwnershipConnection = ownershipConnection;
        OwnershipConnection.PushEvent += OwnershipConnection_PushEvent;
        actions.Add(Init);
        actions.ForEach(x => x());
        OwnershipConnection.PushEvent -= OwnershipConnection_PushEvent;
        OwnershipConnection.Close();
        Console.WriteLine("OwnershipConnection Done!");
    }

    void Init()
    {
        if (!OwnershipConnection.IsConnectionClosed)
        {
            var rsp = OwnershipConnection.Initialize();
            if (rsp != null && rsp.Success)
            {
                Console.WriteLine("OwnershipConnection Init Success!");
            }
        }
    }

    void OwnershipToken()
    {
        if (!OwnershipConnection.IsConnectionClosed)
        {
            var rsp = OwnershipConnection.GetOwnershipToken(0);
            if (rsp.Item2 != 0)
            {
                Console.WriteLine("OwnershipConnection GetOwnershipToken Success!");
            }
        }
    }

    void DeprecatedGetProductFromCdKey()
    {
        if (!OwnershipConnection.IsConnectionClosed)
        {
            /*  Reason why we use this cdkey:
                    This is a first key that will be generated up on start with ProductId 0 and already used factor.
             */
            var rsp = OwnershipConnection.DeprecatedGetProductFromCdKey("000-0000-0000-0000-0000");
            if (rsp != null && rsp.Result == Uplay.Ownership.DeprecatedGetProductFromCdKeyRsp.Types.Result.UsedCdKeyOtherAccount)
            {
                Console.WriteLine("OwnershipConnection .DeprecatedGetProductFromCdKey Success!");
            }
        }
    }

    void UplayPCTicket()
    {
        if (!OwnershipConnection.IsConnectionClosed)
        {
            var rsp = OwnershipConnection.GetUplayPCTicket(0);
            if (rsp != null)
            {
                Console.WriteLine("OwnershipConnection GetUplayPCTicket Success!");
            }
        }
    }

    void ClaimKeystorageKeys()
    {
        if (!OwnershipConnection.IsConnectionClosed)
        {                
            /*  Reason why we use this productIdList:
                    We made that ProductId 0 is already assigned by default. Ergo you cannot claim product that already owns
             */
            var rsp = OwnershipConnection.ClaimKeystorageKeys(new List<uint>() { 0 });
            if (rsp != null && rsp.Result == Uplay.Ownership.ClaimKeystorageKeyRsp.Types.Result.Failure)
            {
                Console.WriteLine("OwnershipConnection ClaimKeystorageKeys Success!");
            }
        }
    }

    void ProductConfig()
    {
        if (!OwnershipConnection.IsConnectionClosed)
        {
            var rsp = OwnershipConnection.GetProductConfig(0);
            if (rsp != null)
            {
                Console.WriteLine("OwnershipConnection GetProductConfig Success!");
            }
        }
    }

    void UnlockProductBranch()
    {
        if (!OwnershipConnection.IsConnectionClosed)
        {
            var rsp = OwnershipConnection.UnlockProductBranch(0,string.Empty);
            if (rsp != null)
            {
                Console.WriteLine("OwnershipConnection UnlockProductBranch Success!");
            }
        }
    }

    void fsds()
    {
        if (!OwnershipConnection.IsConnectionClosed)
        {
            var rsp = OwnershipConnection.SendRequest(new Uplay.Ownership.Req()
            { 
                RequestId = Socket.RequestId,
                SignOwnershipReq = new()
                { 
                    ProductId = 0
                }
            
            });
            if (rsp != null)
            {
                Console.WriteLine("OwnershipConnection UnlockProductBranch Success!");
            }
        }
    }

    private void OwnershipConnection_PushEvent(object? sender, Uplay.Ownership.Push e)
    {
        Console.WriteLine("OwnershipConnection_PushEvent fired!");
        Debug.WriteDebug(e.ToString(), "[OwnershipConnection_PushEvent]");
    }
}
