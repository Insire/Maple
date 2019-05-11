using Maple.Domain;
using MvvmScarletToolkit;

namespace Maple.Core
{
    public class PlayingMediaItemMessage : GenericScarletMessage<IMediaItem>
    {
        public int PlaylistId { get; }

        public PlayingMediaItemMessage(object sender, IMediaItem mediaItem, int playlistId)
            : base(sender, mediaItem)
        {
            PlaylistId = playlistId;
        }
    }
}
