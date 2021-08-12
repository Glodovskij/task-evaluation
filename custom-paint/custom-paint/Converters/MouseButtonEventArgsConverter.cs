using custom_paint.Utility;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace custom_paint.Converters
{
    /// <summary>
    /// This class is created for converting  <see cref="System.Windows.Input.MouseEventArgs"/> by adding current mouse position to it.
    /// </summary>
    public class MouseButtonEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var args = (MouseEventArgs)value;
            var element = (FrameworkElement)parameter;
            var point = args.GetPosition(element);
            MouseEventArgsWithCoordinates argsWithPoint = new MouseEventArgsWithCoordinates() { Coordinates = point, MouseEventArgs = args };
            return argsWithPoint;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
