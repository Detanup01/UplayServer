using Newtonsoft.Json;
using UbiServices.Public;

namespace ClientTester;

internal class Program
{
    static void Main(string[] args)
    {
        ServiceTester.Run();
        var reg = V3.CreateAccount("publictester@test.com", "publictester", "2000-01-01", "publictester", "EU", "US", "-");
        if (reg != null)
        {
            Console.WriteLine("OK! " + JsonConvert.SerializeObject(reg));
        }
        else
        {
            Console.WriteLine("Register was not success!");
        }
        var login = V3.Login("publictester", "publictester");
        if (login != null)
        {
            Console.WriteLine("OK! Ticket");
            new DemuxTest(login);
        }
        else
        {
            Console.WriteLine("Login was not success!");
        }
    }
}