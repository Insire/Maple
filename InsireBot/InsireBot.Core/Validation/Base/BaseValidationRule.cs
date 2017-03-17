using System;
using System.Windows.Controls;

namespace Maple.Core
{
    public abstract class BaseValidationRule : ValidationRule
    {
        public string PropertyName { get; set; }

        public BaseValidationRule(string propertyName)
            : base()
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            PropertyName = propertyName;
        }
    }
}
