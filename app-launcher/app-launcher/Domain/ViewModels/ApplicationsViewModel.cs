using app_launcher.Domain.Commands;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace app_launcher.Domain
{
    public class ApplicationsViewModel : BaseViewModel
    {
        public ObservableCollection<ApplicationViewModel> AppModels { get; set; }

        private ICommand _launchAppCommand;

        public ICommand LaunchAppCommand
        {
            get
            {
                return _launchAppCommand ?? (_launchAppCommand = new RelayCommand(LaunchAppByDoubleClick));
            }
        }

        public ApplicationsViewModel()
        {
            AppModels = new ObservableCollection<ApplicationViewModel>();
            ExtractAppDataFromRegistry(RegistryHive.CurrentUser);
            ExtractAppDataFromRegistry(RegistryHive.LocalMachine);
            Task.Run(() => FillAppModelsWithExecutables());
        }

        private void ExtractAppDataFromRegistry(RegistryHive registryHive)
        {
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (Microsoft.Win32.RegistryKey key = RegistryKey.OpenBaseKey(registryHive, RegistryView.Registry64))
            using (RegistryKey queryKey = key.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in queryKey.GetSubKeyNames())
                {
                    using (RegistryKey subkey = queryKey.OpenSubKey(subkey_name))
                    {
                        try
                        {
                            if (subkey.GetValue("InstallLocation") != null && !string.IsNullOrEmpty(subkey.GetValue("InstallLocation").ToString()))
                            {
                                if(subkey.GetValue("InstallLocation").ToString().Contains("NVIDIA"))
                                {
                                    continue ;
                                }
                                AppModels.Add(new ApplicationViewModel
                                {
                                    DisplayImage = ExtractExecutableIcon(subkey.GetValue("DisplayIcon").ToString()),
                                    DisplayName = subkey.GetValue("DisplayName").ToString(),
                                    InstallLocation = subkey.GetValue("InstallLocation").ToString()

                                });
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
        private BitmapSource ExtractExecutableIcon(string iconPath)
        {
            try
            {
                using (Icon ico = Icon.ExtractAssociatedIcon(iconPath))
                {
                    return Imaging.CreateBitmapSourceFromHIcon(
                            ico.Handle,
                            new Int32Rect(0, 0, ico.Width, ico.Height),
                            BitmapSizeOptions.FromEmptyOptions());
                }
            }

            catch
            {
                return Imaging.CreateBitmapSourceFromHIcon(
                    Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule.FileName).Handle,
                    new Int32Rect(0, 0, 32, 32),
                    BitmapSizeOptions.FromEmptyOptions());
            }
        }

        private void FillAppModelsWithExecutables()
        {
            foreach (ApplicationViewModel appModel in AppModels)
            {
                SearchExecutablesInDirectory(appModel.InstallLocation, appModel);
            }
        }

        private void SearchExecutablesInDirectory(string targetDirectory, ApplicationViewModel appModel)
        {
            try

            {
                string[] fileEntries = Directory.GetFiles(targetDirectory, "*.exe");
                foreach (string fileName in fileEntries)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        appModel.ExecutableModels.Add(new ExecutableViewModel
                        {
                            ExecutableIcon = ExtractExecutableIcon(fileName),
                            ExecutableName = Path.GetFileName(fileName),
                            ExecutablePath = fileName
                        });
                    });
                }
            }
            catch
            {
                return;
            }
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                SearchExecutablesInDirectory(subdirectory, appModel);
            }
        }

        private void LaunchAppByDoubleClick(object obj)
        {
            var kek = obj as ExecutableViewModel;
            Process.Start(kek.ExecutablePath);
        }
    }
}
