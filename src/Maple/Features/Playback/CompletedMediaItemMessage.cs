using Maple.Domain;
using MvvmScarletToolkit;

namespace Maple
{
    public class CompletedMediaItemMessage : GenericScarletMessage<IMediaItem>
    {
        public CompletedMediaItemMessage(object sender, IMediaItem mediaItem)
            : base(sender, mediaItem)
        {
        }
    }
}
