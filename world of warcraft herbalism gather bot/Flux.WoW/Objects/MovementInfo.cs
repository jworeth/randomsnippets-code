using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Flux.WoW.Patchables;

namespace Flux.WoW.Objects
{
    public class MovementInfo
    {
        private IntPtr _pBase;

        public MovementInfo(IntPtr address)
        {
            _pBase = address;
        }

        public bool IsFalling { get { return GetFlag(MovementFlags.Falling); } }
        public bool MovingForward { get { return GetFlag(MovementFlags.Forward); } }
        public bool MovingBackward { get { return GetFlag(MovementFlags.Backward); } }
        public bool MovingStrafeLeft { get { return GetFlag(MovementFlags.StrafeLeft); } }
        public bool MovingStrafeRight { get { return GetFlag(MovementFlags.StrafeRight); } }
        public bool MovingStrafing { get { return GetFlag(MovementFlags.StrafeMask); } }
        public bool MovingTurnLeft { get { return GetFlag(MovementFlags.Left); } }
        public bool MovingTurnRight { get { return GetFlag(MovementFlags.Right); } }

        public bool IsMoving { get { return GetFlag(MovementFlags.MotionMask); } }
        public ulong TransportGuid { get { return GetStorageField<ulong>(MoveInfoOffsets.TransportGuid); } }
        private uint Flags { get { return GetStorageField<uint>(MoveInfoOffsets.Flags); } }
        public uint State { get { return GetStorageField<uint>(MoveInfoOffsets.PlayerState); } }
        public uint FallTime { get { return GetStorageField<uint>(MoveInfoOffsets.FallTime); } }
        public float FallStartHeight { get { return GetStorageField<float>(MoveInfoOffsets.FallStartHeight); } }
        public float LastFallHeight { get { return GetStorageField<float>(MoveInfoOffsets.LastFallHeight); } }
        public float CurrentSpeed { get { return GetStorageField<float>(MoveInfoOffsets.CurrentSpeed); } }

        public override string ToString()
        {
            var tmp = new List<string>();

            for (int i = 0; i < 32; i++)
            {
                tmp.Add(i + " -> " + GetFlag((1 << i)));
            }

            return string.Join(Environment.NewLine,
                               new[]
                                   {
                                       "Current Speed: " + CurrentSpeed, "Moving: " + IsMoving, "Falling: " + IsFalling,
                                       "Forward: " + MovingForward, "Backward: " + MovingBackward,
                                       "Strafe Left: " + MovingStrafeLeft, "Strafe Right: " + MovingStrafeRight,
                                       "Turning Left: " + MovingTurnLeft, "Turning Right: " + MovingTurnRight,
                                   });
        }

        private bool GetFlag(MovementFlags flag)
        {
            return (Flags & (uint) flag) != 0;
        }

        private bool GetFlag(int flag)
        {
            return (Flags & flag) != 0;
        }

        public static implicit operator IntPtr(MovementInfo i)
        {
            return i._pBase;
        }

        private T GetStorageField<T>(MoveInfoOffsets offs) where T : struct
        {
            var field = (uint) offs;
            object ret;
            Type ty = typeof (T);
            int size = Marshal.SizeOf(ty);
            var ba = new byte[size];
            for (int i = 0; i < size; i++)
            {
                ba[i] = Marshal.ReadByte((IntPtr) (_pBase.ToInt64() + field), i);
            }

            switch (Type.GetTypeCode(ty))
            {
                case TypeCode.Boolean:
                    ret = BitConverter.ToBoolean(ba, 0);
                    break;
                case TypeCode.Char:
                    ret = BitConverter.ToChar(ba, 0);
                    break;
                case TypeCode.Byte:
                    ret = ba[0];
                    break;
                case TypeCode.Int16:
                    ret = BitConverter.ToInt16(ba, 0);
                    break;
                case TypeCode.UInt16:
                    ret = BitConverter.ToUInt16(ba, 0);
                    break;
                case TypeCode.Int32:
                    ret = BitConverter.ToInt32(ba, 0);
                    break;
                case TypeCode.UInt32:
                    ret = BitConverter.ToUInt32(ba, 0);
                    break;
                case TypeCode.Int64:
                    ret = BitConverter.ToInt64(ba, 0);
                    break;
                case TypeCode.UInt64:
                    ret = BitConverter.ToUInt64(ba, 0);
                    break;
                case TypeCode.Single:
                    ret = BitConverter.ToSingle(ba, 0);
                    break;
                case TypeCode.Double:
                    ret = BitConverter.ToDouble(ba, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return (T) ret;
        }

        #region Nested type: MOVE_INFO

        private struct MOVE_INFO
        {
            private uint Flags;
            private float Heading;
            private uint PlayerState;
            private Point Position;
            private ulong TransportGuid;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            private byte[] Unk;
        }

        #endregion

        #region Nested type: MoveInfoOffsets

        private enum MoveInfoOffsets
        {
            Pos = 0x10, // Don't use. Use GetPos vfunc
            Heading = 0x1C, // Again, use the vfunc.
            TransportGuid = 0x38, // Haven't tested; possible taxi animal GUID or boat/zep?
            PlayerState = 0x40,
            Flags = 0x44,
            TimeMoved = 0x5C, // Not used/untested
            SinAngle = 0x6C, // Not used/untested
            CosAngle = 0x70, // Not used/untested
            FallTime = 0x7C, // Not used/untested
            FallStartHeight = 0x80, // Not used/untested
            LastFallHeight = 0x84, // Not used/untested
            WalkSpeed = 0x88,
            CurrentSpeed = 0x8C, // The actual current speed. I guess I can just switch these two, but meh
            RunSpeed = 0x90,
            RunBackSpeed = 0x94,
            SwimSpeed = 0x98,
            SwimBackSpeed = 0x9C,
            FlySpeed = 0xA0,
            FlyBackSpeed = 0xA4,
            TurnSpeed = 0xA8,
            JumpVelocity = 0xB4
        }

        #endregion
    }
}