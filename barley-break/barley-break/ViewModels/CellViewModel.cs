namespace barley_break.ViewModels
{
    public class CellViewModel : BaseViewModel
    {
        private int _x;

        public int X
        {
            get { return _x; }
            set { _x = value;  OnPropertyChanged(); }
        }

        private int _y;

        public int Y
        {
            get { return _y; }
            set { _y = value; OnPropertyChanged(); }
        }

        private int _val;

        public int Value
        {
            get { return _val; }
            set { _val = value; OnPropertyChanged(); }
        }

    }
}
