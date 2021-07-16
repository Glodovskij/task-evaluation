using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace images_viewer.DataConventers
{
    public class UriToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bmp = new BitmapImage
            {
                CacheOption = BitmapCacheOption.OnDemand,
                CreateOptions = BitmapCreateOptions.DelayCreation
            };
            try
            {
            bmp.BeginInit();
            bmp.DecodePixelWidth = 160;
            bmp.UriSource = new Uri((string)value);
            bmp.EndInit();

            }
            catch
            {
                return null;
            }
            return bmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
