using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MvvmScarletToolkit;

namespace Maple
{
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public sealed class ImageFilePathFallback : ConverterMarkupExtension<ImageFilePathFallback>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string path))
            {
                return GetDefault();
            }

            if (path.Length <= 3)
            {
                return GetDefault();
            }

            if (!File.Exists(path))
            {
                return GetDefault();
            }

            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();

            return bitmap;
        }

        private static BitmapSource GetDefault()
        {
            var format = PixelFormats.Bgra32;
            var width = 200;
            var height = 200;
            var rawStride = (width * format.BitsPerPixel + 7) / 8;
            var rawImage = new byte[rawStride * height];

            //// Initialize the image with data.
            //Random value = new Random();
            //value.NextBytes(rawImage);

            // Create a BitmapSource.
            var bitmap = BitmapSource.Create(width, height, 96, 96, format, null, rawImage, rawStride);

            return bitmap;
        }
    }

    [ValueConversion(typeof(int), typeof(bool))]
    public sealed class StringLengthToVisibility : ConverterMarkupExtension<StringLengthToVisibility>
    {
        [ConstructorArgument("visibility")]
        public Visibility Visibility { get; set; }

        public StringLengthToVisibility()
        {
            Visibility = Visibility.Hidden;
        }

        public StringLengthToVisibility(Visibility visibility)
        {
            Visibility = visibility;
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return Visibility.Visible;
            }

            if (value is string path)
            {
                return path.Length <= 0 ? Visibility : Visibility.Visible;
            }

            return Visibility.Visible;
        }
    }
}
