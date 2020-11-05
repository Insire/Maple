using MvvmScarletToolkit;

namespace Maple
{
    public class ShuffleModeChangedMessage : GenericScarletMessage<bool>
    {
        public ShuffleModeChangedMessage(object sender, bool content)
            : base(sender, content)
        {
        }
    }
}
