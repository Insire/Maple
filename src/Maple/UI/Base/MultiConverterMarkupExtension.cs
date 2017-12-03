using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Maple
{
    public abstract class MultiConverterMarkupExtension<T> : MarkupExtension, IMultiValueConverter
        where T : class, new()
    {
        private static T _debugConverter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _debugConverter ?? (_debugConverter = new T());
        }

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}
