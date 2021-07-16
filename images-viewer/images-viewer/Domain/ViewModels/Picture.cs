namespace images_viewer.Domain.ViewModels
{
    public class Picture : GalleryObject
    {
        private int _rate;
        public int Rate
        {
            get { return _rate; }
            set { _rate = value; OnPropertyChanged(); }
        }
    }
}
