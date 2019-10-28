using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentValidation;
using FluentValidation.Results;
using Maple.Domain;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public abstract class MapleDomainViewModelBase<TViewModel, TModel> : MapleBusinessViewModelBase<TModel>, INotifyDataErrorInfo, IChangeTrackable
        where TModel : class
        where TViewModel : class, IChangeTrackable
    {
        protected readonly bool SkipChangeTracking;
        protected readonly bool SkipValidation;
        protected readonly IValidator<TViewModel> Validator;
        protected readonly IDictionary<string, ValidationResult> ValidationLookup;
        protected readonly ChangeTracker ChangeTracker;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public event EventHandler Changed;

        [Bindable(true, BindingDirection.OneWay)]
        public bool HasChanged => ChangeTracker.HasChanged;

        private bool _hasErrors;
        [Bindable(true, BindingDirection.OneWay)]
        public bool HasErrors
        {
            get { return _hasErrors; }
            private set { SetValue(ref _hasErrors, value); }
        }

        protected MapleDomainViewModelBase(IMapleCommandBuilder commandBuilder, IValidator<TViewModel> validator)
            : base(commandBuilder)
        {
            Validator = validator ?? throw new ArgumentNullException(nameof(validator));
            ValidationLookup = new Dictionary<string, ValidationResult>();

            SkipValidation = true;
            SkipChangeTracking = true;

            ChangeTracker = new ChangeTracker();

            SkipChangeTracking = false;
            SkipValidation = false;
        }

        protected MapleDomainViewModelBase(IMapleCommandBuilder commandBuilder, IValidator<TViewModel> validator, TModel model)
            : this(commandBuilder, validator)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
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
                    this.OnChanged();
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

        protected virtual void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
