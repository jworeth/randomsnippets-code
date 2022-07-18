using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Flux.Utilities;

namespace Flux.WoW.Native
{
    public static class Patcher
    {
        private static readonly Dictionary<string, List<Patch>> Patches = new Dictionary<string, List<Patch>>();

        public static void ApplyPatch(string tag)
        {
            foreach (var pair in Patches)
            {
                if (tag == null || pair.Key == tag)
                {
                    foreach (Patch patch in pair.Value)
                    {
                        patch.Apply();
                    }
                }
            }
        }

        public static void RemovePatch(string tag)
        {
            foreach (var pair in Patches)
            {
                if (tag == null || pair.Key == tag)
                {
                    foreach (Patch patch in pair.Value)
                    {
                        patch.Remove();
                    }
                }
            }
        }

        public static void RemoveAll()
        {
            foreach (var pair in Patches)
            {
                foreach (Patch patch in pair.Value)
                {
                    patch.Remove();
                }
            }
        }

        public static void ApplyAll()
        {
            foreach (var pair in Patches)
            {
                foreach (Patch patch in pair.Value)
                {
                    patch.Apply();
                }
            }
        }

        public static void DeleteAll()
        {
            RemoveAll();
            Patches.Clear();
        }

        public static void CreatePatch(Patch patch)
        {
            if (!Patches.ContainsKey(patch.Tag))
            {
                Patches.Add(patch.Tag, new List<Patch>());
                Patches[patch.Tag].Add(patch);
            }
            patch.Apply();
        }

        public static void RemovePatch(Patch patch)
        {
            if (Patches.ContainsKey(patch.Tag))
            {
                patch.Remove();
                Patches[patch.Tag].Remove(patch);
            }
        }

        #region Nested type: Patch

        public class Patch
        {
            public Patch(IntPtr address, byte[] patchWith, string tag)
            {
                // Make sure Address is BEFORE GetOriginalData
                Address = address;
                PatchedData = patchWith;
                OriginalData = GetOriginalData(patchWith.Length);
                Tag = tag;
            }

            public byte[] OriginalData { get; set; }
            public byte[] PatchedData { get; set; }
            public IntPtr Address { get; set; }
            public string Tag { get; set; }
            public int Length { get { return PatchedData.Length; } }
            public bool Patched { get; private set; }

            private byte[] GetOriginalData(int length)
            {
                var data = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    data[i] = Marshal.ReadByte(Address, i);
                }
                return data;
            }

            public void Apply()
            {
                if (!Patched)
                {
                    //Logging.WriteDebug("Applying patch: " + Tag + " at 0x" + Address.ToString("X8"));
                    Win32.WriteBytes(Address, PatchedData);
                    Patched = true;
                }
            }

            public void Remove()
            {
                if (Patched)
                {
                    //Logging.WriteDebug("Removeing patch: " + Tag + " at 0x" + Address.ToString("X8"));
                    Win32.WriteBytes(Address, OriginalData);
                    Patched = false;
                }
            }
        }

        #endregion
    }
}