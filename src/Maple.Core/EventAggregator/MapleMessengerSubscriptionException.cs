using System;

namespace Maple.Core
{
    /// <summary>
    /// Thrown when an exceptions occurs while subscribing to a message type
    /// </summary>
    public class MapleMessengerSubscriptionException : Exception
    {
        private const string ERROR_TEXT = "Unable to add subscription for {0} : {1}";

        public MapleMessengerSubscriptionException(Type messageType, string reason)
            : base(string.Format(ERROR_TEXT, messageType, reason))
        {
        }

        public MapleMessengerSubscriptionException(Type messageType, string reason, Exception innerException)
            : base(string.Format(ERROR_TEXT, messageType, reason), innerException)
        {
        }

        public MapleMessengerSubscriptionException()
        {
        }

        public MapleMessengerSubscriptionException(string message) : base(message)
        {
        }

        public MapleMessengerSubscriptionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
