namespace Maple.Core
{
    public abstract class BaseViewModel<TViewModel> : ViewModel
    {
        private TViewModel _model;
        public TViewModel Model
        {
            get { return _model; }
            protected set { SetValue(ref _model, value); }
        }

        protected BaseViewModel(TViewModel model, IMessenger messenger)
            : base(messenger)
        {
            _model = model;
        }
    }
}
