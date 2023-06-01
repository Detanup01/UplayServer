using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports
{
    internal class Achis
    {
        [UnmanagedCallersOnly(EntryPoint = "UPC_AchievementImageFree", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_AchievementImageFree(IntPtr inContext, IntPtr inImageRGBA)
        {
            Basics.Log(nameof(UPC_AchievementImageFree), new object[] { inContext, inImageRGBA });
            return 0;
        }

        [UnmanagedCallersOnly(EntryPoint = "UPC_AchievementImageGet", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_AchievementImageGet(IntPtr inContext, uint inId, IntPtr outImageRGBA, IntPtr inCallback, IntPtr inCallbackData)
        {
            Basics.Log(nameof(UPC_AchievementImageGet), new object[] { inContext, inId, outImageRGBA, inCallback, inCallbackData });
            return 0;
        }

        [UnmanagedCallersOnly(EntryPoint = "UPC_AchievementListFree", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_AchievementListFree(IntPtr inContext, IntPtr inAchievementList)
        {
            Basics.Log(nameof(UPC_AchievementListFree), new object[] { inContext, inAchievementList });
            return 0;
        }

        [UnmanagedCallersOnly(EntryPoint = "UPC_AchievementListGet", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_AchievementListGet(IntPtr inContext, IntPtr inOptUserIdUtf8, uint inFilter, IntPtr outAchievementList, IntPtr inCallback, IntPtr inCallbackData)
        {
            Basics.Log(nameof(UPC_AchievementListGet), new object[] { inContext, inOptUserIdUtf8, outAchievementList, inCallback, inCallbackData });
            var cbList = Main.GlobalContext.Callbacks.ToList();
            cbList.Add(new(inCallback, inCallbackData, -4));
            Main.GlobalContext.Callbacks = cbList.ToArray();
            return 0x200;
        }

        [UnmanagedCallersOnly(EntryPoint = "UPC_AchievementUnlock", CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_AchievementUnlock(IntPtr inContext, uint inId, IntPtr inOptCallback, IntPtr inOptCallbackData)
        {
            Basics.Log(nameof(UPC_AchievementUnlock), new object[] { inContext, inId, inOptCallback, inOptCallbackData });
            return 0;
        }
    }
}
