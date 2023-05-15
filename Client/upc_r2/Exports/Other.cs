using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports
{
    internal class Other
    {
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_CPUScoreGet(IntPtr inContext, IntPtr outScore)
        {
            Basics.Log(nameof(UPC_CPUScoreGet), new object[] { inContext, outScore });
            Marshal.WriteInt32(outScore, 0x1000);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_GPUScoreGet(IntPtr inContext, IntPtr outScore, IntPtr outConfidenceLevel)
        {
            Basics.Log(nameof(UPC_CPUScoreGet), new object[] { inContext, outScore, outConfidenceLevel });
            Marshal.WriteInt32(outScore, 0x1000);
            Marshal.WriteInt64(outConfidenceLevel, (long)0.1f);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_ApplicationIdGet(IntPtr inContext, IntPtr outAppId)
        {
            Basics.Log(nameof(UPC_ApplicationIdGet), new object[] { inContext });
            var str = Marshal.StringToCoTaskMemAnsi(Main.GlobalContext.Config.AppId.ToString());
            Marshal.WriteIntPtr(outAppId, str);
            return 0;
        }

        [UnmanagedCallersOnly(EntryPoint = "UPC_ErrorToString", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr UPC_ErrorToString(int error)
        {
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<json.Root>(File.ReadAllText("upc.json"));
            data.Base.PID = Process.GetCurrentProcess().Id;
            data.ErrorToString.error = error;
            File.WriteAllText("upc.json", Newtonsoft.Json.JsonConvert.SerializeObject(data));
            string switch_ret = "get";
            switch (error)
            {
                default:
                    switch_ret = "Unknown error";
                    break;
                case -14:
                    switch_ret = "Unavailable";
                    break;
                case -13:
                    switch_ret = "Failed precondition";
                    break;
                case -11:
                    switch_ret = "Operation aborted";
                    break;
                case -10:
                    switch_ret = "Internal error";
                    break;
                case -9:
                    switch_ret = "Unauthorized action";
                    break;
                case -8:
                    switch_ret = "Limit reached";
                    break;
                case -7:
                    switch_ret = "End of file";
                    break;
                case -6:
                    switch_ret = "Not found";
                    break;
                case -5:
                    switch_ret = "Memory error";
                    break;
                case -4:
                    switch_ret = "Communication error";
                    break;
                case -3:
                    switch_ret = "Uninitialized subsystem";
                    break;
                case -2:
                    switch_ret = "Invalid arguments";
                    break;
                case -1:
                    switch_ret = "Declined";
                    break;
            }

            var ret = Marshal.StringToHGlobalAnsi(switch_ret);
            Basics.Log(nameof(UPC_ErrorToString), new object[] { error });
            return ret;
        }
    }
}
