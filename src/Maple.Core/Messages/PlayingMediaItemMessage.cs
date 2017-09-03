using Maple.Interfaces;

namespace Maple.Core
{
    public class PlayingMediaItemMessage : GenericMapleMessage<IMediaItem>
    {
        public PlayingMediaItemMessage(object sender, IMediaItem mediaItem) : base(sender, mediaItem)
        {
        }
    }
}
