using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Controls;

namespace Maple.Core
{
    public abstract class BaseViewModel<T> : ObservableObject, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected readonly BusyStack _busyStack;
        protected Dictionary<string, List<string>> Errors { get; set; }
        protected Dictionary<string, (List<ValidationRule> Items, object Value)> Rules { get; set; }

        public bool HasErrors => Errors.Count > 0;
        public bool IsValid => Errors.Count == 0;

        private T _model;
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

            Errors = new Dictionary<string, List<string>>();
            Rules = new Dictionary<string, (List<ValidationRule> Items, object Value)>();
        }

        protected BaseViewModel(T model) : this()
        {
            Model = model;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && Errors.ContainsKey(propertyName)
              ? Errors[propertyName]
              : Enumerable.Empty<string>();
        }

        public IEnumerable<List<string>> GetErrors()
        {
            foreach (var key in Errors.Keys)
                yield return Errors[key];
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void ClearErrors()
        {
            foreach (var propertyName in Errors.Keys.ToList())
            {
                Errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        protected void AddRule(string propertyName, object value, ValidationRule rule)
        {
            var result = (Items: new List<ValidationRule>(), Value: value);
            if (Rules.ContainsKey(propertyName))
                result = Rules[propertyName];

            if (!result.Items.Contains(rule))
                result.Items.Add(rule);

            result.Value = value;

            Rules[propertyName] = result;
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
            Errors[propertyName].Clear();

            foreach (var item in Rules[propertyName].Items)
            {
                var result = item.Validate(Rules[propertyName].Value, current);
                Errors[propertyName].Add(result.ErrorContent.ToString());
            }

            OnErrorsChanged(propertyName);
        }

        private void ValidateAllInternal()
        {
            var current = Thread.CurrentThread.CurrentCulture;

            foreach (var key in Errors.Keys)
                ValidateInternal(key, current);
        }


    }
}
