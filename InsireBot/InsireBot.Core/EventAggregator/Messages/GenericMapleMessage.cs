namespace Maple.Core
{
    /// <summary>
    /// Generic message with user specified content
    /// </summary>
    /// <typeparam name="TContent">Content type to store</typeparam>
    public class GenericMapleMessage<TContent> : MapleMessageBase
    {
        /// <summary>
        /// Contents of the message
        /// </summary>
        public TContent Content { get; protected set; }

        public GenericMapleMessage(object sender, TContent content)
            : base(sender)
        {
            Content = content;
        }
    }
}
