using RestSharp;
using SharedLib.Shared;

namespace ClientKit.UbiServices.Others
{
    public class Asset
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static byte[]? GetAsset(string asset)
        {

            var client = new RestClient(Urls.GetUrl($"download/launcher/assets/{asset}"));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetpath">For example promotions/promo_config.yml</param>
        /// <returns></returns>
        public static byte[]? GetAssetPath(string assetpath)
        {

            var client = new RestClient(Urls.GetUrl($"download/launcher/{assetpath}"));
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
