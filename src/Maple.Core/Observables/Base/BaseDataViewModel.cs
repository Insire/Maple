using System;
using System.Runtime.CompilerServices;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public abstract class BaseDataViewModel<TViewModel, TModel> : ObservableObject
        where TModel : class, IBaseObject
    {
        protected BusyStack BusyStack { get; }
        protected ChangeTracker ChangeTracker { get; }
        protected IMessenger Messenger { get; }
        protected bool SkipChangeTracking { get; set; }

        private TModel _model;
        public TModel Model
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

        public bool IsChanged
        {
            get { return ChangeTracker.HasChanged; }
        }

        protected override bool SetValue<T>(ref T field, T value, Action OnChanging = null, Action OnChanged = null, [CallerMemberName] string propertyName = null)
        {
            var result = base.SetValue(ref field, value, OnChanging, OnChanged, propertyName);

            if (result)
            {
                if (!SkipChangeTracking && ChangeTracker.Update(value, propertyName))
                    OnPropertyChanged(nameof(IsChanged));
            }

            return result;
        }

        protected BaseDataViewModel(IMessenger messenger)
        {
            SkipChangeTracking = true;

            Messenger = messenger ?? throw new ArgumentNullException(nameof(messenger), $"{nameof(messenger)} {Resources.IsRequired}");
            BusyStack = new BusyStack();
            BusyStack.OnChanged += (isBusy) => IsBusy = isBusy;
            ChangeTracker = new ChangeTracker();

            SkipChangeTracking = false;
        }

        protected BaseDataViewModel(TModel model, IMessenger messenger)
            : this(messenger)
        {
            SkipChangeTracking = true;

            Model = model ?? throw new ArgumentNullException(nameof(model));

            SkipChangeTracking = false;
        }
    }
}
