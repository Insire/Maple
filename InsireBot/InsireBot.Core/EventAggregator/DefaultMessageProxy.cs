namespace Maple.Core
{
    /// <summary>
    /// Default "pass through" proxy.
    ///
    /// Does nothing other than deliver the message.
    /// </summary>
    public sealed class DefaultMessageProxy : IMapleMessageProxy
    {
        public void Deliver(IMapleMessage message, IMapleMessageSubscription subscription)
        {
            subscription.Deliver(message);
        }
    }
}
