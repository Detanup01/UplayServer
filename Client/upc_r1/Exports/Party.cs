using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r1.Exports;

internal class Party
{

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_DisablePartyMemberMenuItem", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_DisablePartyMemberMenuItem()
    {
        Basics.Log(nameof(UPLAY_PARTY_DisablePartyMemberMenuItem), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_EnablePartyMemberMenuItem", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_EnablePartyMemberMenuItem()
    {
        Basics.Log(nameof(UPLAY_PARTY_EnablePartyMemberMenuItem), []);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_GetFullMemberList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_GetFullMemberList(IntPtr aOutMemberList)
    {
        Basics.Log(nameof(UPLAY_PARTY_GetFullMemberList), [aOutMemberList]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_GetId", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPLAY_PARTY_GetId()
    {
        Basics.Log(nameof(UPLAY_PARTY_GetId), []);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_GetInGameMemberList", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_GetInGameMemberList(IntPtr aOutMemberList)
    {
        Basics.Log(nameof(UPLAY_PARTY_GetInGameMemberList), [aOutMemberList]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_Init", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_Init(uint aFlags)
    {
        Basics.Log(nameof(UPLAY_PARTY_Init), [aFlags]);
        return true;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_InvitePartyToGame", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_InvitePartyToGame(IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_PARTY_InvitePartyToGame), [aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_InviteToParty", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_InviteToParty(IntPtr aAccountIdUtf8, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_PARTY_InviteToParty), [aAccountIdUtf8, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_IsInParty", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_IsInParty(IntPtr aAccountIdUtf8)
    {
        Basics.Log(nameof(UPLAY_PARTY_IsInParty), [aAccountIdUtf8]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_IsPartyLeader", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_IsPartyLeader(IntPtr aAccountIdUtf8)
    {
        Basics.Log(nameof(UPLAY_PARTY_IsPartyLeader), [aAccountIdUtf8]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_PromoteToLeader", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_PromoteToLeader(IntPtr aAccountIdUtf8, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_PARTY_PromoteToLeader), [aAccountIdUtf8, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_RespondToGameInvite", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_RespondToGameInvite(uint aInvitationId, bool aAccept)
    {
        Basics.Log(nameof(UPLAY_PARTY_RespondToGameInvite), [aInvitationId, aAccept]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_SetGuest", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_SetGuest(IntPtr guestId, IntPtr aOverlapped)
    {
        Basics.Log(nameof(UPLAY_PARTY_SetGuest), [guestId, aOverlapped]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_SetUserData", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_SetUserData(IntPtr aDataBlob)
    {
        Basics.Log(nameof(UPLAY_PARTY_SetUserData), [aDataBlob]);
        return false;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPLAY_PARTY_ShowGameInviteOverlayUI", CallConvs = [typeof(CallConvCdecl)])]
    public static bool UPLAY_PARTY_ShowGameInviteOverlayUI(uint aInvitationId)
    {
        Basics.Log(nameof(UPLAY_PARTY_ShowGameInviteOverlayUI), [aInvitationId]);
        return false;
    }
}
