using System;
using System.Runtime.InteropServices;

using Flux.WoW.Native;
using Flux.WoW.Objects;
using Flux.WoW.Patchables;

namespace Flux.WoW
{
    /// <summary>
    /// The direction of movement in WoW as per the CGInputControl_ToggleControlBit function.
    /// These are actually the flags that are set/unset!
    /// </summary>
    [Flags]
    public enum MovementDirection : uint
    {
        None = 0,
        RMouse = (1 << 0), // 0x1,
        LMouse = (1 << 1), // 0x2,
        // 2 and 3 not used apparently. Possibly for flag masking?
        Forward = (1 << 4), // 0x10,
        Backward = (1 << 5), // 0x20,
        StrafeLeft = (1 << 6), // 0x40,
        StrafeRight = (1 << 7), // 0x80,
        TurnLeft = (1 << 8), // 0x100,
        TurnRight = (1 << 9), // 0x200,
        PitchUp = (1 << 10), // 0x400, For flying/swimming
        PitchDown = (1 << 11), // 0x800, For flying/swimming
        AutoRun = (1 << 12), // 0x1000,
        JumpAscend = (1 << 13), // 0x2000, For flying/swimming
        Descend = (1 << 14), // 0x4000, For flying/swimming

        ClickToMove = (1 << 22), // 0x400000, Note: Only turns the CTM flag on or off. Has no effect on movement!

        // 25 used somewhere. Can't figure out what for. Checked in Lua_IsMouseTurning. Possible camera turn?
        // Or mouse input flag? (Flag used: 0x2000001)
    }

    public class WoWMovement
    {
        #region ClickToMoveType enum

        public enum ClickToMoveType
        {
            FaceTarget = 0x1,
            Face = 0x2,
            /// <summary>
            /// Will throw a UI error. Have not figured out how to avoid it!
            /// </summary>
            Stop_ThrowsException = 0x3,
            Move = 0x4,
            NpcInteract = 0x5,
            Loot = 0x6,
            ObjInteract = 0x7,
            FaceOther = 0x8,
            Skin = 0x9,
            AttackPosition = 0xA,
            AttackGuid = 0xB,
            ConstantFace = 0xC,
            None = 0xD,

            Attack = 0x10,
            Idle = 0x13,
        }

        #endregion

        #region ClickType enum

        public enum ClickType
        {
            None = 0,
            LeftClick = 1,
            RightClick = 4,
        }

        #endregion

        private static readonly ClickTerrainDelegate ClickTerrainHandler;
        private static readonly GetActiveInputControl GetInputCtrl;
        private static readonly ToggleControlBit ToggleBit;

        static WoWMovement()
        {
            ToggleBit = Utilities.RegisterDelegate<ToggleControlBit>((uint) GlobalOffsets.CGInputControl__ToggleControlBit);
            ClickTerrainHandler = Utilities.RegisterDelegate<ClickTerrainDelegate>((uint) GlobalOffsets.ClickTerrain);
            GetInputCtrl =
                Utilities.RegisterDelegate<GetActiveInputControl>((uint) GlobalOffsets.CGInputControl__GetActive);
        }

        private static IntPtr InputCtrl { get { return GetInputCtrl(); } }

        public bool HasFlag(MovementDirection dir)
        {
            if (InputCtrl != IntPtr.Zero)
            {
                int flags = Marshal.ReadInt32(InputCtrl, 4); //inputCtrl bits!
                return (flags & (uint) dir) == 1;
            }
            return false;
        }

        private static void ToggleControlBits(MovementDirection dir, bool stop, int time)
        {
            ToggleBit(InputCtrl, (uint) dir, stop ? 0 : 1, time, 0);
        }

        #region Click Stuff

        private CPlayerC_ClickToMove _clickToMove;

        public uint ControlFlags { get { return Reader.Read<uint>(InputCtrl.ToUInt32() + 4); } }

        public ClickToMoveInfoStruct ClickToMoveInfo { get { return Reader.ReadStruct<ClickToMoveInfoStruct>((IntPtr) GlobalOffsets.ClickToMove_Base); } }

        public bool IsClickMoving { get { return ClickToMoveInfo.IsClickToMoving == 2; } }

        public Point LastClickPosition
        {
            get
            {
                ClickToMoveInfoStruct i = ClickToMoveInfo;
                return i.Click;
            }
        }

        public void ClickTerrain(Point pt, ClickType type)
        {
            var clickPos = new CtmPos {X = pt.X, Y = pt.Y, Z = pt.Z, Flag = type};
            ClickTerrainHandler(ref clickPos);
        }

        public void ClickToMove(Point loc)
        {
            ClickToMove(0, loc, ClickToMoveType.Move);
        }

        public void ClickToMove(Point loc, ClickToMoveType type)
        {
            ClickToMove(0, loc, type);
        }

        public void ClickToMove(ulong guid, Point loc, ClickToMoveType type)
        {
            ClickToMove(guid, loc, type, .5f);
        }

        public void ClickToMove(ulong guid, Point loc, ClickToMoveType type, float precision)
        {
            if (_clickToMove == null)
            {
                _clickToMove =
                    Utilities.RegisterDelegate<CPlayerC_ClickToMove>((uint) GlobalOffsets.CGPlayer_C__ClickToMove);
            }

            Lua.DoString("SetCVar(\"AutoInteract\",\"1\")");
            _clickToMove(FluxWoW.Me, (int) type, ref guid, ref loc, precision);
        }

        #endregion

        #region Face

