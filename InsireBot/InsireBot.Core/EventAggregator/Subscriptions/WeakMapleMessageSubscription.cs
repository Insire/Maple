using Maple.Localization.Properties;
using System;

namespace Maple.Core
{
    internal class WeakMapleMessageSubscription<TMessage> : IMapleMessageSubscription
        where TMessage : class, IMapleMessage
    {
        private readonly ITranslationProvider _translationProvider;

        protected WeakReference DeliveryAction { get; set; }
        protected WeakReference MessageFilter { get; set; }

        public SubscriptionToken SubscriptionToken { get; }

        public bool ShouldAttemptDelivery(IMapleMessage message)
        {
            if (message == null)
                return false;

            if (!(typeof(TMessage).IsAssignableFrom(message.GetType())))
                return false;

            if (!DeliveryAction.IsAlive)
                return false;

            if (!MessageFilter.IsAlive)
                return false;

            return ((Func<TMessage, bool>)MessageFilter.Target).Invoke(message as TMessage);
        }

        public void Deliver(IMapleMessage message)
        {
            if (!(message is TMessage))
                throw new ArgumentException("Message is not the correct type"); // TODO translate

            if (!DeliveryAction.IsAlive)
                return;

            ((Action<TMessage>)DeliveryAction.Target).Invoke(message as TMessage);
        }

        public WeakMapleMessageSubscription(ITranslationProvider translationProvider, SubscriptionToken subscriptionToken, Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter)
        {
            if (deliveryAction == null)
                throw new ArgumentNullException(nameof(deliveryAction), $"{nameof(deliveryAction)} {Resources.IsRequired}");

            if (messageFilter == null)
                throw new ArgumentNullException(nameof(messageFilter), $"{nameof(messageFilter)} {Resources.IsRequired}");

            _translationProvider = translationProvider ?? throw new ArgumentNullException(nameof(translationProvider), $"{nameof(translationProvider)} {Resources.IsRequired}");

            SubscriptionToken = subscriptionToken ?? throw new ArgumentNullException(nameof(subscriptionToken), $"{nameof(subscriptionToken)} {Resources.IsRequired}");
            DeliveryAction = new WeakReference(deliveryAction);
            MessageFilter = new WeakReference(messageFilter);
        }
    }
}
