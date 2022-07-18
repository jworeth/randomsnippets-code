using System;
using System.Runtime.InteropServices;

namespace Flux.WoW
{
    public class WoWBuff
    {
        private readonly IntPtr _address;
        private readonly Aura _aura;

        internal WoWBuff(IntPtr address)
        {
            _address = address;
            if (IsValid)
            {
                _aura = (Aura) Marshal.PtrToStructure(address, typeof (Aura));
            }
        }

        public bool IsValid { get { return _address != IntPtr.Zero; } }

        /// <summary>
        /// Gets the creator GUID of this <see cref="WoWBuff"/>.
        /// </summary>
        /// <value>The creator GUID.</value>
        public ulong CreatorGuid { get { return _aura.CreatorGuid; } }

        /// <summary>
        /// Gets the spell id of this <see cref="WoWBuff"/>.
        /// </summary>
        /// <value>The spell id.</value>
        public int SpellId { get { return _aura.AuraId; } }

        private byte Flags { get { return _aura.Flags; } }

        /// <summary>
        /// Gets the duration of this <see cref="WoWBuff"/>.
        /// </summary>
        /// <value>The duration.</value>
        public uint Duration { get { return _aura.Duration; } }

        /// <summary>
        /// Gets the end time of this <see cref="WoWBuff"/>.
        /// </summary>
        /// <value>The end time.</value>
        public uint EndTime { get { return _aura.EndTime; } }

        /// <summary>
        /// Gets the stack count of this <see cref="WoWBuff"/>.
        /// </summary>
        /// <value>The stack count.</value>
        public uint StackCount { get { return _aura.StackCount; } }

        /// <summary>
        /// Gets the level of this <see cref="WoWBuff"/>.
        /// </summary>
        /// <value>The level.</value>
        public uint Level { get { return _aura.Level; } }

        /// <summary>
        /// Gets a value indicating whether this instance is harmful.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is harmful; otherwise, <c>false</c>.
        /// </value>
        public bool IsHarmful { get { return (Flags & 0x20) != 0; } }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get { return (Flags & 0x80) != 0; } }

        /// <summary>
        /// Gets a value indicating whether this instance is passive.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is passive; otherwise, <c>false</c>.
        /// </value>
        public bool IsPassive { get { return (Flags & 0x10) != 0 && !IsActive; } }

        /// <summary>
        /// Gets the name of this <see cref="WoWBuff"/>.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get { return Spell.Name; } }

        //public bool IsStealable { get { return (Bytes[0] & 0x8) == 0; } } // Not sure if this is correct!

        public WoWSpell Spell { get { return new WoWSpell(_aura.AuraId); } }

        #region Nested type: Aura

        [StructLayout(LayoutKind.Sequential)]
        private struct Aura
        {
            public ulong CreatorGuid;
            public int AuraId;
            public byte Flags;
            public byte Level;
            // Note: This can be a byte array, but the 2nd byte is always 00, so we might as well just do it this way.
            public ushort StackCount;
            public uint Duration;
            public uint EndTime;
        }

        #endregion

        #region Nested type: AuraFlags

        private enum AuraFlags
        {
            Active = 0x80,
            Passive = 0x10, // Check if !Active
            Harmful = 0x20
        }

        #endregion
    }
}