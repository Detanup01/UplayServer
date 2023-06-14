using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports
{
    internal unsafe class User
    {
        #region Stuff
        struct UPC_PresenceImpl
        {
            public static UPC_PresenceImpl BuildFrom(UPC_Presence presence)
            {
                UPC_PresenceImpl impl = new();
                impl.onlineStatus = presence.onlineStatus;
                impl.detailsUtf8 = presence.detailsUtf8;
                impl.titleId = presence.titleId;
                impl.titleNameUtf8 = presence.titleNameUtf8;
                impl.multiplayerId = presence.multiplayerId;
                impl.multiplayerJoinable = presence.multiplayerJoinable;
                impl.multiplayerSize = presence.multiplayerSize;
                impl.multiplayerMaxSize = presence.multiplayerMaxSize;
                impl.multiplayerInternalDataSize = (uint)presence.multiplayerInternalData.Length;
                var ptr = Marshal.AllocHGlobal(sizeof(byte) * (int)presence.multiplayerInternalData.Length);
                Marshal.Copy(presence.multiplayerInternalData, 0, ptr, presence.multiplayerInternalData.Length);
                impl.multiplayerInternalData = ptr;
                return impl;
            }

            public Uplay.Uplaydll.OnlineStatusV2 onlineStatus;
            public string detailsUtf8;
            public uint titleId;
            public string titleNameUtf8;
            public string multiplayerId;
            public int multiplayerJoinable;
            public uint multiplayerSize;
            public uint multiplayerMaxSize;
            public IntPtr multiplayerInternalData;
            public uint multiplayerInternalDataSize;
        }
        public class UPC_Presence
        {
            public Uplay.Uplaydll.OnlineStatusV2 onlineStatus;
            public string detailsUtf8;
            public uint titleId;
            public string titleNameUtf8;
            public string multiplayerId;
            public int multiplayerJoinable;
            public uint multiplayerSize;
            public uint multiplayerMaxSize;
            public byte[] multiplayerInternalData;
        }
        private struct UPC_UserImpl
        {
            public static UPC_UserImpl BuildFrom(UPC_User upc_User)
            {
                UPC_UserImpl impl = new();
                impl.idUtf8 = upc_User.idUtf8;
                impl.nameUtf8 = upc_User.nameUtf8;
                impl.relationship = upc_User.relationship;
                var presetimpl = UPC_PresenceImpl.BuildFrom(upc_User.presence);
                IntPtr ptr = Marshal.AllocHGlobal(sizeof(UPC_PresenceImpl));
                Marshal.StructureToPtr(presetimpl, ptr, false);
                impl.presence = ptr;
                return impl;
            }
            [MarshalAs(UnmanagedType.LPUTF8Str)]
            public string idUtf8;
            [MarshalAs(UnmanagedType.LPUTF8Str)]
            public string nameUtf8;
            [MarshalAs(UnmanagedType.U4)]
            public Uplay.Uplaydll.Relationship relationship;
            [MarshalAs(UnmanagedType.SysInt)]
            public IntPtr presence;
        }
        public class UPC_User
        {
            public string idUtf8;
            public string nameUtf8;
            public Uplay.Uplaydll.Relationship relationship;
            public UPC_Presence presence;
        }
        #endregion

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_UserGet(IntPtr inContext, IntPtr inOptUserIdUtf8, IntPtr outUser, IntPtr inCallback, IntPtr inCallbackData)
        {
            Basics.Log(nameof(UPC_UserGet), new object[] { inContext, inOptUserIdUtf8, outUser, inCallback, inCallbackData });

            var cbList = Main.GlobalContext.Callbacks.ToList();
            cbList.Add(new(inCallback, inCallbackData, 0));
            Main.GlobalContext.Callbacks = cbList.ToArray();

            UPC_User user = new();
            user.idUtf8 = "80f33a39-e682-4d1f-b693-39267e890df2";
            user.nameUtf8 = "user";
            user.relationship = Uplay.Uplaydll.Relationship.Friend;
            user.presence = new()
            {
                onlineStatus = Uplay.Uplaydll.OnlineStatusV2.OnlineStatusOnline,
                multiplayerSize = 0,
                multiplayerMaxSize = 0,
                detailsUtf8 = "",
                multiplayerId = "",
                multiplayerInternalData = { },
                multiplayerJoinable = 1,
                titleId = 0,
                titleNameUtf8 = ""
            };
            var impl = UPC_UserImpl.BuildFrom(user);
            IntPtr ptr = Marshal.AllocHGlobal(sizeof(UPC_UserImpl));
            Marshal.StructureToPtr(impl, ptr, false);
            Marshal.WriteIntPtr(outUser, ptr);
            return 0x10000;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_UserFree(IntPtr inContext, IntPtr inUser)
        {
            Basics.Log(nameof(UPC_UserGet), new object[] { inContext, inUser });

            var user = Basics.IntPtrToStruct<UPC_UserImpl>(inUser);
            var pers = Basics.IntPtrToStruct<UPC_PresenceImpl>(user.presence);
            Marshal.FreeHGlobal(pers.multiplayerInternalData);
            Marshal.DestroyStructure<UPC_PresenceImpl>(user.presence);
            Marshal.FreeHGlobal(user.presence);
            Marshal.DestroyStructure<UPC_UserImpl>(inUser);
            Marshal.FreeHGlobal(inUser);
            return 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int UPC_UserPlayedWithAdd(IntPtr inContext, IntPtr inUserIdUtf8List, uint inListLength)
        {
            Basics.Log(nameof(UPC_UserPlayedWithAdd), new object[] { inContext, inUserIdUtf8List, inListLength });
            return 0;
        }
    }
}
