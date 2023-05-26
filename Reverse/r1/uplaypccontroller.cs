  public class UplayPCController
  {
    public static Action OnOverlayOpened;
    public static Action OnOverlayClosed;
    [SerializeField]
    private UISM_ConfirmMessageData _droppedUplaySdk;
    [SerializeField]
    private UISM_ConfirmMessageData _userSignedOut;
    private bool _isConnectionLost;
    private bool _displayedFatalMessage;
    private static bool _isUplayStarted;
    private static bool _isOverlayShowned;
    private static UPLAY_Overlapped _overlayHandle = new UPLAY_Overlapped();

    private void CloseOverlay()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    [SpecialName]
    public static bool IsUplayStarted() => UplayPCController._isUplayStarted;

    public static string UplayTicket { private set; get; }

    public static UplayStartResult AreabaseStartUplay(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.SetTicket2(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("[AreaBase] TriangleIdGrid is null after initialization.", UplayPCController.MNHAAMHCHGP());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("Generate new collectible save.");
            UplayPCController.MKILLPIFJIM(UplayPCController.EnumNoneError.None);
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = true;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("ParabolaDownTop", aOutErrorString);
          UplayPCController.IFDINHFCELN(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("_GlitchSmallNoiseOffset");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("Displacement Node was not found in monster instance.");
          UplayPCController._isUplayStarted = true;
          UplayPCController.PMBJHLGLLNH(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("[Bootstrap Step] Instanciating Game Controller");
          UplayPCController.JINCLCLCIMG(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    [SpecialName]
    private static void SetTicket(string Ticket) => UplayPCController.TicketField = Ticket;

    [SpecialName]
    public static bool IsOffline() => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() != 1;

    public virtual void KMCGMGAHLCF(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.UplayTicket) || !UplayPCController.IsUplayStarted3() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = false;
        if (!this._displayedFatalMessage)
          UISystemMessageController.LCGNJHHOGKF(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("_LightGroupColorArray", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("WwiseUnity: AkAudioListener.Migrate14 for ", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -146:
              this.BJAHCEONIDD();
              continue;
            case (UPLAY_EventType) -119:
              UISystemMessageController.LCGNJHHOGKF(this._userSignedOut, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = false;
              continue;
            case (UPLAY_EventType) -114:
              this.PDIKEBKHGFC();
              continue;
            default:
              continue;
          }
        }
      }
    }

    [SpecialName]
    public static UplayPCController.EnumNoneError GetErrorField() => UplayPCController.ErrorField;

    [SpecialName]
    public static string GetTicketField() => UplayPCController.TicketField;

    public virtual void AIJLFDHNIMF(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.GetTicketField()) || !UplayPCController.OIHKDHJBNIO() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = true;
        if (!this._displayedFatalMessage)
          UISystemMessageController.LCGNJHHOGKF(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("Build Settings Scenes:", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("Wrong setup on Crystal FX Player: Missing Interactable !", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -137:
              this.MJEGEMEGIOF();
              continue;
            case (UPLAY_EventType) -83:
              this.HJFGDBHCMAK();
              continue;
            case (UPLAY_EventType) 158:
              UISystemMessageController.LCGNJHHOGKF(this._userSignedOut, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = true;
              continue;
            default:
              continue;
          }
        }
      }
    }

    private void AONAIJKKDAJ()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "_ConstructFade", (object) aOutErrorString);
    }

    [SpecialName]
    public static string MDKLFONHCEL() => UplayPCController.TicketField;

    private void LNLLEAMHMFG()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    [SpecialName]
    public static UplayPCController.EnumNoneError HEMFALMHKHH() => UplayPCController.ErrorField;

    public virtual void EAAECOIJFNE()
    {
      if (GameController.BJBOLHFDMOK() != GameController.NMECCJHCPGF.IS_LOADING || !UplayPCController.GICKEPPMONI())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    public virtual void PDBEFAHIEMK(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.MJLFLABLBKL()) || !UplayPCController.IsUplayStarted2() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = false;
        if (!this._displayedFatalMessage)
          UISystemMessageController.GJEAHPGDOAP(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("[VRController] Update VR State End", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("[Out Of Range]", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -154:
              this.FIFGBBCCCJK();
              continue;
            case (UPLAY_EventType) -133:
              this.PLOFCLCGAKA();
              continue;
            case (UPLAY_EventType) -111:
              UISystemMessageController.BCJNHCHPCKM(this._userSignedOut, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = false;
              continue;
            default:
              continue;
          }
        }
      }
    }

    public static UplayStartResult IOKNFMIKABH(uint UplayId)
    {
      UplayStartFlags aFlags = (UplayStartFlags) 1;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.OGIBPODBLFG(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("   Target Fade : ", UplayPCController.BAODJPLFFAI());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("_SecondaryRoomGIOffset");
            UplayPCController.EKBNMLDCGOD(UplayPCController.EnumNoneError.None);
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = false;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("[Loading Controller] Trying to unload with an invalid scene GUID", aOutErrorString);
          UplayPCController.KPPHHPABHEH(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("[Bootstrap Step] Playing Ubi Logo");
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("No Animator on object {0}");
          UplayPCController._isUplayStarted = false;
          UplayPCController.IFDINHFCELN(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("starting {0} but {1} had not been stopped");
          UplayPCController.KADDOMAMICH(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    public static UplayStartResult HCMILBFJGEK(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.INAHJCJANAM(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("Skipped Decorruption Start for room {0} since it is already started", UplayPCController.UplayTicket);
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("Transition Type {0} not supported, will use default Transition (LightSwitches)");
            UplayPCController.IFDINHFCELN(UplayPCController.EnumNoneError.None);
            UplayPCController._isUplayStarted = true;
          }
          else
            UplayPCController._isUplayStarted = false;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("[VRController] Initialize Tracker", aOutErrorString);
          UplayPCController.EPOLBNDOKFL = UplayPCController.EnumNoneError.None;
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("Wrong kernal size.");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("Null List of affected Interactable in CorruptionObject in scene : {0}");
          UplayPCController._isUplayStarted = false;
          UplayPCController.JINCLCLCIMG(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("world");
          UplayPCController.MLDFANLFIAE(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    [SpecialName]
    private static void EJMFPDBGDKD(string Ticket) => UplayPCController.TicketField = Ticket;

    public override void UpdateController(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.UplayTicket) || !UplayPCController.MMIMLNNFODP || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = true;
        if (!this._displayedFatalMessage)
          UISystemMessageController.BOLDIGKIDLK(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("UPLAY_Update Error: {0}", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("UPLAY_EventType.{0}", aEvent.type);
          switch (aEvent.type)
          {
            case UPLAY_EventType.OverlayActivated:
              this.PGCPAHJCAOA();
              continue;
            case UPLAY_EventType.OverlayHidden:
              this.GDJNIANPCOJ();
              continue;
            case UPLAY_EventType.UserAccountSharing:
              UISystemMessageController.BOLDIGKIDLK(this._userSignedOut, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = true;
              continue;
            default:
              continue;
          }
        }
      }
    }

    public virtual void PGDCJLGDGOF(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.HFFCFAJPPHN()) || !UplayPCController.OIHKDHJBNIO() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = false;
        if (!this._displayedFatalMessage)
          UISystemMessageController.DKOFDIAFFDN(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>(">.", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("Room {0} was corrupted.", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -114:
              this.CHFEDOOLCIC();
              continue;
            case (UPLAY_EventType) 110:
              UISystemMessageController.LCGNJHHOGKF(this._userSignedOut, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = true;
              continue;
            case (UPLAY_EventType) 123:
              this.GPGPJJFIFPE();
              continue;
            default:
              continue;
          }
        }
      }
    }

    public static UplayPCController.EnumNoneError EPOLBNDOKFL { private set; get; }

    [SpecialName]
    public static UplayPCController.EnumNoneError HCKOLOLMNLK() => UplayPCController.ErrorField;

    [SpecialName]
    public static string MNHAAMHCHGP() => UplayPCController.TicketField;

    [SpecialName]
    public static bool MBBKPEGIHLP() => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() == 0;

    [SpecialName]
    public static bool KOBKCEMKCBM() => UplayPCController._isUplayStarted;

    public static UplayStartResult BFIMJLMHEOK(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.UplayTicket = UplayPCCAPI.UPLAY_USER_GetTicket();
          Loggers.UPlay.Info<string>("Object VO  of {0} was interrupted. Reseting value in object description", UplayPCController.ANNMBIDOHOO());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("Video");
            UplayPCController.KADDOMAMICH(UplayPCController.EnumNoneError.Error);
            UplayPCController._isUplayStarted = true;
          }
          else
            UplayPCController._isUplayStarted = true;
          UplayPCController._isOverlayShowned = false;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Trying to init audio with a null glitch preset. This will cause issues. Someone might have deleted that particular preset.", aOutErrorString);
          UplayPCController.NDGGLJDFICM(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("CRYSTAL - Stop Constant Effect - {0}");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("No spawn point given in TransferenceTeleport on {0} -- teleporting will not work");
          UplayPCController._isUplayStarted = true;
          UplayPCController.PHIMPMNIGIP(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("Generate new collectible save.");
          UplayPCController.CFBFOONNEPA(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    [SpecialName]
    public static UplayPCController.EnumNoneError NIHEPMFPNNE() => UplayPCController.ErrorField;

    public virtual void HNKFMAAFMJM(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.INCBBDJIGOB()) || !UplayPCController.PLIDNJAIPGF() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = true;
        if (!this._displayedFatalMessage)
          UISystemMessageController.MLHMMKDHNMB(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("{0}", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("Overlay shown", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -50:
              UISystemMessageController.DKOFDIAFFDN(this._userSignedOut, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = true;
              continue;
            case (UPLAY_EventType) 39:
              this.GDJNIANPCOJ();
              continue;
            case (UPLAY_EventType) 137:
              this.CDHCKCEBENI();
              continue;
            default:
              continue;
          }
        }
      }
    }

    [SpecialName]
    private static void PMBJHLGLLNH(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    public static void CGLFMEGOKAI(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.LILOEFNBEPA())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("[VRController] Initialize Tracker", result);
      }
      else
        Loggers.Online.Error("go_WallPart");
    }

    [SpecialName]
    public static bool DIADJHONCEP() => UplayPCController._isUplayStarted;

    public static UplayStartResult DFBGAABEFJC(uint UplayId)
    {
      UplayStartFlags aFlags = (UplayStartFlags) 1;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.LLCGINMNGGG(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("Loading controller doesn't have the same scene count has the build settings. Dumping the scene data for both.", UplayPCController.UplayTicket);
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("{0}\n{1}\n{2}");
            UplayPCController.MLDFANLFIAE(UplayPCController.EnumNoneError.None);
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = true;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Emitter field must no be null", aOutErrorString);
          UplayPCController.PHIMPMNIGIP(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("ScriptedEvent");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("__NAME__");
          UplayPCController._isUplayStarted = false;
          UplayPCController.PMBJHLGLLNH(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("Couldn't load scene");
          UplayPCController.EKBNMLDCGOD(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    public static void KEHPOOOOFPE(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.NPNJANCKLJD())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("LightGroup field must no be null", result);
      }
      else
        Loggers.Online.Error("Player Controller: cannot find Ghost control.");
    }

    private void LJINOFNCOJP()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "Attempting to uncorrupt a room that is not setup : {0} ", (object) aOutErrorString);
    }

    public static UplayStartResult HALIDJIPIFA(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.LEHFNBIHJFI(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("Japanese", UplayPCController.MJLFLABLBKL());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("Settings/TransferenceSettings");
            UplayPCController.NDGGLJDFICM(UplayPCController.EnumNoneError.Error);
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = false;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Camera PostProcessing stack is missing the Color Grading component", aOutErrorString);
          UplayPCController.NNHPMKJDNPF(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("Floor");
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("Uplay achievements platform is null");
          UplayPCController._isUplayStarted = false;
          UplayPCController.BMPBKFECGEJ(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("go_DoorFrameBottom");
          UplayPCController.MLDFANLFIAE(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    private void MJEGEMEGIOF()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    private void GJLAFMOJMFC()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    public virtual void KFDKPAJCGHA(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.GetTicketField()) || !UplayPCController.KOBKCEMKCBM() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = false;
        if (!this._displayedFatalMessage)
          UISystemMessageController.BCJNHCHPCKM(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("Base Layer", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("Camera PostProcessing stack is missing the Color Grading component", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -200:
              this.PKIHIAJEEIG();
              continue;
            case (UPLAY_EventType) 75:
              this.GPGPJJFIFPE();
              continue;
            case (UPLAY_EventType) 92:
              UISystemMessageController.GJEAHPGDOAP(this._userSignedOut, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = true;
              continue;
            default:
              continue;
          }
        }
      }
    }

    private void FODNLLJGDFM()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    private void GBEMNEDMBJD()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "Saved sound emitter length does not correspond to actual number of sound emitters, please resave scene", (object) aOutErrorString);
    }

    public virtual void JJLKLEEJGOI()
    {
      if (GameController.DLBAJABCOIN() != GameController.NMECCJHCPGF.IS_QUITING || !UplayPCController.KOBKCEMKCBM())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    public virtual void LHOLNLCOBBM()
    {
      if (GameController.BJBOLHFDMOK() != GameController.NMECCJHCPGF.IS_PAUSING || !UplayPCController.MMIMLNNFODP)
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    [SpecialName]
    public static bool GICKEPPMONI() => UplayPCController._isUplayStarted;

    private void KNKIKMDIKDK()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "Japanese", (object) aOutErrorString);
    }

    private void PDHBPMDNEDM()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    [SpecialName]
    public static bool OLGNFNFLKNJ() => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() == 0;

    [SpecialName]
    public static UplayPCController.EnumNoneError JIMIIBJJOBP() => UplayPCController.ErrorField;

    [SpecialName]
    public static string PAHGLFMCOHL() => UplayPCController.TicketField;

    public static void DEJKCCDCBKG(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.IsUplayStarted3())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("Load completed {0}: {1}", result);
      }
      else
        Loggers.Online.Error("god_DebugUI");
    }

    private void ECAPAGGGEBP()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    [SpecialName]
    private static void LLCGINMNGGG(string Ticket) => UplayPCController.TicketField = Ticket;

    [SpecialName]
    private static void KCKJOACIIDM(string Ticket) => UplayPCController.TicketField = Ticket;

    public static void MHEPLPAAOIB(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.MMIMLNNFODP)
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("[Uplay] Overlay Opened. (open result={0})", result);
      }
      else
        Loggers.Online.Error("[Uplay] Show Overlay Failed");
    }

    [SpecialName]
    public static UplayPCController.EnumNoneError EPCNGAHHADN() => UplayPCController.ErrorField;

    [SpecialName]
    private static void JFDLILOAANF(string Ticket) => UplayPCController.TicketField = Ticket;

    private void DGFLAPIHBAO()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    [SpecialName]
    private static void KADDOMAMICH(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    public virtual void NGIEJJFBIDI()
    {
      if (GameController.DLBAJABCOIN() != GameController.NMECCJHCPGF.IS_PAUSING || !UplayPCController.MGNMGMOJFDB())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    public virtual void DJFDJNBNHCG(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.GetTicketField()) || !UplayPCController.OANBGPNPLOJ() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = true;
        if (!this._displayedFatalMessage)
          UISystemMessageController.BCJNHCHPCKM(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("_GlitchGlobalGridCellSize", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("[to define]", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -152:
              UISystemMessageController.MLHMMKDHNMB(this._userSignedOut, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = true;
              continue;
            case (UPLAY_EventType) 108:
              this.GDJNIANPCOJ();
              continue;
            case (UPLAY_EventType) 160:
              this.ECAPAGGGEBP();
              continue;
            default:
              continue;
          }
        }
      }
    }

    public static UplayStartResult ILEEBCCNCDD(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.LLCGINMNGGG(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("Update", UplayPCController.BAODJPLFFAI());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("LightGroupDoor has no renderer set");
            UplayPCController.BMPBKFECGEJ(UplayPCController.EnumNoneError.Error);
            UplayPCController._isUplayStarted = true;
          }
          else
            UplayPCController._isUplayStarted = false;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Room {0} was corrupted.", aOutErrorString);
          UplayPCController.KPPHHPABHEH(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("Interaction Category setting shouldn't be null");
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("[AreaBase] TriangleIdGrid is null after initialization.");
          UplayPCController._isUplayStarted = true;
          UplayPCController.PMBJHLGLLNH(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("ParabolaLeftRight");
          UplayPCController.NDGGLJDFICM(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    [SpecialName]
    public static UplayPCController.EnumNoneError MFGGOGKFJCO() => UplayPCController.ErrorField;

    [SpecialName]
    public static UplayPCController.EnumNoneError MHOIBGNJAKB() => UplayPCController.ErrorField;

    [SpecialName]
    public static bool OANBGPNPLOJ() => UplayPCController._isUplayStarted;

    [SpecialName]
    private static void MPLHNKDABAP(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    [SpecialName]
    private static void DNHANAJLNIP(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    [SpecialName]
    private static void CHILLKGGNJM(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    private void OJCPHPNKIOH()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "WwiseUnity: PostCallbacks aborted due to error: Undefined callback type <", (object) aOutErrorString);
    }

    [SpecialName]
    public static UplayPCController.EnumNoneError FHCHDBBIGGG() => UplayPCController.ErrorField;

    [SpecialName]
    private static void GLEEEOHCCPN(string Ticket) => UplayPCController.TicketField = Ticket;

    public static UplayStartResult MMFJLHDCILE(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.JAKAGNBAINP(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("AAAAAABBBBB", UplayPCController.INCBBDJIGOB());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("starting {0} but {1} had not been stopped");
            UplayPCController.CHILLKGGNJM(UplayPCController.EnumNoneError.None);
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = false;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("_movieAsset", aOutErrorString);
          UplayPCController.SetError2(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("Syncing");
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("go_GUI_");
          UplayPCController._isUplayStarted = false;
          UplayPCController.DNHANAJLNIP(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("EDITOR CONTEXT EXITS FROM STATE {0} AND MOVES BACK TO {1}");
          UplayPCController.NDGGLJDFICM(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    [SpecialName]
    private static void NNPPFEOCJAG(string Ticket) => UplayPCController.TicketField = Ticket;

    [SpecialName]
    public static bool NPMJDEHMHIH() => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() != 0;

    public static bool OKPNNGAJCKC => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() != 0;

    private void MDHBFIKHCFK()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "TRANSFERENCE_ONLINE_SAVE", (object) aOutErrorString);
    }

    [SpecialName]
    public static bool EGKDCPOPIJM() => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() != 1;

    [SpecialName]
    private static void KPPHHPABHEH(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    public static void ONOGPCCHNEO(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.IsUplayStarted2())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("SetObjectPosition now requires a top vector. Please change your scripts to use the SetObjectPosition overload that specifies the top vector.", result);
      }
      else
        Loggers.Online.Error("Unsupported controllerType:{0}");
    }

    private void OBBIOBHIMBG()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "I, ", (object) aOutErrorString);
    }

    public virtual void KEKNONFBCON()
    {
      if (GameController.DLBAJABCOIN() != GameController.NMECCJHCPGF.IS_UNLOADING || !UplayPCController.IsUplayStarted())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    public static void NMIGODOHJCC(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.OIHKDHJBNIO())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("AP_Door_Shake_RTPC_Reversed", result);
      }
      else
        Loggers.Online.Error("Blackscreen");
    }

    [SpecialName]
    public static UplayPCController.EnumNoneError LMKACNLBBCO() => UplayPCController.ErrorField;

    public static void PHAMCEIMPIH(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.MMIMLNNFODP)
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("_GlitchGlobalGridSeed", result);
      }
      else
        Loggers.Online.Error("Displacement Node was not found in monster instance.");
    }

    public virtual void HIKDNKGGCEM(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.MJLFLABLBKL()) || !UplayPCController.MNLDEKGHHKL() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = true;
        if (!this._displayedFatalMessage)
          UISystemMessageController.MLHMMKDHNMB(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("English(US)", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("DefaultContextProfile(code generated)", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -185:
              this.FMIPPIENHBB();
              continue;
            case (UPLAY_EventType) 33:
              UISystemMessageController.BOLDIGKIDLK(this._userSignedOut, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = false;
              continue;
            case (UPLAY_EventType) 179:
              this.CKCJDICNLLG();
              continue;
            default:
              continue;
          }
        }
      }
    }

    [SpecialName]
    private static void IGHMMPEAIDA(string Ticket) => UplayPCController.TicketField = Ticket;

    public virtual void PHIMOEOJCMK(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.BAODJPLFFAI()) || !UplayPCController.PLIDNJAIPGF() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = true;
        if (!this._displayedFatalMessage)
          UISystemMessageController.BOLDIGKIDLK(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("Invalid mesh given on DecorruptionInteractableParticles !", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("_GlitchAreaOfEffectGrid", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -82:
              UISystemMessageController.CAMDAFOLNIB(this._userSignedOut, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = true;
              continue;
            case (UPLAY_EventType) 36:
              this.FMIPPIENHBB();
              continue;
            case (UPLAY_EventType) 169:
              this.ILHFBHOKCKC();
              continue;
            default:
              continue;
          }
        }
      }
    }

    [SpecialName]
    private static void EKBNMLDCGOD(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    public virtual void DPMNIKDIFJE()
    {
      if (GameController.CABMILFHAHC() != GameController.NMECCJHCPGF.IS_LOADING || !UplayPCController.OANBGPNPLOJ())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    public static UplayStartResult MCEMPBDCLFE(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.FEPNBBOGIJI(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("Loading state {0} took {1} seconds", UplayPCController.PAHGLFMCOHL());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("go_DoorFrameRight");
            UplayPCController.EKBNMLDCGOD(UplayPCController.EnumNoneError.Error);
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = false;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Stairs", aOutErrorString);
          UplayPCController.CFBFOONNEPA(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("No spawn point given in TransferenceTeleport on {0} -- teleporting will not work");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("[AreaBase] TriangleIdGrid is null after initialization.");
          UplayPCController._isUplayStarted = false;
          UplayPCController.BMPBKFECGEJ(UplayPCController.EnumNoneError.Error);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("Destroy");
          UplayPCController.CHILLKGGNJM(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    private void ILHFBHOKCKC()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    private void PLOFCLCGAKA()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    private void NJPDNPLNAKO()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    [SpecialName]
    public static UplayPCController.EnumNoneError HKPJJAMBLMO() => UplayPCController.ErrorField;

    [SpecialName]
    public static string LJFOIFJAPHN() => UplayPCController.TicketField;

    [SpecialName]
    public static string ANNMBIDOHOO() => UplayPCController.TicketField;

    public static bool MMIMLNNFODP => UplayPCController._isUplayStarted;

    [SpecialName]
    private static void JINCLCLCIMG(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    private void GPGPJJFIFPE()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    public virtual void FFDJOLBPKDI()
    {
      if (GameController.CABMILFHAHC() != GameController.NMECCJHCPGF.IS_UNLOADING || !UplayPCController.MGNMGMOJFDB())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    public static void HHECANHFHBN(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.KOBKCEMKCBM())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("FireAndForget glitch event is not supported anymore", result);
      }
      else
        Loggers.Online.Error("No Animator on object {0}");
    }

    [SpecialName]
    private static void PPMKFKAEEDL(string Ticket) => UplayPCController.TicketField = Ticket;

    public virtual void LLCIOPMNMOB()
    {
      if (GameController.DLBAJABCOIN() != GameController.NMECCJHCPGF.IS_PAUSING || !UplayPCController.KOBKCEMKCBM())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    public static UplayStartResult PGAJKPIPOON(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.UplayTicket = UplayPCCAPI.UPLAY_USER_GetTicket();
          Loggers.UPlay.Info<string>("DynamicObject {0} has no renderer. It might not have been saved properly. Scene : {1}", UplayPCController.PAHGLFMCOHL());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("RoomEmitter_");
            UplayPCController.EPOLBNDOKFL = UplayPCController.EnumNoneError.None;
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = false;
          UplayPCController._isOverlayShowned = false;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("_UberAmbientColor", aOutErrorString);
          UplayPCController.EKBNMLDCGOD(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("Ubiservices state: {0}");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("_DetailNormalMap");
          UplayPCController._isUplayStarted = false;
          UplayPCController.KADDOMAMICH(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("_PrimaryRoomDirection");
          UplayPCController.NDGGLJDFICM(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    [SpecialName]
    private static void MLDFANLFIAE(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    public static void AOMJNALHAHD(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.LILOEFNBEPA())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("perspective", result);
      }
      else
        Loggers.Online.Error("PAGE_TITLE");
    }

    public static UplayStartResult ADJKINBBIMF(uint UplayId)
    {
      UplayStartFlags aFlags = (UplayStartFlags) 1;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.LEHFNBIHJFI(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("Generate new collectible save.", UplayPCController.BNGFOPGNNKC());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("WARNING: Trying to obtain an emitter but none are left in the pool");
            UplayPCController.MPLHNKDABAP(UplayPCController.EnumNoneError.Error);
            UplayPCController._isUplayStarted = true;
          }
          else
            UplayPCController._isUplayStarted = true;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Small Width:", aOutErrorString);
          UplayPCController.PHIMPMNIGIP(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("<PLACEHOLDER>");
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("Camera PostProcessing stack is missing the Color Grading component");
          UplayPCController._isUplayStarted = false;
          UplayPCController.EPOLBNDOKFL = UplayPCController.EnumNoneError.None;
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("_GlitchRandomNormals");
          UplayPCController.CHILLKGGNJM(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    public virtual void FPEHLIOBBEM(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.ANNMBIDOHOO()) || !UplayPCController.IsUplayStarted() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = true;
        if (!this._displayedFatalMessage)
          UISystemMessageController.CAMDAFOLNIB(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("RoomData {0} not registered", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("Attempted to spawn a monster while another monster is busy.", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) 18:
              this.MJEGEMEGIOF();
              continue;
            case (UPLAY_EventType) 72:
              this.GPGPJJFIFPE();
              continue;
            case (UPLAY_EventType) 141:
              UISystemMessageController.GJEAHPGDOAP(this._userSignedOut, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = false;
              continue;
            default:
              continue;
          }
        }
      }
    }

    [SpecialName]
    public static bool OIHKDHJBNIO() => UplayPCController._isUplayStarted;

    private void PDIKEBKHGFC()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    private void DNINDNANLJD()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "Assigning a new key to action:{0}, axisRange:{1}, oldKey:{2}, newkey:{3}", (object) aOutErrorString);
    }

    public static void JINMCEFEPDL(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.DIADJHONCEP())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("License type {0}", result);
      }
      else
        Loggers.Online.Error("Instanciation");
    }

    [SpecialName]
    public static bool LPIALMBBGFK() => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() != 1;

    public virtual void JJLBCDKPAGL(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.INCBBDJIGOB()) || !UplayPCController.OANBGPNPLOJ() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = false;
        if (!this._displayedFatalMessage)
          UISystemMessageController.DKOFDIAFFDN(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("Using invalid mode for this update.", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("Sound engine initialized.", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -10:
              UISystemMessageController.CAMDAFOLNIB(this._userSignedOut, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = true;
              continue;
            case (UPLAY_EventType) 68:
              this.DCBKMDEJCIC();
              continue;
            case (UPLAY_EventType) 90:
              this.DEFEEEMBOPK();
              continue;
            default:
              continue;
          }
        }
      }
    }

    [SpecialName]
    public static string BNGFOPGNNKC() => UplayPCController.TicketField;

    private void PKLLMAGLCAE()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "_SpatialAttenuationStart", (object) aOutErrorString);
    }

    public virtual void GJGGADANKDK()
    {
      if (GameController.BJBOLHFDMOK() != GameController.NMECCJHCPGF.IS_RESTARTING_SEQUENCE || !UplayPCController.PLIDNJAIPGF())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    [SpecialName]
    public static string INCBBDJIGOB() => UplayPCController.TicketField;

    [SpecialName]
    private static void NNHPMKJDNPF(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    [SpecialName]
    public static bool PLIDNJAIPGF() => UplayPCController._isUplayStarted;

    public virtual void DEMHPEOELBA(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.JIPPIHGBLJP()) || !UplayPCController.MNLDEKGHHKL() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = true;
        if (!this._displayedFatalMessage)
          UISystemMessageController.CAMDAFOLNIB(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("_SecondaryRoomGIOffset", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("Blackscreen", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) 51:
              UISystemMessageController.CAMDAFOLNIB(this._userSignedOut, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = true;
              continue;
            case (UPLAY_EventType) 62:
              this.LNLLEAMHMFG();
              continue;
            case (UPLAY_EventType) 172:
              this.PDIKEBKHGFC();
              continue;
            default:
              continue;
          }
        }
      }
    }

    [SpecialName]
    private static void JAKAGNBAINP(string Ticket) => UplayPCController.TicketField = Ticket;

    public virtual void NIKPAHJKGBC(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.MNHAAMHCHGP()) || !UplayPCController.OANBGPNPLOJ() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = true;
        if (!this._displayedFatalMessage)
          UISystemMessageController.BCJNHCHPCKM(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("English(US)", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("Adding Context: {0} ", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -185:
              this.ECAPAGGGEBP();
              continue;
            case (UPLAY_EventType) 39:
              this.FMIPPIENHBB();
              continue;
            case (UPLAY_EventType) 76:
              UISystemMessageController.LCGNJHHOGKF(this._userSignedOut, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = false;
              continue;
            default:
              continue;
          }
        }
      }
    }

    public static void NFNIIBBLMPK(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.LILOEFNBEPA())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("Anim Layer Index Point is invalid (-1) on animator {0} !", result);
      }
      else
        Loggers.Online.Error("Initialize sound engine ...");
    }

    [SpecialName]
    private static void CFBFOONNEPA(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    private void GDJNIANPCOJ()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    private void HOFJMDNKOGI()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "XR Settings enabled (One Frame After LoadDevice) : {0} ", (object) aOutErrorString);
    }

    public virtual void FBLIOKKLNME(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.DJMLKCBCDPB()) || !UplayPCController.DIADJHONCEP() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = false;
        if (!this._displayedFatalMessage)
          UISystemMessageController.MLHMMKDHNMB(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("[Bootstrap Step] Playing Ubi Logo", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("_GlitchUvDeformationMax", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -89:
              UISystemMessageController.BOLDIGKIDLK(this._userSignedOut, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = true;
              continue;
            case (UPLAY_EventType) -57:
              this.GBLGJLIGJNN();
              continue;
            case (UPLAY_EventType) 55:
              this.FIFGBBCCCJK();
              continue;
            default:
              continue;
          }
        }
      }
    }

    [SpecialName]
    private static void IAJACBDKBPB(string Ticket) => UplayPCController.TicketField = Ticket;

    [SpecialName]
    public static UplayPCController.EnumNoneError FHJELEDPNBM() => UplayPCController.ErrorField;

    [SpecialName]
    private static void NDGGLJDFICM(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    public static UplayStartResult IMLFKIMAANL(uint UplayId)
    {
      UplayStartFlags aFlags = (UplayStartFlags) 1;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.KCKJOACIIDM(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>(", am touching ", UplayPCController.DJMLKCBCDPB());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("Oculus Entitlement error");
            UplayPCController.NNHPMKJDNPF(UplayPCController.EnumNoneError.None);
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = true;
          UplayPCController._isOverlayShowned = false;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Using NPC [{0}] from a cutscene that is already running. State of previous cutscene is left indeterminant.", aOutErrorString);
          UplayPCController.CHILLKGGNJM(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info(")");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("GSM State Enter\nOwner : {0}, State Machine: {1}, State: {2}, Transition: {3}");
          UplayPCController._isUplayStarted = false;
          UplayPCController.CFBFOONNEPA(UplayPCController.EnumNoneError.Error);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("_EmissionColor");
          UplayPCController.CFBFOONNEPA(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    private void CHFEDOOLCIC()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    [SpecialName]
    public static bool IsUplayStarted3() => UplayPCController._isUplayStarted;

    [SpecialName]
    private static void SetError2(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    public virtual void AppQuit()
    {
      if (GameController.CABMILFHAHC() != GameController.NMECCJHCPGF.IS_PAUSING || !UplayPCController.IsUplayStarted())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    [SpecialName]
    public static UplayPCController.EnumNoneError GetErrorField4() => UplayPCController.ErrorField;

    [SpecialName]
    public static bool IsUplayStarted4() => UplayPCController._isUplayStarted;

    public virtual void KBEFGPLPDKK()
    {
      if (GameController.DLBAJABCOIN() != GameController.NMECCJHCPGF.IS_LOADING || !UplayPCController.LILOEFNBEPA())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    public static void IFJIEKLEFDO(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.PLIDNJAIPGF())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("[Blackscreen] Trying to call fade to game with a context that wasn't set to blackscreen. [context: {0} | Current Caller: {1}]", result);
      }
      else
        Loggers.Online.Error("_GlitchNoiseAmount");
    }

    public virtual void MIFJNAMBEDP(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.MKAHOKMEECP()) || !UplayPCController.KOBKCEMKCBM() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = false;
        if (!this._displayedFatalMessage)
          UISystemMessageController.MLHMMKDHNMB(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("Could not play monster death sequence since proximityeffect is null", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("_MainTex", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -127:
              UISystemMessageController.LCGNJHHOGKF(this._userSignedOut, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = true;
              continue;
            case (UPLAY_EventType) 123:
              this.CHFEDOOLCIC();
              continue;
            case (UPLAY_EventType) 197:
              this.GDJNIANPCOJ();
              continue;
            default:
              continue;
          }
        }
      }
    }

    private void BJIJACMEAMI()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "SoundRtpcValueProvider - The RTPC you are trying to link is already linked to someone else.  Your result will be undefined", (object) aOutErrorString);
    }

    public static void ICEBPAMIHCP(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.NCMLKACLCHI())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("Can't unregister an event listener that hasn't been register {0}", result);
      }
      else
        Loggers.Online.Error("English(US)");
    }

    [SpecialName]
    private static void NCDFEMDNAOB(string Ticket) => UplayPCController.TicketField = Ticket;

    public static UplayStartResult NDFLFNGOKFE(uint UplayId)
    {
      UplayStartFlags aFlags = (UplayStartFlags) 1;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.BJAKELGAGAG(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("Expected intersection between plane {0} and points {1} and {2} but distance is too great", UplayPCController.JIPPIHGBLJP());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("Father");
            UplayPCController.MKILLPIFJIM(UplayPCController.EnumNoneError.Error);
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = false;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Plugins", aOutErrorString);
          UplayPCController.PMBJHLGLLNH(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("{0} 's Glitch Effect DefaultPreset0 is null");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("WwiseUnity: PostCallbacks aborted due to error: Undefined callback type <");
          UplayPCController._isUplayStarted = false;
          UplayPCController.DNHANAJLNIP(UplayPCController.EnumNoneError.Error);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("Saved sound emitter object length does not correspond to actual number of sound emitter objects, please resave scene");
          UplayPCController.DNHANAJLNIP(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    public virtual void EJENDHOMFPP()
    {
      if (GameController.HFAGKMPMJGD() != GameController.NMECCJHCPGF.IS_PAUSING || !UplayPCController.BPAEKILKGPA())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    public virtual void KJCFKBHMDKC(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.PAHGLFMCOHL()) || !UplayPCController.MMIMLNNFODP || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = true;
        if (!this._displayedFatalMessage)
          UISystemMessageController.LCGNJHHOGKF(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("UberSpotCookieTex", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("Base Layer", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -112:
              this.FIFGBBCCCJK();
              continue;
            case (UPLAY_EventType) 100:
              this.BJAHCEONIDD();
              continue;
            case (UPLAY_EventType) 121:
              UISystemMessageController.GJEAHPGDOAP(this._userSignedOut, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = false;
              continue;
            default:
              continue;
          }
        }
      }
    }

    public static UplayStartResult BJAEFFJFPPD(uint UplayId)
    {
      UplayStartFlags aFlags = (UplayStartFlags) 1;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.BJAKELGAGAG(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("[GameStartSequence] Switch State: Stopped", UplayPCController.BAODJPLFFAI());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("Steam Unavailable");
            UplayPCController.KPPHHPABHEH(UplayPCController.EnumNoneError.Error);
            UplayPCController._isUplayStarted = true;
          }
          else
            UplayPCController._isUplayStarted = true;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("LightGroupDoor has invalid room", aOutErrorString);
          UplayPCController.SetError2(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("GP_Wind");
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("French");
          UplayPCController._isUplayStarted = false;
          UplayPCController.PMBJHLGLLNH(UplayPCController.EnumNoneError.Error);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("_GlitchAmplitude");
          UplayPCController.EKBNMLDCGOD(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    [SpecialName]
    public static bool EOJOEKHKGBL() => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() == 1;

    private void IFMKMMIFCHE()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    public static void IEGLGMKBKJO(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.GICKEPPMONI())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("ReceptacleModel: ", result);
      }
      else
        Loggers.Online.Error("_GlitchRandomNormals");
    }

    public static void CJFLACNDJNF(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.MGNMGMOJFDB())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("Connection lost", result);
      }
      else
        Loggers.Online.Error("[Loading Controller] Trying to load a scene already loaded");
    }

    [SpecialName]
    public static bool MHGCMDCLGLA() => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() == 1;

    private void KLKCDOOLHOI()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "XYZ RotationAxis is not allowed.", (object) aOutErrorString);
    }

    private void AEFCKHKENMK()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    private void CDHCKCEBENI()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    private void DFOBCEBNALJ()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "License type {0}", (object) aOutErrorString);
    }

    public virtual void APNJHOPMCMI()
    {
      if (GameController.CABMILFHAHC() != GameController.NMECCJHCPGF.IS_QUITING || !UplayPCController.DIADJHONCEP())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    private void DCBKMDEJCIC()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    private void IMBIKLJBFDG()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "Out of range access in AkPositionArray", (object) aOutErrorString);
    }

    private void FOLLLFHHDGF()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    private void BLABIHMPPLJ()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "Generate new collectible save.", (object) aOutErrorString);
    }

    [SpecialName]
    public static UplayPCController.EnumNoneError HGMNLENJCJH() => UplayPCController.ErrorField;

    private void BCDNLMMFBLN()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "CheckResources () for {0} should be overwritten.", (object) aOutErrorString);
    }

    [SpecialName]
    public static bool NCMLKACLCHI() => UplayPCController._isUplayStarted;

    [SpecialName]
    public static string GOFICMPOMGD() => UplayPCController.TicketField;

    public virtual void MOOIHKNINEM()
    {
      if (GameController.CABMILFHAHC() != GameController.NMECCJHCPGF.IS_LOADING || !UplayPCController.OANBGPNPLOJ())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    public static void HMLFFJHHMKP(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.KOBKCEMKCBM())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("[Blackscreen] Fade to {0} requested by {1} [context: {2}][fadeTime: {3}]", result);
      }
      else
        Loggers.Online.Error("_inputObject");
    }

    [SpecialName]
    public static bool BPAEKILKGPA() => UplayPCController._isUplayStarted;

    private void CHLIAEAGDPE()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    private void HPHCOOFFNII()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "_BloomAttenuationFactor", (object) aOutErrorString);
    }

    private void HLFDCHIGKDF()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "ExitProcessRequired.", (object) aOutErrorString);
    }

    public virtual void BAHHCOIDIHA()
    {
      if (GameController.BJBOLHFDMOK() != GameController.NMECCJHCPGF.IS_QUITING || !UplayPCController.MMIMLNNFODP)
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    [SpecialName]
    private static void BMPBKFECGEJ(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    public static void NBGCNJMECLC(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.NPNJANCKLJD())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>("Boot", result);
      }
      else
        Loggers.Online.Error("_SecondaryRoomGIOffset");
    }

    public static UplayStartResult AFNAFFJPOIK(uint UplayId)
    {
      UplayStartFlags aFlags = (UplayStartFlags) 1;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.LBGDPHADOGO(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("Unknown CollisionState used", UplayPCController.DJMLKCBCDPB());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("The shader {0} on effect {1} is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package.");
            UplayPCController.MPLHNKDABAP(UplayPCController.EnumNoneError.Error);
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = true;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("_movieController", aOutErrorString);
          UplayPCController.MPLHNKDABAP(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("Start Game");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("file://");
          UplayPCController._isUplayStarted = false;
          UplayPCController.KADDOMAMICH(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("EDITOR CONTEXT MOVES TO {0}");
          UplayPCController.PMBJHLGLLNH(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    private void GBLGJLIGJNN()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    [SpecialName]
    private static void INAHJCJANAM(string Ticket) => UplayPCController.TicketField = Ticket;

    [SpecialName]
    private static void BJAKELGAGAG(string Ticket) => UplayPCController.TicketField = Ticket;

    public virtual void FAMMLDKDJBE(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.UplayTicket) || !UplayPCController.NCMLKACLCHI() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = false;
        if (!this._displayedFatalMessage)
          UISystemMessageController.MLHMMKDHNMB(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("Trying to stack a message twice: {0}", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("go_WallPart_AboveDoor", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -143:
              this.FMIPPIENHBB();
              continue;
            case (UPLAY_EventType) 34:
              this.PLOFCLCGAKA();
              continue;
            case (UPLAY_EventType) 192:
              UISystemMessageController.BOLDIGKIDLK(this._userSignedOut, UISystemMessageController.HPIECFJILGA.High, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = false;
              continue;
            default:
              continue;
          }
        }
      }
    }

    public virtual void BBLDGPKGADD()
    {
      if (GameController.DLBAJABCOIN() != GameController.NMECCJHCPGF.IS_RESTARTING_SEQUENCE || !UplayPCController.NIFPJIDAOLO())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    private void DADICCDJOIN()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "_DispTex", (object) aOutErrorString);
    }

    [SpecialName]
    private static void LBGDPHADOGO(string Ticket) => UplayPCController.TicketField = Ticket;

    [SpecialName]
    private static void MKILLPIFJIM(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    private void BNPOMFKAJPF()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    private void BLKABEPHCGL()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "_GlitchGlobalSmallNoiseOffset", (object) aOutErrorString);
    }

    [SpecialName]
    public static bool IHBIFKHOEHI() => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() == 0;

    private void FCABBBKHLGD()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    [SpecialName]
    public static string LIMEECNBHEF() => UplayPCController.TicketField;

    private void BBJOODPAEHA()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    [SpecialName]
    public static string DJMLKCBCDPB() => UplayPCController.TicketField;

    public static void FBGDDMKOGGL(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.DIADJHONCEP())
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>(")", result);
      }
      else
        Loggers.Online.Error("Closed");
    }

    private void CKCJDICNLLG()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    private void LICBKMBFGLM()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "AAAAAABBBBB", (object) aOutErrorString);
    }

    public static UplayStartResult MMAMFFCDLAP(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.BJAKELGAGAG(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("Trigger.RegisterCallbacks() - {0} has a null parameter. Audio log game event will not be triggered.", UplayPCController.MDKLFONHCEL());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("_SpatialWaveSamples");
            UplayPCController.EKBNMLDCGOD(UplayPCController.EnumNoneError.Error);
            UplayPCController._isUplayStarted = true;
          }
          else
            UplayPCController._isUplayStarted = true;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("[Bootstrap Step] The Game Controller object is null. Skipping Step.", aOutErrorString);
          UplayPCController.NDGGLJDFICM(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("Objects");
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("_MetallicGlossMap");
          UplayPCController._isUplayStarted = true;
          UplayPCController.IFDINHFCELN(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("Remove object {0} from room {1}");
          UplayPCController.MKILLPIFJIM(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    public static UplayStartResult NODOAINDBJF(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.IAJACBDKBPB(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("Failed to initialize the sound engine. Abort.", UplayPCController.UplayTicket);
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("Trying to play a NULL Video from ");
            UplayPCController.KPPHHPABHEH(UplayPCController.EnumNoneError.None);
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = false;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Intro Sequence", aOutErrorString);
          UplayPCController.EKBNMLDCGOD(UplayPCController.EnumNoneError.Error);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("[Bootstrap Step] Playing Ubi Logo");
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("Attempting to corrupt a room that is not setup : {0}");
          UplayPCController._isUplayStarted = false;
          UplayPCController.PMBJHLGLLNH(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("_EmissionColor");
          UplayPCController.MLDFANLFIAE(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    [SpecialName]
    private static void OGIBPODBLFG(string Ticket) => UplayPCController.TicketField = Ticket;

    public static UplayStartResult EEFGODBJCCA(uint UplayId)
    {
      UplayStartFlags aFlags = (UplayStartFlags) 1;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.IGHMMPEAIDA(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("I, ", UplayPCController.LIMEECNBHEF());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("< activating  Watermark: {0} >\n");
            UplayPCController.BMPBKFECGEJ(UplayPCController.EnumNoneError.Error);
            UplayPCController._isUplayStarted = true;
          }
          else
            UplayPCController._isUplayStarted = true;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Timelines", aOutErrorString);
          UplayPCController.JINCLCLCIMG(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("[GameStartSequence] Switch State: Full Loading Start");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("InteractableSequenceList was cleared because it still contained {0} elements");
          UplayPCController._isUplayStarted = true;
          UplayPCController.MLDFANLFIAE(UplayPCController.EnumNoneError.Error);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error(" failed to reload (");
          UplayPCController.NDGGLJDFICM(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    [SpecialName]
    private static void IFDINHFCELN(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    private void BJAHCEONIDD()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    private void FIFGBBCCCJK()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    private void PGCPAHJCAOA()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    private void DEFEEEMBOPK()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    public virtual void EHBADEJFOFJ(float BACFOJCAGOI)
    {
      if (string.IsNullOrEmpty(UplayPCController.GetTicketField()) || !UplayPCController.OIHKDHJBNIO() || this._isConnectionLost)
        return;
      if (UplayPCCAPI.UPLAY_Update() == 0)
      {
        this._isConnectionLost = false;
        if (!this._displayedFatalMessage)
          UISystemMessageController.CAMDAFOLNIB(this._droppedUplaySdk, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
        string aOutErrorString;
        UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
        Loggers.UPlay.Error<string>("German", aOutErrorString);
      }
      else
      {
        UPLAY_Event aEvent;
        while (UplayPCCAPI.UPLAY_GetNextEvent(out aEvent) != (byte) 0)
        {
          Loggers.UPlay.Info<UPLAY_EventType>("go_WallPart_BelowOpening", aEvent.type);
          switch (aEvent.type)
          {
            case (UPLAY_EventType) -143:
              this.LNLLEAMHMFG();
              continue;
            case (UPLAY_EventType) 60:
              UISystemMessageController.LCGNJHHOGKF(this._userSignedOut, UISystemMessageController.HPIECFJILGA.Medium, (Action<UISystemMessageController.MFHMHAMPNJP>) null);
              this._displayedFatalMessage = true;
              continue;
            case (UPLAY_EventType) 143:
              this.MHEMLHEABCK();
              continue;
            default:
              continue;
          }
        }
      }
    }

    private void AHCKCMLACKN()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "Steam Entitlement error", (object) aOutErrorString);
    }

    [SpecialName]
    public static UplayPCController.EnumNoneError CCCCPPHKKMO() => UplayPCController.ErrorField;

    public static UplayStartResult GKLLIMAJIEF(uint UplayId)
    {
      UplayStartFlags aFlags = (UplayStartFlags) 1;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.IGHMMPEAIDA(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("Video", UplayPCController.ANNMBIDOHOO());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("{0} {1}");
            UplayPCController.PHIMPMNIGIP(UplayPCController.EnumNoneError.None);
            UplayPCController._isUplayStarted = true;
          }
          else
            UplayPCController._isUplayStarted = false;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("_SpatialWaveSamples", aOutErrorString);
          UplayPCController.DNHANAJLNIP(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("_GlitchGlobalWidth");
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("Glitch");
          UplayPCController._isUplayStarted = false;
          UplayPCController.MPLHNKDABAP(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("Categories state not found");
          UplayPCController.PHIMPMNIGIP(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    public virtual void KGBOKLMALMB()
    {
      if (GameController.DLBAJABCOIN() != GameController.NMECCJHCPGF.IS_QUITING || !UplayPCController.DIADJHONCEP())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    public static void FBIFLPKDHAC(UPLAY_OVERLAY_Section DPNKJHGODEM)
    {
      if (UplayPCController._isOverlayShowned || !UplayPCController.MMIMLNNFODP)
        return;
      if (UplayPCCAPI.ShowOverlay(DPNKJHGODEM, ref UplayPCController._overlayHandle) != 0)
      {
        UPLAY_OverlappedResult result;
        UplayPCCAPI.GetOverlappedResult(ref UplayPCController._overlayHandle, out result);
        Loggers.Online.Info<UPLAY_OverlappedResult>(">", result);
      }
      else
        Loggers.Online.Error("Debug text is missing a reference in Inventory Controller Prefab");
    }

    private void FMIPPIENHBB()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    [SpecialName]
    public static bool MNLDEKGHHKL() => UplayPCController._isUplayStarted;

    [SpecialName]
    private static void PHIMPMNIGIP(UplayPCController.EnumNoneError Ticket) => UplayPCController.ErrorField = Ticket;

    public virtual void HEJFCNDAHKK()
    {
      if (GameController.CABMILFHAHC() != GameController.NMECCJHCPGF.IS_UNLOADING || !UplayPCController.IsUplayStarted3())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    public static UplayStartResult HCCLLPNGFKF(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.UplayTicket = UplayPCCAPI.UPLAY_USER_GetTicket();
          Loggers.UPlay.Info<string>("Uplay started. User Uplay Ticket: {0}", UplayPCController.UplayTicket);
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("Player doesn't own this game.");
            UplayPCController.EPOLBNDOKFL = UplayPCController.EnumNoneError.Error;
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = true;
          UplayPCController._isOverlayShowned = false;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Uplay startup failed! Error: {0}", aOutErrorString);
          UplayPCController.EPOLBNDOKFL = UplayPCController.EnumNoneError.Error;
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("ExitProcessRequired.");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("Uplay installation error.");
          UplayPCController._isUplayStarted = false;
          UplayPCController.EPOLBNDOKFL = UplayPCController.EnumNoneError.Error;
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("Uplay DesktopInteractionRequired.");
          UplayPCController.EPOLBNDOKFL = UplayPCController.EnumNoneError.Error;
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    [SpecialName]
    public static bool CKLMLGOLNLE() => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() == 1;

    [SpecialName]
    private static void KHIFHGBKFJL(string Ticket) => UplayPCController.TicketField = Ticket;

    [SpecialName]
    public static bool NPNJANCKLJD() => UplayPCController._isUplayStarted;

    [SpecialName]
    public static bool LILOEFNBEPA() => UplayPCController._isUplayStarted;

    private void PKIHIAJEEIG()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    [SpecialName]
    public static string MKAHOKMEECP() => UplayPCController.TicketField;

    [SpecialName]
    public static string MJLFLABLBKL() => UplayPCController.TicketField;

    [SpecialName]
    public static bool DCOECEPHMNI() => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() != 0;

    public virtual void MDJCICGNPBH()
    {
      if (GameController.CABMILFHAHC() != GameController.NMECCJHCPGF.IS_RUNNING || !UplayPCController.NIFPJIDAOLO())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    [SpecialName]
    private static void LEHFNBIHJFI(string Ticket) => UplayPCController.TicketField = Ticket;

    private void MHEMLHEABCK()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    public override void StopController()
    {
      if (GameController.HFAGKMPMJGD() != GameController.NMECCJHCPGF.IS_QUITING || !UplayPCController.MMIMLNNFODP)
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    private void NBAIAFPJEDJ()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "go_DoorFrameRight", (object) aOutErrorString);
    }

    [SpecialName]
    public static string BAODJPLFFAI() => UplayPCController.TicketField;

    public virtual void LPPEAEEGCAD()
    {
      if (GameController.HFAGKMPMJGD() != GameController.NMECCJHCPGF.IS_PAUSING || !UplayPCController.GICKEPPMONI())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    [SpecialName]
    public static string HFFCFAJPPHN() => UplayPCController.TicketField;

    public static UplayStartResult KMICCMABGMB(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.JAKAGNBAINP(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("go_StairFront_", UplayPCController.HFFCFAJPPHN());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("The native dll is already loaded, this should not happen if ReleaseManagedResources is used and Steam.Initialize() is only called once.");
            UplayPCController.KADDOMAMICH(UplayPCController.EnumNoneError.Error);
            UplayPCController._isUplayStarted = false;
          }
          else
            UplayPCController._isUplayStarted = true;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Chinese", aOutErrorString);
          UplayPCController.CHILLKGGNJM(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("VolFogLight Gizmo.tga");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("Floor");
          UplayPCController._isUplayStarted = false;
          UplayPCController.PHIMPMNIGIP(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("enabled");
          UplayPCController.EPOLBNDOKFL = UplayPCController.EnumNoneError.Error;
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    [SpecialName]
    public static bool NIFPJIDAOLO() => UplayPCController._isUplayStarted;

    public virtual void POGLBLBLMIN()
    {
      if (GameController.CABMILFHAHC() != GameController.NMECCJHCPGF.IS_UNLOADING || !UplayPCController.KOBKCEMKCBM())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    private void HJFGDBHCMAK()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayClosed.SafeInvoke();
    }

    [SpecialName]
    private static void FEPNBBOGIJI(string Ticket) => UplayPCController.TicketField = Ticket;

    public static UplayStartResult GFEBMFBAOOB(uint UplayId)
    {
      UplayStartFlags aFlags = UplayStartFlags.None;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.NCDFEMDNAOB(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("type", UplayPCController.ANNMBIDOHOO());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("AnimatedCollider");
            UplayPCController.NNHPMKJDNPF(UplayPCController.EnumNoneError.None);
            UplayPCController._isUplayStarted = true;
          }
          else
            UplayPCController._isUplayStarted = true;
          UplayPCController._isOverlayShowned = true;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("[GameStartSequence] Switch State: Stopped", aOutErrorString);
          UplayPCController.KADDOMAMICH(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("EyesSaccadesTargetRef");
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("Attempted to trigger a Force Kill Player With Monster while the monster was already in a kill sequence !");
          UplayPCController._isUplayStarted = true;
          UplayPCController.MLDFANLFIAE(UplayPCController.EnumNoneError.None);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("DecodedBanks");
          UplayPCController.KPPHHPABHEH(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    public virtual void OJCALFBOGLL()
    {
      if (GameController.DLBAJABCOIN() != GameController.NMECCJHCPGF.IS_BOOTING || !UplayPCController.IsUplayStarted4())
        return;
      UplayPCCAPI.UPLAY_Quit();
    }

    private void OverlayWork()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    [SpecialName]
    public static UplayPCController.EnumNoneError GetErrorField3() => UplayPCController.ErrorField;

    [SpecialName]
    public static string JIPPIHGBLJP() => UplayPCController.TicketField;

    [SpecialName]
    public static bool JKHLFJHCFAJ() => UplayPCCAPI.UPLAY_USER_IsInOfflineMode() == 1;

    [SpecialName]
    public static UplayPCController.EnumNoneError GetErrorField2() => UplayPCController.ErrorField;

    private void OverlayOpened()
    {
      UplayPCController._isOverlayShowned = false;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    [SpecialName]
    private static void SetTicket2(string Ticket) => UplayPCController.TicketField = Ticket;

    [SpecialName]
    public static bool IsUplayStarted2() => UplayPCController._isUplayStarted;

    private void GetLastError()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "Uplay Error: {0}", (object) aOutErrorString);
    }

    private void OverlayWork2()
    {
      UplayPCController._isOverlayShowned = true;
      UplayPCController.OnOverlayOpened.SafeInvoke();
    }

    public static UplayStartResult EOGDLCICNDJ(uint UplayId)
    {
      UplayStartFlags aFlags = (UplayStartFlags) 1;
      UplayStartResult uplayStartResult = UplayPCCAPI.UPLAY_Start(UplayId, aFlags);
      switch (uplayStartResult)
      {
        case UplayStartResult.Ok:
          UplayPCController.LBGDPHADOGO(UplayPCCAPI.UPLAY_USER_GetTicket());
          Loggers.UPlay.Info<string>("_gameController", UplayPCController.JIPPIHGBLJP());
          if (UplayPCCAPI.UPLAY_USER_IsOwned(UplayId) == 0)
          {
            Loggers.UPlay.Error("ConfigFileHelper");
            UplayPCController.EKBNMLDCGOD(UplayPCController.EnumNoneError.None);
            UplayPCController._isUplayStarted = true;
          }
          else
            UplayPCController._isUplayStarted = false;
          UplayPCController._isOverlayShowned = false;
          UplayPCController._overlayHandle = new UPLAY_Overlapped();
          return uplayStartResult;
        case UplayStartResult.Failed:
          string aOutErrorString;
          UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
          Loggers.UPlay.Error<string>("Could not post event ID \"", aOutErrorString);
          UplayPCController.SetError2(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.ExitProcessRequired:
          Loggers.UPlay.Info("Using invalid mode for this update.");
          UplayPCController._isUplayStarted = true;
          return uplayStartResult;
        case UplayStartResult.InstallationError:
          Loggers.UPlay.Error("###################################################################");
          UplayPCController._isUplayStarted = false;
          UplayPCController.IFDINHFCELN(UplayPCController.EnumNoneError.Error);
          return uplayStartResult;
        case UplayStartResult.DesktopInteractionRequired:
          Loggers.UPlay.Error("_GlitchGlobalHeight");
          UplayPCController.SetError2(UplayPCController.EnumNoneError.None);
          UplayPCController._isUplayStarted = false;
          return uplayStartResult;
        default:
          return uplayStartResult;
      }
    }

    [SpecialName]
    public static bool MGNMGMOJFDB() => UplayPCController._isUplayStarted;

    private void HLNCDDNGENG()
    {
      string aOutErrorString;
      UplayPCCAPI.UPLAY_GetLastError(out aOutErrorString);
      Loggers.UPlay.ErrorWithContext((UnityEngine.Object) this.gameObject, "#ofLarge:", (object) aOutErrorString);
    }

    public enum EnumNoneError
    {
      None,
      Error,
    }
  }