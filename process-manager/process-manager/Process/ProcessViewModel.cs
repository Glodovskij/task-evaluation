using NvAPIWrapper.GPU;
using process_manager.Commands;
using process_manager.Marshaling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace process_manager.ProcessFiles
{
    public class ProcessViewModel : BaseModel
    {
        public ObservableCollection<WrappedProcess> Processes { get; set; }

        public WrappedProcess SelectedWrappedProcess { get; set; }

        private object _collectionLock = new object();
        private Timer _updateProcessesTimer;
        private ManagementEventWatcher _newProcessWatcher;


        private double _memoryUtilization;

        public double MemoryUtilization
        {
            get
            {
                return _memoryUtilization;
            }
            set
            {
                _memoryUtilization = value;
                OnPropertyChanged();
            }
        }

        private ulong _cpuUtilization;

        public ulong CpuUtilization
        {
            get
            {
                return _cpuUtilization;
            }
            set
            {
                _cpuUtilization = value;
                OnPropertyChanged();
            }
        }

        private double _gpuUtilization;

        public double GpuUtilization
        {
            get
            {
                return _gpuUtilization;
            }
            set
            {
                _gpuUtilization = value;
                OnPropertyChanged();
            }
        }



        public RelayCommand KillProcessCommand
        {
            get
            {
                return new RelayCommand(() => KillProcessAndRelatedProcesses());
            }
        }

        public ProcessViewModel()
        {
            Processes = new ObservableCollection<WrappedProcess>();

            foreach (Process process in Process.GetProcesses())
            {
                Processes.Add(new WrappedProcess(process));
            }
            Task.Run(() =>
            {
                _newProcessWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
                _newProcessWatcher.EventArrived += StartWatch_EventArrived;
                _newProcessWatcher.Start();
                var thre = Thread.CurrentThread.ManagedThreadId;
            });

            _updateProcessesTimer = new Timer(new TimerCallback(UpdateProcessColection), null, 0, 1000);
            var thre = Thread.CurrentThread.ManagedThreadId;
        }

        private void StartWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string[] processName = e.NewEvent.Properties["ProcessName"].Value.ToString().Split('.');

            List<Process> processes = Process.GetProcessesByName(processName[0]).ToList();
            lock (_collectionLock)
            {
                processes.ForEach((process) =>
                {

                    if (!Processes.ContatinsProcess(process))
                    {
                        App.Current.Dispatcher.Invoke(() => Processes.Add(new WrappedProcess(process)));
                    }

                });
                var thre = Thread.CurrentThread.ManagedThreadId;
            }
        }

        private void UpdateProcessColection(object obj)
        {
            lock (_collectionLock)
            {
                RefreshResourceUtilization();

                for (int i = Processes.Count - 1; i >= 0; i--)
                {
                    Processes[i].Refresh();
                    if(Processes[i].HasExited)
                    {
                        App.Current.Dispatcher.Invoke(() => Processes.RemoveAt(i));
                    }
                }
            }
        }

        private void KillProcessAndRelatedProcesses()
        {
            try
            {
                SelectedWrappedProcess.Process.Kill(true);
            }
            catch
            {

            }
        }

        private void RefreshResourceUtilization()
        {
            ulong cpuUtil = 0;
            double gpuUtil = 0, memUtil = 0;
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                cpuUtil = Convert.ToUInt64(queryObj["PercentProcessorTime"]);
            }

            gpuUtil = PhysicalGPU.GetPhysicalGPUs().First().UsageInformation.GPU.Percentage;
            memUtil = MemoryUtilization = GeneralHardwareService.GetMemoryUtilization();

            App.Current.Dispatcher.Invoke(() =>
            {
                CpuUtilization = cpuUtil;
                GpuUtilization = gpuUtil;
                MemoryUtilization = memUtil;
            });
        }

    }
}
