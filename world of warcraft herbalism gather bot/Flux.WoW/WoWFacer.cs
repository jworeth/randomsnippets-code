using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Flux.Utilities;
using Flux.WoW.Native;
using Flux.WoW.Patchables;

namespace Flux.WoW
{
    internal static class WoWFacer
    {
        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr GetActiveCameraDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate int GetCursorDelegate(IntPtr cursorPoint);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate int SetCursorDelegate(int x, int y);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int UpdateFreeLookFacingDelegate(IntPtr instance, float rotH, float rotV, int unk);

        #endregion

        public static readonly GetActiveCameraDelegate GetActiveCamera;

        public static readonly GetCursorDelegate GetCursorPos;
        public static readonly GetCursorDelegate GetPhysicalCursorPos;
        public static readonly GetCursorDelegate HookGetCursorPos = GetCursorPosHook;
        public static readonly GetCursorDelegate HookGetPhysicalCursorPos = GetPhysicalCursorPosHook;

        public static readonly SetCursorDelegate HookSetCursorPos = SetCursorPosHook;
        public static readonly SetCursorDelegate HookSetPhysicalCursorPos = SetPhysicalCursorPosHook;
        public static readonly SetCursorDelegate SetCursorPos;
        public static readonly SetCursorDelegate SetPhysicalCursorPos;
        private static readonly SyncFreeLookFacingDelegate SyncFreeLookFacing;
        public static readonly UpdateFreeLookFacingDelegate UpdateFreeLookFacing;
        private static double _destHeading;
        private static WoWFacingState _facingState = WoWFacingState.End;
        private static double _finalisationPrecision;
        private static double _initiationPrecision;
        private static System.Drawing.Point _lastCursorPos;
        private static System.Drawing.Point _lastPhysicalCursorPos;
        private static long _lastPulseTime;
        private static int CamSpeed = 5;

        static WoWFacer()
        {
            //load the library user32
            int hwnd = Win32.LoadLibrary("user32.dll");

            GetActiveCamera =
                Utilities.RegisterDelegate<GetActiveCameraDelegate>((uint) GlobalOffsets.CGWorldFrame__GetActiveCamera);
            UpdateFreeLookFacing =
                Utilities.RegisterDelegate<UpdateFreeLookFacingDelegate>(
                    (uint) GlobalOffsets.CGCamera__UpdateFreeLookFacing);
            SyncFreeLookFacing =
                Utilities.RegisterDelegate<SyncFreeLookFacingDelegate>((uint) GlobalOffsets.CGCamera__SyncFreeLookFacing);

            // Note: This will work on XP/Vista/W7 just fine.
            SetCursorPos = Utilities.RegisterDelegate<SetCursorDelegate>(Win32.GetProcAddress(hwnd, "SetCursorPos"));
            GetCursorPos = Utilities.RegisterDelegate<GetCursorDelegate>(Win32.GetProcAddress(hwnd, "GetCursorPos"));
            Detours.Add(SetCursorPos, HookSetCursorPos, "HookSetCursorPos").Apply();
            Detours.Add(GetCursorPos, HookGetCursorPos, "HookGetCursorPos").Apply();

            if (Environment.OSVersion.Version.Major > 5)
            {
                // NOTE: XP doesn't have the Get/SetPhysicalCursorPos functions. So we'll just completely
                // ignore their existence :D
                SetPhysicalCursorPos =
                    Utilities.RegisterDelegate<SetCursorDelegate>(Win32.GetProcAddress(hwnd, "SetPhysicalCursorPos"));
                GetPhysicalCursorPos =
                    Utilities.RegisterDelegate<GetCursorDelegate>(Win32.GetProcAddress(hwnd, "GetPhysicalCursorPos"));

                Detours.Add(SetPhysicalCursorPos, HookSetPhysicalCursorPos, "HookSetPhysicalCursorPos")
                    .Apply();
                Detours.Add(GetPhysicalCursorPos, HookGetPhysicalCursorPos, "HookGetPhysicalCursorPos")
                    .Apply();
            }
        }

        private static bool IsFacing { get { return FluxWoW.Movement.HasFlag(MovementDirection.RMouse); } }

        public static void Pulse()
        {
            if (_facingState == WoWFacingState.End)
            {
                return;
            }

            // calculate the difference between where we are and where we want to be
            double currentHeading = FluxWoW.Me.Facing;
            double headingDiff;

            int directionCoeff;

            GetHeadingDiff(currentHeading, _destHeading, out headingDiff, out directionCoeff);

            // stop moving if we're facing where we want to face
            double checkPrecision = 0.0;

            switch (_facingState)
            {
                case WoWFacingState.Start:
                    checkPrecision = _initiationPrecision;
                    break;
                case WoWFacingState.Facing:
                    checkPrecision = _finalisationPrecision;
                    break;
            }

            if (Math.Abs(headingDiff) <= checkPrecision)
            {
               // Logging.WriteDebug("Stoping Face " + checkPrecision);
                StopFacing();
                return;
            }

            // initiate a right-mouse hold
            if (_facingState == WoWFacingState.Start)
            {
                SaveCusorPos();
               // Logging.WriteDebug("initiate a right-mouse hold");

                _facingState = WoWFacingState.Facing;
                //Lua.DoString("TurnOrActionStart()");
                FluxWoW.Movement.Move(MovementDirection.RMouse);
                _lastPulseTime = GetLastPulseTime();
            }

            if (!IsFacing)
            {
                _facingState = WoWFacingState.Start;
                return;
            }
            // calculate how far we have left to turn
            var unitsLeftToTurn = (int) Math.Round(headingDiff * 200);

            // now calculate how far we would turn based on the time passed since the last facing pulse
            long pulseDiff = GetLastPulseTime() - _lastPulseTime;
            var unitsForTimePassed = (int) ((pulseDiff / 1000000.0) * CamSpeed * 200);

            _lastPulseTime = GetLastPulseTime();

            // the minimum of the two is how far we are actually going to turn this pulse
            int unitsThisTurn = Math.Min(unitsLeftToTurn, unitsForTimePassed) * directionCoeff;

            UpdateFreeLookFacing(GetActiveCamera(), unitsThisTurn, 0, 0);
            SyncFreeLookFacing(GetActiveCamera());
        }

