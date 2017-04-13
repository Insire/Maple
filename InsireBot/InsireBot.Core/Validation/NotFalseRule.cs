using Maple.Localization.Properties;
using System.Globalization;
using System.Windows.Controls;

namespace Maple.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Maple.Core.BaseValidationRule" />
    public class NotFalseRule : BaseValidationRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFalseRule"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public NotFalseRule(string propertyName)
            : base(propertyName)
        {
        }

        /// <summary>
        /// When overridden in a derived class, performs validation checks on a value.
        /// </summary>
        /// <param name="value">The value from the binding target to check.</param>
        /// <param name="cultureInfo">The culture to use in this rule.</param>
        /// <returns>
        /// A <see cref="T:System.Windows.Controls.ValidationResult" /> object.
        /// </returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var isValid = (value as bool?) == true;

            return new ValidationResult(isValid, $"{nameof(PropertyName)} {Resources.IsRequired}");
        }
    }
}
