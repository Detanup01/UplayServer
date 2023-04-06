using System.Diagnostics;
using System.IO.Pipes;
using System.Reflection.Metadata;
using Google.Protobuf;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2
{
    public class Basics
    {
        [UnmanagedCallersOnly(EntryPoint = "Test", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int TEST()
        {
            SendReq(new Uplay.Uplaydll.Req() {  LaunchAppReq = new() { ProductId = 0 } }, out var rsp);
            return 0;
        }

        public static void Log(string actionName, object[] parameters)
        {
            File.AppendAllText("upc_r2.log",$"{Process.GetCurrentProcess().Id} | {actionName} {string.Join(", ",parameters)}\n");
        }

        public static void LogReq(Uplay.Uplaydll.Req req)
        {
            File.AppendAllText("upc_r2_req.log", $"{req.ToString()}\n");
        }

        public static void LogRsp(Uplay.Uplaydll.Rsp rsp)
        {
            File.AppendAllText("upc_r2_rsp.log", $"{rsp.ToString()}\n");
        }

        public static uint ReqId = uint.MinValue;

        public static void SendReq(Uplay.Uplaydll.Req req, out Uplay.Uplaydll.Rsp rsp)
        {
            if (IfReqLog())
            {
                LogReq(req);
            }

            Uplay.Demux.Upstream upstream = new()
            {
                Request = new()
                {
                    RequestId = ReqId,
                    ServiceRequest = new()
                    { 
                        Service = "uplaydll",
                        Data = ByteString.CopyFrom(req.ToByteArray())
                    }
                }
            };
            ReqId++;
            rsp = new();
            NamePipe.NamePipeReqRsp(upstream, out rsp);
            if (IfRspLog())
            {
                LogRsp(rsp);
            }
            Log("SendReq", new object[] { "Done!" });
        }

        public static bool IfReqLog()
        {
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<json.Root>(File.ReadAllText("upc.json"));
            if (data != null && data.Base.ReqLog)
            {
                return data.Base.ReqLog;
            }
            else return false;
        }

        public static bool IfRspLog()
        {
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<json.Root>(File.ReadAllText("upc.json"));
            if (data != null && data.Base.RspLog)
            {
                return data.Base.RspLog;
            }
            else return false;
        }
    }
}