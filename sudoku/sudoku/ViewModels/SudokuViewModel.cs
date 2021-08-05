using System;
using System.Windows;
using System.Windows.Controls;

namespace sudoku
{
    public class SudokuViewModel : BaseViewModel
    {
        private SudokuCellViewModel[,] sudokuField;

        public SudokuCellViewModel[,] SudokuField
        {
            get { return sudokuField; }
            set { sudokuField = value; OnPropertyChanged(); }
        }

        private bool _isFinished;

        public bool IsGameInProgress
        {
            get { return _isFinished; }
            set { _isFinished = value; OnPropertyChanged(); }
        }

        public SudokuViewModel(SudokuCellViewModel[,] field)
        {
            SudokuField = field;
            IsGameInProgress = true;
        }

        private RelayCommand _contextButtonClickedCommand;

        public RelayCommand ContextButtonClickedCommand
        {
            get { return _contextButtonClickedCommand ?? (_contextButtonClickedCommand = new RelayCommand(ContextButtonClickedCommandHandler)); }
        }

        private RelayCommand _backToMenuCommand;

        public RelayCommand BackToMenuCommand
        {
            get { return _backToMenuCommand ?? (_backToMenuCommand = new RelayCommand(BackToMenuCommandHandler)); }
        }

        private RelayCommand _checkForSolvingCommand;

        public RelayCommand CheckForSolvingCommand
        {
            get { return _checkForSolvingCommand ?? (_checkForSolvingCommand = new RelayCommand(CheckForSolvingCommandHandler)); }
        }

        public static Action BackToMenuButtonPressed;

        private void ContextButtonClickedCommandHandler(object obj)
        {
            object[] commandParam = obj as object[];
            SudokuCellViewModel cell = ((commandParam[1] as ContextMenu).PlacementTarget as Button).DataContext as SudokuCellViewModel;

            cell.Value = int.Parse(commandParam[0].ToString());

            (commandParam[1] as ContextMenu).IsOpen = false;
        }

        private void CheckForSolvingCommandHandler(object obj)
        {
            if(SudokuFieldManager.CheckForSolution(SudokuField))
            {
                MessageBox.Show("Congratulations, you've completed sudoku");
                IsGameInProgress = false;
                BackToMenuButtonPressed.Invoke();
            }    
            else
            {
                MessageBox.Show("Nope. Try Again!");
            }
        }

        private void BackToMenuCommandHandler(object obj)
        {
            BackToMenuButtonPressed.Invoke();
        }
    }
}
