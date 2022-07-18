using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

using Flux.Utilities;

namespace Flux.WoW.Native
{
    /// <summary>
    /// This is Shynd's FindPattern class. I just changed a single method to use Marshal instead
    /// of his whole memory lib. (We don't really need all of it!)
    /// </summary>
    public static class FindPatterns
    {
        /// <summary>
        /// Finds a pattern or signature inside another process.
        /// </summary>
        /// <param name="startAddress">Address on which the search will start.</param>
        /// <param name="size">Number of bytes in memory that will be searched.</param>
        /// <param name="pattern">A character-delimited string representing the pattern to be found.</param>
        /// <param name="mask">A string of 'x' (match), '!' (not-match), or '?' (wildcard).</param>
        /// <param name="delimiter">Determines how the string will be split.  If null, defaults to ' '.</param>
        /// <returns>Returns 0 on failure, or the address of the start of the pattern on success.</returns>
        public static uint Find(uint startAddress, int size, string pattern, string mask, params char[] delimiter)
        {
            if (delimiter == null)
            {
                delimiter = new[] {' '};
            }
            //Logging.WriteDebug("[FINDPATTERN] Pattern: " + pattern + ", Mask: " + mask);
            string[] patternSplit = pattern.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            //Logging.WriteDebug("[FINDPATTERN] Split pattern into " + patternSplit.Length + " chunks.");
            var patternBytes = new byte[patternSplit.Length];

            for (int i = 0; i < patternSplit.Length; i++)
            {
                patternBytes[i] = Convert.ToByte(patternSplit[i], 0x10);
            }
            //Logging.WriteDebug("[FINDPATTERN] Pattern bytes: " + patternBytes.Length);
            return Find(startAddress, size, patternBytes, mask);
        }

        /// <summary>
        /// Finds a pattern or signature inside another process.
        /// </summary>
        /// <param name="startAddress">Address on which the search will start.</param>
        /// <param name="size">Number of bytes in memory that will be searched.</param>
        /// <param name="patternBytes">A byte-array representing the pattern to be found.</param>
        /// <param name="mask">A string of 'x' (match), '!' (not-match), or '?' (wildcard).</param>
        /// <returns>Returns 0 on failure, or the address of the start of the pattern on success.</returns>
        public static uint Find(uint startAddress, int size, byte[] patternBytes, string mask)
        {
            if (patternBytes == null || patternBytes.Length == 0)
            {
                throw new ArgumentNullException("patternBytes");
            }

            if (patternBytes.Length != mask.Length)
            {
                throw new ArgumentException("data and mask must be of the same size");
            }

            var data = new byte[size];

            //Win32.ReadBytes((IntPtr) startAddress, size);
            for (int i = 0; i < size; i++)
            {
                data[i] = Marshal.ReadByte(new IntPtr(startAddress), i);
            }

            //Logging.WriteDebug("Found data!");

            if (data == null)
            {
                throw new Exception("Could not read memory in FindPattern.");
            }

            return ((startAddress + Find(data, patternBytes, mask)));
        }

        /// <summary>
        /// Finds a given pattern in an array of bytes.
        /// </summary>
        /// <param name="data">Array of bytes in which to search for the pattern.</param>
        /// <param name="patternBytes">A byte-array representing the pattern to be found.</param>
        /// <param name="mask">A string of 'x' (match), '!' (not-match), or '?' (wildcard).</param>
        /// <returns>Returns 0 on failure, or the address of the start of the pattern on success.</returns>
        public static uint Find(byte[] data, byte[] patternBytes, string mask)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException("data");
            }

            if (patternBytes == null || patternBytes.Length == 0)
            {
                throw new ArgumentNullException("patternBytes");
            }

            if (mask == string.Empty)
            {
                throw new ArgumentNullException("mask");
            }

            if (patternBytes.Length != mask.Length)
            {
                throw new ArgumentException("Pattern and Mask lengths must be the same.");
            }

            int patternLength = patternBytes.Length;
            int dataLength = data.Length - patternLength;

            for (int i = 0; i < dataLength; i++)
            {
                bool found = true;
                for (int j = 0; j < patternLength; j++)
                {
                    if ((mask[j] == 'x' && patternBytes[j] != data[i + j]) ||
                        (mask[j] == '!' && patternBytes[j] == data[i + j]))
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return (uint) i;
                }
            }

