using System.Windows.Media;

namespace process_manager.ProcessFiles
{
    public class ProcessModel : BaseModel
    {
        private float _gpuUsage;

        public float GpuUsage
        {
            get
            {
                return _gpuUsage;
            }
            set
            {
                _gpuUsage = value;
                OnPropertyChanged();
            }
        }


        private double _cpuUsage;

        public double CpuUsage
        {
            get
            {
                return _cpuUsage;
            }
            set
            {
                _cpuUsage = value;
                OnPropertyChanged();
            }
        }



        private ImageSource _imgSource;

        public ImageSource ProcessIcon
        {
            get
            {
                return _imgSource;
            }
            set
            {
                _imgSource = value;
                OnPropertyChanged();
            }
        }


        private string _name;
        public string ProcessName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private double _ram;
        public double WorkingSet
        {
            get
            {
                return _ram;
            }
            set
            {
                _ram = value;
                OnPropertyChanged();
            }
        }

        private int _threadsCount;
        public int ThreadsCount
        {
            get
            {
                return _threadsCount;
            }
            set
            {
                _threadsCount = value;
                OnPropertyChanged();
            }
        }
    }
}
