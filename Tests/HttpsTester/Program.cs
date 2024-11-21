namespace HTTPSTESTER
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("req URL:");
            var reqURL = Console.ReadLine();

            HttpClient httpClient = new();
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Parse(Console.ReadLine()!), reqURL);
            httpRequestMessage.Headers.Add("Ubi-AppId", "685a3038-2b04-47ee-9c5a-6403381a46aa");
            httpRequestMessage.Headers.Add("GenomeId", "487091d6-a285-471c-9036-d4bce349f212");
            httpRequestMessage.Headers.Add("Authorization", "sasd");
            httpRequestMessage.Headers.Add("TEST", Console.ReadLine());
            httpRequestMessage.Headers.Add("Ubi-RequestedPlatformType", "uplay");
            httpRequestMessage.Headers.Add("Content-Type", "application/json");
            if (httpRequestMessage.Method.Method == "POST")
            {
                Console.WriteLine("data");
                var data = Console.ReadLine();
                httpRequestMessage.Content = new StringContent(data!);

            }
            var rsp = httpClient.Send(httpRequestMessage);

            if (rsp.Content.Headers.ContentType != null && rsp.Content.Headers.ContentType.ToString().Contains("stream"))
            {
                MemoryStream ms = new();
                rsp.Content.ReadAsStream().CopyTo(ms);
                File.WriteAllBytes("test", ms.ToArray());
            }
            else
            {
                string result = string.Empty;
                using (var streamReader = new StreamReader(rsp.Content.ReadAsStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                Console.WriteLine(result);
            }

        }
    }
}