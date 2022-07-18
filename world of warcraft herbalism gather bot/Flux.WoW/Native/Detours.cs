using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Flux.Utilities;

namespace Flux.WoW.Native
{
    public static class Detours
    {
        private static readonly Dictionary<string, Detour> _detours = new Dictionary<string, Detour>();

        /// <summary>
        /// Adds a Detour to the _detours list
        /// </summary>
        /// <param name="target">The address of your target function</param>
        /// <param name="hook">The address of the function you want the target to be detoured to</param>
        /// <param name="name">The name for your detour</param>
        /// <returns>A Detour</returns>
        public static Detour Add(Delegate target, Delegate hook, string name)
        {
            _detours.Add(name, new Detour(target, hook, name));
            return _detours[name];
        }

        /// <summary>
        /// Deletes a Detour from _detours 
        /// </summary>
        /// <param name="name">Name of the detour you want to delete</param>
        public static void Delete(string name)
        {
            _detours.Remove(name);
        }

        public static void DeleteAll()
        {
            RemoveAll();
            _detours.Clear();
        }

        /// <summary>
        /// Applys all of your detours
        /// </summary>
        public static void ApplyAll()
        {
            foreach (var pair in _detours)
            {
                pair.Value.Apply();
            }
        }

        /// <summary>
        /// Removes all of your detours
        /// </summary>
        public static void RemoveAll()
        {
            foreach (var pair in _detours)
            {
                pair.Value.Remove();
            }
        }

        /// <summary>
        /// Gets a specific detour
        /// </summary>
        /// <param name="name">Name of your Detour</param>
        /// <returns>A Detour</returns>
        public static Detour Get(string name)
        {
            return _detours[name];
        }

        #region Nested type: Detour

        public class Detour
        {
            private readonly IntPtr _hook;
            private readonly Delegate _hookDelegate;
            private readonly List<byte> _new;
            private readonly List<byte> _orginal;
            private readonly IntPtr _target;
            private readonly Delegate _targetDelegate;

            public Detour(Delegate target, Delegate hook, string name)
            {
                Name = name;
                _targetDelegate = target;
                _target = Marshal.GetFunctionPointerForDelegate(target);
                _hookDelegate = hook;
                _hook = Marshal.GetFunctionPointerForDelegate(hook);

                //Store the orginal bytes
                _orginal = new List<byte>();
                _orginal.AddRange(Win32.ReadBytes(_target, 6));

                //Setup the detour bytes
                _new = new List<byte> {0x68};
                byte[] tmp = BitConverter.GetBytes(_hook.ToUInt32());
                _new.AddRange(tmp);
                _new.Add(0xC3);
            }

            public string Name { get; private set; }

            /// <summary>
            /// Apply the detour
            /// </summary>
            public void Apply()
            {
                //Logging.WriteDebug("[DETOUR] Applying: " + Name + " at 0x"+_target.ToString("X8"));
                Win32.WriteBytes(_target, _new.ToArray());
            }

            /// <summary>
            /// Remove the detour
            /// </summary>
            public void Remove()
            {
                //Logging.WriteDebug("[DETOUR] Removing: " + Name + " at 0x" + _target.ToString("X8"));
                Win32.WriteBytes(_target, _orginal.ToArray());
            }

            /// <summary>
            /// Calls the original function, and returns a return value.
            /// </summary>
            /// <param name="args">The arguments to pass. If it is a 'void' argument list,
            /// you MUST pass 'null'.</param>
            /// <returns>An object containing the original functions return value.</returns>
            public object CallOriginal(params object[] args)
            {
                Remove();
                object ret = _targetDelegate.DynamicInvoke(args);
                Apply();
                return ret;
            }
        }

        #endregion
    }
}