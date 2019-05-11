using System.Windows.Media;
using MvvmScarletToolkit;

namespace Maple.Core
{
    public class UiPrimaryColorChangedMessage : GenericScarletMessage<Color>
    {
        public UiPrimaryColorChangedMessage(object sender, Color color)
            : base(sender, color)
        {
        }
    }
}