        /// <summary>
        /// Calculate the angle for points two "face" each other. 
        /// </summary>
        /// <param name="start">The start point eg. Me</param>
        /// <param name="target">The target point eg. Target</param>
        /// <returns>The facing angle (in radians)</returns>
        public static float FaceToCalc(Point start, Point target)
        {
            var faceAngle = (float) Math.Atan2(target.Y - start.Y, target.X - start.X);
            if (faceAngle < 0f)
            {
                faceAngle += (float) Math.PI * 2;
            }
            else if (faceAngle >= (Math.PI * 2))
            {
                faceAngle -= (float) Math.PI * 2;
            }
            return faceAngle;
        }

        public void Face(Point pt)
        {
            Face(pt, 0.2);
        }

        public void Face(Point pt, double precision)
        {
            Face(pt, precision, 5);
        }

        public void Face(Point pt, double precision, int speed)
        {
            Face(FaceToCalc(FluxWoW.Me.Position, pt), precision, speed);
        }

        public void Face(WoWObject other)
        {
            Face(other.Position);
        }

        public void Face(WoWObject other, double precision)
        {
            Face(other.Position, precision);
        }

        public void Face(WoWObject other, double precision, int speed)
        {
            Face(other.Position, precision, speed);
        }

        public void Face(float heading)
        {
            Face(heading, 0.5);
        }

        public void Face(float heading, double precision)
        {
            Face(heading, precision, 5);
        }

        public void Face(float heading, double precision, int speed)
        {
            WoWFacer.SetFaceDestination(heading, precision, precision, speed);
        }

        #endregion

        #region Movement 

        public void Move(MovementDirection dir)
        {
            Move(dir, 0);
        }

        public void Move(MovementDirection dir, int time)
        {
            ToggleControlBits(dir, false, time);
        }

        public void MoveStop()
        {
            if (ClickToMoveInfo.ActionType != (uint) ClickToMoveType.None)
            {
                Move(MovementDirection.Forward);
                MoveStop(MovementDirection.Forward);
            }

            MoveStop(MovementDirection.Forward | MovementDirection.Backward | MovementDirection.StrafeLeft |
                     MovementDirection.StrafeRight | MovementDirection.JumpAscend | MovementDirection.Descend);
        }

        public void MoveStop(MovementDirection dir)
        {
            MoveStop(dir, 0);
        }

        public void MoveStop(MovementDirection dir, int time)
        {
            ToggleControlBits(dir, true, time);
        }

        #endregion

        #region Nested type: ClickTerrainDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ClickTerrainDelegate(ref CtmPos pLoc);

        #endregion

        #region Nested type: ClickToMoveHandler

        internal class ClickToMoveHandler
        {
            /// <summary>
            /// The base address we're going to be reading/writing from.
            /// </summary>
            private const uint Base = (uint) GlobalOffsets.ClickToMove_Base;

            public static float TurnScale { get { return Reader.Read<float>(Base + 0x4); } set { Win32.WriteBytes((IntPtr) (Base + 0x4), BitConverter.GetBytes(value)); } }

            public static float InteractDistance { get { return Reader.Read<float>(Base); } }
        }

        #endregion

        #region Nested type: ClickToMoveInfoStruct

        [StructLayout(LayoutKind.Sequential)]
        public struct ClickToMoveInfoStruct
        {
            public float Unknown1F;
            public float TurnScale;
            public float Unknown2F;

            /// <summary>
            /// This can be left alone. There is no need to write to it!!
            /// </summary>
            public float InteractionDistance;

            public float Unknown3F;
            public float Unknown4F;
            public uint Unknown5U;

            /// <summary>
            /// As per the ClickToMoveAction enum members.
            /// </summary>
            public uint ActionType;

            public ulong InteractGuid;

            /// <summary>
            /// Check == 2 (This might be some sort of flag?)
            /// Always 2 when using some form of CTM action. 0 otherwise.
            /// </summary>
            public uint IsClickToMoving;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            public uint[] Unknown6U;

            /// <summary>
            /// This will change in memory as WoW figures out where exactly we're going to stop. (Also the actual end location)
            /// </summary>
            public Point Dest;

            /// <summary>
            /// This is wherever we actually 'clicked' in game.
            /// </summary>
            public Point Click;

            /// <summary>
            /// Returns the fully qualified type name of this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> containing a fully qualified type name.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public override string ToString()
            {
                string tmp = "";
                for (int i = 0; i < Unknown6U.Length; i++)
                {
                    tmp += "Unknown6U_" + i + ": " + Unknown6U[i].ToString("X") + ", ";
                }
                return
                    string.Format(
                        "TurnScale: {0}, InteractionDistance: {1}, ActionType: {2}, InteractGuid: {3}, IsClickToMoving: {4}, ClickX: {5}, ClickY: {6}, ClickZ: {7}, DestX: {8}, DestY: {9}, DestZ: {10}, Unk4f: {12}, UNK: {11}",
                        TurnScale, InteractionDistance, (ClickToMoveType) ActionType, InteractGuid.ToString("X"),
                        IsClickToMoving == 2, Click.X, Click.Y, Click.Z, Dest.X, Dest.Y, Dest.Z, tmp, Unknown4F);
            }
        }

        #endregion

        #region Nested type: CPlayerC_ClickToMove

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate byte CPlayerC_ClickToMove(
            IntPtr thisObj, int clickType, ref ulong guid, ref Point clickPos, float precision);

        #endregion

        #region Nested type: CtmPos

        [StructLayout(LayoutKind.Sequential)]
        private struct CtmPos
        {
            public float X;
            public float Y;
            public float Z;
            public ClickType Flag;
        }

        #endregion

        #region Nested type: GetActiveInputControl

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr GetActiveInputControl();

        #endregion

        #region Nested type: ToggleControlBit

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int ToggleControlBit(IntPtr instance, uint flag, int enable, int timestamp, int unkPassZero);

        #endregion
    }
}