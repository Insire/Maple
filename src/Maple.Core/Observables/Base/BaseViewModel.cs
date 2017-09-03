using Maple.Localization.Properties;
using System;

namespace Maple.Core
{
    public abstract class BaseViewModel<TViewModel> : ObservableObject
    {
        protected readonly IMessenger _messenger;
        protected readonly BusyStack _busyStack;

        private TViewModel _model;
        public TViewModel Model
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

        protected BaseViewModel(IMessenger messenger)
        {
            _busyStack = new BusyStack();
            _busyStack.OnChanged += (isBusy) => IsBusy = isBusy;

            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger), $"{nameof(messenger)} {Resources.IsRequired}");
        }

        protected BaseViewModel(TViewModel model, IMessenger messenger)
            : this(messenger)
        {
            Model = model;
        }
    }
}
