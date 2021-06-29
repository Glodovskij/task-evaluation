using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace process_manager.Marshaling
{
    public class HardwareService
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetProcessTimes(
            IntPtr hProcess,
            out FILETIME lpCreationTime,
            out FILETIME lpExitTime,
            out FILETIME lpKernelTime,
            out FILETIME lpUserTime);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void GetSystemTimeAsFileTime(out FILETIME ftime);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetSystemTimes(
            out FILETIME lpIdleTime,
            out FILETIME lpKernelTime,
            out FILETIME lpUserTime
            );

        uint numProcessors;
        ulong lastSysCPU, lastUserCPU, lastCPU;
        private IntPtr _processHandler;

        public HardwareService(IntPtr processHandler)
        {
            _processHandler = processHandler;
            numProcessors = (uint)Environment.ProcessorCount;


            GetSystemTimeAsFileTime(out FILETIME ftime);
            lastSysCPU = (ulong)ftime.dwLowDateTime;

            GetProcessTimes(_processHandler, out ftime, out ftime, out FILETIME fsys, out FILETIME fuser);
            lastSysCPU = (ulong)fsys.dwLowDateTime;
            lastUserCPU = (ulong)fuser.dwLowDateTime;
        }

        public double GetCpuPerProcessUsage()
        {
            ulong now, sys, user;
            double percent;

            GetSystemTimeAsFileTime(out FILETIME ftime);
            now = (ulong)ftime.dwLowDateTime;

            GetProcessTimes(_processHandler, out ftime, out ftime, out FILETIME fsys, out FILETIME fuser);
            sys = (ulong)fsys.dwLowDateTime;
            user = (ulong)fuser.dwLowDateTime;

            percent = (sys - lastSysCPU) +
                (user - lastUserCPU);
            percent /= (now - lastCPU);
            percent /= numProcessors;
            lastCPU = now;
            lastUserCPU = user;
            lastSysCPU = sys;

            return Math.Round(percent * 100, 2);
        }

    }
}
