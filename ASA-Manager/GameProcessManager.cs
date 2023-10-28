using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ARKServerCreationTool
{
    internal static class GameProcessManager
    {
        private static Process gameProcess = new Process();

        public static bool IsRunning
        {
            get
            {
                return false;
            }
        }

        public static void Start()
        {
            Thread.Sleep(100);

        }

        public static void Stop()
        {
            Thread.Sleep(100);
        }
    }
}
