using System;

namespace Maple.Core
{
    internal class StrongMapleMessageSubscription<TMessage> : IMapleMessageSubscription
        where TMessage : class, IMapleMessage
    {
        private readonly ITranslationProvider _translationProvider;

        protected Action<TMessage> DeliveryAction { get; set; }
        protected Func<TMessage, bool> MessageFilter { get; set; }

        public SubscriptionToken SubscriptionToken { get; }

        public bool ShouldAttemptDelivery(IMapleMessage message)
        {
            if (message == null)
                return false;

            if (!(typeof(TMessage).IsAssignableFrom(message.GetType())))
                return false;

            return MessageFilter.Invoke(message as TMessage);
        }

        public void Deliver(IMapleMessage message)
        {
            if (!(message is TMessage))
                throw new ArgumentException("Message is not the correct type"); // TODO translate

            DeliveryAction.Invoke(message as TMessage);
        }

        public StrongMapleMessageSubscription(ITranslationProvider translationProvider, SubscriptionToken subscriptionToken, Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter)
        {
            SubscriptionToken = subscriptionToken ?? throw new ArgumentNullException(nameof(subscriptionToken));
            DeliveryAction = deliveryAction ?? throw new ArgumentNullException(nameof(deliveryAction));
            MessageFilter = messageFilter ?? throw new ArgumentNullException(nameof(messageFilter));

            _translationProvider = translationProvider ?? throw new ArgumentNullException(nameof(translationProvider));
        }
    }
}
