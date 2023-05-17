using ClientKit.Demux;
using ClientKit.Demux.Connection;
using SharedLib.Shared;

namespace ClientTester.DMX_Test
{
    internal class OwnershipTest
    {
        static Socket Socket;
        static OwnershipConnection OwnershipConnection;
        static List<Action> actions = new();
        public static void Run(Socket socket, OwnershipConnection ownershipConnection)
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

        static void Init()
        {
            if (!OwnershipConnection.IsConnectionClosed)
            {
                var rsp = OwnershipConnection.Initialize(new());
                if (OwnershipConnection.IsServiceSuccess && rsp != null && rsp.Success)
                {
                    Console.WriteLine("OwnershipConnection Init Success!");
                }
            }
        }

        static void OwnershipToken()
        {
            if (!OwnershipConnection.IsConnectionClosed)
            {
                var rsp = OwnershipConnection.GetOwnershipToken(0);
                if (OwnershipConnection.IsServiceSuccess && rsp.Expiration != 0)
                {
                    Console.WriteLine("OwnershipConnection GetOwnershipToken Success!");
                }
            }
        }

        static void DeprecatedGetProductFromCdKey()
        {
            if (!OwnershipConnection.IsConnectionClosed)
            {
                /*  Reason why we use this cdkey:
                        This is a first key that will be generated up on start with ProductId 0 and already used factor.
                 */
                var rsp = OwnershipConnection.DeprecatedGetProductFromCdKey("000-0000-0000-0000-0000");
                if (OwnershipConnection.IsServiceSuccess && rsp != null && rsp.Result == Uplay.Ownership.DeprecatedGetProductFromCdKeyRsp.Types.Result.UsedCdKeyOtherAccount)
                {
                    Console.WriteLine("OwnershipConnection .DeprecatedGetProductFromCdKey Success!");
                }
            }
        }

        static void UplayPCTicket()
        {
            if (!OwnershipConnection.IsConnectionClosed)
            {
                var rsp = OwnershipConnection.GetUplayPCTicket(0);
                if (OwnershipConnection.IsServiceSuccess && rsp != null)
                {
                    Console.WriteLine("OwnershipConnection GetUplayPCTicket Success!");
                }
            }
        }

        static void ClaimKeystorageKeys()
        {
            if (!OwnershipConnection.IsConnectionClosed)
            {                
                /*  Reason why we use this productIdList:
                        We made that ProductId 0 is already assigned by default. Ergo you cannot claim product that already owns
                 */
                var rsp = OwnershipConnection.ClaimKeystorageKeys(new List<uint>() { 0 });
                if (OwnershipConnection.IsServiceSuccess && rsp != null && rsp.Result == Uplay.Ownership.ClaimKeystorageKeyRsp.Types.Result.Failure)
                {
                    Console.WriteLine("OwnershipConnection ClaimKeystorageKeys Success!");
                }
            }
        }

        static void ProductConfig()
        {
            if (!OwnershipConnection.IsConnectionClosed)
            {
                var rsp = OwnershipConnection.GetProductConfig(0);
                if (OwnershipConnection.IsServiceSuccess && rsp != null)
                {
                    Console.WriteLine("OwnershipConnection GetProductConfig Success!");
                }
            }
        }

        static void UnlockProductBranch()
        {
            if (!OwnershipConnection.IsConnectionClosed)
            {
                var rsp = OwnershipConnection.UnlockProductBranch(0,string.Empty);
                if (OwnershipConnection.IsServiceSuccess && rsp != null)
                {
                    Console.WriteLine("OwnershipConnection UnlockProductBranch Success!");
                }
            }
        }

        static void fsds()
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
                if (OwnershipConnection.IsServiceSuccess && rsp != null)
                {
                    Console.WriteLine("OwnershipConnection UnlockProductBranch Success!");
                }
            }
        }

        private static void OwnershipConnection_PushEvent(object? sender, Uplay.Ownership.Push e)
        {
            Console.WriteLine("OwnershipConnection_PushEvent fired!");
            Debug.WriteDebug(e.ToString(), "[OwnershipConnection_PushEvent]");
        }
    }
}
