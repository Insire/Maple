﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

using FluentValidation;
using FluentValidation.Results;

using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public abstract class ValidableBaseDataViewModel<TViewModel, TModel, TKeyDataType> : BaseDataViewModel<TModel>, INotifyDataErrorInfo
        where TViewModel : BaseDataViewModel<TModel>, ISequence
        where TModel : class, IEntity<TKeyDataType>
    {
        protected bool SkipValidation { get; set; }

        protected IValidator<TViewModel> Validator { get; }
        protected IDictionary<string, ValidationResult> Messages { get; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => Messages.Any(p => !p.Value.IsValid);

        protected ValidableBaseDataViewModel(TModel model, IValidator<TViewModel> validator, IMessenger messenger)
            : base(model, messenger)
        {
            SkipChangeTracking = true;
            SkipValidation = true;

            Messages = new Dictionary<string, ValidationResult>();

            Validator = validator ?? throw new ArgumentNullException(nameof(validator), $"{nameof(validator)} {Resources.IsRequired}"); //order is important in this case
            Model = model ?? throw new ArgumentNullException(nameof(model), $"{nameof(model)} {Resources.IsRequired}");

            SkipChangeTracking = false;
        }

        public virtual void Validate()
        {
            SkipValidation = false;
            var result = Validator.Validate(this);
            // TODO figure out how i can get update the errors dictionary from this
            // aka get propertyNames from the validator

            // run validation on all properties and rules
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return Messages.SelectMany(p => p.Value.Errors.Select(f => f.ErrorMessage));

            if (Messages?.ContainsKey(propertyName) != true)
                return Enumerable.Empty<string>();

            return Messages[propertyName].Errors.Select(p => p.ErrorMessage);
        }

        public virtual void Validate(string propertyName)
        {
            if (SkipValidation)
                return;

            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(propertyName, $"{nameof(propertyName)} {Resources.IsRequired}");

            AddOrUpdateValidationResults(Validator.Validate(this, propertyName), propertyName);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (SkipValidation)
                return;

            AddOrUpdateValidationResults(Validator.Validate(this, propertyName), propertyName);
        }

        private void AddOrUpdateValidationResults(ValidationResult result, string propertyName)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result), $"{nameof(result)} {Resources.IsRequired}");

            if (SkipValidation)
                return;

            var hadErrors = false;

            if (Messages.ContainsKey(propertyName))
            {
                hadErrors = Messages[propertyName].Errors.Count > 0;

                Messages[propertyName].Errors.Clear();
                Messages[propertyName] = result;
            }
            else
                Messages.Add(propertyName, result);

            if (hadErrors != (result?.IsValid ?? false))
                return;

            OnErrorsChanged(propertyName);

            if (Debugger.IsAttached)
            {
                foreach (var item in Messages[propertyName].Errors)
                    Debug.WriteLine(item);
            }
        }

        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
