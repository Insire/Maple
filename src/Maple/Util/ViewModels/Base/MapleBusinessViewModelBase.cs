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
        protected readonly ILoggerFactory LogFactory;
        protected readonly ILocalizationService LocalizationService;
        protected readonly ISequenceService SequenceService;

        private readonly ConcurrentStack<SubscriptionToken> _messageTokens;

        private bool _disposed;

        protected MapleBusinessViewModelBase(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
            LogFactory = commandBuilder.Log ?? throw new ArgumentNullException(nameof(LogFactory));
            LocalizationService = commandBuilder.LocalizationService ?? throw new ArgumentNullException(nameof(LocalizationService));
            SequenceService = commandBuilder.SequenceService ?? throw new ArgumentNullException(nameof(SequenceService));

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
