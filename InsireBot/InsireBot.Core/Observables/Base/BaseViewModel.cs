namespace Maple.Core
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseViewModel<T> : ObservableObject
    {
        /// <summary>
        /// The busy stack
        /// </summary>
        protected readonly BusyStack _busyStack;

        private T _model;
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public T Model
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

        protected BaseViewModel()
        {
            _busyStack = new BusyStack();
            _busyStack.OnChanged += (isBusy) => IsBusy = isBusy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel{T}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        protected BaseViewModel(T model)
            : this()
        {
            Model = model;
        }
    }
}
