using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using MvvmScarletToolkit;

namespace Maple
{
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
