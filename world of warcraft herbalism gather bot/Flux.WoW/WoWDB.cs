//#define DUMP_DBC_ENUM

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

//using Flux.Utilities;
using Flux.WoW.Native;
using Flux.WoW.Patchables;

namespace Flux.WoW
{
    public class WoWDB
    {
        private static readonly Dictionary<ClientDb, DbTable> Tables = new Dictionary<ClientDb, DbTable>();

        public WoWDB()
        {
#if DUMP_DBC_ENUM
            string enumDump = "public enum ClientDb\n{";
            int dbCount = 0;
#endif
            for (var tableBase = (int) GlobalOffsets.ClientDb_RegisterBase;
                 Reader.Read<byte>(new IntPtr(tableBase)) != 0xC3;
                 tableBase += 0x11)
            {
                var index = Reader.Read<uint>(new IntPtr(tableBase + 1));
                var tablePtr = new IntPtr(Reader.Read<int>(new IntPtr(tableBase + 0xB)) + 0x18);
                Tables.Add((ClientDb) index, new DbTable(tablePtr));

#if DUMP_DBC_ENUM
                uint pDb = (Marshal.ReadIntPtr((IntPtr)(tableBase + 0xB)).ToUInt32());
                var pRegisterFunc = Reader.Read<uint>((uint) (tableBase + 0xB), 0, 4);
                var pGetFunc = Reader.Read<IntPtr>(new IntPtr(pRegisterFunc + 0x10));
                pGetFunc = new IntPtr(pGetFunc.ToUInt32() + pRegisterFunc + 0x10 + 4);
                string name = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(new IntPtr(pGetFunc.ToUInt32() + 0x1)));
                Logging.WriteDebug(((ClientDb)index).ToString());
                name = name.Replace("\\", "").Replace("DBFilesClient", "").Replace(".dbc", "");
                enumDump += string.Format("\t{0} = 0x{1}, // 0x{2}\n", name, index.ToString("X8"), pDb.ToString("X8"));
                dbCount++;
#endif
            }
#if DUMP_DBC_ENUM
            enumDump += "}";
            Logging.Write(enumDump.Replace("{", "{{").Replace("}", "}}"));
            Logging.WriteDebug("Found " + dbCount + " DBCs");
            foreach (KeyValuePair<ClientDb, DbTable> pair in Tables)
            {
                Logging.LogMessage(pair.Key + " -> " + pair.Value.Address.ToString("X8"));
            }
#endif
        }

        public DbTable this[ClientDb db] { get { return Tables[db]; } }

        #region Nested type: DbTable

        public class DbTable
        {
            internal readonly IntPtr Address;
            private ClientDb_GetLocalizedRow _getLocalizedRow;
            private ClientDb_GetRow _getRow;

            public DbTable(IntPtr address)
            {
                Address = address;
                var h = (DbHeader) Marshal.PtrToStructure(Address, typeof (DbHeader));
                MaxIndex = h.MaxIndex;
                MinIndex = h.MinIndex;
            }

            public uint MaxIndex { get; private set; }
            public uint MinIndex { get; private set; }

            public Row GetRow(int index)
            {
                return GetRowFromDelegate(index);
            }

            public Row GetLocalizedRow(int index)
            {
                if (_getLocalizedRow == null)
                {
                    _getLocalizedRow =
                        Utilities.RegisterDelegate<ClientDb_GetLocalizedRow>(
                            (uint) GlobalOffsets.ClientDb_GetLocalizedRow);
                }
                IntPtr rowPtr = Marshal.AllocHGlobal(4 * 4 * 256);
                int tmp = _getLocalizedRow(new IntPtr(Address.ToInt32() - 0x18), index, rowPtr);
                if (tmp != 0)
                {
                    return new Row(rowPtr, true);
                }
                Marshal.FreeHGlobal(rowPtr);
                return null;
            }

            private Row GetRowFromDelegate(int index)
            {
                if (_getRow == null)
                {
                    _getRow = Utilities.RegisterDelegate<ClientDb_GetRow>((uint) GlobalOffsets.GetRow_ClientDB);
                }
                var ret = new IntPtr(_getRow(new IntPtr(Address.ToInt32()), index));
                return ret == IntPtr.Zero ? null : new Row(ret, false);
            }

            #region Nested type: ClientDb_GetLocalizedRow

