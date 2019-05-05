using System;
using System.Collections.Generic;
using System.Linq;

using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public sealed partial class MapleMessenger : IMessenger
    {
        private readonly object _SubscriptionsPadlock = new object();
        private readonly List<SubscriptionItem> _subscriptions = new List<SubscriptionItem>();

        private readonly IMapleMessageProxy _mapleMessageProxy;
        private readonly ITranslationProvider _translationProvider;
        private readonly ILoggingService _log;

        public MapleMessenger(ITranslationProvider translationProvider, ILoggingService log, IMapleMessageProxy mapleMessageProxy)
        {
            _translationProvider = translationProvider ?? throw new ArgumentNullException(nameof(translationProvider), $"{nameof(translationProvider)} {Resources.IsRequired}");
            _log = log ?? throw new ArgumentNullException(nameof(log), $"{nameof(log)} {Resources.IsRequired}");
            _mapleMessageProxy = mapleMessageProxy ?? throw new ArgumentNullException(nameof(mapleMessageProxy), $"{nameof(mapleMessageProxy)} {Resources.IsRequired}");
        }

        public SubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction)
            where TMessage : class, IMapleMessage
        {
            return AddSubscriptionInternal(deliveryAction, (m) => true, true, _mapleMessageProxy);
        }

        public SubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, IMapleMessageProxy proxy)
            where TMessage : class, IMapleMessage
        {
            return AddSubscriptionInternal(deliveryAction, (m) => true, true, proxy);
        }

        public SubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, bool useStrongReferences)
            where TMessage : class, IMapleMessage
        {
            return AddSubscriptionInternal(deliveryAction, (m) => true, useStrongReferences, _mapleMessageProxy);
        }

        public SubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, bool useStrongReferences, IMapleMessageProxy proxy)
            where TMessage : class, IMapleMessage
        {
            return AddSubscriptionInternal(deliveryAction, (m) => true, useStrongReferences, proxy);
        }

        public SubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter)
            where TMessage : class, IMapleMessage
        {
            return AddSubscriptionInternal(deliveryAction, messageFilter, true, _mapleMessageProxy);
        }

        public SubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter, IMapleMessageProxy proxy)
            where TMessage : class, IMapleMessage
        {
            return AddSubscriptionInternal(deliveryAction, messageFilter, true, proxy);
        }

        public SubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter, bool useStrongReferences)
            where TMessage : class, IMapleMessage
        {
            return AddSubscriptionInternal(deliveryAction, messageFilter, useStrongReferences, _mapleMessageProxy);
        }

        public SubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter, bool useStrongReferences, IMapleMessageProxy proxy)
            where TMessage : class, IMapleMessage
        {
            return AddSubscriptionInternal(deliveryAction, messageFilter, useStrongReferences, proxy);
        }

        public void Unsubscribe<TMessage>(SubscriptionToken subscriptionToken)
            where TMessage : class, IMapleMessage
        {
            RemoveSubscriptionInternal<TMessage>(subscriptionToken);
        }

        public void Publish<TMessage>(TMessage message)
            where TMessage : class, IMapleMessage
        {
            PublishInternal(message);
        }

        public void PublishAsync<TMessage>(TMessage message)
            where TMessage : class, IMapleMessage
        {
            PublishAsyncInternal(message, null);
        }

        public void PublishAsync<TMessage>(TMessage message, AsyncCallback callback)
            where TMessage : class, IMapleMessage
        {
            PublishAsyncInternal(message, callback);
        }

        private SubscriptionToken AddSubscriptionInternal<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter, bool strongReference, IMapleMessageProxy proxy)
                where TMessage : class, IMapleMessage
        {
            if (deliveryAction == null)
                throw new ArgumentNullException(nameof(deliveryAction), $"{nameof(deliveryAction)} {Resources.IsRequired}");

            if (messageFilter == null)
                throw new ArgumentNullException(nameof(messageFilter), $"{nameof(messageFilter)} {Resources.IsRequired}");

            if (proxy == null)
                throw new ArgumentNullException(nameof(proxy), $"{nameof(proxy)} {Resources.IsRequired}");

            lock (_SubscriptionsPadlock)
            {
                var subscriptionToken = new SubscriptionToken(this, typeof(TMessage));

                IMapleMessageSubscription subscription;
                if (strongReference)
                    subscription = new StrongMapleMessageSubscription<TMessage>(_translationProvider, subscriptionToken, deliveryAction, messageFilter);
                else
                    subscription = new WeakMapleMessageSubscription<TMessage>(_translationProvider, subscriptionToken, deliveryAction, messageFilter);

                _subscriptions.Add(new SubscriptionItem(proxy, subscription));

                return subscriptionToken;
            }
        }

        private void RemoveSubscriptionInternal<TMessage>(SubscriptionToken subscriptionToken)
                where TMessage : class, IMapleMessage
        {
            if (subscriptionToken == null)
                throw new ArgumentNullException(nameof(subscriptionToken), $"{nameof(subscriptionToken)} {Resources.IsRequired}");

            lock (_SubscriptionsPadlock)
            {
                var currentlySubscribed = (from sub in _subscriptions
                                           where ReferenceEquals(sub.Subscription.SubscriptionToken, subscriptionToken)
                                           select sub).ToList();

                currentlySubscribed.ForEach(sub => _subscriptions.Remove(sub));
            }
        }

        private void PublishInternal<TMessage>(TMessage message)
                where TMessage : class, IMapleMessage
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message), $"{nameof(message)} {Resources.IsRequired}");

            List<SubscriptionItem> currentlySubscribed;
            lock (_SubscriptionsPadlock)
            {
                currentlySubscribed = (from sub in _subscriptions
                                       where sub.Subscription.ShouldAttemptDelivery(message)
                                       select sub).ToList();
            }

            currentlySubscribed.ForEach(sub =>
            {
                try
                {
                    sub.Proxy.Deliver(message, sub.Subscription);
                }
                catch (Exception exception)
                {
                    _log.Error(message, exception);
                }
            });
        }

        private void PublishAsyncInternal<TMessage>(TMessage message, AsyncCallback callback)
            where TMessage : class, IMapleMessage
        {
            ((Action)(() => { PublishInternal(message); })).BeginInvoke(callback, null);
        }
    }
}
