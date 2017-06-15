using System;

namespace Maple.Core
{
    /// <summary>
    /// Represents an active subscription to a message
    /// </summary>
    public sealed class SubscriptionToken : IDisposable
    {
        private readonly WeakReference _hub;
        private readonly Type _messageType;

        public SubscriptionToken(IMessenger hub, Type messageType)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            if (!typeof(IMapleMessage).IsAssignableFrom(messageType))
                throw new ArgumentOutOfRangeException(nameof(messageType));

            _hub = new WeakReference(hub);
            _messageType = messageType;
        }

        public void Dispose()
        {
            if (_hub.IsAlive && _hub.Target is IMessenger hub)
            {
                var unsubscribeMethod = typeof(IMessenger).GetMethod("Unsubscribe", new Type[] { typeof(SubscriptionToken) });
                unsubscribeMethod = unsubscribeMethod.MakeGenericMethod(_messageType);
                unsubscribeMethod.Invoke(hub, new object[] { this });
            }

            GC.SuppressFinalize(this);
        }
    }
}
