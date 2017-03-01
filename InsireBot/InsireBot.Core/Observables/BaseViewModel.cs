using Maple.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Maple.Core
{
    public abstract class BaseViewModel<T> : ObservableObject, INotifyDataErrorInfo where T : IModel
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected readonly BusyStack _busyStack;
        protected readonly Dictionary<string, List<string>> Errors;

        public bool HasErrors => Errors.Any();

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
        }

        protected BaseViewModel(T model) : this()
        {
            Model = model;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return propertyName != null && Errors.ContainsKey(propertyName)
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
    }
}