            [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
            private delegate int ClientDb_GetLocalizedRow(IntPtr instance, int index, IntPtr rowPtr);

            #endregion

            #region Nested type: ClientDb_GetRow

            [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
            private delegate int ClientDb_GetRow(IntPtr instance, int idx);

            #endregion

            #region Nested type: DbHeader

            [StructLayout(LayoutKind.Sequential)]
            private struct DbHeader
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
                public uint[] Junk;

                public uint MaxIndex;
                public uint MinIndex;
            }

            #endregion

            #region Nested type: Row

            public class Row : IDisposable
            {
                private readonly bool _managedMemory;
                private IntPtr _rowPtr;

                private Row(IntPtr rowPtr)
                {
                    _rowPtr = rowPtr;
                }

                internal Row(IntPtr rowPtr, bool isManagedMem) : this(rowPtr)
                {
                    _managedMemory = isManagedMem;
                }

                #region IDisposable Members

                public void Dispose()
                {
                    if (_managedMemory)
                    {
                        Marshal.FreeHGlobal(_rowPtr);
                    }

                    _rowPtr = IntPtr.Zero;
                    GC.SuppressFinalize(this);
                }

                #endregion

                public T GetField<T>(uint index)
                {
                    try
                    {
                        if (typeof (T) == typeof (string))
                        {
                            // Sometimes.... generics fucking suck
                            object s = Marshal.PtrToStringAnsi(Reader.Read<IntPtr>(_rowPtr.ToUInt32() + (index * 4)));
                            return (T) s;
                        }

                        return Reader.Read<T>(_rowPtr.ToUInt32() + (index * 4));
                    }
                    catch
                    {
                        return default(T);
                    }
                }

                public void SetField(uint index, int value)
                {
                    byte[] bs = BitConverter.GetBytes(value);
                    Win32.WriteBytes((IntPtr) (_rowPtr.ToUInt32() + (index * 4)), bs);
                }
            }

            #endregion
        }

