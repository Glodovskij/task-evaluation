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
    public class ApplicationViewModel : BaseModel
    {
        public ObservableCollection<ApplicationModel> AppModels { get; set; }

        private ICommand _launchAppCommand;

        public ICommand LaunchAppCommand
        {
            get
            {
                return _launchAppCommand ?? (_launchAppCommand = new RelayCommand(LaunchAppByDoubleClick));
            }
        }

        public ApplicationViewModel()
        {
            AppModels = new ObservableCollection<ApplicationModel>();
            ExtractAppDataFromRegistry();
            Task.Run(() => FillAppModelsWithExecutables());
        }

        private void ExtractAppDataFromRegistry()
        {
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        if (subkey.GetValue("InstallLocation") == null ||
                            subkey.GetValue("DisplayIcon") == null ||
                            subkey.GetValue("DisplayName") == null)
                        {
                            continue;
                        }

                        try
                        {
                            AppModels.Add(new ApplicationModel
                            {
                                DisplayImage = ExtractExecutableIcon(subkey.GetValue("DisplayIcon").ToString()),
                                DisplayName = subkey.GetValue("DisplayName").ToString(),
                                InstallLocation = subkey.GetValue("InstallLocation").ToString()

                            });
                        }
                        catch(FileNotFoundException)
                        {
                            //error while parsing icon
                        }
                        catch(ArgumentException)
                        {
                            //not happened yet
                        }
                    }
                }
            }
        }
        private BitmapSource ExtractExecutableIcon(string iconPath)
        {
            using (Icon ico = Icon.ExtractAssociatedIcon(iconPath))
            {
                return Imaging.CreateBitmapSourceFromHIcon(
                        ico.Handle,
                        new Int32Rect(0, 0, ico.Width, ico.Height),
                        BitmapSizeOptions.FromEmptyOptions());
            }
        }

        private void FillAppModelsWithExecutables()
        {
            foreach (ApplicationModel appModel in AppModels)
            {
                SearchExecutablesInDirectory(appModel.InstallLocation, appModel);
            }
        }

        private void SearchExecutablesInDirectory(string targetDirectory, ApplicationModel appModel)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory, "*.exe");
            foreach (string fileName in fileEntries)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    appModel.ExecutableModels.Add(new ExecutableModel
                    {
                        ExecutableIcon = ExtractExecutableIcon(fileName),
                        ExecutableName = Path.GetFileName(fileName),
                        ExecutablePath = fileName
                    });
                });
            }
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                SearchExecutablesInDirectory(subdirectory, appModel);
            }
        }

        private void LaunchAppByDoubleClick(object obj)
        {
            var kek = obj as ExecutableModel;
            Process.Start(kek.ExecutablePath);
        }
    }
}
