namespace Maple.Core
{
    public class ShuffleModeChangedMessage : GenericMapleMessage<bool>
    {
        public ShuffleModeChangedMessage(object sender, bool content) : base(sender, content)
        {
        }
    }
}
