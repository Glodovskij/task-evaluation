using System;
using System.Runtime.InteropServices;

namespace process_manager.Marshaling
{
    public static class GeneralHardwareService
    {
        public static double GetMemoryUtilization()
        {
            Int64 phav = GetPhysicalAvailableMemoryInMiB();
            Int64 tot = GetTotalMemoryInMiB();
            double percentFree = ((double)phav / (double)tot) * 100;
            double percentOccupied = 100 - percentFree;

            return Math.Round(percentOccupied);
        }

        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPerformanceInfo([Out] out PerformanceInformation PerformanceInformation, [In] int Size);

        [StructLayout(LayoutKind.Sequential)]
        public struct PerformanceInformation
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }

        public static Int64 GetPhysicalAvailableMemoryInMiB()
        {
            PerformanceInformation perfInfo = new PerformanceInformation();
            if (GetPerformanceInfo(out perfInfo, Marshal.SizeOf(perfInfo)))
            {
                return Convert.ToInt64((perfInfo.PhysicalAvailable.ToInt64() * perfInfo.PageSize.ToInt64() / 1048576));
            }
            else
            {
                return -1;
            }

        }

        public static Int64 GetTotalMemoryInMiB()
        {
            PerformanceInformation perfInfo = new PerformanceInformation();
            if (GetPerformanceInfo(out perfInfo, Marshal.SizeOf(perfInfo)))
            {
                return Convert.ToInt64((perfInfo.PhysicalTotal.ToInt64() * perfInfo.PageSize.ToInt64() / 1048576));
            }
            else
            {
                return -1;
            }

        }

    }
}
