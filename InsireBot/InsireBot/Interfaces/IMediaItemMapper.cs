namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    public interface IMediaItemMapper : IBaseMapper<MediaItem, Core.MediaItem, Data.MediaItem>
    {
        MediaItem GetNewMediaItem(int sequence, int playlistId);
        Data.MediaItem GetDataNewMediaItem(int playlistId);
    }
}
