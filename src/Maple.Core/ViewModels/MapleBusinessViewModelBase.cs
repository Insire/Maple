using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using Maple.Domain;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Observables;

namespace Maple.Core
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
    }

    /// <summary>
    /// ViewModelBase that provides common services required in Maple
    /// </summary>
    public abstract class MapleBusinessViewModelBase : BusinessViewModelBase, IDisposable
    {
        protected readonly IMessenger Messenger;
        protected readonly ILoggingService Log;
        protected readonly ILoggingNotifcationService NotificationService;
        protected readonly ILocalizationService LocalizationService;
        protected readonly ISequenceService SequenceService;

        private readonly ConcurrentStack<SubscriptionToken> _messageTokens;

        protected MapleBusinessViewModelBase(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
            Messenger = commandBuilder.Messenger ?? throw new ArgumentNullException(nameof(Messenger));
            Log = commandBuilder.Log ?? throw new ArgumentNullException(nameof(Log));
            NotificationService = commandBuilder.NotificationService ?? throw new ArgumentNullException(nameof(NotificationService));
            LocalizationService = commandBuilder.LocalizationService ?? throw new ArgumentNullException(nameof(LocalizationService));
            SequenceService = commandBuilder.SequenceService ?? throw new ArgumentNullException(nameof(SequenceService));

            _messageTokens = new ConcurrentStack<SubscriptionToken>();
        }

        protected void Add(SubscriptionToken token)
        {
            _messageTokens.Push(token);
        }

        public virtual void Dispose()
        {
            ClearSubscriptions();
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
