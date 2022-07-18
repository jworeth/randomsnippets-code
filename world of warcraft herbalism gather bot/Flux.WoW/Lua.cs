using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Flux.Utilities;
using Flux.WoW.Native;
using Flux.WoW.Patchables;

namespace Flux.WoW
{
    public static class Lua
    {
        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int RegisteredLuaCommandHandler(IntPtr L);

        #endregion

        private static readonly RegisteredLuaCommandHandler CommandParser = FluxInputHandler;
        private static readonly LuaDoString DoStringHandler;
        private static readonly RegisteredLuaCommandHandler EventParser = FluxEventHandler;
        private static readonly RegisteredLuaCommandHandler MyParser = FluxLocalPlayerWrapper;
        private static readonly LuaGetTop GetTopHandler;
        private static readonly List<string> LuaValues = new List<string>();
        private static readonly LuaRegisterCommand RegisterCommandHandler;
        private static readonly LuaToString ToStringHandler;

        static Lua()
        {
           // Logging.WriteDebug("Lua subsystem initializing...");
            // Yes, I know these next 2 calls are redundant.
            // For some odd reason, they don't always work properly, so this fixes that issue. :)
            Process.EnterDebugMode();
            Win32.MemoryOpen();
            RegisterCommandHandler = Utilities.RegisterDelegate<LuaRegisterCommand>((uint) GlobalOffsets.FrameScript_RegisterFunction);
            DoStringHandler = Utilities.RegisterDelegate<LuaDoString>((uint) GlobalOffsets.FrameScript_Execute);
            GetTopHandler = Utilities.RegisterDelegate<LuaGetTop>((uint) GlobalOffsets.FrameScript_GetTop);
            ToStringHandler = Utilities.RegisterDelegate<LuaToString>((uint) GlobalOffsets.FrameScript_ToString);
            RegisterCommand("FluxInput", CommandParser);
            RegisterCommand("FluxEvents", EventParser);
            RegisterCommand("MyPlayer", MyParser);
            CreateEventFrame();
        }

        public static bool IsEventFrameAvailable
        {
            get
            {
                if (_frameCreated)
                    return true;
                const string execute = "tostring(fluxEventFrame:GetScript('OnEvent'))";
                return GetReturnVal<bool>(execute, 0);
            }
        }

        private static bool _frameCreated;
        public static void CreateEventFrame()
        {
            if (_frameCreated)
                return;
            if (!FluxWoW.IsInGame)
                return;
            DoString(
                "fluxEventFrame = CreateFrame(\"Frame\", \"FluxEventFrame\"); fluxEventFrame:RegisterAllEvents(); fluxEventFrame:SetScript(\"OnEvent\", FluxEvents);");
            _frameCreated = true;
        }

        private static int FluxInputHandler(IntPtr L)
        {
            LuaValues.Clear();
            int num = GetTop(L);
            for (int i = 0; i < num; i++)
            {
                string tmp = ToString(L, i);
                LuaValues.Add(tmp);
            }
            return 1;
        }

        private static int FluxLocalPlayerWrapper(IntPtr L)
        {
            int top = GetTop(L);
            if (top>=2)
            {
                string func = ToString(L, 0);

                if (func == "Face")
                {
                    if (top == 2)
                    {
                        float heading = float.Parse(ToString(L, 1));
                        Logging.WriteDebug("Facing " + heading);
                        FluxWoW.Movement.Face(heading);
                    }
                    else if (top == 4)
                    {
                        float x = float.Parse(ToString(L, 1));
                        float y = float.Parse(ToString(L, 2));
                        float z = float.Parse(ToString(L, 3));

                        FluxWoW.Movement.Face(new Point(x, y, z));
                    }
                }
                if (func == "Morph")
                {
                    int displayId = int.Parse(ToString(L, 1));
                    FluxWoW.Me.DisplayId = displayId;
                }
            }
            return 1;
        }

        private static int FluxEventHandler(IntPtr L)
        {
            int top = GetTop(L);
            string eventName = ToString(L, 1);
            var tmp = new List<string>();
            for (int i = 2; i < top; i++)
            {
                tmp.Add(ToString(L, i));
            }
            WoWEvents.ExecuteEvent(eventName, tmp.ToArray());
            return 1;
        }

