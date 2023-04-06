using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Uplay.Uplaydll;
using static upc_r2.Enums;

namespace upc_r2
{
    public class Results
    {
        [UnmanagedCallersOnly(EntryPoint = "UPC_Init", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static uint UPC_Init(uint inVersion, uint uplayId)
        {
            var id = Environment.CurrentManagedThreadId;
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<json.Root>(File.ReadAllText("upc.json"));
            data.Base.PID = id;
            data.Init.ApiVersion = inVersion;
            data.Init.ApiVersion = uplayId;
            File.WriteAllText("upc.json", Newtonsoft.Json.JsonConvert.SerializeObject(data));
            Basics.Log(nameof(UPC_Init), new object[] { inVersion, uplayId });

            Req req = new();
            {

                new InitProcessReq()
                {
                    UplayId = uplayId,
                    ApiVersion = inVersion,
                    ProcessId = (uint)id,
                    UplayEnvIsSet = false
                };
            };

            Basics.SendReq(req, out Rsp rsp);
            var iprsp = rsp.InitProcessRsp;

            data.Init.Result = iprsp.Result.ToString();
            File.WriteAllText("upc.json", Newtonsoft.Json.JsonConvert.SerializeObject(data));

            switch (iprsp.Result)
            {
                default:
                    return (uint)UPC_InitResult.UPC_InitResult_Failed;
                case InitResult.Success:
                    return (uint)UPC_InitResult.UPC_InitResult_Ok;
                case InitResult.RestartWithGameLauncherRequired:
                    return (uint)UPC_InitResult.UPC_InitResult_DesktopInteractionRequired;
                case InitResult.ReconnectRequired:
                    return (uint)UPC_InitResult.UPC_InitResult_DesktopInteractionRequired;
            }
        }


        [UnmanagedCallersOnly(EntryPoint = "UPC_Update", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static uint UPC_Update()
        {


            return 0;
        }
    }
}
