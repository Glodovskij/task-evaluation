using barley_break.Commands;
using System.Windows;

namespace barley_break.ViewModels
{
    public class BarleyBreakViewModelcs : BaseViewModel
    {   
        private CellViewModel[,] _gameField;

        public CellViewModel[,] GameField
        {
            get { return _gameField; }
            set { _gameField = value; OnPropertyChanged(); }
        }

        private RelayCommand _buttonPressedCommand;

        public RelayCommand ButtonPressedCommand    
        {
            get { return _buttonPressedCommand ??(_buttonPressedCommand = new RelayCommand(ButtonPressedCommandHandled)); }
        }

        private bool _isGameInProgress; 

        public bool IsGameInProgress
        {
            get { return _isGameInProgress; }
            set { _isGameInProgress = value; OnPropertyChanged(); }
        }

        private int _clickss;

        public int NumberOfClicks
        {
            get { return _clickss; }
            set { _clickss = value; OnPropertyChanged(); }
        }


        private RelayCommand _restartGame;

        public RelayCommand RestartGameCommand
        {
            get { return _restartGame ?? (_restartGame = new RelayCommand(RestartGameCommandHandler)); }
        }

        public BarleyBreakViewModelcs()
        {
            GameField = BarleyBreakFieldManager.Generate();
            IsGameInProgress = true;
        }

        private void RestartGameCommandHandler(object obj)
        {
            GameField = BarleyBreakFieldManager.Generate();
            IsGameInProgress = true;
            NumberOfClicks = 0;
        }

        private void ButtonPressedCommandHandled(object obj)
        {
            CellViewModel cell = obj as CellViewModel;

            if(cell.Y - 1 >= 0)
            {
                if(CellContainsZero(cell.Y - 1, cell.X))
                {
                    SwapValues(cell, GameField[cell.Y - 1, cell.X]);
                }
            }
            if (cell.Y + 1 <= 3)
            {
                if (CellContainsZero(cell.Y + 1, cell.X))
                {
                    SwapValues(cell, GameField[cell.Y + 1, cell.X]);
                }
            }

            if (cell.X - 1 >= 0)
            {
                if (CellContainsZero(cell.Y, cell.X - 1))
                {
                    SwapValues(cell, GameField[cell.Y, cell.X - 1]);
                }
            }
            if (cell.X + 1 <= 3)
            {
                if (CellContainsZero(cell.Y, cell.X + 1))
                {
                    SwapValues(cell, GameField[cell.Y, cell.X + 1]);
                }
            }
            if(BarleyBreakFieldManager.CheckForSolving(GameField))
            {
                IsGameInProgress = false;
                MessageBox.Show($"Congratulations. You finished 15-puzzle for {NumberOfClicks} taps.");
            }
        }

        private bool CellContainsZero(int y, int x)
        {
            if(GameField[y, x].Value == 0)
            {
                return true;
            }
            return false;
        }

        private void SwapValues(CellViewModel a, CellViewModel b)
        {
            NumberOfClicks++;
            int temp = a.Value;
            a.Value = b.Value;
            b.Value = temp;
        }
    }
}