        public static void DoString(string lua)
        {
            DoStringHandler(lua, "Flux.lua", 0);
        }

        public static string[] GetReturnValues(string lua)
        {
            DoString(string.Format("FluxInput({0})", lua));
            return LuaValues.ToArray();
        }

        public static T GetReturnVal<T>(string lua, uint retVal)
        {
            DoString(string.Format("FluxInput({0})", lua));

            try
            {
                object tmp;
                if (LuaValues.Count < retVal || LuaValues.Count == 0)
                {
                    return default(T);
                }
                string val = LuaValues[(int) retVal];
                if (string.IsNullOrEmpty(val) || val == "nil")
                {
                    return default(T);
                }

                if (typeof (T) == typeof (bool))
                {
                    tmp = val != "nil" || val != "0" || val.ToLower() != "false";
                }
                else
                {
                    tmp = (T) Convert.ChangeType(val, typeof (T));
                }
                return (T) tmp;
            }
            catch (Exception e)
            {
                Logging.LogException(e);
                return default(T);
            }
        }

        public static void RegisterCommand(string commandName, RegisteredLuaCommandHandler handler)
        {
            RegisterCommandHandler(commandName, WriteLuaCallback(Marshal.GetFunctionPointerForDelegate(handler)));
           // Logging.WriteDebug(string.Format("Registered lua command: {0}", commandName));
            return;
        }

        private static IntPtr WriteLuaCallback(IntPtr callbackPtr)
        {
            //System.Windows.Forms.MessageBox.Show(callbackPtr.ToString("X"));
            // Find a spot in .text where we can write our JMP [callbackPtr]
            // PATTERN: CC CC CC CC CC CC (xxxxxx)
            IntPtr writePtr = Offsets.GetRandomSixByteCave;
            //Logging.WriteDebug("Callback address: " + writePtr.ToString("X"));

            var toWrite = new List<byte> {0x68}; // PUSH is 0x68
            byte[] tmp = BitConverter.GetBytes(callbackPtr.ToUInt32());
            toWrite.AddRange(tmp);
            toWrite.Add(0xC3);

            Patcher.CreatePatch(new Patcher.Patch(writePtr, toWrite.ToArray(),
                                                  "Lua_RegisterCommandPatch_" + writePtr.ToString("X")));

            return writePtr;
        }

        private static int GetTop(IntPtr pLuaState)
        {
            return GetTopHandler(pLuaState);
        }

        private static string ToString(IntPtr pLuaState, int index)
        {
            return ToStringHandler(pLuaState, index + 1, 0);
        }

        internal static void Shutdown()
        {
            DoString("fluxEventFrame:SetScript(\"OnEvent\", nil)");
        }

        public static void Init()
        {
            //Logging.WriteDebug("Lua subsystem initialized");
        }

        #region Nested type: LuaDoString

        /// <summary>
        /// Calls WoW's internal FrameScript_Execute (Lua_DoString)
        /// </summary>
        /// <param name="lua">The actual Lua code to execute.</param>
        /// <param name="fileName">A file name to use in case of an error. (Will be shown as: x error (fileName) ln y)</param>
        /// <param name="pState">Pass 0. This isn't really used as FrameScript_Execute is just a wrapper around the actual lua_dostring
        /// You do NOT need to pass the actual Lua state. You can retrieve that from the function callbacks!</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void LuaDoString(string lua, string fileName, uint pState);

        #endregion

        #region Nested type: LuaGetTop

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int LuaGetTop(IntPtr pLuaState);

        #endregion

        #region Nested type: LuaRegisterCommand

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate uint LuaRegisterCommand(string szName, IntPtr pFunc);

        #endregion

        #region Nested type: LuaToString

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate string LuaToString(IntPtr pLuaState, int idx, int length);

        #endregion

        #region Nested type: LuaUnregisterFunction

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void LuaUnregisterFunction(string funcName);

        #endregion

        //private static readonly LuaUnregisterFunction UnregisterFunctionHandler =
        //    Utilities.RegisterDelegate<LuaUnregisterFunction>((uint) Luas.Lua_Unregister);
    }
}