﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace custom_paint_inccanvas.Converters
{
    public class CurrentFigureObjectToTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.GetType() ?? Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
