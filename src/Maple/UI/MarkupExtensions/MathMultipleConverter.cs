using System;
using System.Globalization;
using System.Windows.Data;
using MahApps.Metro.Converters;

namespace Maple
{
    [ValueConversion(typeof(object[]), typeof(double))]
    public sealed class MathMultipleConverter : MultiConverterMarkupExtension<MathMultipleConverter>, IMultiValueConverter
    {
        public MathOperation Operation { get; set; }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2 || values[0] == null || values[1] == null)
                return Binding.DoNothing;

            if (!double.TryParse(values[0].ToString(), out var value1) || !double.TryParse(values[1].ToString(), out var value2))
                return 0;

            switch (Operation)
            {
                default:
                    // (case MathOperation.Add:)
                    return value1 + value2;

                case MathOperation.Divide:
                    return value1 / value2;

                case MathOperation.Multiply:
                    return value1 * value2;

                case MathOperation.Subtract:
                    return value1 - value2;
            }
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[]
            {
               Binding.DoNothing,
               Binding.DoNothing,
           };
        }
    }
}
