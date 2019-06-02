using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    /// <summary>
    /// Generic CollectionViewModelBase that provides common services required in Maple
    /// </summary>
    public abstract class MapleBusinessViewModelListBase<TModel, TViewModel> : MapleBusinessViewModelListBase<TViewModel>
        where TModel : class, IBaseObject
        where TViewModel : MapleBusinessViewModelBase<TModel>, ISequence
    {
        private TModel _model;
        [Bindable(true, BindingDirection.OneWay)]
        public TModel Model
        {
            get { return _model; }
            protected set { SetValue(ref _model, value); }
        }

        protected MapleBusinessViewModelListBase(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }

        public override async Task Remove(TViewModel item)
        {
            using (BusyStack.GetToken())
            {
                while (Items.Contains(item))
                {
                    await base.Remove(item);
                    item.Model.IsDeleted = true;
                }
            }
        }

        public override async Task Add(TViewModel item)
        {
            await base.Add(item);

            item.Sequence = SequenceService.Get(Items.Cast<ISequence>().ToList());
        }
    }

    /// <summary>
    /// Collection ViewModelBase that provides common services required in Maple
    /// </summary>
    public abstract class MapleBusinessViewModelListBase<TViewModel> : BusinessViewModelListBase<TViewModel>, IDisposable
        where TViewModel : class, INotifyPropertyChanged
    {
        protected readonly IScarletMessenger Messenger;
        protected readonly ILoggerFactory Log;
        protected readonly ILocalizationService LocalizationService;
        protected readonly ISequenceService SequenceService;

        private readonly ConcurrentStack<SubscriptionToken> _messageTokens;

        public bool IsDisposed { get; private set; }

        protected MapleBusinessViewModelListBase(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
            Messenger = commandBuilder.Messenger ?? throw new ArgumentNullException(nameof(Messenger));
            Log = commandBuilder.Log ?? throw new ArgumentNullException(nameof(Log));
            LocalizationService = commandBuilder.LocalizationService ?? throw new ArgumentNullException(nameof(LocalizationService));
            SequenceService = commandBuilder.SequenceService ?? throw new ArgumentNullException(nameof(SequenceService));

            _messageTokens = new ConcurrentStack<SubscriptionToken>();
        }

        protected void Add(SubscriptionToken token)
        {
            _messageTokens.Push(token);
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
    }
}
