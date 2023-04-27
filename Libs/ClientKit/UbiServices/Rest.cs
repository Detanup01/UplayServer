using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SharedLib.Shared;
using System.Net;

namespace ClientKit.UbiServices
{
    public class Rest
    {
        #region JObject
        public static JObject? Put(RestClient client, RestRequest request)
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.PutAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.Content != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return JObject.Parse(response.Content);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }

        public static JObject? Post(RestClient client, RestRequest request)
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.PostAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.Content != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return JObject.Parse(response.Content);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }

        public static JObject? Get(RestClient client, RestRequest request)
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.GetAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.Content != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return JObject.Parse(response.Content);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }
        #endregion
        #region T Class
        public static T? Put<T>(RestClient client, RestRequest request) where T : class
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.PutAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.Content != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }

        public static T? Post<T>(RestClient client, RestRequest request) where T : class
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.PostAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.Content != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }

        public static T? Get<T>(RestClient client, RestRequest request) where T : class
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.GetAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.Content != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }
        #endregion
        #region StatusCode
        public static HttpStatusCode? Delete(RestClient client, RestRequest request)
        {
            try
            {
                RestResponse response = client.DeleteAsync(request).Result;
                return response.StatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }
        #endregion
        #region String
        public static string? GetString(RestClient client, RestRequest request)
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.GetAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.Content != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return response.Content;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }

        public static string? PostString(RestClient client, RestRequest request)
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.PostAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.Content != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return response.Content;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }

        public static string? PutString(RestClient client, RestRequest request)
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.PutAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.Content != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return response.Content;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }

        public static string? DeleteString(RestClient client, RestRequest request)
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.DeleteAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.Content != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return response.Content;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }
        #endregion
        #region Bytes
        public static byte[]? GetBytes(RestClient client, RestRequest request)
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.GetAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.RawBytes != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return response.RawBytes;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }

        public static byte[]? PostBytes(RestClient client, RestRequest request)
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.PostAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.RawBytes != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return response.RawBytes;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }

        public static byte[]? PutBytes(RestClient client, RestRequest request)
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.PutAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.RawBytes != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return response.RawBytes;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }

        public static byte[]? DeleteBytes(RestClient client, RestRequest request)
        {
            try
            {
                Debug.WriteDebug(JsonConvert.SerializeObject(request), "ubiservice_rest.txt");
                RestResponse response = client.DeleteAsync(request).Result;
                Debug.WriteDebug(JsonConvert.SerializeObject(response), "ubiservice_rest.txt");
                if (response.RawBytes != null)
                {
                    Console.WriteLine(response.StatusCode);
                    return response.RawBytes;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Ex.Handler(ex, "UbiServices");
                return null;
            }
        }
        #endregion
    }
}
