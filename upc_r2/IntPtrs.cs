using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static upc_r2.Basics;

namespace upc_r2
{
    internal class IntPtrs
    {
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
            Log(nameof(UPC_ErrorToString), new object[] { error });
            return ret;
        }
    }
}
