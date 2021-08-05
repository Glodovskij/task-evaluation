namespace sudoku
{
    public class SudokuCellViewModel : BaseViewModel
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; 
                if(value == 0)
                {
                    IsEnabled = true;
                }
                else
                {
                    IsEnabled = false;
                }

                OnPropertyChanged(); }
        }

        public bool IsEnabled { get; set; }

        private int _x;

        public int X
        {
            get { return _x; }
            set { _x = value; OnPropertyChanged(); }
        }

        private int _y;

        public int Y
        {
            get { return _y; }
            set { _y = value; OnPropertyChanged(); }
        }


    }
}
