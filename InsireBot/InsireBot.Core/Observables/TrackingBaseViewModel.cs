using Maple.Data;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Maple.Core
{
    public abstract class TrackingBaseViewModel<T> : ValidationBaseViewModel, IValidatableTrackingObject, IValidatableObject, ISaveable
    {
        private readonly Dictionary<string, object> _originalValues;
        private readonly List<IValidatableTrackingObject> _trackingObjects;
        private readonly IRepository<T> _repository;
        private readonly List<string> _propetyBlackList = new List<string> { "isbusy", "isselected" };
        protected readonly BusyStack _busyStack;

        public EventHandler Saving;
        public EventHandler Saved;

        public T Model { get; private set; }
        public bool IsChanged => _originalValues.Count > 0 || _trackingObjects.Any(t => t.IsChanged);
        public bool IsValid => !HasErrors && _trackingObjects.All(t => t.IsValid);

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetValue(ref _isBusy, value); }
        }

        public TrackingBaseViewModel(T model, IRepository<T> repository) : base()
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), $"{nameof(model)} {Resources.IsRequired}");

            if (repository == null)
                throw new ArgumentNullException(nameof(repository), $"{nameof(repository)} {Resources.IsRequired}");

            _originalValues = new Dictionary<string, object>();
            _trackingObjects = new List<IValidatableTrackingObject>();
            _busyStack = new BusyStack();
            _busyStack.OnChanged += (isBusy) => IsBusy = isBusy;
            _repository = repository;

            Model = model;
        }

        public void AcceptChanges()
        {
            using (_busyStack.GetToken())
            {
                _originalValues.Clear();

                foreach (var trackingObject in _trackingObjects)
                    trackingObject.AcceptChanges();

                OnPropertyChanged("");
            }
        }

        public void RejectChanges()
        {
            using (_busyStack.GetToken())
            {
                foreach (var originalValueEntry in _originalValues)
                    typeof(T).GetProperty(originalValueEntry.Key).SetValue(Model, originalValueEntry.Value);

                _originalValues.Clear();

                foreach (var trackingObject in _trackingObjects)
                    trackingObject.RejectChanges();

                Validate();
                OnPropertyChanged("");
            }
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

        protected bool SetValue(ref T field, T value, Action Changing = null, Action Changed = null, [CallerMemberName]string propertyName = null)
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

        public void Validate()
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
                if (_propetyBlackList.Contains(propertyName.ToLowerInvariant()))
                    return;

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

        public void Save()
        {
            if (!IsValid || !IsChanged)
                return;

            Saving?.Raise(this);

            _repository.Save(Model);
            AcceptChanges();

            Saved?.Raise(this);
        }
    }
}
