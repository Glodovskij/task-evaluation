using System;
using System.Windows;

namespace sudoku
{
    public class MenuViewModel : BaseViewModel
    {
        private RelayCommand _newGameCommand;

        public RelayCommand NewGameCommand
        {
            get { return _newGameCommand ?? (_newGameCommand = new RelayCommand(NewGameCommandHandler)); }
        }

        private RelayCommand _quitCommand;

        public RelayCommand QuitCommand
        {
            get { return _quitCommand ?? (_quitCommand = new RelayCommand(QuitCommandHandler)); }
        }

        private RelayCommand _continueCommand;

        public RelayCommand ContinueCommand
        {
            get { return _continueCommand ??(_continueCommand = new RelayCommand(ContinueGameHandler)); }
        }

        public static Action OnNewGameStart;
        public static Action OnContinueGame;

        private void NewGameCommandHandler(object obj)
        {
            OnNewGameStart.Invoke();
        }

        private void QuitCommandHandler(object obj)
        {
            if(MessageBox.Show("Are you sure?", "Quit", button: MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        private void ContinueGameHandler(object obj)
        {
            OnContinueGame.Invoke();
        }

    }
}
