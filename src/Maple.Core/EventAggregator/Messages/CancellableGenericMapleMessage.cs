using System;

using Maple.Localization.Properties;

namespace Maple.Core
{
    /// <summary>
    /// Basic "cancellable" generic message
    /// </summary>
    /// <typeparam name="TContent">Content type to store</typeparam>
    public class CancellableGenericMapleMessage<TContent> : MapleMessageBase
    {
        /// <summary>
        /// Cancel action
        /// </summary>
        public Action Cancel { get; protected set; }

        /// <summary>
        /// Contents of the message
        /// </summary>
        public TContent Content { get; protected set; }

        /// <summary>
        /// Create a new instance of the CancellableGenericTinyMessage class.
        /// </summary>
        /// <param name="sender">Message sender (usually "this")</param>
        /// <param name="content">Contents of the message</param>
        /// <param name="cancelAction">Action to call for cancellation</param>
        public CancellableGenericMapleMessage(object sender, TContent content, Action cancelAction)
            : base(sender)
        {
            Content = content;
            Cancel = cancelAction ?? throw new ArgumentNullException(nameof(cancelAction), $"{nameof(cancelAction)} {Resources.IsRequired}");
        }
    }
}
