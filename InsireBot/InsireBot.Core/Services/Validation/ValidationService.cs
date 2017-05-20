using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace Maple.Core.Services.Validation
{
    public class ValidationService : ObservableObject, IValidationService
    {
        private readonly Dictionary<string, List<string>> _errors;
        private readonly Dictionary<string, (List<BaseValidationRule> Items, object Value)> _rules;

        protected readonly BusyStack _busyStack;

        public bool IsValid => _errors.Count == 0;
        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetValue(ref _isBusy, value); }
        }

        public ValidationService()
        {
            _busyStack = new BusyStack();
            _busyStack.OnChanged += (isBusy) => IsBusy = isBusy;

            _errors = new Dictionary<string, List<string>>();
            _rules = new Dictionary<string, (List<BaseValidationRule> Items, object Value)>();
        }

        public void AddRule(object value, BaseValidationRule rule)
        {
            var propertyName = rule.PropertyName;
            var result = (Items: new List<BaseValidationRule>(), Value: value);

            if (_rules.ContainsKey(propertyName))
                result = _rules[propertyName];

            if (!result.Items.Contains(rule))
                result.Items.Add(rule);

            result.Value = value;

            _rules[propertyName] = result;
        }

        public virtual void Validate(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                ValidateAllInternal();
            else
                ValidateInternal(propertyName);
        }

        private void ValidateInternal(string propertyName, CultureInfo culture = null)
        {
            var current = culture ?? Thread.CurrentThread.CurrentCulture;

            if (_errors.ContainsKey(propertyName))
                _errors[propertyName].Clear();

            foreach (var item in _rules[propertyName].Items)
            {
                var result = item.Validate(_rules[propertyName].Value, current);
                if (_errors.ContainsKey(propertyName))
                    _errors[propertyName].Add(result.ErrorContent.ToString());
                else
                {
                    _errors.Add(propertyName, new List<string>()
                    {
                        result.ErrorContent.ToString(),
                    });
                }
            }

            OnErrorsChanged(propertyName);
        }

        private void ValidateAllInternal()
        {
            var current = Thread.CurrentThread.CurrentCulture;

            foreach (var key in _errors.Keys)
                ValidateInternal(key, current);
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
