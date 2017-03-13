using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Maple.Core
{
    public abstract class BaseViewModel<T> : ObservableObject, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors;
        private readonly Dictionary<string, (List<BaseValidationRule> Items, object Value)> _rules;

        protected readonly BusyStack _busyStack;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _errors.Count > 0;
        public bool IsValid => _errors.Count == 0;

        private T _model;
        public T Model
        {
            get => _model;
            protected set => SetValue(ref _model, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetValue(ref _isBusy, value);
        }

        private BaseViewModel()
        {
            _busyStack = new BusyStack();
            _busyStack.OnChanged += (isBusy) => IsBusy = isBusy;

            _errors = new Dictionary<string, List<string>>();
            _rules = new Dictionary<string, (List<BaseValidationRule> Items, object Value)>();
        }

        protected BaseViewModel(T model) : this()
        {
            Model = model;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && _errors.ContainsKey(propertyName)
              ? _errors[propertyName]
              : Enumerable.Empty<string>();
        }

        public IEnumerable<List<string>> GetErrors()
        {
            foreach (var key in _errors.Keys)
                yield return _errors[key];
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void ClearErrors()
        {
            foreach (var propertyName in _errors.Keys.ToList())
            {
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        protected void AddRule(object value, BaseValidationRule rule)
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

        protected virtual void Validate(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                ValidateAllInternal();
            else
                ValidateInternal(propertyName);
        }

        private void ValidateInternal(string propertyName, CultureInfo culture = null)
        {
            var current = culture ?? Thread.CurrentThread.CurrentCulture;
            _errors[propertyName].Clear();

            foreach (var item in _rules[propertyName].Items)
            {
                var result = item.Validate(_rules[propertyName].Value, current);
                _errors[propertyName].Add(result.ErrorContent.ToString());
            }

            OnErrorsChanged(propertyName);
        }

        private void ValidateAllInternal()
        {
            var current = Thread.CurrentThread.CurrentCulture;

            foreach (var key in _errors.Keys)
                ValidateInternal(key, current);
        }
    }
}
