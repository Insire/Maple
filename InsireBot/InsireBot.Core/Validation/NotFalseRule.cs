using Maple.Localization.Properties;
using System.Globalization;
using System.Windows.Controls;

namespace Maple.Core
{
    public class NotFalseRule : BaseValidationRule
    {
        public NotFalseRule(string propertyName)
            : base(propertyName)
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var isValid = (value as bool?) == true;

            return new ValidationResult(isValid, $"{nameof(PropertyName)} {Resources.IsRequired}");
        }
    }
}
