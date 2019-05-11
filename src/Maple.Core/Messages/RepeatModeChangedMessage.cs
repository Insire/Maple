using Maple.Domain;
using MvvmScarletToolkit;

namespace Maple.Core
{
    public class RepeatModeChangedMessage : GenericScarletMessage<RepeatMode>
    {
        public RepeatModeChangedMessage(object sender, RepeatMode content)
            : base(sender, content)
        {
        }
    }
}
