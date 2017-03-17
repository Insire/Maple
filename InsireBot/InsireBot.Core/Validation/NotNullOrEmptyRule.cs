using Maple.Localization.Properties;
using System.Globalization;
using System.Windows.Controls;

namespace Maple.Core
{
    public class NotNullOrEmptyRule : BaseValidationRule
    {
        public NotNullOrEmptyRule(string propertyName) 
            : base(propertyName)
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var isValid = !string.IsNullOrEmpty(value.ToString());

            return new ValidationResult(isValid, $"{nameof(PropertyName)} {Resources.IsRequired}");
        }
    }
}
