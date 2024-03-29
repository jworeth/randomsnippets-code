namespace Flux.WoW.Patchables
{
    // ReSharper disable InconsistentNaming
    public enum GlobalOffsets : uint
    {
        pGxDevice = 0x01254928,
        CGWorldFrameRenderHook = 0x00499A4F,
        CGLootInfo__ItemArray = 0x011533A4,
        LocalPlayerComboPoints = 0x0113D849,
        CorpsePointStruct = 0x0113DA34,
        LocalPlayerKnownSpells = 0x0113F7E0,
        LocalPlayerCurrentContinentId = 0x00A38390,
        RealZoneText = 0x0113D77C,
        NameplateDistance = 0x01051DCC,
        Base_DbCache = 0x01254030,
        ClickToMove_Base = 0x01297920,
        IsInGame = 0x010508A0,
        LocalPlayerSpellsOnCooldown = 0x0133B7C0,

        AddLineToExecFile = 0x006E1490,
        AdditionalCheck1 = 0x00404560,
        AdditionalCheck2 = 0x00821940,
        AsyncFileReadWaitAll = 0x0045FB80,
        BATCHEDRENDERFONTDESC___BATCHEDRENDERFONTDESC = 0x0063FE20,
        BuyItem = 0x0064CDD0,
        CBackdropGenerator__LoadXML = 0x00447F10,
        CCharCreateInfo__CreateCharacter = 0x004825D0,
        CCharCreateInfo__CycleCharCustomization = 0x004824D0,
        CCharCreateInfo__RandomizeCharCustomization = 0x00483610,
        CCharCreateInfo__ResetCharCustomizeInfo = 0x00483D70,
        CCharCreateInfo__SetSelectedClass = 0x00483560,
        CCharCreateInfo__SetSelectedRace = 0x00483E40,
        CCharCreateInfo__SetSelectedSex = 0x00483430,
        CDataStore__DetachBuffer = 0x00421500,
        CDataStore__Finalize = 0x00401150,
        CDataStore__GetBufferParams = 0x004214D0,
        CDataStore__GetString = 0x00421B70,
        CDataStore__Get_5 = 0x00421AB0,
        CDataStore__Get_9 = 0x00421A30,
        CDataStore__InternalDestroy = 0x00421540,
        CDataStore__InternalFetchRead = 0x008CF570,
        CDataStore__InternalFetchWrite = 0x00421590,
        CDataStore__IsRead = 0x004010F0,
        CDataStore__Put_1 = 0x004217F0,
        CDataStore__Put_6 = 0x00421790,
        CDataStore__Put_9 = 0x004216D0,
        CDataStore__Reset = 0x00401100,
        CDataStore___scalar_deleting_destructor_ = 0x00403390,
        CDebugSCritSect__DumpAllEntries = 0x006EA650,
        CDebugSRWLock__DumpAllEntries = 0x006EA6A0,
        CEffect__AddEffect = 0x00672320,
        CEffect__UpdateAttachment = 0x00671AD0,
        CGActionBar__GetCooldown = 0x0052A720,
        CGActionBar__GetTexture = 0x0052B0D0,
        CGActionBar__IsCurrentAction = 0x0052BB20,
        CGActionBar__PickupAction = 0x0052D760,
        CGActionBar__PutActionInSlot = 0x0052CA00,
        CGActionBar__UseAction = 0x0052D4A0,
        CGBag_C__FindItem = 0x006CBA50,
        CGBag_C__FindItem_0 = 0x006CB470,
        CGBankInfo__OnCloseBank = 0x005012F0,
        CGCamera_Unknown0 = 0x0058B730,
        CGCamera_Unknown1 = 0x00589C60,
        CGCamera_Unknown2 = 0x00589EB0,
        CGCamera_Unknown3 = 0x0058A6F0,
        CGCamera_Unknown4 = 0x004765B0,
        CGCamera_Unknown5 = 0x00589D90,
        CGCamera__CreateViewFromCamera = 0x00583900,
        CGCamera__ParentToWorld = 0x00585EE0,
        CGCamera__ResetView = 0x00589BC0,
        CGCamera__SetView = 0x005886B0,
        CGCamera__SyncFreeLookFacing = 0x00587750,
        CGCamera__UpdateFreeLookFacing = 0x00587430,
        CGCamera_virt00 = 0x00584930,
        CGCamera_virt04 = 0x00585FA0,
        CGCamera_virt08 = 0x00586040,
        CGCamera_virt0C = 0x005860E0,
        CGCharacterInfo__PutItemInBackpack = 0x0056F0F0,
        CGChat__AddChatMessage = 0x004AA640,
        CGClassTrainer__AddServices = 0x00518650,
        CGClassTrainer__GetSkillLineIndexFromService = 0x005164B0,
        CGClassTrainer__SetTrainer = 0x005162B0,
        CGContainerInfo__LeaveWorld = 0x005614C0,
        CGContainer_C__CGContainer_C = 0x00681E40,
        CGContainer_C_virt00 = 0x00683870,
        CGCorpse_C__PostInit = 0x0067E210,
        CGDuelInfo__AcceptDuel__ = 0x005580F0,
        CGDuelInfo__CancelDuel = 0x00558170,
        CGDuelInfo__InitializeGame = 0x005582F0,
        CGDynamicObject_C__PostInit = 0x0067D8B0,
        CGGameObject_C__Initialize = 0x0068C5D0,
        CGGameObject_C__OnRightClick = 0x00689700,
        CGGameObject_C__PostInit = 0x0068B4A0,
        CGGameObject_C__Shutdown = 0x0068C6D0,
        CGGameObject_C_virt00 = 0x0068C450,
        CGGameObject_C_virt04 = 0x0068B610,
        CGGameObject_C_virt08 = 0x00687390,
        CGGameObject_C_virt0C = 0x0068B6A0,
        CGGameObject_C_virt10 = 0x006852B0,
        CGGameObject_C_virt18 = 0x00687F80,
        CGGameObject_C_virt38 = 0x00689760,
        CGGameObject_C_virt3C = 0x0068A9A0,
        CGGameObject_C_virt40 = 0x006874C0,
        CGGameObject_C_virt64 = 0x0068C480,
        CGGameObject_C_virt84 = 0x00687BC0,
        CGGameUI__ClearInteractTarget = 0x004B8BF0,
        CGGameUI__CloseInteraction = 0x004B2E10,
        CGGameUI__DisplayError = 0x004C0E10,
        CGGameUI__GetCursorItem = 0x004B3580,
        CGGameUI__GetPartyMember = 0x004B3770,
        CGGameUI__Idle = 0x004CACB0,
        CGGameUI__Initialize = 0x004CA6D0,
        CGGameUI__InitializeGame = 0x004CB170,
        CGGameUI__LastErrorMessage = 0x004B3C20,
        CGGameUI__OpenGuildInvite = 0x004B26F0,
        CGGameUI__Reload = 0x004B3450,
        CGGameUI__SetCursorMoney = 0x004C0280,
        CGGameUI__SetInteractTarget = 0x004BF810,
        CGGameUI__ShowCombatFeedback = 0x004B32F0,
        CGGameUI__ShowCombatFeedback_0 = 0x004B32C0,
        CGGameUI__Shutdown = 0x004C8BE0,
        CGGameUI__ShutdownGame = 0x004C8E20,
        CGGameUI__StartCinematicCamera = 0x004C6080,
        CGGameUI__StopCinematicInternal = 0x004BD950,
        CGGameUI__Target = 0x004C4940,
        CGInputControl__CGInputControl = 0x00582190,
        CGInputControl__GetActive = 0x0057E9E0,
        CGInputControl__OnMouseMoveRel = 0x0057FFB0,
        CGInputControl__SetControlBit = 0x0057F580,
        CGInputControl__ToggleControlBit = 0x00581230,
        CGInputControl__UnsetControlBit = 0x0057F890,
        CGItemText__SetItem = 0x0050FDC0,
        CGItem_C__CGItem_C = 0x0067F090,
        CGItem_C__GetClassID = 0x0067F890,
        CGItem_C__GetInventoryArt = 0x00682FA0,
        CGItem_C__Initialize = 0x00681A40,
        CGItem_C__OnRightClick = 0x00680840,
        CGItem_C__PostInit = 0x00683600,
        CGItem_C__Shutdown = 0x00682010,
        CGItem_C_virt00 = 0x0067F1D0,
        CGItem_C_virt04 = 0x00683640,
        CGItem_C_virt08 = 0x0067F010,
        CGItem_C_virt0C = 0x00683630,
        CGItem_C_virt5C = 0x0067F7C0,
        CGLootInfo__LootSlot = 0x0050EEA0,
        CGMerchantInfo__CloseMerchant = 0x0050A430,
        CGObject_C_Destructor = 0x00478D90,
        CGObject_C__AddWorldObject = 0x006BA6D0,
        CGObject_C__CGObject_C = 0x006BCC50,
        CGObject_C__GetFacing = 0x00583990,
        CGObject_C__GetObjectName = 0x006A2150,
        CGObject_C__GetPosition = 0x00478C40,
        CGObject_C__GetScale = 0x00478C90,
        CGObject_C__Initialize = 0x006BCEB0,
        CGObject_C__PostInit = 0x00477C40,
        CGObject_C__PreAnimate = 0x006BAD70,
        CGObject_C__Shutdown = 0x006BBD60,
        CGObject_C_virt04 = 0x006BBC30,
        CGObject_C_virt08 = 0x006BBCC0,
        CGObject_C_virt0C = 0x006BAF80,
        CGObject_C_virt14 = 0x006BA840,
        CGObject_C_virt18 = 0x00684F50,
        CGObject_C_virt20 = 0x006BC0C0,
        CGObject_C_virt2C = 0x00478C60,
        CGObject_C_virt34 = 0x00478C80,
        CGObject_C_virt3C = 0x00478CA0,
        CGObject_C_virt40 = 0x00478CB0,
        CGObject_C_virt48 = 0x006BCB90,
        CGObject_C_virt4C = 0x006BB380,
        CGObject_C_virt50 = 0x006BB400,
        CGObject_C_virt98 = 0x006BA1C0,
        CGObject_C_virt9C = 0x006BA250,
        CGObject_C_virtA0 = 0x006BB2A0,
        CGObject_C_virtA4 = 0x00478D30,
        CGObject_C_virtAC = 0x00684490,
        CGObject_C_virtB0 = 0x006BA400,
        CGObject_C_virtB4 = 0x006BA450,
        CGObject_C_virtB8 = 0x006BA4A0,
        CGObject_C_virtC0 = 0x008879A0,
        CGObject_C_virtD4 = 0x00478D00,
        CGObject_C_virtD8 = 0x00478D20,
        CGObject_C_virtDC = 0x006BB250,
        CGObject_C_virtE4 = 0x006BA2A0,
        CGObject_C_virtE8 = 0x006BA270,
        CGPartyInfo__IsMember = 0x004F7110,
        CGPartyInfo__IsMember_0 = 0x004B2B10,
        CGPetInfo__PetAbandon = 0x0055C2C0,
        CGPetInfo__PetDismiss = 0x0055C0B0,
        CGPetInfo__SetMode = 0x0055BBF0,
        CGPetitionInfo__SetPetition = 0x00557420,
        CGPlayer_C__AcceptGroup__ = 0x0064D580,
        CGPlayer_C__AcceptGuild = 0x0064D880,
        CGPlayer_C__AcceptResurrectRequest = 0x0064BD30,
        CGPlayer_C__AddKnownSpell = 0x00660E60,
        CGPlayer_C__AutoEquipCursorItem = 0x006593C0,
        CGPlayer_C__AutoEquipItem = 0x00659660,
        CGPlayer_C__CGPlayer_C = 0x0065E8A0,
        CGPlayer_C__ClearPendingEquip = 0x00659910,
        CGPlayer_C__ClickToMove = 0x0069F2D0,
        CGPlayer_C__CompleteQuest = 0x0064DDA0,
        CGPlayer_C__DeclineGroup = 0x0064D5F0,
        CGPlayer_C__DeclineGuild = 0x0064D8F0,
        CGPlayer_C__Disable = 0x0065D8E0,
        CGPlayer_C__GetAFKText = 0x00656490,
        CGPlayer_C__GetDNDText = 0x00656500,
        CGPlayer_C__GetGMText = 0x00656540,
        CGPlayer_C__GetSoulstone = 0x006569D0,
        CGPlayer_C__GiveQuestItems = 0x0064DE30,
        CGPlayer_C__HandleRepopRequest = 0x0064C950,
        CGPlayer_C__Initialize = 0x0065D5A0,
        CGPlayer_C__LeaveGroup = 0x0064D660,
        CGPlayer_C__OnAttackIconPressed = 0x006A4170,
        CGPlayer_C__OnBuyFailed = 0x00648450,
        CGPlayer_C__OnBuySucceeded = 0x006485C0,
        CGPlayer_C__OnLootMoneyNotify = 0x00649220,
        CGPlayer_C__OnPetitionDecline = 0x00649510,
        CGPlayer_C__OnPetitionRename = 0x00656FA0,
        CGPlayer_C__OnPetitionShowList = 0x0064F7C0,
        CGPlayer_C__OnPetitionShowSignatures = 0x0064FA80,
        CGPlayer_C__OnQuestGiverChooseReward = 0x0064AC30,
        CGPlayer_C__OnQuestGiverInvalidQuest = 0x00648010,
        CGPlayer_C__OnQuestGiverListQuests = 0x0064A4F0,
        CGPlayer_C__OnQuestGiverQuestComplete = 0x0064B190,
        CGPlayer_C__OnQuestGiverQuestFailed = 0x006482C0,
        CGPlayer_C__OnQuestGiverRequestItems = 0x0064AA80,
        CGPlayer_C__OnQuestGiverSendQuest = 0x0064A6B0,
        CGPlayer_C__OnQuestGiverStatus = 0x0064B230,
        CGPlayer_C__OnSellResponse = 0x0064B5B0,
        CGPlayer_C__OnSignedResults = 0x0064FBF0,
        CGPlayer_C__OnTrainerList = 0x0064B330,
        CGPlayer_C__OnTurnInPetitionResults = 0x00649570,
        CGPlayer_C__OnVendorInventory = 0x006527C0,
        CGPlayer_C__PostInit = 0x00661540,
        CGPlayer_C__PostInit_0 = 0x00661240,
        CGPlayer_C__PreAnimate = 0x0064EE70,
        CGPlayer_C__ReadItemResult = 0x0064ED50,
        CGPlayer_C__ReceiveResurrectRequest = 0x00648760,
        CGPlayer_C__SendTextEmote = 0x00657710,
        CGPlayer_C__SetActiveMirrorHandlers = 0x0065C7A0,
        CGPlayer_C__SetCombatMode = 0x006583F0,
        CGPlayer_C__SetPlayerMirrorHandlers = 0x00658F60,
        CGPlayer_C__ShouldRender = 0x0065A0D0,
        CGPlayer_C__Shutdown = 0x0065D720,
        CGPlayer_C__TalkToTrainer = 0x0064BF20,
        CGPlayer_C__UnsetActiveMirrorHandlers = 0x0065CF30,
        CGPlayer_C___CGPlayer_C = 0x0065ED30,
        CGPlayer_C__destructor = 0x00660C30,
        CGPlayer_C_virt0C = 0x0065BC60,
        CGPlayer_C_virt100 = 0x0064EBB0,
        CGPlayer_C_virt108 = 0x00659D60,
        CGPlayer_C_virt10C = 0x00656360,
        CGPlayer_C_virt110 = 0x00659E60,
        CGPlayer_C_virt118 = 0x00655F40,
        CGPlayer_C_virt124 = 0x0064F6A0,
        CGPlayer_C_virtF4 = 0x006D9930,
        CGPlayer_C_virtF8 = 0x006D9B90,
        CGPlayer_C_virtFC = 0x006D9C90,
        CGQuestInfo__AcceptQuest = 0x005125A0,
        CGQuestInfo__CompleteQuest = 0x00512540,
        CGQuestInfo__ConfirmAcceptQuest = 0x005117E0,
        CGQuestInfo__DeclineQuest = 0x00512690,
        CGQuestInfo__GiveQuestItems = 0x005127D0,
        CGQuestInfo__IsCompletable = 0x00512440,
        CGQuestInfo__QueryQuest = 0x005124B0,
        CGQuestInfo__QuestGiverFinished = 0x00512300,
        CGQuestLog__AbandonSelectedQuest__ = 0x00567F00,
        CGSimpleHealthBar__RemoveMirrorHandlers = 0x009502E0,
        CGSpellBook__CastSpell = 0x004E9BB0,
        CGSpellBook__PickupSpell = 0x004E9A60,
        CGSpellBook__UpdateSpells = 0x004EA850,
        CGTabardCreationFrame__Close = 0x0051B070,
        CGTaxiMap__TakeTaxiNode = 0x00513D80,
        CGTaxiMap__TaxiNodeType = 0x005137E0,
        CGTradeInfo__HandleTradeMessage = 0x0050D1C0,
        CGTradeInfo__SetTradePartner = 0x005133C0,
        CGTradeSkillInfo__GetSubClassIndexFromSkill = 0x00561DA0,
        CGTradeSkillInfo__SetInvTypeFilter = 0x00564200,
        CGTradeSkillInfo__SetSubClassFilter = 0x005644C0,
        CGTutorial__ClearTutorials = 0x004CBCD0,
        CGTutorial__ResetTutorials = 0x004CB7A0,
        CGUIBindings__GetCommand = 0x004DC390,
        CGUIBindings__GetCommandAction = 0x004E0080,
        CGUIBindings__GetCommandKey_0 = 0x004DFED0,
        CGUnit_C__CGUnit_C = 0x006B5690,
        CGUnit_C__DisplayInfoNeedsUpdate = 0x006A25D0,
        CGUnit_C__GetAura = 0x0055D490,
        CGUnit_C__GetGroundNormal = 0x0068E980,
        CGUnit_C__GetObjectName = 0x0065EC30,
        CGUnit_C__GetParryingItem = 0x006CBC60,
        CGUnit_C__GetPosition = 0x0065EC40,
        CGUnit_C__GetWorldMatrix = 0x006983C0,
        CGUnit_C__Initialize = 0x006B9240,
        CGUnit_C__OnJump = 0x00669BC0,
        CGUnit_C__OnMoveStart = 0x00669AE0,
        CGUnit_C__OnMoveStop = 0x00669CA0,
        CGUnit_C__OnPitchStart = 0x00690C80,
        CGUnit_C__OnPitchStop = 0x00667D00,
        CGUnit_C__OnRightClick = 0x006A8E80,
        CGUnit_C__OnSetRunMode = 0x00669EC0,
        CGUnit_C__OnStrafeStart = 0x00669B50,
        CGUnit_C__OnStrafeStop = 0x00669D00,
        CGUnit_C__OnTeleport = 0x00669F10,
        CGUnit_C__OnTurnStart = 0x00690C50,
        CGUnit_C__OnTurnStop = 0x00669E70,
        CGUnit_C__PlayEmoteAnimation = 0x006B1420,
        CGUnit_C__PostInit = 0x006B7D20,
        CGUnit_C__PostShutdown = 0x0069D440,
        CGUnit_C__QueryModelStats = 0x0069E000,
        CGUnit_C__RefreshDataPointers = 0x006A57B0,
        CGUnit_C__SetImpactKitEffect = 0x006BD320,
        CGUnit_C__Shutdown = 0x006B9B60,
        CGUnit_C__UnitReaction = 0x0069D760,
        CGUnit_C__UpdateDisplayInfo = 0x006B7720,
        CGUnit_C__UpdateUnitCollisionBox = 0x00650780,
        CGUnit_C_virt00 = 0x006AE7D0,
        CGUnit_C_virt04 = 0x006ABC60,
        CGUnit_C_virt08 = 0x0069BFB0,
        CGUnit_C_virt0C = 0x006B8310,
        CGUnit_C_virt10 = 0x006ADD30,
        CGUnit_C_virt108 = 0x00697A80,
        CGUnit_C_virt10C = 0x00697B80,
        CGUnit_C_virt110 = 0x00690ED0,
        CGUnit_C_virt118 = 0x00692E40,
        CGUnit_C_virt124 = 0x00693360,
        CGUnit_C_virt38 = 0x006A1C30,
        CGUnit_C_virt3C = 0x00691690,
        CGUnit_C_virt40 = 0x0068FEE0,
        CGUnit_C_virt48 = 0x0069D390,
        CGUnit_C_virt4C = 0x0069D3C0,
        CGUnit_C_virt50 = 0x006A1CF0,
        CGUnit_C_virt54 = 0x006A1F50,
        CGUnit_C_virt58 = 0x006A3700,
        CGUnit_C_virt5C = 0x00690E80,
        CGUnit_C_virt64 = 0x006B45D0,
        CGUnit_C_virt74 = 0x00691A10,
        CGUnit_C_virt78 = 0x006B15F0,
        CGUnit_C_virt84 = 0x00692760,
        CGUnit_C_virt98 = 0x006B2A00,
        CGUnit_C_virt9C = 0x006996A0,
        CGUnit_C_virtA0 = 0x0069AD70,
        CGUnit_C_virtA4 = 0x0069B140,
        CGUnit_C_virtA8 = 0x0068EB60,
        CGUnit_C_virtB0 = 0x0068DAA0,
        CGUnit_C_virtB4 = 0x0065ECF0,
        CGUnit_C_virtB8 = 0x006A1930,
        CGUnit_C_virtCC = 0x006C2600,
        CGUnit_C_virtE4 = 0x00478D80,
        CGUnit_C_virtF4 = 0x006BE0F0,
        CGUnit_C_virtF8 = 0x006BD2B0,
        CGUnit_C_virtFC = 0x006BD1C0,
        CGWorldFrame__GetActiveCamera = 0x00496350,
        CGWorldFrame__Intersect = 0x0073ACC0,
        CGWorldFrame__OnFrameRender = 0x0049B830,
        CGWorldFrame__OnLayerTrackObject = 0x004989D0,
        CGWorldFrame__OnLayerUpdate = 0x0049A740,
        CGWorldFrame__OnWorldUpdate = 0x0049ACE0,
        CGWorldFrame__Render = 0x00499710,
        CGWorldFrame__RenderWorld = 0x0049B740,
        CGWorldFrame___scalar_deleting_destructor_ = 0x0049B710,
        CGWorldFrame___vector_deleting_destructor_ = 0x0049AC50,
        CGWorldMap__ProcessClick = 0x004CF6E0,
        CGWorldMap__SetMap = 0x004CF0E0,
        CGlueMgr__ChangeRealm = 0x0047B720,
        CGlueMgr__CreateCharacter = 0x0047B7A0,
        CGlueMgr__DefaultServerLogin = 0x0047B5C0,
        CGlueMgr__DeleteCharacter = 0x0047B7F0,
        CGlueMgr__EnterWorld = 0x0047C6A0,
        CGlueMgr__Initialize = 0x0047E350,
        CGlueMgr__NetDisconnectHandler = 0x0047D400,
        CGlueMgr__Resume = 0x0047D070,
        CGlueMgr__Shutdown = 0x0047E530,
        CGlueMgr__StatusDialogClick = 0x0047C370,
        CGlueMgr__UpdateCurrentScreen = 0x0047AF30,
        CGxDeviceD3d__DeviceSetFormat = 0x00611B40,
        CGxDeviceD3d__ILoadD3dLib = 0x00610320,
        CGxDeviceOpenGl__DeviceSetFormat = 0x0060E400,
        CGxDevice__AdapterMonitorModes = 0x0060B5C0,
        CGxDevice__BuildSelectionMatrix = 0x006BB0C0,
        CGxDevice__DeviceAdapterID = 0x0060AD10,
        CGxDevice__DeviceAdapterInfer = 0x0060AEB0,
        CGxDevice__DeviceOverride = 0x00605F90,
        CGxDevice__LogOpen = 0x00604620,
        CGxDevice__Pop = 0x00606E40,
        CGxDevice__ProjectTex2D = 0x0079BBF0,
        CGxDevice__Push = 0x00409510,
        CGxDevice__SetCircleRenderStates = 0x006BBDC0,
        CMapChunk__CreateChunkLayerTex = 0x00771580,
        CMapChunk__CreateIndices = 0x0077B810,
        CMapChunk__CreateVerticesLocal = 0x0077C6C0,
        CMapChunk__CreateVerticesLocal__ = 0x0077CCA0,
        CMapChunk__CreateVerticesWorld = 0x0077BC10,
        CMapChunk__CreateVerticesWorld_0 = 0x0077C340,
        CMapChunk__UnpackAlphaBits = 0x00771000,
        CMapChunk__UnpackAlphaShadowBits = 0x007706B0,
        CMapObj__Create = 0x00769170,
        CMap__CreateMapObjDef = 0x00777030,
        CMap__CreateMapObjDef_0 = 0x00777370,
        CMap__Load = 0x00777BB0,
        CMap__LoadTexture = 0x00791500,
        CMap__LoadWdt = 0x00777780,
        CMap__SafeOpen = 0x00775400,
        CMap__SafeRead = 0x00775450,
        CModelComplex__CModelComplex_0 = 0x005199D0,
        CModelComplex__CopyCameras = 0x005196E0,
        CMovement__OnMoveStop = 0x0094CA50,
        CMovement__OnPitchStop = 0x00949160,
        CMovement__OnStrafeStop = 0x00948F10,
        CMovement__OnTurnStop = 0x009495A0,
        CMovement__UpdateStatus = 0x00666A40,
        CNetClient__Process = 0x005B6050,
        CNetClient__ResetHandler = 0x005B6030,
        CRenderBatch__Clear = 0x0042B7D0,
        CRenderBatch__QueueCallback = 0x0042B740,
        CSRWLock__Enter = 0x004236E0,
        CSRWLock__Leave = 0x00427850,
        CSimpleFontString__UpdateString = 0x0042CA10,
        CSimpleFrame__LoadXML = 0x00438D50,
        CSimpleFrame__OnFrameRender = 0x00436260,
        CTMFace = 0x006A3580,
        CVGxApiCallback = 0x006DF5B0,
        CVGxColorBitsCallback = 0x006DF440,
        CVGxDepthBitsCallback = 0x006DF4D0,
        CVGxRefreshCallback = 0x006E0780,
        CVGxResolutionCallback = 0x006E0420,
        CVar__Destroy = 0x006DE420,
        CVar__Initialize = 0x006DF350,
        CVar__Lookup = 0x006DE470,
        CVar__Register = 0x006DEFD0,
        CVar__RegisterAll = 0x004BDB50,
        CVar__Set = 0x006DDD50,
        CWorld__ObjectCreate = 0x0073EDC0,
        CWorld__UnloadMap = 0x00497900,
        CameraCreate = 0x00463CE0,
        CameraDuplicate = 0x00463D20,
        CaptureScreen = 0x0044DE30,
        ChannelCommand = 0x0049E820,
        ChannelPlayerCommand = 0x0064C500,
        CharCreateRegisterScriptFunctions = 0x00482170,
        Checksum = 0x006346A0,
        ClickTerrain = 0x004C74E0,
        ClickToMove__GetInteractDistanceOfAction = 0x006939F0,
        ClientConnection__ClientConnection = 0x0041CD40,
        ClientConnection__HandleAuthResponse = 0x0041C160,
        ClientConnection__HandleCharEnum = 0x0041C870,
        ClientConnection__HandleCharacterCreate = 0x0041C270,
        ClientConnection__HandleCharacterDelete = 0x0041C2A0,
        ClientConnection__HandleCharacterLoginFailed = 0x0041BFB0,
        ClientConnection___ClientConnection = 0x0041CF70,
        ClientConnection___scalar_deleting_destructor_ = 0x0062BDE0,
        ClientDb_GetLocalizedRow = 0x00472B00,
        ClientDb_StringLookup = 0x005B7EF0,
        ClientDestroyGame = 0x004064A0,
        ClientIdle = 0x00402E00,
        ClientInitializeGame = 0x00405520,
        ClientInitializeGameTime = 0x0079A3F0,
        ClientInitializeGame_0 = 0x00662130,
        ClientRegisterConsoleCommands = 0x00403A90,
        ClientServices_CharacterCreate = 0x0062C340,
        ClientServices_CharacterForceLogout = 0x0062CEB0,
        ClientServices_CharacterLogout = 0x0062C650,
        ClientServices_ClearMessageHandler = 0x0062B960,
        ClientServices_Connect = 0x0062C0E0,
        ClientServices_Disconnect = 0x0062B780,
        ClientServices_GetCurrent = 0x0062B7B0,
        ClientServices_GetErrorToken = 0x0062BCA0,
        ClientServices_Initialize = 0x0062CF40,
        ClientServices_PollStatus = 0x0062BE60,
        ClientServices_Send = 0x0062B920,
        ClientServices_SetMessageHandler = 0x0062B940,
        ClientServices_ValidDisconnect = 0x0062BCC0,
        ClntObjMgrCreate = 0x0047A480,
        ClntObjMgrDestroy = 0x004794D0,
        ClntObjMgrGetActivePlayer = 0x00476580,
        ClntObjMgrInitialize = 0x0047A410,
        ClntObjMgrInitializeShared = 0x00477860,
        ClntObjMgrObjectPtr = 0x00477B50,
        ClntObjMgrUnsetObjMirrorHandler = 0x004788E0,
        CloseLoot = 0x004C2FA0,
        CmdLineGetBool = 0x00421DE0,
        CmdLineProcess = 0x00421E10,
        CompletionRoutine = 0x0085CCB0,
        ConsoleCommandExecute = 0x006DC010,
        ConsoleCommandInitialize = 0x006DD840,
        ConsoleCommandRegister = 0x006DD780,
        ConsoleCommandUnregister = 0x006DD060,
        ConsoleCommandWriteHelp = 0x006DD160,
        ConsoleDeviceInitialize = 0x006E0DA0,
        ConsolePrintf = 0x006DBB20,
        ConsoleScreenDestroy = 0x006DCA10,
        ConsoleScreenInitialize = 0x006DC8B0,
        ConsoleWrite = 0x006DB9E0,
        ConsoleWriteA = 0x006DBAD0,
        CopyAndExpandDescriptors = 0x004960B0,
        CopyMatrixByGuid = 0x006C22D0,
        CreateCombatLogEntry = 0x006C46E0,
        CreateObject = 0x00479980,
        CreateToolhelp32Snapshot = 0x008597CC,
        DBCache_CGPetition_int_HASHKEY_INT___DBCache_CGPetition_int_HASHKEY_INT_ = 0x005F7830,
        DBCache_CreatureStats_C_int_HASHKEY_INT___DBCache_CreatureStats_C_int_HASHKEY_INT_ = 0x005F6E90,
        DBCache_GameObjectStats_C_int_HASHKEY_INT___DBCache_GameObjectStats_C_int_HASHKEY_INT_ = 0x005F6D60,
        DBCache_GuildStats_C_int_HASHKEY_INT___DBCache_GuildStats_C_int_HASHKEY_INT_ = 0x005F7350,
        DBCache_ItemStats_C_int_HASHKEY_INT___DBCache_ItemStats_C_int_HASHKEY_INT_ = 0x005F6FC0,
        DBCache_NPCText_int_HASHKEY_INT___DBCache_NPCText_int_HASHKEY_INT_ = 0x005F70F0,
        DBCache_NameCache_unsigned___int64_CHashKeyGUID___DBCache_NameCache_unsigned___int64_CHashKeyGUID_ = 0x005F7220,
        DBCache_PageTextCache_C_int_HASHKEY_INT___DBCache_PageTextCache_C_int_HASHKEY_INT_ = 0x005F75B0,
        DBCache_PetNameCache_int_HASHKEY_INT___DBCache_PetNameCache_int_HASHKEY_INT_ = 0x005F76E0,
        DBCache_QuestCache_int_HASHKEY_INT___DBCache_QuestCache_int_HASHKEY_INT_ = 0x005F7480,
        DBCache__CancelCallback = 0x005FAE60,
        DBCache__CancelCallback_0 = 0x005FB300,
        DBItemCache_GetInfoBlockByID = 0x005FD9A0,
        DNameNode__DNameNode = 0x00473690,
        DbArenaTeamCache_GetInfoBlockById = 0x006011C0,
        DbCreatureCache_GetInfoBlockById = 0x005FCCB0,
        DbDanceCache_GetInfoBlockById = 0x00601710,
        DbGameObjectCache_GetInfoBlockById = 0x005FC620,
        DbGuildCache_GetInfoBlockById = 0x005FE8A0,
        DbItemNameCache_GetInfoBlockById = 0x005FD350,
        DbItemTextCache_GetInfoBlockById = 0x00600430,
        DbNameCache_GetInfoBlockById = 0x005FE6E0,
        DbNpcCache_GetInfoBlockById = 0x005FE040,
        DbPageTextCache_GetInfoBlockById = 0x005FF350,
        DbPetNameCache_GetInfoBlockById = 0x005FF9A0,
        DbPetitionCache_GetInfoBlockById = 0x005FFEE0,
        DbQuestCache_GetInfoBlockById = 0x005FEE00,
        DbWoWCache_GetInfoBlockById = 0x00600AD0,
        DbWoWCache_Shutdown_WARDEN_UNLOAD = 0x00792490,
        DetectHardware = 0x006E1C20,
        DirectInput8Create = 0x008594EC,
        EnableCallback = 0x00401A90,
        EnumVisibleObjects = 0x004778D0,
        ErrorDisplayFilterCallback = 0x00401A10,
        EventIsKeyDown = 0x00423830,
        EventRegister = 0x00423D70,
        EventRegisterEx = 0x004239C0,
        EventSetMouseMode = 0x00423C90,
        EventSetTimer_1 = 0x00423B30,
        EventUnregister = 0x00423D90,
        ExceptionFilterWin32 = 0x006E7650,
        FrameScript_DisplayError = 0x008049F0,
        FrameScript_Execute = 0x007CF6B0,
        FrameScript_GetText = 0x007D01E0,
        FrameScript_GetTop = 0x00803340,
        FrameScript_GetVariable = 0x007CE4E0,
        FrameScript_RegisterFunction = 0x007CE460,
        FrameScript_SignalEvent = 0x007D1150,
        FrameScript_UnregisterFunction = 0x007CE4A0,
        FriendList__AddIgnore = 0x006318B0,
        FriendList__AddOrDelIgnore = 0x006308F0,
        FriendList__DelIgnore = 0x00631AE0,
        FriendList__Destroy = 0x0062FCD0,
        FriendList__FriendList = 0x0062DE40,
        FriendList__Initialize = 0x00633390,
        FriendList__RemoveFriend = 0x006319A0,
        FriendList__RemoveFriend_0 = 0x0062FFE0,
        FriendList__SendWho = 0x00630150,
        GenPacket = 0x00401070,
        GetACP = 0x0081ED70,
        GetBagAtIndex = 0x0055E980,
        GetBagItem = 0x006CB110,
        GetClickToMoveState = 0x0068E190,
        GetClickToMoveStruct = 0x0068E220,
        GetCurrentProcessId = 0x006E7A10,
        GetCurrentThreadId = 0x006E7A00,
        GetExceptionNameWin32 = 0x006E64C0,
        GetFileVersionInfoA = 0x0084E56E,
        GetFileVersionInfoSizeA = 0x0084E574,
        GetGUIDByKeyword = 0x0058FC90,
        GetItemIDByName = 0x00682470,
        GetLuaState = 0x007CE280,
        GetObjectPtr = 0x00477950,
        GetRow_ClientDB = 0x00484250,
        GetSpellFailedEventString = 0x007B42D0,
        GetSpellIdByName = 0x004ED9E0,
        GetSpellManaCostByID = 0x007B8670,
        GetUnitFromName = 0x005911D0,
        GetUnitType = 0x00697940,
        GuildCharterTurnInCallback = 0x00657090,
        GxAdapterMonitorModes = 0x00602250,
        GxuFontCreateFont = 0x006387E0,
        HeapUsage = 0x004755D0,
        HidD_FreePreparsedData = 0x00987046,
        HidD_GetAttributes = 0x0098705E,
        HidD_GetHidGuid = 0x00987070,
        HidD_GetPreparsedData = 0x00987064,
        HidD_GetProductString = 0x00987052,
        HidD_GetSerialNumberString = 0x0098704C,
        HidD_SetFeature = 0x0098706A,
        HidP_GetCaps = 0x00987058,
        ILayerPaint = 0x0044E420,
        IStockInitialize = 0x0044E680,
        ImmAssociateContext = 0x0084E592,
        ImmAssociateContextEx = 0x0084E59E,
        ImmGetCandidateListA = 0x0084E5AA,
        ImmGetCompositionStringA = 0x0084E58C,
        ImmGetContext = 0x0084E586,
        ImmGetConversionStatus = 0x0084E580,
        ImmNotifyIME = 0x0084E5A4,
        ImmReleaseContext = 0x0084E57A,
        ImmSetConversionStatus = 0x0084E598,
        InitObject = 0x00476DE0,
        InitializeGlobal = 0x00406780,
        InputControlDestroy = 0x00582850,
        InputControlRegisterScriptFunctions = 0x0057E990,
        InputEvent = 0x0081E3A0,
        InstallGameConsoleCommands = 0x00407870,
        IsValidSpell = 0x004EDE30,
        LoadMovePacket = 0x006975C0,
        LoadNewWorld = 0x00403660,
        LoadScriptFunctions_ = 0x004B2280,
        LoadWardenModule = 0x008279D0,
        LogObjectInfo = 0x00402F30,
        MirrorInitialize = 0x00496110,
        MovementDestroy = 0x00669A20,
        MovementInit = 0x00401520,
        NDCToDDCHeight = 0x00422C30,
        NETEVENTQUEUE__AddEvent = 0x005B6CD0,
        NTempest__CMath__exp2_ = 0x00467C80,
        NTempest__CMath__log2_ = 0x00467BD0,
        NetClient__DelayedDelete = 0x005B6400,
        NetClient__Destroy = 0x005B6310,
        NetClient__Disconnect = 0x005B5F40,
        NetClient__GetNetStats = 0x005B6200,
        NetClient__HandleData = 0x005B65F0,
        NetClient__HandleDisconnect = 0x005B66D0,
        NetClient__Initialize = 0x005B5E60,
        NetClient__NetClient = 0x005B68E0,
        NetClient__PopObjMgr = 0x004764F0,
        NetClient__PushObjMgr = 0x004764B0,
        NetClient__SetMessageHandler = 0x005B6010,
        NetClient__WCDisconnected = 0x005B6880,
        NetClient__WCMessageReady = 0x005B6570,
        NetClient___NetClient = 0x005B69C0,
        NetClient___NetClient_0 = 0x005B6A40,
        NetClient___scalar_deleting_destructor_ = 0x005B6AA0,
        ObjDelete = 0x00479D40,
        ObjectTracking = 0x006566C0,
        ObjectUpdateHandler = 0x00479E40,
        ObjectUpdateHandler_0 = 0x00479B20,
        OnChar = 0x006DB100,
        OnIdle_0 = 0x006DC130,
        OnKeyDown = 0x006DC260,
        OnKeyDownRepeat = 0x006DB300,
        OnKeyUp = 0x006DA390,
        OnMouseDown = 0x006DB180,
        OnMouseMove = 0x006DA180,
        OnPaint = 0x0044DFE0,
        OnUnitMoveEvent = 0x00690D60,
        OsTlsGetValue = 0x00822D50,
        OutputTime = 0x006EACF0,
        PartialUpdateFromFullUpdate = 0x00479C00,
        PerformanceCounter = 0x00820420,
        PlayerClientInitialize = 0x00661670,
        PlayerNameInitialize = 0x0079E080,
        PlayerNameShutdown = 0x0079CE70,
        PossessNPC = 0x006ADFF0,
        PostInitObject = 0x00479130,
        PrintFilterMask = 0x00401840,
        PropGet = 0x00423260,
        PtFuncCompare = 0x009505C0,
        RandomRollNameQueryCallback = 0x00646310,
        RegisterGxCVars = 0x006E0830,
        RegisterHandlers_1 = 0x006DC760,
        RegisterInterfaceEvents = 0x007D1A90,
        ReleasePacket = 0x00403370,
        RepairItem = 0x0056E990,
        RtlUnwind = 0x0041BDB8,
        SCmdGetBool = 0x006E9810,
        SCmdGetNum = 0x006E9400,
        SCmdProcess = 0x006E9830,
        SCmdProcessCommandLine = 0x006E9930,
        SCmdRegisterArgList = 0x006E9530,
        SCritSect__Enter = 0x006EA0D0,
        SCritSect__Leave = 0x006EA0E0,
        SCritSect__SCritSect = 0x006EA0B0,
        SErrCatchUnhandledExceptions = 0x006E79F0,
        SErrDestroy = 0x006E6C70,
        SErrInitialize = 0x006E6020,
        SErrRegisterHandler = 0x006E6B60,
        SErrSetLogCallback = 0x006E68E0,
        SErrSetLogTitleString = 0x006E6870,
        SEvent__SEvent = 0x006EA390,
        SLogCreate = 0x006EB230,
        SLogDestroy = 0x006EB330,
        SMemAlloc = 0x006E4720,
        SMemFree = 0x006E4780,
        SMutex__Create = 0x006EA250,
        SMutex__Create_0 = 0x006EA400,
        SRWLock__IAllocEvent = 0x006E9DF0,
        SRWLock__IFreeEvent = 0x006E9EA0,
        SRegLoadValue = 0x006E8380,
        SRegSaveValue = 0x006E84E0,
        SServerInitialize = 0x006F43F0,
        SStrCmpI = 0x006E4960,
        SStrCopy = 0x006E4EF0,
        SStrInitialize = 0x006E5B80,
        SStrLen = 0x006E4F70,
        SStrPrintf = 0x006E51B0,
        SStrToInt = 0x006E5210,
        SStrToUnsigned = 0x006E5280,
        SaveHardware = 0x006E1810,
        ScrnInitialize = 0x0044E5A0,
        ScrnLayerCreate = 0x0044E430,
        SellItem = 0x0064CD30,
        SendErrorLog = 0x00402FC0,
        SendPacket = 0x005B64E0,
        SetFacing = 0x00949CC0,
        SetupDiDestroyDeviceInfoList = 0x00987034,
        SetupDiEnumDeviceInfo = 0x0098703A,
        SetupDiEnumDeviceInterfaces = 0x0098702E,
        SetupDiGetClassDevsA = 0x00987040,
        SetupDiGetDeviceInterfaceDetailA = 0x00987028,
        SetupDiGetDeviceRegistryPropertyA = 0x00987022,
        SkillRankChangeHandler = 0x006532A0,
        SkySunGlare = 0x007A4590,
        SmartScreenRectClearAllGrids = 0x0059A2B0,
        SndInterfaceSetGlueMusic = 0x009461F0,
        SoulStoneCompare = 0x006568F0,
        SpellTableInitialize = 0x007C8720,
        Spell_C_CastSpell = 0x007C4640,
        Spell_C_HandleSpriteRay = 0x007BA750,
        Spell_C_TargetTradeItem = 0x004C0D10,
        Spell_C__GetItemCooldown = 0x007BFD70,
        Spell_C__GetSpellCooldown = 0x007BE690,
        Spell_C__GetSpellCooldown_Proxy = 0x007BFD40,
        Spell_C__GetSpellRange = 0x007B9C80,
        StartAddress = 0x006E7B80,
        StormRtlDestroy = 0x006E4650,
        SysMsgAdd = 0x0045A7E0,
        TSHashTable_BATCHEDRENDERFONTDESC_HASHKEY_PTR___Destroy = 0x00639A30,
        TSHashTable_BATCHEDRENDERFONTDESC_HASHKEY_PTR___InternalDelete = 0x006398E0,
        TSHashTable_BATCHEDRENDERFONTDESC_HASHKEY_PTR___InternalNew = 0x00639910,
        TSHashTable_BATCHEDRENDERFONTDESC_HASHKEY_PTR____scalar_deleting_destructor_ = 0x00639A90,
        TSHashTable_DBCache_CGPetition_int_HASHKEY_INT___DBCACHEHASH_HASHKEY_INT___Destroy = 0x005F57C0,
        TSHashTable_DBCache_CGPetition_int_HASHKEY_INT___DBCACHEHASH_HASHKEY_INT___InternalDelete = 0x005FA120,
        TSHashTable_DBCache_CGPetition_int_HASHKEY_INT___DBCACHEHASH_HASHKEY_INT___InternalNew = 0x005F2450,
        TSHashTable_DBCache_CGPetition_int_HASHKEY_INT___DBCACHEHASH_HASHKEY_INT____scalar_deleting_destructor_ = 0x005F6820,
        TSHashTable_DBCache_GameObjectStats_C_int_HASHKEY_INT___DBCACHEHASH_HASHKEY_INT___Destroy = 0x005F3FA0,
        TSHashTable_DBCache_GameObjectStats_C_int_HASHKEY_INT___DBCACHEHASH_HASHKEY_INT___InternalDelete = 0x005F9F70,
        TSHashTable_DBCache_GameObjectStats_C_int_HASHKEY_INT___DBCACHEHASH_HASHKEY_INT___InternalNew = 0x005F1400,
        TSHashTable_DBCache_GameObjectStats_C_int_HASHKEY_INT___DBCACHEHASH_HASHKEY_INT____scalar_deleting_destructor_ = 0x005F64C0,
        TSHashTable_DBCache_ItemStats_C_int_HASHKEY_INT___DBCACHEHASH_HASHKEY_INT___Destroy = 0x005F4500,
        TSHashTable_DBCache_ItemStats_C_int_HASHKEY_INT___DBCACHEHASH_HASHKEY_INT___InternalDelete = 0x005F9FD0,
        TSHashTable_DBCache_ItemStats_C_int_HASHKEY_INT___DBCACHEHASH_HASHKEY_INT___InternalNew = 0x005F17A0,
        TSHashTable_DBCache_ItemStats_C_int_HASHKEY_INT___DBCACHEHASH_HASHKEY_INT____scalar_deleting_destructor_ = 0x005F6580,
        TSHashTable_ITEMCOOLDOWNHASHNODE_HASHKEY_NONE___Destroy = 0x007C1210,
        TSHashTable_ITEMCOOLDOWNHASHNODE_HASHKEY_NONE___InternalDelete = 0x007C1140,
        TSHashTable_ITEMCOOLDOWNHASHNODE_HASHKEY_NONE___InternalNew = 0x007C10E0,
        TSHashTable_ITEMCOOLDOWNHASHNODE_HASHKEY_NONE____scalar_deleting_destructor_ = 0x007C1460,
        TextBlockCreate = 0x00461AE0,
        TextBlockGenerateFont = 0x00462A00,
        Thread32First = 0x008597C6,
        Thread32Next = 0x008597C0,
        TimerFunc = 0x0070DAF0,
        TlsAlloc = 0x00822D10,
        TraceLine = 0x0075C740,
        Trade_C_AddMoney = 0x0067C930,
        Trade_C_BeginTrade = 0x0067C510,
        Trade_C_CancelTrade = 0x0067C750,
        Trade_C_Destroy = 0x0067C3E0,
        Trade_C_Initialize = 0x0067CFE0,
        UninstallGameConsoleCommands = 0x00406EF0,
        UnitTracking = 0x00656630,
        UnloadScriptFunctions_0 = 0x004B2410,
        UnregisterHandlers_1 = 0x006DC840,
        UpdateGameTime = 0x006E3930,
        UpdateGameTime2 = 0x006E3840,
        UpdateMountModel = 0x006B5D00,
        UpdateTime = 0x00799F50,
        UseItem = 0x00681260,
        ValidateFormatMonitor = 0x006E0010,
        ValidateNameDestroy = 0x007999D0,
        VerQueryValueA = 0x0084E568,
        WSAAsyncGetHostByName = 0x0098701C,
        WSACancelAsyncRequest = 0x00987016,
        WSACleanup = 0x00421150,
        WSACleanup_0 = 0x00858E50,
        WSAGetLastError = 0x00858E08,
        WSAStartup = 0x00858E4A,
        WorldTextInitialize = 0x0079F760,
        WowClientDestroy = 0x004023D0,
        WowLogHeader = 0x004049F0,
        WowTime__WowGetTimeString_0 = 0x006E2F30,
        accept = 0x00858E26,
        acmFormatSuggest = 0x00986FFE,
        acmStreamConvert = 0x0098700A,
        acmStreamOpen = 0x00986FF8,
        acmStreamPrepareHeader = 0x00987010,
        acmStreamSize = 0x00986FF2,
        acmStreamUnprepareHeader = 0x00987004,
        bind = 0x00858E3E,
        closesocket = 0x00858E02,
        connect = 0x00858E2C,
        fnInternetCallback = 0x00825300,
        fptc = 0x008D1270,
        htonl = 0x00858E44,
        htons = 0x00858DFC,
        inet_addr = 0x00858E32,
        ioctlsocket = 0x00858E20,
        j_CGTabardCreationFrame__Close = 0x0051B0F0,
        j_CGUnit_C_virtCC = 0x00836C50,
        j_CGxDevice__LogOpen = 0x00602840,
        j_ClntObjMgrGetActivePlayer = 0x004011E0,
        j_ILayerPaint = 0x0044E580,
        j_PerformanceCounter = 0x0085EC10,
        j__atol = 0x0040B4BB,
        j_nullsub_6 = 0x00421E50,
        listen = 0x00858E38,
        ntohs = 0x00858DF6,
        pCallback = 0x00497320,
        recv = 0x00858E0E,
        select = 0x00858E56,
        send = 0x00858E14,
        FrameScript_ToString = 0x00803850,
        ClientDb_RegisterBase = 0x5B6E50,
    }
}