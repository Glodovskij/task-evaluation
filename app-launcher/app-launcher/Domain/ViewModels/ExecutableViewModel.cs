using System.Windows.Media;

namespace app_launcher.Domain
{
    public class ExecutableViewModel : BaseViewModel
    {
        private string _name;

        public string ExecutableName
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private ImageSource _icon;

        public ImageSource ExecutableIcon
        {
            get { return _icon; }
            set { _icon = value; OnPropertyChanged(); }
        }

        private string _path;

        public string ExecutablePath
        {
            get { return _path; }
            set { _path = value; OnPropertyChanged(); }
        }

    }
}
