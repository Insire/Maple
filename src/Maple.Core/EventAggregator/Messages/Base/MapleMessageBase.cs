using System;

using Maple.Localization.Properties;

namespace Maple.Core
{
    /// <summary>
    /// Base class for messages that provides weak refrence storage of the sender
    /// </summary>
    public abstract class MapleMessageBase : IMapleMessage
    {
        /// <summary>
        /// Store a WeakReference to the sender just in case anyone is daft enough to
        /// keep the message around and prevent the sender from being collected.
        /// </summary>
        private readonly WeakReference _sender;
        public object Sender
        {
            get
            {
                return _sender?.Target;
            }
        }

        protected MapleMessageBase(object sender)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender), $"{nameof(sender)} {Resources.IsRequired}");

            _sender = new WeakReference(sender);
        }
    }
}
