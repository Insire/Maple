using System;
using System.Runtime.CompilerServices;

namespace Maple.Core
{
    public abstract class BaseDataViewModel<TViewModel, TModel> : ObservableObject
        where TModel : class
    {
        protected BusyStack BusyStack { get; }
        protected ChangeTracker ChangeTracker { get; }
        protected IMessenger Messenger { get; }

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

            if (result && ChangeTracker.Update(value, propertyName))
                OnPropertyChanged(nameof(IsChanged));

            return result;
        }


        protected BaseDataViewModel(IMessenger messenger)
        {
            Messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            BusyStack = new BusyStack();
            BusyStack.OnChanged += (isBusy) => IsBusy = isBusy;
            ChangeTracker = new ChangeTracker();
        }

        protected BaseDataViewModel(TModel model, IMessenger messenger)
            : this(messenger)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }
    }
}
