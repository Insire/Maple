using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace InsireBotCore
{
    /// <summary>
    /// ever wondered why your binding in WPF didn't work? drop this converter inside your binding and check whats wrong
    /// </summary>
    public class DebugConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debugger.Break();
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debugger.Break();
            return value;
        }
    }
}
