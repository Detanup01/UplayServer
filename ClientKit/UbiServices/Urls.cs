namespace ClientKit.UbiServices
{
    public class Urls
    {
        public static readonly string Local_Base = "https://local-ubiservices.ubi.com:7777/";
        public static readonly string Public_Base = "https://public-ubiservices.ubi.com/";

        public static bool IsLocalTest = true;

        public static string GetUrl(string UrlEnd)
        {
            if (IsLocalTest)
                return Local_Base + UrlEnd;
            return Public_Base + UrlEnd;
        }

    }
}
