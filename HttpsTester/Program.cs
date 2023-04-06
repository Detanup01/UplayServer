using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace HTTPSTESTER
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("req URL:");
            var reqURL = Console.ReadLine();
            var httpRequest = (HttpWebRequest)WebRequest.Create(reqURL);
            Console.WriteLine("Method");
            var method = Console.ReadLine();
            httpRequest.Method = method;
            httpRequest.Headers["Ubi-AppId"] = "685a3038-2b04-47ee-9c5a-6403381a46aa";
            httpRequest.Headers["GenomeId"] = "487091d6-a285-471c-9036-d4bce349f212";
            httpRequest.Headers["Authorization"] = "Basic TEST";
            Console.WriteLine("header");
            var header = Console.ReadLine();
            httpRequest.Headers["TEST"] = header;
            httpRequest.Headers["Ubi-RequestedPlatformType"] = "uplay";
            httpRequest.ContentType = "application/json";
            httpRequest.ServerCertificateValidationCallback = serverCertificateValidationCallback;

            if (method == "POST")
            {
                Console.WriteLine("data");
                var data = Console.ReadLine();

                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(data);
                }

            }


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            var result = "";

            if (httpResponse.ContentType.Contains("stream"))
            {
                MemoryStream ms = new();
                httpResponse.GetResponseStream().CopyTo(ms);
                File.WriteAllBytes("test", ms.ToArray());
            }
            else
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                Console.WriteLine(result);
            }

        }
        private static bool serverCertificateValidationCallback(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}