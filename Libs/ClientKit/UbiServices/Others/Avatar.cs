using RestSharp;
using SharedLib.Shared;

namespace ClientKit.UbiServices.Others
{
    public class Avatar
    {
        /// <summary>
        /// Get Avatar
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <param name="size">256</param>
        /// <returns>Byte Array or null</returns>
        public static byte[]? GetAvatar(string UserId, string size)
        {

            var client = new RestClient(Urls.GetUrl($"download/avatars/{UserId}/default_{size}_{size}.png"));
            var request = new RestRequest();

            try
            {
                RestResponse response = client.GetAsync(request).Result;
                if (response.Content != null)
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
    }
}
