namespace Maple.Core
{
    internal class SubscriptionItem
    {
        public IMapleMessageProxy Proxy { get; private set; }
        public IMapleMessageSubscription Subscription { get; private set; }

        public SubscriptionItem(IMapleMessageProxy proxy, IMapleMessageSubscription subscription)
        {
            Proxy = proxy;
            Subscription = subscription;
        }
    }
}
