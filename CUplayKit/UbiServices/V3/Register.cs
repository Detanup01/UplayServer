using CUplayKit.UbiServices.Records;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace CUplayKit.UbiServices.Public
{
    public partial class V3
    {
        public static readonly string URL_Register = Urls.GetUrl("v3/profiles/register");

        public static RegisterResponse? Register(string email, string password, string username)
        {
            string b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{email}:{password}"));
            var client = new RestClient(URL_Register);
            var request = new RestRequest();
            Console.WriteLine(URL_Register);
            request.AddHeader("Authorization", $"Basic {b64}");
            request.AddHeader("User-Agent", UserAgent);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Ubi-RequestedPlatformType", "uplay");
            request.AddHeader("Ubi-AppId", AppID);
            RegisterBody rem = new()
            {
                Name = username
            };
            request.AddBody(JsonConvert.SerializeObject(rem), ContentType.Json);

            return Rest.Post<RegisterResponse>(client, request);
        }

        public class RegisterBody
        {
            public string Name;
        }
        public class RegisterResponse
        {
            public string UserId;
        }
    }
}
