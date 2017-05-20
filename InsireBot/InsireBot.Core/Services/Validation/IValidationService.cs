using System;
using System.ComponentModel;

namespace Maple.Core
{
    public interface IValidationService
    {
        bool HasErrors { get; }
        bool IsBusy { get; set; }
        bool IsValid { get; }

        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        void AddRule(object value, BaseValidationRule rule);
        void Validate(string propertyName);
    }
}