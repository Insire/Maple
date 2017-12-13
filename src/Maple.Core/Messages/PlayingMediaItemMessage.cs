using Maple.Domain;

namespace Maple.Core
{
    public class PlayingMediaItemMessage : GenericMapleMessage<IMediaItem>
    {
        public int PlaylistId { get; }
        public PlayingMediaItemMessage(object sender, IMediaItem mediaItem, int playlistId)
            : base(sender, mediaItem)
        {
            PlaylistId = playlistId;
        }
    }
}
