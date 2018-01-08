using Maple.Domain;

namespace Maple.Core
{
    public class RepeatModeChangedMessage : GenericMapleMessage<RepeatMode>
    {
        public RepeatModeChangedMessage(object sender, RepeatMode content) : base(sender, content)
        {
        }
    }
}