            return 0;
        }

        /// <summary>
        /// Finds a pattern or signature inside another process. This will find all occurrences of the pattern.
        /// </summary>
        /// <param name="startAddress">Address on which the search will start.</param>
        /// <param name="size">Number of bytes in memory that will be searched.</param>
        /// <param name="pattern">A character-delimited string representing the pattern to be found.</param>
        /// <param name="mask">A string of 'x' (match), '!' (not-match), or '?' (wildcard).</param>
        /// <param name="delimiter">Determines how the string will be split.  If null, defaults to ' '.</param>
        /// <returns>Returns 0 on failure, or the address of the start of the pattern on success.</returns>
        public static uint[] FindAll(uint startAddress, int size, string pattern, string mask, params char[] delimiter)
        {
            if (delimiter == null)
            {
                delimiter = new[] {' '};
            }
            string[] patternSplit = pattern.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            var patternBytes = new byte[patternSplit.Length];

            for (int i = 0; i < patternSplit.Length; i++)
            {
                patternBytes[i] = Convert.ToByte(patternSplit[i], 0x10);
            }
            return FindAll(startAddress, size, patternBytes, mask);
        }

        /// <summary>
        /// Finds a pattern or signature inside another process. This will find all occurrences of the pattern.
        /// </summary>
        /// <param name="startAddress">Address on which the search will start.</param>
        /// <param name="size">Number of bytes in memory that will be searched.</param>
        /// <param name="patternBytes">A byte-array representing the pattern to be found.</param>
        /// <param name="mask">A string of 'x' (match), '!' (not-match), or '?' (wildcard).</param>
        /// <returns>Returns 0 on failure, or the address of the start of the pattern on success.</returns>
        public static uint[] FindAll(uint startAddress, int size, byte[] patternBytes, string mask)
        {
            if (patternBytes == null || patternBytes.Length == 0)
            {
                throw new ArgumentNullException("patternBytes");
            }

            if (patternBytes.Length != mask.Length)
            {
                throw new ArgumentException("data and mask must be of the same size");
            }

            var data = new byte[size];

            //Win32.ReadBytes((IntPtr) startAddress, size);
            for (int i = 0; i < size; i++)
            {
                data[i] = Marshal.ReadByte(new IntPtr(startAddress), i);
            }

            //Logging.WriteDebug("Found data!");

            if (data == null)
            {
                throw new Exception("Could not read memory in FindPattern.");
            }

            //            return ((startAddress + FindAll(data, patternBytes, mask)));
            uint[] ret = FindAll(data, patternBytes, mask);
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] += startAddress;
            }
            return ret;
        }

        public static uint[] FindAll(byte[] data, byte[] patternBytes, string mask)
        {
            var ret = new List<uint>();
            for (int i = 0; i < data.Length - patternBytes.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < patternBytes.Length; j++)
                {
                    if ((mask[j] == 'x' && patternBytes[j] != data[i + j]) || (mask[j] == '!' && patternBytes[j] == data[i + j]))
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    ret.Add((uint) i);
                }
            }

            return ret.ToArray();
        }
    }

    /// <summary>
    /// Credits to Dominik, Patrick, Bobbysing, and whoever else I forgot, for most of the ripped code here!
    /// </summary>
    public class FindPattern
    {
        private readonly Dictionary<string, uint> _patterns = new Dictionary<string, uint>();

        /// <summary>
        /// Creates a new instance of the <see cref="FindPattern"/> class. This class will read from a specified patterns XML file
        /// and search out those patterns in the specified process's memory.
        /// </summary>
        /// <param name="patternFile">The full path to the pattern XML file.</param>
        /// <param name="processHandle">An open process handle to the process to read memory from.</param>
        /// <param name="startAddr">The 'base' address of the process (or module)</param>
        /// <param name="endAddr">The 'end' of the process (or module). Eg; where to stop reading memory from.</param>
        public FindPattern(string patternFile, uint startAddr, uint endAddr)
        {
            // Get a temporary set of data to work with. :)
            byte[] data = Win32.ReadBytes((IntPtr) startAddr, (int) (endAddr - startAddr));
            LoadFile(XElement.Load(patternFile), data, startAddr);
        }

        /// <summary>
        /// Retrieves an address from the found patterns stash.
        /// </summary>
        /// <param name="name">The name of the pattern, as per the XML file provided in the constructor of this class instance.</param>
        /// <returns></returns>
        public uint this[string name] { get { return Get(name); } }

        /// <summary>
        /// Retrieves an address from the found patterns stash.
        /// </summary>
        /// <param name="name">The name of the pattern, as per the XML file provided in the constructor of this class instance.</param>
        /// <returns></returns>
        public uint Get(string name)
        {
            return _patterns[name];
        }

        private void LoadFile(XContainer file, byte[] data, uint start)
        {
            // Grab all the <Pattern /> elements from the XML.
            IEnumerable<XElement> pats = from p in file.Descendants("Pattern")
                                         select p;

            // Each Pattern element needs to be handled seperately.
            // The enumeration we're goinv over, is in document order, so attributes such as 'start'
            // should work perfectly fine.
            foreach (XElement pat in pats)
            {
                uint tmpStart = 0;

                string name = pat.Attribute("desc").Value;
                string mask = pat.Attribute("mask").Value;
                byte[] patternBytes = GetBytesFromPattern(pat.Attribute("pattern").Value);

                // Make sure we're not getting some sort of screwy XML data.
                if (mask.Length != patternBytes.Length)
                {
                    throw new Exception("Pattern and mask lengths do not match!");
                }

                // If we run into a 'start' attribute, we need to remember that we're working from a 0
                // based 'memory pool'. So we just remove the 'start' from the address we found earlier.
                if (pat.Attribute("start") != null)
                {
                    tmpStart = Get(pat.Attribute("start").Value) - start + 1;
                }

                // Actually search for the pattern match...
                uint found = Find(data, mask, patternBytes, tmpStart);

                if (found == 0)
                {
                    throw new Exception("FindPattern failed... figure it out ****tard!");
                }

                // Handle specific child elements for the pattern.
                // <Lea> <Rel> <Add> <Sub> etc
                foreach (XElement e in pat.Elements())
                {
                    switch (e.Name.LocalName)
                    {
                        case "Lea":
                            found = BitConverter.ToUInt32(data, (int) found);
                            break;
                        case "Rel":
                            int instructionSize = int.Parse(e.Attribute("size").Value, NumberStyles.HexNumber);
                            int operandOffset = int.Parse(e.Attribute("offset").Value, NumberStyles.HexNumber);
                            found = (uint) (BitConverter.ToUInt32(data, (int) found) + found + instructionSize - operandOffset);
                            break;
                        case "Add":
                            found += uint.Parse(e.Attribute("value").Value, NumberStyles.HexNumber);
                            break;
                        case "Sub":
                            found -= uint.Parse(e.Attribute("value").Value, NumberStyles.HexNumber);
                            break;
                    }
                }

                _patterns.Add(name, found + start);
                Logging.WriteDebug("[FINDPATTERN] " + name + " = 0x" + (found + start).ToString("X8"));
            }
        }

        private static byte[] GetBytesFromPattern(string pattern)
        {
            // Because I'm lazy, and this just makes life easier.
            string[] split = pattern.Split(new[] {'\\', 'x'}, StringSplitOptions.RemoveEmptyEntries);
            var ret = new byte[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                ret[i] = byte.Parse(split[i], NumberStyles.HexNumber);
            }
            return ret;
        }

        private static uint Find(byte[] data, string mask, byte[] byteMask, uint start)
        {
            // There *has* to be a better way to do this stuff,
            // but for now, we'll deal with it.
            for (uint i = start; i < data.Length; i++)
            {
                if (DataCompare(data, (int) i, byteMask, mask))
                {
                    return i;
                }
            }
            return 0;
        }

        private static bool DataCompare(byte[] data, int offset, byte[] byteMask, string mask)
        {
            // Only check for 'x' mismatches. As we'll assume anything else is a wildcard.
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] == 'x' && byteMask[i] != data[i + offset])
                {
                    return false;
                }
            }
            return true;
        }
    }
}