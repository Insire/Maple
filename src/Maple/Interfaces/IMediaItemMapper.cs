namespace Maple
{
    public interface IMediaItemMapper : IBaseMapper<MediaItem, Data.MediaItem>
    {
        MediaItem GetNewMediaItem(int sequence, Playlist playlist);
        Data.MediaItem GetDataNewMediaItem(Data.Playlist playlist);
    }
}
