using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Maple.Domain;

namespace Maple
{
    /// <summary>
    /// Collection ViewModelBase that provides Validation and ChangeTracking for itself and all its children
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public abstract class MapleDomainViewModelListBase<TListViewModel, TViewModel> : MapleBusinessViewModelListBase<TViewModel>, INotifyDataErrorInfo
        where TListViewModel : MapleDomainViewModelListBase<TListViewModel, TViewModel>
        where TViewModel : class, INotifyPropertyChanged, IChangeState, INotifyDataErrorInfo

    {
        protected readonly IDictionary<string, ValidationResult> ValidationLookup;
        protected readonly IValidator<TListViewModel> Validator;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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
        }

        protected override bool SetValue<T>(ref T field, T value, Action OnChanging, Action OnChanged, [CallerMemberName] string propertyName = null)
        {
            if (base.SetValue(ref field, value, OnChanging, OnChanged, propertyName))
            {
                AddOrUpdateValidationResults(Validator?.Validate(this, propertyName), propertyName);

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
            item.ErrorsChanged += Item_ErrorsChanged;
        }

        private void Item_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HaveChildrenErrors));
            OnPropertyChanged(nameof(HasErrors));
        }

        public sealed override async Task Remove(TViewModel item)
        {
            if (!Items.Contains(item))
            {
                Debug.WriteLine("Trying to remove an untracked entity!");
                return;
            }

            await base.Remove(item);

            item.ErrorsChanged -= Item_ErrorsChanged;
        }

        // TODO add event aggregator
    }
}
