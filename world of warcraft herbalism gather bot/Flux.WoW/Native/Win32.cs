using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Flux.WoW.Native
{
    public class Win32
    {
        #region ProcessAccessFlags enum

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        #endregion

        private static IntPtr _openProc = IntPtr.Zero;

        public static IntPtr ProcessHandle
        {
            get
            {
                if (_openProc == IntPtr.Zero)
                {
                    _openProc = MemoryOpen(Process.GetProcessesByName("Wow")[0].Id, ProcessAccessFlags.All);
                }
                return _openProc;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer,
                                                     int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize,
                                                      out int lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        public static extern int LoadLibrary(string librayName);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        public static extern int GetProcAddress(int hwnd, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceCounter(ref long lpPerformanceCount);

        [DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceFrequency(ref long lpFrequency);

        [DllImport("user32.dll")]
        public static extern int GetCursorPos(ref System.Drawing.Point pt);

        [DllImport("user32.dll")]
        public static extern int GetPhysicalCursorPos(ref System.Drawing.Point pt);

        public static int WriteByte(IntPtr address, byte val)
        {
            return WriteBytes(address, new[] {val});
        }

        public static int WriteBytes(IntPtr address, byte[] val)
        {
            int written;
            IntPtr handle = ProcessHandle;
            if (handle == IntPtr.Zero)
            {
                throw new Exception("Couldn't open the god damned process you jew!");
            }
            if (WriteProcessMemory(ProcessHandle, address, val, (uint) val.Length, out written))
            {
                return written;
            }
            throw new AccessViolationException("Could not write the specified bytes! " + address.ToString("X8") + " [" +
                                               Marshal.GetLastWin32Error() + "]");
        }

        public static byte[] ReadBytes(IntPtr address, int count)
        {
            var ret = new byte[count];
            int numRead;
            if (ReadProcessMemory(ProcessHandle, address, ret, count, out numRead) && numRead == count)
            {
                return ret;
            }
            return null;
        }

        public static void MemoryOpen()
        {
            _openProc = MemoryOpen(Process.GetCurrentProcess().Id,
                                   ProcessAccessFlags.VMWrite | ProcessAccessFlags.VMRead | ProcessAccessFlags.CreateThread |
                                   ProcessAccessFlags.DupHandle | ProcessAccessFlags.VMOperation | ProcessAccessFlags.Terminate);
        }

        public static IntPtr MemoryOpen(int processID, ProcessAccessFlags desiredAccess)
        {
            Process myProc = Process.GetProcessById(processID);
            if (myProc.HandleCount > 0)
            {
                return OpenProcess((uint) desiredAccess, true, processID);
            }
            return IntPtr.Zero;
        }
    }
}