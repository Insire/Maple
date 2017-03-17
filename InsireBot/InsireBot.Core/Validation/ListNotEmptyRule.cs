using Maple.Localization.Properties;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Maple.Core
{
    public class ListNotEmptyRule : BaseValidationRule
    {
        public ListNotEmptyRule(string propertyName)
            : base(propertyName)
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var isValid = (value as IEnumerable<object>)?.Any() == true;

            return new ValidationResult(isValid, $"{nameof(PropertyName)} {Resources.IsRequired}");
        }
    }
}
