using System.Windows.Media;

namespace Maple.Core
{
    public class UiPrimaryColorChangedMessage : GenericMapleMessage<Color>
    {
        public UiPrimaryColorChangedMessage(object sender, Color color) : base(sender, color)
        {
        }
    }
}
