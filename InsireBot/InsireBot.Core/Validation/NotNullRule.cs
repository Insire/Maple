using Maple.Localization.Properties;
using System.Globalization;
using System.Windows.Controls;

namespace Maple.Core
{
    public class NotNullRule : BaseValidationRule
    {
        public NotNullRule(string propertyName)
            : base(propertyName)
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var isValid = value != null;

            return new ValidationResult(isValid, $"{nameof(PropertyName)} {Resources.IsRequired}");
        }
    }
}
