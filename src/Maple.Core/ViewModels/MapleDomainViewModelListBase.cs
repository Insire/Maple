using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Maple.Domain;
using MvvmScarletToolkit.Observables;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MvvmScarletToolkit.Abstractions;

namespace Maple.Core
{
    /// <summary>
    /// Collection ViewModelBase that provides Validation and ChangeTracking for itself and all its children
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public abstract class MapleDomainViewModelListBase<TListViewModel, TViewModel> : MapleBusinessViewModelListBase<TViewModel>, INotifyDataErrorInfo
        where TListViewModel : MapleDomainViewModelListBase<TListViewModel, TViewModel>
        where TViewModel : class, INotifyPropertyChanged, IChangeState, IChangeTrackable, INotifyDataErrorInfo

    {
        protected readonly bool SkipChangeTracking;
        protected readonly bool SkipValidation;
        protected readonly ChangeTracker ChangeTracker;
        protected readonly IDictionary<string, ValidationResult> ValidationLookup;
        protected readonly IValidator<TListViewModel> Validator;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        // ChangeTracking
        [Bindable(true, BindingDirection.OneWay)]
        public bool HasChanged => HasSelfChanged || HaveChildrenChanged;

        // ChangeTracking
        [Bindable(true, BindingDirection.OneWay)]
        public bool HasSelfChanged => ChangeTracker.HasChanged;

        // ChangeTracking
        [Bindable(true, BindingDirection.OneWay)]
        public bool HaveChildrenChanged => Items.Any(p => p.HasChanged);

        private bool _hasErrors;
        /// <summary>
        /// Whether this entity has errors
        /// </summary>
        [Bindable(true, BindingDirection.OneWay)]
        public bool HasErrors
        {
            get { return _hasErrors; }
            private set { SetValue(ref _hasErrors, value); }
        }

        /// <summary>
        /// whether the children of this entity have errors
        /// </summary>
        [Bindable(true, BindingDirection.OneWay)]
        public bool HaveChildrenErrors => Items.Any(p => p.HasErrors);

        protected MapleDomainViewModelListBase(IMapleCommandBuilder commandBuilder, IValidator<TListViewModel> validator)
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

        protected override bool SetValue<T>(ref T field, T value, Action OnChanging, Action OnChanged, [CallerMemberName] string propertyName = null)
        {
            if (base.SetValue(ref field, value, OnChanging, OnChanged, propertyName))
            {
                if (!SkipChangeTracking && ChangeTracker.Update(value, propertyName))
                {
                    OnPropertyChanged(nameof(HasChanged));
                    OnPropertyChanged(nameof(HasSelfChanged));
                }

                if (!SkipValidation)
                    AddOrUpdateValidationResults(Validator.Validate(this, propertyName), propertyName);

                return true;
            }

            return false;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return ValidationLookup?.SelectMany(p => p.Value.Errors.Select(f => f.ErrorMessage)) ?? Enumerable.Empty<string>();

            if (ValidationLookup?.ContainsKey(propertyName) != true)
                return Enumerable.Empty<string>();

            return ValidationLookup[propertyName].Errors.Select(p => p.ErrorMessage);
        }

        private void AddOrUpdateValidationResults(ValidationResult result, string propertyName)
        {
            if (result is null)
                throw new ArgumentNullException(nameof(result));

            if (SkipValidation)
                return;

            var hadErrors = false;

            if (ValidationLookup.ContainsKey(propertyName))
            {
                var validation = ValidationLookup[propertyName];
                hadErrors = validation.Errors.Count > 0;

                validation.Errors.Clear();
                ValidationLookup[propertyName] = result;

                HasErrors = result?.IsValid ?? false;
            }
            else
            {
                ValidationLookup.Add(propertyName, result);
                HasErrors = true;
            }

            if (hadErrors != (result?.IsValid ?? false))
                return;

            OnErrorsChanged(propertyName);

            if (Debugger.IsAttached)
            {
                foreach (var item in ValidationLookup[propertyName].Errors)
                    Debug.WriteLine(item);
            }
        }

        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public sealed override async Task Add(TViewModel item)
        {
            if (Items.Contains(item))
            {
                return;
            }

            await base.Add(item);
            item.Changed += Item_Changed;
            item.ErrorsChanged += Item_ErrorsChanged;
        }

        private void Item_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HaveChildrenErrors));
            OnPropertyChanged(nameof(HasErrors));
        }

        private void Item_Changed(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(HaveChildrenChanged));
            OnPropertyChanged(nameof(HasChanged));
        }

        public sealed override async Task Remove(TViewModel item)
        {
            if (!Items.Contains(item))
            {
                Debug.WriteLine("Trying to remove an untracked entity!");
                return;
            }

            await base.Remove(item);

            item.Changed -= Item_Changed;
            item.ErrorsChanged -= Item_ErrorsChanged;
        }

        // TODO add event aggregator
    }
}
