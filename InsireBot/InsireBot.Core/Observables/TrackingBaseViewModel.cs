using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Maple.Core
{
    public abstract class TrackingBaseViewModel<T> : ValidationBaseViewModel, IValidatableTrackingObject, IValidatableObject
    {
        private Dictionary<string, object> _originalValues;
        private List<IValidatableTrackingObject> _trackingObjects;

        public T Model { get; private set; }
        public bool IsChanged => _originalValues.Count > 0 || _trackingObjects.Any(t => t.IsChanged);
        public bool IsValid => !HasErrors && _trackingObjects.All(t => t.IsValid);

        public TrackingBaseViewModel(T model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), $"{nameof(model)} {Resources.IsRequired}");

            Model = model;
            _originalValues = new Dictionary<string, object>();
            _trackingObjects = new List<IValidatableTrackingObject>();

            InitializeComplexProperties(model);
            InitializeCollectionProperties(model);

            Validate();
        }

        protected virtual void InitializeComplexProperties(T model)
        {
        }

        protected virtual void InitializeCollectionProperties(T model)
        {
        }

        public void AcceptChanges()
        {
            _originalValues.Clear();

            foreach (var trackingObject in _trackingObjects)
                trackingObject.AcceptChanges();

            OnPropertyChanged("");
        }

        public void RejectChanges()
        {
            foreach (var originalValueEntry in _originalValues)
                typeof(T).GetProperty(originalValueEntry.Key).SetValue(Model, originalValueEntry.Value);

            _originalValues.Clear();

            foreach (var trackingObject in _trackingObjects)
                trackingObject.RejectChanges();

            Validate();
            OnPropertyChanged("");
        }

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            var propertyInfo = Model.GetType().GetProperty(propertyName);
            return (TValue)propertyInfo.GetValue(Model);
        }

        protected TValue GetOriginalValue<TValue>(string propertyName)
        {
            return _originalValues.ContainsKey(propertyName)
              ? (TValue)_originalValues[propertyName]
              : GetValue<TValue>(propertyName);
        }

        protected bool GetIsChanged(string propertyName)
        {
            return _originalValues.ContainsKey(propertyName);
        }

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
        protected override bool SetValue<T>(ref T field, T value, Action Changing = null, Action Changed = null, [CallerMemberName] string propertyName = null)
#pragma warning restore CS0693
        {
            var currentValue = field;
            var newChanging = new Action(() =>
            {
                Changing?.Invoke();
                UpdateOriginalValue(currentValue, value, propertyName);
            });

            var newChanged = new Action(() =>
            {
                Validate();

                OnPropertyChanged(propertyName);
                OnPropertyChanged(propertyName + nameof(IsChanged));

                Changed?.Invoke();
            });

            return base.SetValue(ref field, value, newChanging, newChanged);
        }

        private void Validate()
        {
            ClearErrors();

            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);
            Validator.TryValidateObject(this, context, results, true);

            if (results.Any())
            {
                var propertyNames = results.SelectMany(r => r.MemberNames).Distinct().ToList();

                foreach (var propertyName in propertyNames)
                {
                    Errors[propertyName] = results
                      .Where(r => r.MemberNames.Contains(propertyName))
                      .Select(r => r.ErrorMessage)
                      .Distinct()
                      .ToList();
                    OnErrorsChanged(propertyName);
                }
            }

            OnPropertyChanged(nameof(IsValid));
        }

        private void UpdateOriginalValue(object currentValue, object newValue, string propertyName)
        {
            if (!_originalValues.ContainsKey(propertyName))
            {
                _originalValues.Add(propertyName, currentValue);
                OnPropertyChanged(nameof(IsChanged));
            }
            else
            {
                if (Equals(_originalValues[propertyName], newValue))
                {
                    _originalValues.Remove(propertyName);
                    OnPropertyChanged(nameof(IsChanged));
                }
            }
        }

        protected void RegisterCollection<TWrapper, TModel>(ChangeTrackingCollection<TWrapper> wrapperCollection, List<TModel> modelCollection)
            where TWrapper : TrackingBaseViewModel<TModel>
        {
            wrapperCollection.CollectionChanged += (s, e) =>
            {
                modelCollection.Clear();
                modelCollection.AddRange(wrapperCollection.Select(w => w.Model));
                Validate();
            };

            RegisterTrackingObject(wrapperCollection);
        }

        protected void RegisterComplex<TModel>(TrackingBaseViewModel<TModel> wrapper)
        {
            RegisterTrackingObject(wrapper);
        }

        private void RegisterTrackingObject(IValidatableTrackingObject trackingObject)
        {
            if (!_trackingObjects.Contains(trackingObject))
            {
                _trackingObjects.Add(trackingObject);
                trackingObject.PropertyChanged += TrackingObjectPropertyChanged;
            }
        }

        private void TrackingObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsChanged))
                OnPropertyChanged(nameof(IsChanged));
            else
            {
                if (e.PropertyName == nameof(IsValid))
                    OnPropertyChanged(nameof(IsValid));
            }
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
