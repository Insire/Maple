using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using Maple.Domain;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    /// <summary>
    /// Generic ViewModelBase that provides common services required in Maple
    /// </summary>
    public abstract class MapleBusinessViewModelBase<TModel> : MapleBusinessViewModelBase
        where TModel : class
    {
        private TModel _model;
        [Bindable(true, BindingDirection.OneWay)]
        public TModel Model
        {
            get { return _model; }
            protected set { SetValue(ref _model, value); }
        }

        protected MapleBusinessViewModelBase(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }

        protected MapleBusinessViewModelBase(IMapleCommandBuilder commandBuilder, TModel model)
            : base(commandBuilder)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }
    }

    /// <summary>
    /// ViewModelBase that provides common services required in Maple
    /// </summary>
    public abstract class MapleBusinessViewModelBase : BusinessViewModelBase, IDisposable
    {
        protected readonly IScarletMessenger Messenger;
        protected readonly ILoggerFactory LogFactory;
        protected readonly ILocalizationService LocalizationService;
        protected readonly ISequenceService SequenceService;
        protected readonly Func<IUnitOfWork> ContextFactory;

        private readonly ConcurrentStack<SubscriptionToken> _messageTokens;

        public bool IsDisposed { get; private set; }

        protected MapleBusinessViewModelBase(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
            Messenger = commandBuilder.Messenger ?? throw new ArgumentNullException(nameof(Messenger));
            LogFactory = commandBuilder.Log ?? throw new ArgumentNullException(nameof(LogFactory));
            LocalizationService = commandBuilder.LocalizationService ?? throw new ArgumentNullException(nameof(LocalizationService));
            SequenceService = commandBuilder.SequenceService ?? throw new ArgumentNullException(nameof(SequenceService));
            ContextFactory = commandBuilder.ContextFactory ?? throw new ArgumentNullException(nameof(ContextFactory));

            _messageTokens = new ConcurrentStack<SubscriptionToken>();
        }

        protected void Add(SubscriptionToken token)
        {
            _messageTokens.Push(token);
        }

        protected void ClearSubscriptions()
        {
            using (BusyStack.GetToken())
            {
                foreach (var subscription in _messageTokens.ToArray())
                {
                    subscription.Dispose();
                }
            }
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (IsDisposed)
                return;

            // If disposing equals true, dispose all managed
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.
                ClearSubscriptions();
            }

            // Call the appropriate methods to clean up
            // unmanaged resources here.
            // If disposing is false,
            // only the following code is executed.

            // Note disposing has been done.
            IsDisposed = true;
        }
    }
}
