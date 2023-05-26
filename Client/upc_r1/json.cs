using Newtonsoft.Json;

namespace upc_r1
{
    internal class json
    {
        public class Base
        {
            [JsonProperty("ReqLog", NullValueHandling = NullValueHandling.Ignore)]
            public bool ReqLog { get; set; }

            [JsonProperty("RspLog", NullValueHandling = NullValueHandling.Ignore)]
            public bool RspLog { get; set; }

            [JsonProperty("PID", NullValueHandling = NullValueHandling.Ignore)]
            public int PID { get; set; }
        }

        public class ErrorToString
        {
            [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
            public int error { get; set; }
        }

        public class Init
        {
            [JsonProperty("ApiVersion", NullValueHandling = NullValueHandling.Ignore)]
            public uint ApiVersion { get; set; }

            [JsonProperty("UplayId", NullValueHandling = NullValueHandling.Ignore)]
            public uint UplayId { get; set; }

            [JsonProperty("Result", NullValueHandling = NullValueHandling.Ignore)]
            public string Result { get; set; }
        }

        public class Root
        {
            [JsonProperty("Base", NullValueHandling = NullValueHandling.Ignore)]
            public Base Base { get; set; }

            [JsonProperty("ErrorToString", NullValueHandling = NullValueHandling.Ignore)]
            public ErrorToString ErrorToString { get; set; }

            [JsonProperty("Init", NullValueHandling = NullValueHandling.Ignore)]
            public Init Init { get; set; }
        }

    }
}
