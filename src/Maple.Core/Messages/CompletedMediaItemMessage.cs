namespace Maple.Core
{
    public class CompletedMediaItemMessage : GenericMapleMessage<IMediaItem>
    {
        public CompletedMediaItemMessage(object sender, IMediaItem mediaItem) : base(sender, mediaItem)
        {
        }
    }
}
