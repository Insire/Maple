using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Maple.Core
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseViewModel<T> : ObservableObject, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors;
        private readonly Dictionary<string, (List<BaseValidationRule> Items, object Value)> _rules;

        /// <summary>
        /// The busy stack
        /// </summary>
        protected readonly BusyStack _busyStack;

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        public bool HasErrors => _errors.Count > 0;
        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid => _errors.Count == 0;

        private T _model;
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public T Model
        {
            get { return _model; }
            protected set { SetValue(ref _model, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetValue(ref _isBusy, value); }
        }

        private BaseViewModel()
        {
            _busyStack = new BusyStack();
            _busyStack.OnChanged += (isBusy) => IsBusy = isBusy;

            _errors = new Dictionary<string, List<string>>();
            _rules = new Dictionary<string, (List<BaseValidationRule> Items, object Value)>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel{T}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        protected BaseViewModel(T model) : this()
        {
            Model = model;
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; or null or <see cref="F:System.String.Empty" />, to retrieve entity-level errors.</param>
        /// <returns>
        /// The validation errors for the property or entity.
        /// </returns>
        public IEnumerable GetErrors(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && _errors.ContainsKey(propertyName)
              ? _errors[propertyName]
              : Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<List<string>> GetErrors()
        {
            foreach (var key in _errors.Keys)
                yield return _errors[key];
        }

        /// <summary>
        /// Called when [errors changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Clears the errors.
        /// </summary>
        protected void ClearErrors()
        {
            foreach (var propertyName in _errors.Keys.ToList())
            {
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        /// <summary>
        /// Adds the rule.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="rule">The rule.</param>
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

        /// <summary>
        /// Validates the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
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
