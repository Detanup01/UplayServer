using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports;

internal unsafe class User
{
    #region Stuff

    public class UPC_Presence
    {
        public Uplay.Uplaydll.OnlineStatusV2 onlineStatus;
        public string detailsUtf8 = string.Empty;
        public uint titleId;
        public string titleNameUtf8 = string.Empty;
        public string multiplayerId = string.Empty;
        public int multiplayerJoinable;
        public uint multiplayerSize;
        public uint multiplayerMaxSize;
        public byte[] multiplayerInternalData = [];
    }

    public class UPC_User
    {
        public string idUtf8 = string.Empty;
        public string nameUtf8 = string.Empty;
        public Uplay.Uplaydll.Relationship relationship;
        public UPC_Presence presence = new();
    }

    public static UPC_PresenceImpl BuildFrom(UPC_Presence presence)
    {
        UPC_PresenceImpl impl = new();
        impl.onlineStatus = (uint)presence.onlineStatus;
        impl.detailsUtf8 = Marshal.StringToHGlobalAnsi(presence.detailsUtf8);
        impl.titleId = presence.titleId;
        impl.titleNameUtf8 = Marshal.StringToHGlobalAnsi(presence.titleNameUtf8);
        impl.multiplayerId = Marshal.StringToHGlobalAnsi(presence.multiplayerId);
        impl.multiplayerJoinable = presence.multiplayerJoinable;
        impl.multiplayerSize = presence.multiplayerSize;
        impl.multiplayerMaxSize = presence.multiplayerMaxSize;
        impl.multiplayerInternalDataSize = (uint)presence.multiplayerInternalData.Length;
        var ptr = Marshal.AllocHGlobal(sizeof(byte) * (int)presence.multiplayerInternalData.Length);
        Marshal.Copy(presence.multiplayerInternalData, 0, ptr, presence.multiplayerInternalData.Length);
        impl.multiplayerInternalData = ptr;
        return impl;
    }
    public static UPC_UserImpl BuildFrom(UPC_User upc_User)
    {
        UPC_UserImpl impl = new();
        impl.idUtf8 = Marshal.StringToHGlobalAnsi(upc_User.idUtf8);
        impl.nameUtf8 = Marshal.StringToHGlobalAnsi(upc_User.nameUtf8);
        impl.relationship = (uint)upc_User.relationship;
        var presetimpl = BuildFrom(upc_User.presence);
        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(impl));
        Marshal.StructureToPtr(presetimpl, ptr, false);
        impl.presence = ptr;
        return impl;
    }
    #endregion

    [UnmanagedCallersOnly(EntryPoint = "UPC_UserGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_UserGet(IntPtr inContext, IntPtr inOptUserIdUtf8, IntPtr outUser, IntPtr inCallback, IntPtr inCallbackData)
    {
        Basics.Log(nameof(UPC_UserGet), [inContext, inOptUserIdUtf8, outUser, inCallback, inCallbackData]);
        Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, 0));

        UPC_User user = new();
        user.idUtf8 = Main.GlobalContext.Config.Saved.account.AccountId;
        user.nameUtf8 = Main.GlobalContext.Config.Saved.account.NameOnPlatform;
        user.relationship = Uplay.Uplaydll.Relationship.Friend;
        user.presence = new()
        {
            onlineStatus = Uplay.Uplaydll.OnlineStatusV2.OnlineStatusOnline,
            multiplayerSize = 0,
            multiplayerMaxSize = 0,
            detailsUtf8 = "yeet",
            multiplayerId = "yeet",
            multiplayerInternalData = [0x0, 0x1, 0x00, 0xAA],
            multiplayerJoinable = 1,
            titleId = 0,
            titleNameUtf8 = "yeet"
        };
        var impl = BuildFrom(user);
        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<UPC_UserImpl>());
        Marshal.StructureToPtr(impl, ptr, false);
        Marshal.WriteIntPtr(outUser, ptr);
        return 1000;
    }
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_UserFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_UserFree(IntPtr inContext, IntPtr inUser)
    {
        Basics.Log(nameof(UPC_UserFree), [inContext, inUser]);
        var user = Marshal.PtrToStructure<UPC_UserImpl>(inUser);
        var pers = Marshal.PtrToStructure<UPC_PresenceImpl>(user.presence);
        Marshal.FreeHGlobal(pers.multiplayerInternalData);
        Marshal.DestroyStructure<UPC_PresenceImpl>(user.presence);
        Marshal.FreeHGlobal(user.presence);
        Marshal.DestroyStructure<UPC_UserImpl>(inUser);
        Marshal.FreeHGlobal(inUser);
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
