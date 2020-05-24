using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using MvvmScarletToolkit;

namespace Maple
{
    [ValueConversion(typeof(object), typeof(object))]
    public class PrintingDebugConverter : ConverterMarkupExtension<PrintingDebugConverter>, IValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine(value);
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine(value);
            return value;
        }
    }
}
