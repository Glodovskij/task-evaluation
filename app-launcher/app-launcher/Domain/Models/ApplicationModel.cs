using System.Collections.ObjectModel;
using System.Windows.Media;

namespace app_launcher.Domain
{
    public class ApplicationModel : BaseModel
    {
        private string _appPath;

        public string InstallLocation
        {
            get { return _appPath; }
            set { _appPath = value; }
        }

        private string _appName;

        public string DisplayName
        {
            get { return _appName; }
            set { _appName = value; OnPropertyChanged(); }
        }

        private ImageSource _img;

        public ImageSource DisplayImage
        {
            get { return _img; }
            set { _img = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ExecutableModel> ExecutableModels { get; set; }

        public ApplicationModel()
        {
            ExecutableModels = new ObservableCollection<ExecutableModel>();
        }
    }
}
