using Maple.Localization.Properties;
using System.Globalization;
using System.Windows.Controls;

namespace Maple.Core
{
    public class NotNullOrEmptyRule : ValidationRule
    {
        public string PropertyName { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var isValid = !string.IsNullOrEmpty(value.ToString());

            return new ValidationResult(isValid, $"{nameof(PropertyName)} {Resources.IsRequired}");
        }
    }

    public class NotNullRule : ValidationRule
    {
        public string PropertyName { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var isValid = value != null;

            return new ValidationResult(isValid, $"{nameof(PropertyName)} {Resources.IsRequired}");
        }
    }

    public class NotFalseRule : ValidationRule
    {
        public string PropertyName { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var isValid = (value as bool?) == true;

            return new ValidationResult(isValid, $"{nameof(PropertyName)} {Resources.IsRequired}");
        }
    }
}
