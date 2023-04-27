using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace dummydll
{
    public class dummy
    {

        [UnmanagedCallersOnly(EntryPoint = "_yeet", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int _()
        {
            return 1337;
        }
    }
}