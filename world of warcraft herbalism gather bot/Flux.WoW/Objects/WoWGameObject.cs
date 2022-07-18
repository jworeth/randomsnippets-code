using System;
using System.Runtime.InteropServices;

using Flux.Utilities;
using Flux.WoW.Native;
using Flux.WoW.Patchables;

namespace Flux.WoW.Objects
{
    public class WoWGameObject : WoWObject
    {
        #region GameObjectFlags enum

        [Flags]
        public enum GameObjectFlags // :ushort
        {
            /// <summary>
            /// 0x1
            /// Disables interaction while animated
            /// </summary>
            InUse = 0x01,
            /// <summary>
            /// 0x2
            /// Requires a key, spell, event, etc to be opened. 
            /// Makes "Locked" appear in tooltip
            /// </summary>
            Locked = 0x02,
            /// <summary>
            /// 0x4
            /// Objects that require a condition to be met before they are usable
            /// </summary>
            ConditionalInteraction = 0x04,
            /// <summary>
            /// 0x8
            /// any kind of transport? Object can transport (elevator, boat, car)
            /// </summary>
            Transport = 0x08,
            GOFlag_0x10 = 0x10,
            /// <summary>
            /// 0x20
            /// These objects never de-spawn, but typically just change state in response to an event
            /// Ex: doors
            /// </summary>
            DoesNotDespawn = 0x20,
            /// <summary>
            /// 0x40
            /// Typically, summoned objects. Triggered by spell or other events
            /// </summary>
            Triggered = 0x40,

            GOFlag_0x80 = 0x80,
            GOFlag_0x100 = 0x100,
            GOFlag_0x200 = 0x200,
            GOFlag_0x400 = 0x400,
            GOFlag_0x800 = 0x800,
            GOFlag_0x1000 = 0x1000,
            GOFlag_0x2000 = 0x2000,
            GOFlag_0x4000 = 0x4000,
            GOFlag_0x8000 = 0x8000,

            Flag_0x10000 = 0x10000,
            Flag_0x20000 = 0x20000,
            Flag_0x40000 = 0x40000,
        }

        #endregion

        public WoWGameObject(IntPtr objectPtr) : base(objectPtr)
        {
            
        }

        public ulong CreatedBy { get { return GetStorageField<ulong>(GameObjectFields.OBJECT_FIELD_CREATED_BY); } }
        public int DisplayId { get { return GetStorageField<int>(GameObjectFields.GAMEOBJECT_DISPLAYID); } }
        public bool IsBobbing { get { return Reader.Read<byte>(this.ObjPtr.ToUInt32() + (uint) GameObjectOffsets.Animating) == 1; } }
        public int AnimState { get { return Reader.Read<byte>(this.ObjPtr.ToUInt32() + (uint)GameObjectOffsets.Animating); } }
        public uint Flags { get { return GetStorageField<uint>(GameObjectFields.GAMEOBJECT_FLAGS); } }
        public bool Locked { get { return HasFlag(Flags, (uint) GameObjectFlags.Locked); } }
        public bool Transport { get { return HasFlag(Flags, (uint) GameObjectFlags.Transport); } }
        public bool InUse { get { return HasFlag(Flags, (uint) GameObjectFlags.InUse); } }

        public bool IsHerb { get { return Model.ToUpper().Contains("\\BUSH_"); } }
        public bool IsMine { get { return Model.ToUpper().Contains("_MININGNODE_"); } }

        public byte[] Bytes1 { get { return BitConverter.GetBytes(GetStorageField<uint>(GameObjectFields.GAMEOBJECT_BYTES_1)); } }

        public GameObjectType SubType { get { return (GameObjectType) Bytes1[1]; } }

        public int Level { get { return GetStorageField<int>(GameObjectFields.GAMEOBJECT_LEVEL); } }

        private T GetStorageField<T>(GameObjectFields field) where T : struct
        {
            return GetStorageField<T>((uint) field);
        }

        public void DumpGOStack()
        {
            Logging.WriteDebug(Win32.ReadBytes(this, 0xFF).ToRealString());
        }

        #region Nested type: GameObjectOffsets

        private enum GameObjectOffsets
        {
            Animating = 0xBC
        }

        #endregion
    }

    public enum GameObjectType : uint
    {
        Door = 0x0,
        Button = 0x1,
        Questgiver = 0x2,
        Container = 0x3,
        Binder = 0x4,
        Generic = 0x5,
        Trap = 0x6,
        Chair = 0x7,
        SpellFocus = 0x8,
        Text = 0x9,
        Goober = 0xA,
        Transport = 0xB,
        Areadamage = 0xC,
        Camera = 0xD,
        MapObject = 0xE,
        MapObjectTransport = 0xF,
        DuelArbiter = 0x10,
        FishingBobber = 0x11,
        Ritual = 0x12,
        Mailbox = 0x13,
        AuctionHouse = 0x14,
        Guardpost = 0x15,
        Portal = 0x16,
        MeetingStone = 0x17,
        Flagstand = 0x18,
        FishingHole = 0x19,
        Flagdrop = 0x1A,
        MiniGame = 0x1B,
        LotteryKiosk = 0x1C,
        CapturePoint = 0x1D,
        AuraGenerator = 0x1E,
        DungeonDifficulty = 0x1F,
        BarberChair = 0x20,
        DestructibleBuilding = 0x21,
        Guildbank = 0x22,
        Trapdoor = 0x23,

        FORCEDWORD = 0xFFFFFFFF,
    }
}