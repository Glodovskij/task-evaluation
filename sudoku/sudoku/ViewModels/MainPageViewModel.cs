namespace sudoku
{
    public class MainPageViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        private SudokuViewModel _currentSudokuGame;

        public SudokuViewModel CurrentSudokuGame
        {
            get { return _currentSudokuGame; }
            set { _currentSudokuGame = value; OnPropertyChanged(); }
        }

        public MainPageViewModel()
        {
            CurrentViewModel = new MenuViewModel();

            MenuViewModel.OnNewGameStart += NewGameStartEventHandler;
            MenuViewModel.OnContinueGame += OnContinueGameEventHandler;
            DifficultyLevelViewModel.DifficultyLevelPicked += DifficultyLevelPickedHandler;
            SudokuViewModel.BackToMenuButtonPressed += BackToMenuButtonPressedHandler;
        }

        private void BackToMenuButtonPressedHandler()
        {
            CurrentViewModel = new MenuViewModel();
        }

        private void OnContinueGameEventHandler()
        {
            if(CurrentSudokuGame != null && CurrentSudokuGame.IsGameInProgress)
            {
                CurrentViewModel = CurrentSudokuGame;
            }
            else
            {
                //MessageBox.Show();
            }
        }

        private void NewGameStartEventHandler()
        {
            CurrentViewModel = new DifficultyLevelViewModel();
        }

        private void DifficultyLevelPickedHandler(object obj)
        {
            CurrentSudokuGame = new SudokuViewModel(obj as SudokuCellViewModel[,]);
            CurrentViewModel = CurrentSudokuGame;
        }
    }
}
