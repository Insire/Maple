using Maple.Domain;
using MvvmScarletToolkit;

namespace Maple
{
    public class RepeatModeChangedMessage : GenericScarletMessage<RepeatMode>
    {
        public RepeatModeChangedMessage(object sender, RepeatMode content)
            : base(sender, content)
        {
        }
    }
}
