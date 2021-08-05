using barley_break.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace barley_break.Converters
{
    public class MatrixArrayToArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is CellViewModel[,])
            {
                CellViewModel[,] sourceField = value as CellViewModel[,];
                CellViewModel[] singleDimensionField = new CellViewModel[16];

                for (int row = 0, cell = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
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
            if (value != null && value is CellViewModel[])
            {
                CellViewModel[] sourceField = value as CellViewModel[];
                CellViewModel[,] matrixField = new CellViewModel[4, 4];

                for (int row = 0, cell = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
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