        #endregion
    }

// ReSharper disable InconsistentNaming
    public enum ClientDb
    {
        Achievement = 0x000000EB, // 0x0104A988
        Achievement_Criteria = 0x000000EC, // 0x0104A9AC
        Achievement_Category = 0x000000ED, // 0x0104A9D0
        AnimationData = 0x000000EE, // 0x0104A9F4
        AreaGroup = 0x000000EF, // 0x0104AA18
        AreaPOI = 0x000000F0, // 0x0104AA3C
        AreaTable = 0x000000F1, // 0x0104AA60
        AreaTrigger = 0x000000F2, // 0x0104AA84
        AttackAnimKits = 0x000000F3, // 0x0104AAA8
        AttackAnimTypes = 0x000000F4, // 0x0104AACC
        AuctionHouse = 0x000000F5, // 0x0104AAF0
        BankBagSlotPrices = 0x000000F6, // 0x0104AB14
        BannedAddOns = 0x000000F7, // 0x0104AB38
        BarberShopStyle = 0x000000F8, // 0x0104AB5C
        BattlemasterList = 0x000000F9, // 0x0104AB80
        CameraShakes = 0x000000FA, // 0x0104ABA4
        Cfg_Categories = 0x000000FB, // 0x0104ABC8
        Cfg_Configs = 0x000000FC, // 0x0104ABEC
        CharBaseInfo = 0x000000FD, // 0x0104AC10
        CharHairGeosets = 0x000000FE, // 0x0104AC34
        CharSections = 0x000000FF, // 0x0104AC58
        CharStartOutfit = 0x00000100, // 0x0104AC7C
        CharTitles = 0x00000101, // 0x0104ACA0
        CharacterFacialHairStyles = 0x00000102, // 0x0104ACC4
        ChatChannels = 0x00000103, // 0x0104ACE8
        ChatProfanity = 0x00000104, // 0x0104AD0C
        ChrClasses = 0x00000105, // 0x0104AD30
        ChrRaces = 0x00000106, // 0x0104AD54
        CinematicCamera = 0x00000107, // 0x0104AD78
        CinematicSequences = 0x00000108, // 0x0104AD9C
        CreatureDisplayInfo = 0x00000109, // 0x0104ADE4
        CreatureDisplayInfoExtra = 0x0000010A, // 0x0104ADC0
        CreatureFamily = 0x0000010B, // 0x0104AE08
        CreatureModelData = 0x0000010C, // 0x0104AE2C
        CreatureMovementInfo = 0x0000010D, // 0x0104AE50
        CreatureSoundData = 0x0000010E, // 0x0104AE74
        CreatureSpellData = 0x0000010F, // 0x0104AE98
        CreatureType = 0x00000110, // 0x0104AEBC
        CurrencyTypes = 0x00000111, // 0x0104AEE0
        CurrencyCategory = 0x00000112, // 0x0104AF04
        DanceMoves = 0x00000113, // 0x0104AF28
        DeathThudLookups = 0x00000114, // 0x0104AF4C
        DestructibleModelData = 0x00000115, // 0x0104AFB8
        DungeonMap = 0x00000116, // 0x0104AFDC
        DungeonMapChunk = 0x00000117, // 0x0104B000
        DurabilityCosts = 0x00000118, // 0x0104B024
        DurabilityQuality = 0x00000119, // 0x0104B048
        Emotes = 0x0000011A, // 0x0104B06C
        EmotesText = 0x0000011B, // 0x0104B0D8
        EmotesTextData = 0x0000011C, // 0x0104B090
        EmotesTextSound = 0x0000011D, // 0x0104B0B4
        EnvironmentalDamage = 0x0000011E, // 0x0104B0FC
        Exhaustion = 0x0000011F, // 0x0104B120
        Faction = 0x00000120, // 0x0104B168
        FactionGroup = 0x00000121, // 0x0104B144
        FactionTemplate = 0x00000122, // 0x0104B18C
        FileData = 0x00000123, // 0x0104B1B0
        FootprintTextures = 0x00000124, // 0x0104B1D4
        FootstepTerrainLookup = 0x00000125, // 0x0104B1F8
        GameObjectArtKit = 0x00000126, // 0x0104B21C
        GameObjectDisplayInfo = 0x00000127, // 0x0104B240
        GameTables = 0x00000128, // 0x0104B264
        GameTips = 0x00000129, // 0x0104B288
        GemProperties = 0x0000012A, // 0x0104B2AC
        GlyphProperties = 0x0000012B, // 0x0104B2D0
        GlyphSlot = 0x0000012C, // 0x0104B2F4
        GMSurveyAnswers = 0x0000012D, // 0x0104B318
        GMSurveyCurrentSurvey = 0x0000012E, // 0x0104B33C
        GMSurveyQuestions = 0x0000012F, // 0x0104B360
        GMSurveySurveys = 0x00000130, // 0x0104B384
        GMTicketCategory = 0x00000131, // 0x0104B3A8
        GroundEffectDoodad = 0x00000132, // 0x0104B3CC
        GroundEffectTexture = 0x00000133, // 0x0104B3F0
        gtBarberShopCostBase = 0x00000134, // 0x0104B414
        gtCombatRatings = 0x00000135, // 0x0104B438
        gtChanceToMeleeCrit = 0x00000136, // 0x0104B45C
        gtChanceToMeleeCritBase = 0x00000137, // 0x0104B480
        gtChanceToSpellCrit = 0x00000138, // 0x0104B4A4
        gtChanceToSpellCritBase = 0x00000139, // 0x0104B4C8
        gtNPCManaCostScaler = 0x0000013A, // 0x0104B4EC
        gtOCTClassCombatRatingScalar = 0x0000013B, // 0x0104B510
        gtOCTRegenHP = 0x0000013C, // 0x0104B534
        gtOCTRegenMP = 0x0000013D, // 0x0104B558
        gtRegenHPPerSpt = 0x0000013E, // 0x0104B57C
        gtRegenMPPerSpt = 0x0000013F, // 0x0104B5A0
        HelmetGeosetVisData = 0x00000140, // 0x0104B5C4
        HolidayDescriptions = 0x00000141, // 0x0104B5E8
        HolidayNames = 0x00000142, // 0x0104B60C
        Holidays = 0x00000143, // 0x0104B630
        Item = 0x00000144, // 0x0104B654
        ItemBagFamily = 0x00000145, // 0x0104B678
        ItemClass = 0x00000146, // 0x0104B69C
        ItemCondExtCosts = 0x00000147, // 0x0104B6C0
        ItemDisplayInfo = 0x00000148, // 0x0104B6E4
        ItemExtendedCost = 0x00000149, // 0x0104B708
        ItemGroupSounds = 0x0000014A, // 0x0104B72C
        ItemLimitCategory = 0x0000014B, // 0x0104B750
        ItemPetFood = 0x0000014C, // 0x0104B774
        ItemPurchaseGroup = 0x0000014D, // 0x0104B798
        ItemRandomProperties = 0x0000014E, // 0x0104B7BC
        ItemRandomSuffix = 0x0000014F, // 0x0104B7E0
        ItemSet = 0x00000150, // 0x0104B804
        ItemSubClass = 0x00000151, // 0x0104B84C
        ItemSubClassMask = 0x00000152, // 0x0104B828
        ItemVisualEffects = 0x00000153, // 0x0104B870
        ItemVisuals = 0x00000154, // 0x0104B894
        LanguageWords = 0x00000155, // 0x0104B8B8
        Languages = 0x00000156, // 0x0104B8DC
        LfgDungeons = 0x00000157, // 0x0104B900
        Light = 0x00000158, // 0x0106D5D0
        LightFloatBand = 0x00000159, // 0x0106D588
        LightIntBand = 0x0000015A, // 0x0106D564
        LightParams = 0x0000015B, // 0x0106D5AC
        LightSkybox = 0x0000015C, // 0x0106D540
        LiquidType = 0x0000015D, // 0x0104B924
        LiquidMaterial = 0x0000015E, // 0x0104B948
        LoadingScreens = 0x0000015F, // 0x0104B96C
        LoadingScreenTaxiSplines = 0x00000160, // 0x0104B990
        Lock = 0x00000161, // 0x0104B9B4
        LockType = 0x00000162, // 0x0104B9D8
        MailTemplate = 0x00000163, // 0x0104B9FC
        Map = 0x00000164, // 0x0104BA20
        MapDifficulty = 0x00000165, // 0x0104BA44
        Material = 0x00000166, // 0x0104BA68
        Movie = 0x00000167, // 0x0104BA8C
        MovieFileData = 0x00000168, // 0x0104BAB0
        MovieVariation = 0x00000169, // 0x0104BAD4
        NameGen = 0x0000016A, // 0x0104BAF8
        NPCSounds = 0x0000016B, // 0x0104BB1C
        NamesProfanity = 0x0000016C, // 0x0104BB40
        NamesReserved = 0x0000016D, // 0x0104BB64
        OverrideSpellData = 0x0000016E, // 0x0104BB88
        Package = 0x0000016F, // 0x0104BBAC
        PageTextMaterial = 0x00000170, // 0x0104BBD0
        PaperDollItemFrame = 0x00000171, // 0x0104BBF4
        ParticleColor = 0x00000172, // 0x0104BC18
        PetPersonality = 0x00000173, // 0x0104BC3C
        PowerDisplay = 0x00000174, // 0x0104BC60
        QuestInfo = 0x00000175, // 0x0104BC84
        QuestSort = 0x00000176, // 0x0104BCA8
        Resistances = 0x00000177, // 0x0104BCCC
        RandPropPoints = 0x00000178, // 0x0104BCF0
        ScalingStatDistribution = 0x00000179, // 0x0104BD14
        ScalingStatValues = 0x0000017A, // 0x0104BD38
        ScreenEffect = 0x0000017B, // 0x0104BD5C
        ServerMessages = 0x0000017C, // 0x0104BD80
        SheatheSoundLookups = 0x0000017D, // 0x0104BDA4
        SkillCostsData = 0x0000017E, // 0x0104BDC8
        SkillLineAbility = 0x0000017F, // 0x0104BDEC
        SkillLineCategory = 0x00000180, // 0x0104BE10
        SkillLine = 0x00000181, // 0x0104BE34
        SkillRaceClassInfo = 0x00000182, // 0x0104BE58
        SkillTiers = 0x00000183, // 0x0104BE7C
        SoundAmbience = 0x00000184, // 0x0104BEA0
        SoundEmitters = 0x00000185, // 0x0104BEE8
        SoundEntries = 0x00000186, // 0x0104BEC4
        SoundProviderPreferences = 0x00000187, // 0x0104BF0C
        SoundSamplePreferences = 0x00000188, // 0x0104BF30
        SoundWaterType = 0x00000189, // 0x0104BF54
        SpamMessages = 0x0000018A, // 0x0104BF78
        SpellCastTimes = 0x0000018B, // 0x0104BF9C
        SpellCategory = 0x0000018C, // 0x0104BFC0
        SpellChainEffects = 0x0000018D, // 0x0104BFE4
        Spell = 0x0000018E, // 0x0104C200
        SpellDescriptionVariables = 0x0000018F, // 0x0104C008
        SpellDispelType = 0x00000190, // 0x0104C02C
        SpellDuration = 0x00000191, // 0x0104C050
        SpellEffectCameraShakes = 0x00000192, // 0x0104C074
        SpellFocusObject = 0x00000193, // 0x0104C098
        SpellIcon = 0x00000194, // 0x0104C0BC
        SpellItemEnchantment = 0x00000195, // 0x0104C0E0
        SpellItemEnchantmentCondition = 0x00000196, // 0x0104C104
        SpellMechanic = 0x00000197, // 0x0104C128
        SpellMissile = 0x00000198, // 0x0104C14C
        SpellMissileMotion = 0x00000199, // 0x0104C170
        SpellRadius = 0x0000019A, // 0x0104C194
        SpellRange = 0x0000019B, // 0x0104C1B8
        SpellRuneCost = 0x0000019C, // 0x0104C1DC
        SpellShapeshiftForm = 0x0000019D, // 0x0104C224
        SpellVisual = 0x0000019E, // 0x0104C2D8
        SpellVisualEffectName = 0x0000019F, // 0x0104C248
        SpellVisualKit = 0x000001A0, // 0x0104C26C
        SpellVisualKitAreaModel = 0x000001A1, // 0x0104C290
        SpellVisualKitModelAttach = 0x000001A2, // 0x0104C2B4
        StableSlotPrices = 0x000001A3, // 0x0104C2FC
        Stationery = 0x000001A4, // 0x0104C320
        StringLookups = 0x000001A5, // 0x0104C344
        SummonProperties = 0x000001A6, // 0x0104C368
        Talent = 0x000001A7, // 0x0104C38C
        TalentTab = 0x000001A8, // 0x0104C3B0
        TaxiNodes = 0x000001A9, // 0x0104C3D4
        TaxiPath = 0x000001AA, // 0x0104C41C
        TaxiPathNode = 0x000001AB, // 0x0104C3F8
        TerrainType = 0x000001AC, // 0x0104C440
        TerrainTypeSounds = 0x000001AD, // 0x0104C464
        TotemCategory = 0x000001AE, // 0x0104C488
        TransportAnimation = 0x000001AF, // 0x0104C4AC
        TransportPhysics = 0x000001B0, // 0x0104C4D0
        TransportRotation = 0x000001B1, // 0x0104C4F4
        UISoundLookups = 0x000001B2, // 0x0104C518
        UnitBlood = 0x000001B3, // 0x0104C560
        UnitBloodLevels = 0x000001B4, // 0x0104C53C
        Vehicle = 0x000001B5, // 0x0104C584
        VehicleSeat = 0x000001B6, // 0x0104C5A8
        VocalUISounds = 0x000001B7, // 0x0104C5CC
        WMOAreaTable = 0x000001B8, // 0x0104C5F0
        WeaponImpactSounds = 0x000001B9, // 0x0104C614
        WeaponSwingSounds2 = 0x000001BA, // 0x0104C638
        Weather = 0x000001BB, // 0x0104C65C
        WorldMapArea = 0x000001BC, // 0x0104C680
        WorldMapTransforms = 0x000001BD, // 0x0104C6EC
        WorldMapContinent = 0x000001BE, // 0x0104C6A4
        WorldMapOverlay = 0x000001BF, // 0x0104C6C8
        WorldSafeLocs = 0x000001C0, // 0x0104C710
        WorldStateUI = 0x000001C1, // 0x0104C734
        ZoneIntroMusicTable = 0x000001C2, // 0x0104C758
        ZoneMusic = 0x000001C3, // 0x0104C77C
        WorldStateZoneSounds = 0x000001C4, // 0x0104C7A0
        WorldChunkSounds = 0x000001C5, // 0x0104C7C4
        SoundEntriesAdvanced = 0x000001C6, // 0x0104C7E8
        ObjectEffect = 0x000001C7, // 0x0104C80C
        ObjectEffectGroup = 0x000001C8, // 0x0104C830
        ObjectEffectModifier = 0x000001C9, // 0x0104C854
        ObjectEffectPackage = 0x000001CA, // 0x0104C878
        ObjectEffectPackageElem = 0x000001CB, // 0x0104C89C
        SoundFilter = 0x000001CC, // 0x0104C8C0
        SoundFilterElem = 0x000001CD, // 0x0104C8E4
    }

    // ReSharper restore InconsistentNaming
}