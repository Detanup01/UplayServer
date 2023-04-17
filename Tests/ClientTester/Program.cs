using ClientKit.UbiServices.Public;

namespace ClientTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceTester.Run();
            var reg = V3.Register("publictester", "publictester", "publictester");
            if (reg != null)
            {
                var UserID = reg.UserId;
                Console.WriteLine("OK! " + UserID);
            }
            else
            {
                Console.WriteLine("Register was not success!");
            }
            var login = V3.Login("publictester", "publictester");
            if (login != null)
            {
                Console.WriteLine("OK! Ticket");
                DemuxTest.Run(login);
            }
            else
            {
                Console.WriteLine("Login was not success!");
            }
        }
    }
}