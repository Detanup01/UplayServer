using Newtonsoft.Json;
using SharedLib.Server.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.HTTP
{
    internal class CloudSave
    {
        public static string GET(string URL, Dictionary<string, string> headers, out string contentType)
        {
            contentType = "application/json; charset=UTF-8";
            return "{\"ok\":true}";
        }

        public static string PUT(string URL, Dictionary<string, string> headers, byte[] body, out string contentType)
        {
            contentType = "text/plain; charset=utf-8";
            return "OK!";
        }

        public static bool DELETE(string URL, Dictionary<string, string> headers, out string errorRSP)
        {
            errorRSP = "application/json; charset=UTF-8";
            return false;
        }
    }
}
