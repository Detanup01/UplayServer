namespace upc_r2
{
    public class Enums
    {
        public enum UPC_InitResult : uint
        {
            UPC_InitResult_Ok,
            UPC_InitResult_Failed,
            UPC_InitResult_IncompatibleApiVersion,
            UPC_InitResult_ExitProcessRequired,
            UPC_InitResult_InstallationError,
            UPC_InitResult_DesktopInteractionRequired,
            UPC_InitResult_AlreadyInitialized
        }

        public enum UPC_Result
        {
            UPC_Result_Ok,
            UPC_Result_Declined = -1,
            UPC_Result_InvalidArgs = -2,
            UPC_Result_UninitializedSubsystem = -3,
            UPC_Result_CommunicationError = -4,
            UPC_Result_MemoryError = -5,
            UPC_Result_NotFound = -6,
            UPC_Result_EOF = -7,
            UPC_Result_LimitReached = -8,
            UPC_Result_UnauthorizedAction = -9,
            UPC_Result_InternalError = -10,
            UPC_Result_Aborted = -11,
            UPC_Result_Unknown = -12,
            UPC_Result_FailedPrecondition = -13,
            UPC_Result_Unavailable = -14
        }

        public enum UPC_UserShutdownReason
        {
            UPC_UserShutdownReason_AccountSharing = 1
        }

        public enum UPC_OverlaySection : uint
        {
            UPC_OverlaySection_None,
            UPC_OverlaySection_Home,
            UPC_OverlaySection_Achievements,
            UPC_OverlaySection_Actions,
            UPC_OverlaySection_Chat,
            UPC_OverlaySection_Friends,
            UPC_OverlaySection_Party,
            UPC_OverlaySection_Rewards,
            UPC_OverlaySection_Shop,
            UPC_OverlaySection_ProductActivation,
            UPC_OverlaySection_PendingGameInvites,
            UPC_OverlaySection_Challenges,
            UPC_OverlaySection_GameOptions
        }

        public enum UPC_OnlineStatus : uint
        {
            UPC_OnlineStatus_Offline = 1U,
            UPC_OnlineStatus_DoNotDisturb,
            UPC_OnlineStatus_Away = 4U,
            UPC_OnlineStatus_Online = 8U
        }

        [Flags]
        public enum UPC_ContextSubsystem : uint
        {
            UPC_ContextSubsystem_None = 0U,
            UPC_ContextSubsystem_Achievement = 1U,
            UPC_ContextSubsystem_Product = 2U,
            UPC_ContextSubsystem_Install = 4U,
            UPC_ContextSubsystem_Storage = 8U,
            UPC_ContextSubsystem_Overlay = 16U,
            UPC_ContextSubsystem_Friend = 32U,
            UPC_ContextSubsystem_Multiplayer = 64U,
            UPC_ContextSubsystem_Store = 128U,
            UPC_ContextSubsystem_Streaming = 256U
        }

        [Flags]
        public enum UPC_ProductType : uint
        {
            UPC_ProductType_Game = 1U,
            UPC_ProductType_Addon = 2U,
            UPC_ProductType_Package = 3U,
            UPC_ProductType_Consumable = 4U,
            UPC_ProductType_ConsumablePack = 5U,
            UPC_ProductType_Bundle = 6U
        }

        public enum UPC_ProductState : uint
        {
            UPC_ProductState_Visible = 1U,
            UPC_ProductState_Installable,
            UPC_ProductState_Playable,
            UPC_ProductState_Expired
        }

        public enum UPC_ProductOwnership : uint
        {
            UPC_ProductOwnership_Owned = 1U,
            UPC_ProductOwnership_Preordered,
            UPC_ProductOwnership_Suspended,
            UPC_ProductOwnership_Revoked,
            UPC_ProductOwnership_NotOwned = 4U,
            UPC_ProductOwnership_Locked
        }

        public enum UPC_ProductActivation : uint
        {
            UPC_ProductActivation_Purchased = 1U,
            UPC_ProductActivation_Trial,
            UPC_ProductActivation_Subscription
        }

        public enum UPC_MultiplayerSessionJoinability : uint
        {
            UPC_MultiplayerSessionJoinability_Closed = 1U,
            UPC_MultiplayerSessionJoinability_InviteOnly,
            UPC_MultiplayerSessionJoinability_FriendsOnly,
            UPC_MultiplayerSessionJoinability_Open
        }

        public enum UPC_FileOpenMode : uint
        {
            UPC_FileOpenMode_Read = 1U,
            UPC_FileOpenMode_Write
        }

        public enum UPC_StoreTag : uint
        {
            UPC_StoreTag_DLCs,
            UPC_StoreTag_Skins,
            UPC_StoreTag_Currencies,
            UPC_StoreTag_Bundles,
            UPC_StoreTag_SeasonPass,
            UPC_StoreTag_ULCs,
            UPC_StoreTag_Sets
        }

        [Flags]
        public enum UPC_StorePartner : uint
        {
            UPC_StorePartner_None = 0U,
            UPC_StorePartner_EpicGames = 1U
        }

        public enum UPC_StreamingType : uint
        {
            UPC_StreamingType_None,
            UPC_StreamingType_CloudPlay,
            UPC_StreamingType_RemotePlay,
            UPC_StreamingType_SharePlay
        }

        public enum UPC_StreamingDeviceType : uint
        {
            UPC_StreamingDeviceType_Unspecified,
            UPC_StreamingDeviceType_Desktop,
            UPC_StreamingDeviceType_Smartphone,
            UPC_StreamingDeviceType_Tablet,
            UPC_StreamingDeviceType_TV
        }

        public enum UPC_StreamingInputType : uint
        {
            UPC_StreamingInputType_Unspecified,
            UPC_StreamingInputType_Gamepad,
            UPC_StreamingInputType_KeyboardMouse,
            UPC_StreamingInputType_Touch
        }

        public enum UPC_GamepadType : uint
        {
            GamepadType_Invalid,
            GamepadType_Generic,
            GamepadType_Nintendo,
            GamepadType_PlayStation
        }

        public enum UPC_Relationship : uint
        {
            UPC_Relationship_None,
            UPC_Relationship_RequestSent,
            UPC_Relationship_RequestReceived,
            UPC_Relationship_Friends
        }

        public enum UPC_EventType : uint
        {
            UPC_Event_FriendAdded,
            UPC_Event_FriendNameUpdated,
            UPC_Event_FriendPresenceUpdated,
            UPC_Event_FriendRemoved,
            UPC_Event_MultiplayerSessionCleared = 1000U,
            UPC_Event_MultiplayerSessionUpdated,
            UPC_Event_MultiplayerInviteReceived,
            UPC_Event_MultiplayerInviteAccepted,
            UPC_Event_MultiplayerInviteDeclined,
            UPC_Event_MultiplayerJoiningRequested,
            UPC_Event_OverlayShown = 2000U,
            UPC_Event_OverlayHidden,
            UPC_Event_ProductAdded = 3000U,
            UPC_Event_ProductOwnershipUpdated,
            UPC_Event_ProductStateUpdated,
            UPC_Event_ProductBalanceUpdated,
            UPC_Event_InstallChunkInstalled = 4000U,
            UPC_Event_InstallChunkProgress,
            UPC_Event_InstallProgress,
            UPC_Event_UpdateAvailable,
            UPC_Event_StoreProductsListUpdated = 5000U,
            UPC_Event_StoreStatusUpdated,
            UPC_Event_StoreCheckoutStarted,
            UPC_Event_StoreCheckoutFinished,
            UPC_Event_UserShutdown = 6000U,
            UPC_Event_StreamingCurrentUserCountryUpdated = 7000U,
            UPC_Event_StreamingDeviceUpdated,
            UPC_Event_StreamingInputTypeUpdated,
            UPC_Event_StreamingResolutionUpdated
        }
        public enum UPC_AvatarSize : uint
        {
            UPC_AvatarSize_64x64,
            UPC_AvatarSize_128x128,
            UPC_AvatarSize_256x256
        }

        public enum UplayStartResult
        {
            Ok,
            Failed,
            ExitProcessRequired,
            InstallationError,
            DesktopInteractionRequired
        }

        [Flags]
        public enum UplayStartFlags
        {
            None = 0
        }

        public enum UPLAY_EventType
        {
            FriendsFriendListUpdated = 10000,
            FriendsFriendUpdated,
            FriendsGameInviteAccepted,
            FriendsMenuItemSelected,
            PartyMemberListChanged = 20000,
            PartyMemberUserDataUpdated,
            PartyLeaderChanged,
            PartyGameInviteReceived,
            PartyGameInviteAccepted,
            PartyMemberMenuItemSelected,
            PartyMemberUpdated,
            PartyInviteReceived,
            PartyMemberJoined,
            PartyMemberLeft,
            OverlayActivated = 30000,
            OverlayHidden,
            RewardRedeemed = 40000,
            UnitBalanceChanged,
            UserAccountSharing = 50000,
            UserConnectionLost,
            UserConnectionRestored,
            UserOwnershipAdded,
            UserConsumableUpdated,
            UserOwnershipRemoved,
            InstallerChunkInstalled = 60000,
            InstallerChunkProgress,
            InstallerProgress,
            ChatMessageSent = 70000,
            ChatMessageSentDeliveryUpdated,
            ChatMessageReceived,
            ChatMessagesRead
        }

        public enum UPLAY_OverlappedResult
        {
            UPLAY_OverlappedResult_INVALID = -1,
            UPLAY_OverlappedResult_Ok,
            UPLAY_OverlappedResult_InvalidArgument,
            UPLAY_OverlappedResult_ConnectionError,
            UPLAY_OverlappedResult_NotFound,
            UPLAY_OverlappedResult_NotAPartyLeader,
            UPLAY_OverlappedResult_PartyFull,
            UPLAY_OverlappedResult_Failed,
            UPLAY_OverlappedResult_AlreadyOpened,
            UPLAY_OverlappedResult_SlotLocked,
            UPLAY_OverlappedResult_OverwriteNotAllowed
        }

        public enum UPLAY_OVERLAY_Section : uint
        {
            UPLAY_OverlaySection_Show,
            UPLAY_OverlaySection_Home,
            UPLAY_OverlaySection_Achievements,
            UPLAY_OverlaySection_Actions,
            UPLAY_OverlaySection_Chat,
            UPLAY_OverlaySection_Friends,
            UPLAY_OverlaySection_Party,
            UPLAY_OverlaySection_Rewards,
            UPLAY_OverlaySection_Shop,
            UPLAY_OverlaySection_ProductActivation,
            UPLAY_OverlaySection_PendingGameInvites,
            UPLAY_OverlaySection_Challenges,
            UPLAY_OverlaySection_GameOptions
        }
        public enum UPLAY_SAVE_Mode
        {
            UPLAY_SAVE_MODE_Read,
            UPLAY_SAVE_MODE_Write
        }
    }
}
