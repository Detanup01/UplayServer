using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports;

internal class User
{
    [UnmanagedCallersOnly(EntryPoint = "UPC_UserGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_UserGet(IntPtr inContext, IntPtr inOptUserIdUtf8, IntPtr outUser, IntPtr inCallback, IntPtr inCallbackData)
    {
        Basics.Log(nameof(UPC_UserGet), [inContext, inOptUserIdUtf8, outUser, inCallback, inCallbackData]);
        Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, 0));

        UPC_User user = new()
        {
            idUtf8 = Main.GlobalContext.Config.Saved.account.AccountId,
            nameUtf8 = Main.GlobalContext.Config.Saved.account.NameOnPlatform,
            relationship = Uplay.Uplaydll.Relationship.None,
            presence = new()
            {
                onlineStatus = Uplay.Uplaydll.OnlineStatusV2.OnlineStatusOnline,
                multiplayerSize = 1,
                multiplayerMaxSize = 1,
                detailsUtf8 = $"Playing {Main.GlobalContext.Config}",
                multiplayerId = Guid.NewGuid().ToString(),
                multiplayerInternalData = [],
                multiplayerJoinable = 0, // Disable joinable lobbies
                titleId = Main.GlobalContext.Config.ProductId,
                titleNameUtf8 = $"Product {Main.GlobalContext.Config}"
            }
        };
        var impl = UPC_UserImpl.BuildFrom(user);
        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<UPC_UserImpl>());
        Marshal.StructureToPtr(impl, ptr, false);
        Marshal.WriteIntPtr(outUser, ptr);
        return 0x10000;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_UserFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_UserFree(IntPtr inContext, IntPtr inUser)
    {
        Basics.Log(nameof(UPC_UserFree), [inContext, inUser]);
        if (inUser == IntPtr.Zero)
            return 0;
        var user = Marshal.PtrToStructure<UPC_UserImpl>(inUser);
        UPC_UserImpl.Free(user);
        Marshal.DestroyStructure<UPC_UserImpl>(inUser);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_UserPlayedWithAdd", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_UserPlayedWithAdd(IntPtr inContext, IntPtr inUserIdUtf8List, uint inListLength)
    {
        Basics.Log(nameof(UPC_UserPlayedWithAdd), [inContext, inUserIdUtf8List, inListLength]);
        return 0;
    }


    [UnmanagedCallersOnly(EntryPoint = "UPC_UserPlayedWithAdd_Extended", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_UserPlayedWithAdd_Extended(IntPtr inContext, IntPtr inUserIdUtf8List, uint inListLength, IntPtr unk1, IntPtr unk2)
    {
        Basics.Log(nameof(UPC_UserPlayedWithAdd_Extended), [inContext, inUserIdUtf8List, inListLength, unk1, unk2]);
        return 0;
    }
}
