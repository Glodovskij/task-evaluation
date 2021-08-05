using System;

namespace sudoku
{
    public enum SudokuDifficulty
    {
        Easy,
        Hard
    }
    public static class SudokuFieldManager
    {
        public static int ColumnAmount { get; private set; } = 9;
        public static int RowAmount { get; private set; } = 9;
        private static Random _random = new Random();
        public static SudokuCellViewModel[,] Generate(SudokuDifficulty difficulty = SudokuDifficulty.Easy)
        {
            SudokuCellViewModel[,] field = new SudokuCellViewModel[9, 9];
            int startingValue = 0;
            int cellValue = 0;
            int rowOffset = 0;

            for (int row = 0; row < RowAmount; row++)
            {
                if (row / 3 == 1)
                {
                    rowOffset = 1;
                }
                if (row / 3 == 2)
                {
                    rowOffset = 2;
                }
                for (int col = 0; col < ColumnAmount; col++)
                {
                    cellValue = col + startingValue + rowOffset;
                    if (cellValue >= 9)
                    {
                        cellValue = cellValue % 9;
                    }
                    field[col, row] = new SudokuCellViewModel() 
                    { 
                        IsEnabled = false,
                        Value = cellValue + 1,
                        X = col,
                        Y = row
                    };
                    //
                }
                startingValue += 3;
            }

            RemoveRandomCells(ref field, difficulty);

            ShuffleField(field);

            return field;
        }

        private static void RemoveRandomCells(ref SudokuCellViewModel[,] field, SudokuDifficulty sudokuDifficulty)
        {
            int numberOfCellsToBeDeleted = 0;
            int x, y;
            if (sudokuDifficulty == SudokuDifficulty.Easy)
            {
                numberOfCellsToBeDeleted = 1;
            }
            else
            {
                numberOfCellsToBeDeleted = 50;
            }

            for (int i = 0; i < numberOfCellsToBeDeleted; i++)
            {
                do
                {
                    x = _random.Next(0, 9);
                    y = _random.Next(0, 9);
                } while (field[y, x].Value == 0);
                field[y, x].Value = 0;
            }
        }

        private static void ShuffleField(SudokuCellViewModel[,] field)
        {
            for (int i = 0; i < _random.Next(1, 100); i++)
            {
                SwapRegionsCols(field);
                SwapRegionsRows(field);

                SwapConcreteCols(field);
                SwapConcreteRows(field);

            }
        }


        private static void SwapRegionsCols(SudokuCellViewModel[,] field)
        {
            int firstRegionToSwap;
            int secondRegionToSwap;
            do
            {
                firstRegionToSwap = _random.Next(1, 4);
                secondRegionToSwap = _random.Next(1, 4);
            } while (firstRegionToSwap == secondRegionToSwap);


            SwapColumnValues(field, firstRegionToSwap * 3 - 1, secondRegionToSwap * 3 - 1);
            SwapColumnValues(field, firstRegionToSwap * 3 - 2, secondRegionToSwap * 3 - 2);
            SwapColumnValues(field, firstRegionToSwap * 3 - 3, secondRegionToSwap * 3 - 3);

        }

        private static void SwapRegionsRows(SudokuCellViewModel[,] field)
        {
            int firstRegionToSwap;
            int secondRegionToSwap;
            do
            {
                firstRegionToSwap = _random.Next(1, 4);
                secondRegionToSwap = _random.Next(1, 4);
            } while (firstRegionToSwap == secondRegionToSwap);

            SwapRowValues(field, firstRegionToSwap * 3 - 1, secondRegionToSwap * 3 - 1);
            SwapRowValues(field, firstRegionToSwap * 3 - 2, secondRegionToSwap * 3 - 2);
            SwapRowValues(field, firstRegionToSwap * 3 - 3, secondRegionToSwap * 3 - 3);
        }

        private static void SwapConcreteRows(SudokuCellViewModel[,] field)
        {
            int firstRegionToSwap;
            int secondRegionToSwap;
            do
            {
                firstRegionToSwap = _random.Next(1, 4);
                secondRegionToSwap = _random.Next(1, 4);
            } while (firstRegionToSwap == secondRegionToSwap);

            int rowSection = _random.Next(1, 4);

            SwapRowValues(field, rowSection * 3 - firstRegionToSwap, rowSection * 3 - secondRegionToSwap);
        }

        private static void SwapConcreteCols(SudokuCellViewModel[,] field)
        {
            int firstRegionToSwap;
            int secondRegionToSwap;
            do
            {
                firstRegionToSwap = _random.Next(1, 4);
                secondRegionToSwap = _random.Next(1, 4);
            } while (firstRegionToSwap == secondRegionToSwap);

            int rowSection = _random.Next(1, 4);

            SwapColumnValues(field, rowSection * 3 - firstRegionToSwap, rowSection * 3 - secondRegionToSwap);
        }

        private static void SwapColumnValues(SudokuCellViewModel[,] field, int col1, int col2)
        {
            for (int row = 0; row < RowAmount; row++)
            {
                int temp = field[row, col1].Value;
                field[row, col1].Value = field[row, col2].Value;
                field[row, col2].Value = temp;
            }
        }

        static void SwapRowValues(SudokuCellViewModel[,] field, int row1, int row2)
        {
            for (int col = 0; col < RowAmount; col++)
            {
                int temp = field[col, row1].Value;
                field[col, row1].Value = field[col, row2].Value;
                field[col, row2].Value = temp;
            }
        }

        static public bool CheckForSolution(SudokuCellViewModel[,] field)
        {
            int counter = 0;
            for (int row = 0; row < RowAmount; row++)
            {
                counter = 0;
                for (int col = 0; col < ColumnAmount; col++)
                {
                    counter += field[row, col].Value;
                }
                if(counter != 45)
                {
                    return false;
                }
            }

            for (int col = 0; col < ColumnAmount; col++)
            {
                counter = 0;
                for (int row = 0; row < RowAmount; row++)
                {
                    counter += field[row, col].Value;
                }
                if (counter != 45)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
