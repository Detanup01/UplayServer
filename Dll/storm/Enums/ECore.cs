namespace Storm.Enums;

// Token: 0x020000A9 RID: 169
public enum ECode
{
    // Token: 0x04000159 RID: 345
    NotAssigned,
    // Token: 0x0400015A RID: 346
    OK,
    // Token: 0x0400015B RID: 347
    OK_Async,
    // Token: 0x0400015C RID: 348
    SocketRebindUpnpMappingConflict,
    // Token: 0x0400015D RID: 349
    SocketRebindNATTypeIncoherent,
    // Token: 0x0400015E RID: 350
    Failed = -1000000,
    // Token: 0x0400015F RID: 351
    NotImplemented,
    // Token: 0x04000160 RID: 352
    NotSupported,
    // Token: 0x04000161 RID: 353
    FeatureNotActivated,
    // Token: 0x04000162 RID: 354
    HandlerRefIdInvalid,
    // Token: 0x04000163 RID: 355
    HandlerRefIdRemoved,
    // Token: 0x04000164 RID: 356
    InitStateUnexpected,
    // Token: 0x04000165 RID: 357
    ResultEventNotAvailable,
    // Token: 0x04000166 RID: 358
    PeerMustBeHost,
    // Token: 0x04000167 RID: 359
    CanNotBeDoneToYourself,
    // Token: 0x04000168 RID: 360
    ProxyNotFound,
    // Token: 0x04000169 RID: 361
    ProxyNotReady,
    // Token: 0x0400016A RID: 362
    WrongObjectType,
    // Token: 0x0400016B RID: 363
    WrongObjectTypeId,
    // Token: 0x0400016C RID: 364
    WrongObjectGroupType,
    // Token: 0x0400016D RID: 365
    WrongProtocolEntryId,
    // Token: 0x0400016E RID: 366
    InvalidGUID,
    // Token: 0x0400016F RID: 367
    Timeout,
    // Token: 0x04000170 RID: 368
    VersionMismatch,
    // Token: 0x04000171 RID: 369
    InvalidParameter,
    // Token: 0x04000172 RID: 370
    ModuleDisabled,
    // Token: 0x04000173 RID: 371
    ModuleNotCreated,
    // Token: 0x04000174 RID: 372
    ModuleNotInitialized,
    // Token: 0x04000175 RID: 373
    ModuleNotStarted,
    // Token: 0x04000176 RID: 374
    ModuleNotInAppropriateState,
    // Token: 0x04000177 RID: 375
    ApiRequestCancelled,
    // Token: 0x04000178 RID: 376
    NotificationHandlerAlreadyRegistered,
    // Token: 0x04000179 RID: 377
    NotificationHandlerNotRegistered,
    // Token: 0x0400017A RID: 378
    IdPoolExhausted,
    // Token: 0x0400017B RID: 379
    StreamReadFailed,
    // Token: 0x0400017C RID: 380
    StreamWriteFailed,
    // Token: 0x0400017D RID: 381
    DataContainerCreateFailed,
    // Token: 0x0400017E RID: 382
    TransmissionStreamReadError,
    // Token: 0x0400017F RID: 383
    TransmissionInvalidConnectionState,
    // Token: 0x04000180 RID: 384
    TransmissionInvalidPrototypeType,
    // Token: 0x04000181 RID: 385
    TransmissionMissingTransmissionTable,
    // Token: 0x04000182 RID: 386
    TransmissionChecksumMismatch,
    // Token: 0x04000183 RID: 387
    TransmissionMessageNotSent,
    // Token: 0x04000184 RID: 388
    TransmissionMessageSentButNotConfirmed,
    // Token: 0x04000185 RID: 389
    TransmissionMessageSentButNotDelivered,
    // Token: 0x04000186 RID: 390
    SocketBindFailed,
    // Token: 0x04000187 RID: 391
    SocketOpenFailed,
    // Token: 0x04000188 RID: 392
    SocketSetBlockingFailed,
    // Token: 0x04000189 RID: 393
    SocketAlreadyCreated,
    // Token: 0x0400018A RID: 394
    SocketReuseAddressFailed,
    // Token: 0x0400018B RID: 395
    SocketSetBroadcastFailed,
    // Token: 0x0400018C RID: 396
    SocketClosed,
    // Token: 0x0400018D RID: 397
    SocketNotAvailable,
    // Token: 0x0400018E RID: 398
    LanModuleMatchmakingSocketSetupFailed,
    // Token: 0x0400018F RID: 399
    LanModuleDiscoverySocketSetupFailed,
    // Token: 0x04000190 RID: 400
    NetEngineNotCreated,
    // Token: 0x04000191 RID: 401
    NetEngineAlreadyStarted,
    // Token: 0x04000192 RID: 402
    NetEngineNotShutdown,
    // Token: 0x04000193 RID: 403
    NetEngineNotStarted,
    // Token: 0x04000194 RID: 404
    NetEngineProtocolValidationFailed,
    // Token: 0x04000195 RID: 405
    NetEngineNoShutdownDuringUpdate,
    // Token: 0x04000196 RID: 406
    NetEngineModuleNotAvailable,
    // Token: 0x04000197 RID: 407
    NetEngineOnlyAvailableInSession,
    // Token: 0x04000198 RID: 408
    PeerChannelTargetPeerInvalid,
    // Token: 0x04000199 RID: 409
    PeerChannelTargetPeerIsGone,
    // Token: 0x0400019A RID: 410
    PeerChannelUnicastToSelf,
    // Token: 0x0400019B RID: 411
    PeerChannelTransmissionFull,
    // Token: 0x0400019C RID: 412
    ObjectRefIdInvalid,
    // Token: 0x0400019D RID: 413
    ObjectRouteIsNotAvailable,
    // Token: 0x0400019E RID: 414
    ObjectRouteAlreadyExists,
    // Token: 0x0400019F RID: 415
    ObjectConnectionNotPublished,
    // Token: 0x040001A0 RID: 416
    ObjectMessageNotSentNoValidRouteFound,
    // Token: 0x040001A1 RID: 417
    ObjectMessageResolvedReplicaNotReady,
    // Token: 0x040001A2 RID: 418
    ObjectWrongNetId,
    // Token: 0x040001A3 RID: 419
    ObjectMessageTransmissionFull,
    // Token: 0x040001A4 RID: 420
    ObjectObserverAlreadyRemoved,
    // Token: 0x040001A5 RID: 421
    SessionRefIdInvalid,
    // Token: 0x040001A6 RID: 422
    SessionHostMigrationDisabled,
    // Token: 0x040001A7 RID: 423
    SessionHostMigrationInvalidTopology,
    // Token: 0x040001A8 RID: 424
    SessionKicked,
    // Token: 0x040001A9 RID: 425
    SessionDissolved,
    // Token: 0x040001AA RID: 426
    SessionRefused,
    // Token: 0x040001AB RID: 427
    SessionHostGone,
    // Token: 0x040001AC RID: 428
    SessionPeerGone,
    // Token: 0x040001AD RID: 429
    SessionInvalid,
    // Token: 0x040001AE RID: 430
    SessionRefIdPoolExhausted,
    // Token: 0x040001AF RID: 431
    SessionPeerNotFound,
    // Token: 0x040001B0 RID: 432
    SessionMaxSession,
    // Token: 0x040001B1 RID: 433
    SessionVersionMismatch,
    // Token: 0x040001B2 RID: 434
    SessionExists,
    // Token: 0x040001B3 RID: 435
    SessionLeaving,
    // Token: 0x040001B4 RID: 436
    SessionHostMigrationRunning,
    // Token: 0x040001B5 RID: 437
    SessionRefIdError,
    // Token: 0x040001B6 RID: 438
    GroupRefIdInvalid,
    // Token: 0x040001B7 RID: 439
    GroupRefIdPoolExhausted,
    // Token: 0x040001B8 RID: 440
    GroupRequestDuplicated,
    // Token: 0x040001B9 RID: 441
    GroupPeerAlreadyExists,
    // Token: 0x040001BA RID: 442
    GroupVersionMismatch,
    // Token: 0x040001BB RID: 443
    GroupPeerIdPoolExhausted,
    // Token: 0x040001BC RID: 444
    GroupPeerIdInvalid,
    // Token: 0x040001BD RID: 445
    GroupRequestNotFound,
    // Token: 0x040001BE RID: 446
    GroupPeerDescriptorInvalid,
    // Token: 0x040001BF RID: 447
    GroupDeleted,
    // Token: 0x040001C0 RID: 448
    GroupMaxPeerReached,
    // Token: 0x040001C1 RID: 449
    GroupDescriptorInvalid,
    // Token: 0x040001C2 RID: 450
    GroupRefIdError,
    // Token: 0x040001C3 RID: 451
    GroupAddPeerRefused,
    // Token: 0x040001C4 RID: 452
    AudioHardwareError,
    // Token: 0x040001C5 RID: 453
    NoValidLocalUser,
    // Token: 0x040001C6 RID: 454
    Link_Fail,
    // Token: 0x040001C7 RID: 455
    Link_ResolveFail,
    // Token: 0x040001C8 RID: 456
    Link_ResolveTimeout,
    // Token: 0x040001C9 RID: 457
    Link_ReachFail,
    // Token: 0x040001CA RID: 458
    Link_ReachRefused,
    // Token: 0x040001CB RID: 459
    Link_Drop,
    // Token: 0x040001CC RID: 460
    Link_Disconnected,
    // Token: 0x040001CD RID: 461
    Connection_Fail,
    // Token: 0x040001CE RID: 462
    Connection_RequestFail,
    // Token: 0x040001CF RID: 463
    Connection_ReachFail,
    // Token: 0x040001D0 RID: 464
    Connection_CipherFail,
    // Token: 0x040001D1 RID: 465
    Connection_AuthenticationFail,
    // Token: 0x040001D2 RID: 466
    Connection_ReachRefused,
    // Token: 0x040001D3 RID: 467
    Connection_Drop,
    // Token: 0x040001D4 RID: 468
    Connection_Disconnection,
    // Token: 0x040001D5 RID: 469
    Connection_Timeout,
    // Token: 0x040001D6 RID: 470
    TransportColorPoolExhausted,
    // Token: 0x040001D7 RID: 471
    PunchClientNotificationHandlerAlreadyRegistered,
    // Token: 0x040001D8 RID: 472
    PunchClientNotificationHandlerNotRegistered,
    // Token: 0x040001D9 RID: 473
    PunchClientMpePacketHandlerAlreadyRegistered,
    // Token: 0x040001DA RID: 474
    PunchClientMpePacketHandlerNotRegistered,
    // Token: 0x040001DB RID: 475
    PunchTraversalMpeAddressAlreadyRegistered,
    // Token: 0x040001DC RID: 476
    PunchTraversalMpeAddressNotRegistered,
    // Token: 0x040001DD RID: 477
    PunchTraversalAlreadyUsingServer,
    // Token: 0x040001DE RID: 478
    PunchTraversalNoServerInUse,
    // Token: 0x040001DF RID: 479
    PunchTraversalConnectionLost,
    // Token: 0x040001E0 RID: 480
    PunchTraversalConnectionTimeout,
    // Token: 0x040001E1 RID: 481
    PunchTraversalUrlNotResolved,
    // Token: 0x040001E2 RID: 482
    PunchServerDetect1Unreachable,
    // Token: 0x040001E3 RID: 483
    PunchServerDetect2Unreachable,
    // Token: 0x040001E4 RID: 484
    PunchDetectDetectAlreadyInProgress,
    // Token: 0x040001E5 RID: 485
    PunchDetectMap1Failed,
    // Token: 0x040001E6 RID: 486
    PunchDetectTimedOut,
    // Token: 0x040001E7 RID: 487
    PunchDetectAlreadyEnabled,
    // Token: 0x040001E8 RID: 488
    PunchUpnpNoDeviceFound,
    // Token: 0x040001E9 RID: 489
    PunchUpnpThreadNotStarted,
    // Token: 0x040001EA RID: 490
    PunchUpnpCancelInProgress,
    // Token: 0x040001EB RID: 491
    PunchUpnpCommandAlreadyInProgress,
    // Token: 0x040001EC RID: 492
    Audio_DeviceNotFound,
    // Token: 0x040001ED RID: 493
    Audio_DeviceNotConnected,
    // Token: 0x040001EE RID: 494
    Audio_DeviceAlreadyInUse,
    // Token: 0x040001EF RID: 495
    Audio_DeviceNotSupported,
    // Token: 0x040001F0 RID: 496
    Audio_NotAuthorized,
    // Token: 0x040001F1 RID: 497
    Audio_WaitingForAuthorization,
    // Token: 0x040001F2 RID: 498
    Video_DeviceNotFound,
    // Token: 0x040001F3 RID: 499
    Video_DeviceNotConnected,
    // Token: 0x040001F4 RID: 500
    Video_DeviceAlreadyInUse,
    // Token: 0x040001F5 RID: 501
    Video_DeviceNotSupported,
    // Token: 0x040001F6 RID: 502
    Video_CodecNotSupported,
    // Token: 0x040001F7 RID: 503
    Video_PixelFormatNotSupported,
    // Token: 0x040001F8 RID: 504
    Video_ResolutionNotSupported,
    // Token: 0x040001F9 RID: 505
    Video_FramerateNotSupported,
    // Token: 0x040001FA RID: 506
    Video_NotAuthorized,
    // Token: 0x040001FB RID: 507
    Video_WaitingForAuthorization,
    // Token: 0x040001FC RID: 508
    Video_EncodeFailed,
    // Token: 0x040001FD RID: 509
    Video_DecodeFailed,
    // Token: 0x040001FE RID: 510
    Video_FrameDropped,
    // Token: 0x040001FF RID: 511
    Services_USError,
    // Token: 0x04000200 RID: 512
    Services_SMMError,
    // Token: 0x04000201 RID: 513
    Services_PunchError,
    // Token: 0x04000202 RID: 514
    Services_LocalAddressError,
    // Token: 0x04000203 RID: 515
    Registry_SessionTypeNotFound,
    // Token: 0x04000204 RID: 516
    Registry_SessionTypeCommitted,
    // Token: 0x04000205 RID: 517
    Registry_SessionTypeNotCommitted,
    // Token: 0x04000206 RID: 518
    Registry_GroupTypeNotCommitted,
    // Token: 0x04000207 RID: 519
    EndMarker
}
