using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace upc_r2.Exports
{
    internal class Other
    {
        [UnmanagedCallersOnly(EntryPoint = "UPC_CPUScoreGet", CallConvs = [typeof(CallConvCdecl)])]
        public static int UPC_CPUScoreGet(IntPtr inContext, IntPtr outScore)
        {
            Basics.Log(nameof(UPC_CPUScoreGet), [inContext, outScore]);
            Marshal.WriteInt32(outScore, 0x1000);
            return 0;
        }

        [UnmanagedCallersOnly(EntryPoint = "UPC_GPUScoreGet", CallConvs = [typeof(CallConvCdecl)])]
        public static int UPC_GPUScoreGet(IntPtr inContext, IntPtr outScore, IntPtr outConfidenceLevel)
        {
            Basics.Log(nameof(UPC_GPUScoreGet), [inContext, outScore, outConfidenceLevel]);
            Marshal.WriteInt32(outScore, 0x1000);
            Marshal.WriteInt64(outConfidenceLevel, (long)0.1f);
            return 0;
        }

        [UnmanagedCallersOnly(EntryPoint = "UPC_ApplicationIdGet", CallConvs = [typeof(CallConvCdecl)])]
        public static int UPC_ApplicationIdGet(IntPtr inContext, IntPtr outAppId)
        {
            Basics.Log(nameof(UPC_ApplicationIdGet), [inContext]);
            Marshal.WriteIntPtr(outAppId, Marshal.StringToHGlobalAnsi(Main.GlobalContext.Config.Saved.ApplicationId));
            return 0;
        }
        public struct UPC_RichPresenceToken
        {
            [MarshalAs(UnmanagedType.LPUTF8Str)]
            public string idUtf8 = string.Empty;
            [MarshalAs(UnmanagedType.LPUTF8Str)]
            public string valueIdUtf8 = string.Empty;

            public UPC_RichPresenceToken()
            {

            }
        }
        [UnmanagedCallersOnly(EntryPoint = "UPC_RichPresenceSet", CallConvs = [typeof(CallConvCdecl)])]
        public static int UPC_RichPresenceSet(IntPtr inContext, uint inId, IntPtr inOptTokenList)
        {
            Basics.Log(nameof(UPC_RichPresenceSet), [inContext, inId, inOptTokenList]);

            var list = Basics.IntPtrToStruct<BasicList>(inOptTokenList);
            Basics.Log(nameof(UPC_RichPresenceSet), [JsonSerializer.Serialize(list, JsonSourceGen.Default.BasicList)]);
            try
            {
                for (int i = 0; i < list.count; i++)
                {
                   
                    var ptr = Marshal.ReadIntPtr(list.list, i * Marshal.SizeOf<UPC_RichPresenceToken>());
                    var token = Basics.IntPtrToStruct<UPC_RichPresenceToken>(ptr);
                    Basics.Log(nameof(UPC_RichPresenceSet), [JsonSerializer.Serialize(token, JsonSourceGen.Default.UPC_RichPresenceToken)]);
                }
            }
            catch (Exception ex)
            {
                Basics.Log(nameof(UPC_RichPresenceSet), [ex]);
            }
            return 0;
        }

        [UnmanagedCallersOnly(EntryPoint = "UPC_RichPresenceSet_Extended", CallConvs = [typeof(CallConvCdecl)])]
        public static int UPC_RichPresenceSet_Extended(IntPtr inContext, uint inId, IntPtr inOptTokenList, IntPtr unk1, IntPtr unk2)
        {
            Basics.Log(nameof(UPC_RichPresenceSet_Extended), [inContext, inId, inOptTokenList]);

            var list = Basics.IntPtrToStruct<BasicList>(inOptTokenList);
            Basics.Log(nameof(UPC_RichPresenceSet_Extended), [JsonSerializer.Serialize(list, JsonSourceGen.Default.BasicList)]);
            try
            {
                for (int i = 0; i < list.count; i++)
                {

                    var ptr = Marshal.ReadIntPtr(list.list, i * Marshal.SizeOf<UPC_RichPresenceToken>());
                    var token = Basics.IntPtrToStruct<UPC_RichPresenceToken>(ptr);
                    Basics.Log(nameof(UPC_RichPresenceSet_Extended), [JsonSerializer.Serialize(token, JsonSourceGen.Default.UPC_RichPresenceToken)]);
                }
            }
            catch (Exception ex)
            {
                Basics.Log(nameof(UPC_RichPresenceSet_Extended), [ex]);
            }
            return 0;
        }

        [UnmanagedCallersOnly(EntryPoint = "UPC_LaunchApp", CallConvs = [typeof(CallConvCdecl)])]
        public static int UPC_LaunchApp(IntPtr inContext, uint inProductId, IntPtr MustBeZero)
        {
            Basics.Log(nameof(UPC_LaunchApp), [inContext, inProductId, MustBeZero]);
            return 1;
        }

        [UnmanagedCallersOnly(EntryPoint = "UPC_ErrorToString", CallConvs = [typeof(CallConvCdecl)])]
        public static IntPtr UPC_ErrorToString(int error)
        {
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
            Basics.Log(nameof(UPC_ErrorToString), [error, switch_ret]);
            return ret;
        }

        [UnmanagedCallersOnly(EntryPoint = "UPC_IsCrossBootAllowed", CallConvs = [typeof(CallConvCdecl)])]
        public static int UPC_IsCrossBootAllowed(IntPtr inContext, uint inProductId, IntPtr outIsCrossBootAllowed, IntPtr unk1, IntPtr unk2)
        {
            Basics.Log(nameof(UPC_IsCrossBootAllowed), [inContext, inProductId, outIsCrossBootAllowed, unk1, unk2]);
            var mem = Marshal.AllocHGlobal(1);
            Marshal.WriteByte(mem, 0);
            Marshal.WriteIntPtr(outIsCrossBootAllowed, 0, mem);
            return 0;
        }
    }
}