        private static int GetCursorPosHook(IntPtr cursorPoint)
        {
            try
            {
                if (_facingState == WoWFacingState.Facing)
                {
                    Marshal.WriteInt32(cursorPoint, 0, _lastCursorPos.X);
                    Marshal.WriteInt32(cursorPoint, 4, _lastCursorPos.Y);
                    return 1;
                }

                return (int) Detours.Get("HookGetCursorPos").CallOriginal(cursorPoint);
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
                throw;
            }
        }

        private static int SetCursorPosHook(int x, int y)
        {
            try
            {
                if (_facingState == WoWFacingState.Facing)
                {
                    _lastCursorPos.X = x;
                    _lastCursorPos.Y = y;
                    return 1;
                }

                if (FluxWoW.Movement.HasFlag(MovementDirection.LMouse))
                {
                    return 1;
                }

                return (int) Detours.Get("HookSetCursorPos").CallOriginal(x, y);
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
                throw;
            }
        }

        private static int GetPhysicalCursorPosHook(IntPtr cursorPoint)
        {
            try
            {
                if (_facingState == WoWFacingState.Facing)
                {
                    Marshal.WriteInt32(cursorPoint, 0, _lastPhysicalCursorPos.X);
                    Marshal.WriteInt32(cursorPoint, 4, _lastPhysicalCursorPos.Y);
                    return 1;
                }

                return (int) Detours.Get("HookGetPhysicalCursorPos").CallOriginal(cursorPoint);
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
                throw;
            }
        }

        private static int SetPhysicalCursorPosHook(int x, int y)
        {
            try
            {
                if (_facingState == WoWFacingState.Facing)
                {
                    _lastPhysicalCursorPos.X = x;
                    _lastPhysicalCursorPos.Y = y;
                    return 1;
                }

                if (FluxWoW.Movement.HasFlag(MovementDirection.LMouse))
                {
                    return 1;
                }

                return (int) Detours.Get("HookSetPhysicalCursorPos").CallOriginal(x, y);
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
                throw;
            }
        }

        #region Helper Functions

        private static long GetLastPulseTime()
        {
            long counter = 0;
            long freq = 0;

            Win32.QueryPerformanceCounter(ref counter);
            Win32.QueryPerformanceFrequency(ref freq);

            return (counter * 1000000) / freq;
        }

        public static void GetHeadingDiff(double currentHeading, double destHeading, out double headingDiff,
                                          out int directionCoeff)
        {
            headingDiff = currentHeading - destHeading;
            directionCoeff = (int) (headingDiff / Math.Abs(headingDiff));
            headingDiff = Math.Abs(headingDiff);

            if (headingDiff > Math.PI)
            {
                headingDiff = (2.0 * Math.PI) - headingDiff;
                directionCoeff *= -1;
            }
        }

        private static void SaveCusorPos()
        {
            // if this is called with the facing state set to Facing, something has gone wrong... reset and try again
            if (_facingState == WoWFacingState.Facing)
            {
                _facingState = WoWFacingState.End;
                return;
            }

            Win32.GetCursorPos(ref _lastCursorPos);
        }

        private static void StopFacing()
        {
            //if (IsFacing)
            //{
            FluxWoW.Movement.MoveStop(MovementDirection.RMouse);
            //}

            _facingState = WoWFacingState.End;
        }

        public static void SetFaceDestination(double heading, double initiationPrecision, double finalisationPrecision)
        {
            _destHeading = heading;
            _initiationPrecision = initiationPrecision;
            _finalisationPrecision = finalisationPrecision;

            if (_facingState != WoWFacingState.Facing)
            {
                _facingState = WoWFacingState.Start;
            }
        }

        public static void SetFaceDestination(double heading, double initiationPrecision, double finalisationPrecision, int speed)
        {
            CamSpeed = speed;
            _destHeading = heading;
            _initiationPrecision = initiationPrecision;
            _finalisationPrecision = finalisationPrecision;

            if (_facingState != WoWFacingState.Facing)
            {
                _facingState = WoWFacingState.Start;
            }
        }

        #endregion

        #region Nested type: SyncFreeLookFacingDelegate

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void SyncFreeLookFacingDelegate(IntPtr instance);

        #endregion

        #region Nested type: WoWFacingState

        private enum WoWFacingState
        {
            Start,
            Facing,
            End
        }

        #endregion
    }
}