using System;
using System.Globalization;
using System.Windows.Data;

namespace sudoku
{
    public class SudokuArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && value is SudokuCellViewModel[,])
            {
                SudokuCellViewModel[,] sourceField = value as SudokuCellViewModel[,];
                SudokuCellViewModel[] singleDimensionField = new SudokuCellViewModel[81];

                for (int row = 0, cell = 0; row < SudokuFieldManager.RowAmount; row++)
                {
                    for (int col = 0; col < SudokuFieldManager.ColumnAmount; col++)
                    {
                        singleDimensionField[cell] = sourceField[row, col];
                        cell++;

                    }
                }
                return singleDimensionField;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is SudokuCellViewModel[])
            {
                SudokuCellViewModel[] sourceField = value as SudokuCellViewModel[];
                SudokuCellViewModel[,] matrixField = new SudokuCellViewModel[SudokuFieldManager.RowAmount, SudokuFieldManager.ColumnAmount];

                for (int row = 0, cell = 0; row < SudokuFieldManager.RowAmount; row++)
                {
                    for (int col = 0; col < SudokuFieldManager.ColumnAmount; col++)
                    {
                        matrixField[row, col] = sourceField[cell];
                        cell++;

                    }
                }
                return matrixField;
            }
            return null;
        }
    }
}
