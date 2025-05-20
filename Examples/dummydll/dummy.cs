using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace dummydll
{
    public unsafe class Dummy
    {

        [UnmanagedCallersOnly(EntryPoint = "test", CallConvs = [typeof(CallConvCdecl)])]
        public static int test()
        {
            return 1337;
        }
    }
}