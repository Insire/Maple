using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Threading;
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

        public override async Task Remove(TViewModel item, CancellationToken token)
        {
            using (BusyStack.GetToken())
            {
                while (Items.Contains(item))
                {
                    await base.Remove(item, token);
                    item.Model.IsDeleted = true;
                }
            }
        }

        public override async Task Add(TViewModel item, CancellationToken token)
        {
            await base.Add(item, token);

            item.Sequence = SequenceService.Get(Items.Cast<ISequence>().ToList());
        }
    }

    /// <summary>
    /// Collection ViewModelBase that provides common services required in Maple
    /// </summary>
    public abstract class MapleBusinessViewModelListBase<TViewModel> : BusinessViewModelListBase<TViewModel>, IDisposable
        where TViewModel : class, INotifyPropertyChanged
    {
        protected readonly ILoggerFactory Log;
        protected readonly ILocalizationService LocalizationService;
        protected readonly ISequenceService SequenceService;
        protected readonly Func<IUnitOfWork> ContextFactory;

        private readonly ConcurrentStack<SubscriptionToken> _messageTokens;

        private bool _disposed;

        protected MapleBusinessViewModelListBase(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
            Log = commandBuilder.Log ?? throw new ArgumentNullException(nameof(Log));
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

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;

            if (disposing)
            {
                // Dispose managed resources.
                ClearSubscriptions();
            }

            base.Dispose(disposing);
        }
    }
}
