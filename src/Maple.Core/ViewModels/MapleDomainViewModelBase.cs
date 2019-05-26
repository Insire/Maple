using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentValidation;
using FluentValidation.Results;
using Maple.Domain;
using MvvmScarletToolkit.Observables;

namespace Maple.Core
{
    public abstract class MapleDomainViewModelBase<TModel> : MapleBusinessViewModelBase<TModel>, INotifyDataErrorInfo, IObservable<MapleDomainViewModelBase<TModel>>
        where TModel : class
    {
        private readonly ConcurrentDictionary<IObserver<MapleDomainViewModelBase<TModel>>, IObserver<MapleDomainViewModelBase<TModel>>> _observers;

        protected readonly bool SkipChangeTracking;
        protected readonly bool SkipValidation;
        protected readonly IValidator<MapleDomainViewModelBase<TModel>> Validator;
        protected readonly IDictionary<string, ValidationResult> ValidationLookup;
        protected readonly ChangeTracker ChangeTracker;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        [Bindable(true, BindingDirection.OneWay)]
        public bool HasChanged => ChangeTracker.HasChanged;

        private bool _hasErrors;
        [Bindable(true, BindingDirection.OneWay)]
        public bool HasErrors
        {
            get { return _hasErrors; }
            private set { SetValue(ref _hasErrors, value); }
        }

        protected MapleDomainViewModelBase(IMapleCommandBuilder commandBuilder, IValidator<MapleDomainViewModelBase<TModel>> validator)
            : base(commandBuilder)
        {
            Validator = validator ?? throw new ArgumentNullException(nameof(validator));
            ValidationLookup = new Dictionary<string, ValidationResult>();

            _observers = new ConcurrentDictionary<IObserver<MapleDomainViewModelBase<TModel>>, IObserver<MapleDomainViewModelBase<TModel>>>();

            SkipValidation = true;
            SkipChangeTracking = true;

            ChangeTracker = new ChangeTracker();

            SkipChangeTracking = false;
            SkipValidation = false;
        }

        protected override bool SetValue<T>(ref T field, T value, Action OnChanging, Action OnChanged, [CallerMemberName] string propertyName = null)
        {
            if (base.SetValue(ref field, value, OnChanging, OnChanged, propertyName))
            {
                var shouldNotifySubscribers = false;
                if (!SkipChangeTracking && ChangeTracker.Update(value, propertyName))
                {
                    shouldNotifySubscribers = true;
                    OnPropertyChanged(nameof(HasChanged));
                }

                if (!SkipValidation)
                {
                    AddOrUpdateValidationResults(ref shouldNotifySubscribers, Validator.Validate(this, propertyName), propertyName);
                }

                if (shouldNotifySubscribers)
                {
                    NotifySubscribers();
                }

                return true;
            }

            return false;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return ValidationLookup.SelectMany(p => p.Value.Errors.Select(f => f.ErrorMessage));

            if (ValidationLookup?.ContainsKey(propertyName) != true)
                return Enumerable.Empty<string>();

            return ValidationLookup[propertyName].Errors.Select(p => p.ErrorMessage);
        }

        private void AddOrUpdateValidationResults(ref bool shouldNotifySubscribers, ValidationResult result, string propertyName)
        {
            if (result is null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (SkipValidation)
            {
                return;
            }

            var wasValid = false;

            if (ValidationLookup.ContainsKey(propertyName))
            {
                var validation = ValidationLookup[propertyName];
                wasValid = validation?.Errors?.Count.Equals(0) ?? true;

                validation?.Errors?.Clear();
                ValidationLookup[propertyName] = result;

                HasErrors = result?.IsValid ?? false;
            }
            else
            {
                ValidationLookup.Add(propertyName, result);
                HasErrors = true;
            }

            var didIsValidStayTheSame = wasValid == (result?.IsValid ?? true);
            if (didIsValidStayTheSame)
            {
                return;
            }

            shouldNotifySubscribers = true;

            OnErrorsChanged(propertyName);

            if (Debugger.IsAttached)
            {
                foreach (var item in ValidationLookup[propertyName].Errors)
                    Debug.WriteLine(item);
            }
        }

        private void NotifySubscribers()
        {
            foreach (var observer in _observers.Keys.ToArray())
            {
                observer.OnNext(this);
            }
        }

        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IDisposable Subscribe(IObserver<MapleDomainViewModelBase<TModel>> observer)
        {
            return new DisposalToken<MapleDomainViewModelBase<TModel>>(observer, _observers);
        }

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
            {
                _observers.Clear();
            }

            base.Dispose(disposing);
        }
    }
}
