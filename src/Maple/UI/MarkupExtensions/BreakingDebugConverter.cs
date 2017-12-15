using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace Maple
{
    [ValueConversion(typeof(object), typeof(object))]
    public class BreakingDebugConverter : ConverterMarkupExtension<BreakingDebugConverter>, IValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debugger.Break();
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debugger.Break();
            return value;
        }
    }
}
