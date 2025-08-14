using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WPFTest.Converters
{
    public class ArrayByteToImageConverter : IValueConverter
    {
        private static readonly string DefaultImagePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "NotFound.png");

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] byteArray && byteArray.Length > 0)
            {
                using (var stream = new MemoryStream(byteArray))
                {
                    var bitmap = new BitmapImage();

                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();

                    return bitmap;
                }
            }
            return LoadDefaultImage();
        }

        private BitmapImage? LoadDefaultImage()
        {
            if (!File.Exists(DefaultImagePath)) return null;

            try
            {
                var defaultImageBytes = File.ReadAllBytes(DefaultImagePath);

                using (var stream = new MemoryStream(defaultImageBytes))
                {
                    var bitmap = new BitmapImage();

                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();

                    return bitmap;
                }
            }
            catch
            {
                return null; 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
