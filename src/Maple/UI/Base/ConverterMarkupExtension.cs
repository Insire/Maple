using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Maple
{
    public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        private static T _debugConverter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_debugConverter == null)
                _debugConverter = new T();

            return _debugConverter;
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
