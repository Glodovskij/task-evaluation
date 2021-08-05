using System;

namespace sudoku
{
    public class DifficultyLevelViewModel : BaseViewModel
    {
        public DifficultyLevelViewModel()
        {
            EasyLevel = SudokuFieldManager.Generate(SudokuDifficulty.Easy);
            HardLevel = SudokuFieldManager.Generate(SudokuDifficulty.Hard);
        }

        private SudokuCellViewModel[,] _easySudokuLevel;

        public SudokuCellViewModel[,] EasyLevel
        {
            get { return _easySudokuLevel; }
            private set { _easySudokuLevel = value; }
        }

        private SudokuCellViewModel[,] _hardLevel;

        public SudokuCellViewModel[,] HardLevel
        {
            get { return _hardLevel; }
            private set { _hardLevel = value; }
        }

        private RelayCommand _choseDifficultyLevel;

        public RelayCommand ChoseDifficultyLevelCommand
        {
            get { return _choseDifficultyLevel ?? (_choseDifficultyLevel = new RelayCommand(PickDifficulltyLevelCommandHandler)); }
        }

        public static Action<object> DifficultyLevelPicked;

        private void PickDifficulltyLevelCommandHandler(object obj)
        {
            DifficultyLevelPicked.Invoke(obj);
        }


    }
}
