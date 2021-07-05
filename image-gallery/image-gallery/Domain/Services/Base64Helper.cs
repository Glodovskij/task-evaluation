using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace image_gallery.Domain.Services
{
    public static class Base64Helper
    {
        public static string Encode(string path)
        {
            using MemoryStream memoryStream = new();

            File.OpenRead(path).CopyTo(memoryStream);

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static BitmapImage Decode(string imageData)
        {
            using MemoryStream memoryStream = new(Convert.FromBase64String(imageData));

            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.StreamSource = memoryStream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }
    }
}
