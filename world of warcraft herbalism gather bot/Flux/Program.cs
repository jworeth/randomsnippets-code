using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Flux.Utilities;
using Flux.WoW;

namespace Flux
{
    internal static class Program
    {
        private static bool _started;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (Environment.OSVersion.Version.Major == 6)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
            }
            Application.Run(new FormMain());

            Pulsator.PulseShutdown();
        }



        public static void OnFrame()
        {
            if (!_started)
            {
                _started = true;
                Pulsator.PulseStartup();
            }

            Pulsator.PulseFrame();
        }
    }
}