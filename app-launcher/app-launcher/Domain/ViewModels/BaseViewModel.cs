using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace app_launcher.Domain
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            if(!string.IsNullOrEmpty(propName))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
