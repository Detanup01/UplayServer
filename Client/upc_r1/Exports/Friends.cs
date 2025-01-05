using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;
public class Friends
{
    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_AddPlayedWith", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_AddPlayedWith(IntPtr aDescriptionUtf8, IntPtr aAccountIdListUtf8, uint aAccountIdListLength)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_AddPlayedWith), [aDescriptionUtf8, aDescriptionUtf8, aAccountIdListLength]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_AddToBlackList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_AddToBlackList(IntPtr aAccountIdUtf8, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_AddToBlackList), [aAccountIdUtf8, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_DisableFriendMenuItem", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_DisableFriendMenuItem(uint aId)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_DisableFriendMenuItem), [aId]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_EnableFriendMenuItem", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_EnableFriendMenuItem(uint aId, uint aMenuItemMode, uint aFilter)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_EnableFriendMenuItem), [aId, aMenuItemMode, aFilter]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_GetFriendList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_GetFriendList(uint aFriendListFilter, IntPtr aOutFriendList)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_GetFriendList), [aFriendListFilter, aOutFriendList]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_Init", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_Init(uint aFlags)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_Init), [aFlags]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_InviteToGame", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_InviteToGame(IntPtr aAccountIdUtf8, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_InviteToGame), [aAccountIdUtf8, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_IsBlackListed", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_IsBlackListed(IntPtr aAccountIdUtf8)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_IsBlackListed), [aAccountIdUtf8]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_IsFriend", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_IsFriend(IntPtr aAccountIdUtf8)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_IsFriend), [aAccountIdUtf8]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_RemoveFriendship", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_RemoveFriendship(IntPtr aAccountIdUtf8, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_RemoveFriendship), [aAccountIdUtf8, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_RemoveFromBlackList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_RemoveFromBlackList(IntPtr aAccountIdUtf8, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_RemoveFromBlackList), [aAccountIdUtf8, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_RequestFriendship", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_RequestFriendship(IntPtr aSearchStringUtf8, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_RequestFriendship), [aSearchStringUtf8, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_RespondToGameInvite", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_RespondToGameInvite(uint aInvitationId, IntPtr aAccept)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_RespondToGameInvite), [aInvitationId, aAccept]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_ShowFriendSelectionUI", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_ShowFriendSelectionUI(IntPtr aAccountIdFilterListUTF8, uint aAccountIdFilterListLength, IntPtr aOverlapped, IntPtr aOutResult)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_ShowFriendSelectionUI), [aAccountIdFilterListUTF8, aAccountIdFilterListLength, aOverlapped, aOutResult]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_FRIENDS_ShowInviteFriendsToGameUI", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_FRIENDS_ShowInviteFriendsToGameUI(IntPtr aAccountIdFilterListUtf8, uint aAccountIdFilterListLength)
    {
        Basics.Log(nameof(UPLAY_FRIENDS_ShowInviteFriendsToGameUI), [aAccountIdFilterListUtf8, aAccountIdFilterListLength]);
        return false;
    }
}
