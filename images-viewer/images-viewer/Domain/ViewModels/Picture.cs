namespace images_viewer.Domain.ViewModels
{
    public class Picture : Base
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public string FolderPath { get; set; }

        private int _rate;
        public int Rate
        {
            get { return _rate; }
            set { _rate = value; OnPropertyChanged(); }
        }
    }
}
