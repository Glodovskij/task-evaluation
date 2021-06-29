using process_manager.Marshaling;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Management.Automation;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace process_manager.ProcessFiles
{
    public class WrappedProcess : ProcessModel
    {
        public Process Process { get; private set; }
        private HardwareService HardwareService;

        public bool HasExited
        {
            get
            {
                try
                {
                    return Process.HasExited;
                }
                catch
                {
                    return false;
                }
            }
        }

        public WrappedProcess(Process process)
        {
            Process = process;
            ProcessName = Process.ProcessName;
            try
            {
                HardwareService = new HardwareService(process.Handle);
            }
            catch
            {
                HardwareService = null;
            }
            SetProcessIcon();

            RefreshProperties();
        }

        private void RefreshProperties()
        {
            try
            {
                WorkingSet = Math.Round(Process.WorkingSet64 / 1024.0 / 1024.0, 2);
                ThreadsCount = Process.Threads.Count;
                //GetGpuLoad();

                if (HardwareService != null)
                {
                    CpuUsage = HardwareService.GetCpuPerProcessUsage();
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        private void SetProcessIcon()
        {

            try
            {
                using (Icon ico = Icon.ExtractAssociatedIcon(Process.MainModule.FileName))
                {
                    ProcessIcon = Imaging.CreateBitmapSourceFromHIcon(
                        ico.Handle,
                        new Int32Rect(0, 0, ico.Width, ico.Height),
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }
            catch 
            {
                ProcessIcon = Imaging.CreateBitmapSourceFromHIcon(
                    Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule.FileName).Handle,
                    new Int32Rect(0, 0, 32, 32),
                    BitmapSizeOptions.FromEmptyOptions());
            }
        }

        private void GetGpuLoad()
        {
            using (PowerShell powerShell = PowerShell.Create())
            {
                string str = $"Get-Counter \"\\GPU Engine(pid_$({Process.Id})*engtype_3D)\\Utilization Percentage\" |" +
                   "foreach { Write-Output \"$([math]::Round($_,2))%\"}";


                powerShell.AddScript("Import-Module Get-Counter").Invoke();
                powerShell.AddCommand(str);

                var output = powerShell.Invoke();
            }

        }

        public void Refresh()
        {
            Process.Refresh();

            if (!HasExited)
                RefreshProperties();
        }
    }
}
