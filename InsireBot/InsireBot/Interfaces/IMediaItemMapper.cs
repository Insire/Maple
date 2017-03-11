namespace Maple
{
    public interface IMediaItemMapper
    {
        Core.MediaItem GetCore(MediaItem mediaitem);

        Core.MediaItem GetCore(Data.MediaItem mediaitem);

        Data.MediaItem GetData(Core.MediaItem mediaitem);

        Data.MediaItem GetData(MediaItem mediaitem);

        MediaItem Get(Data.MediaItem mediaitem);

        MediaItem Get(Core.MediaItem mediaitem);
    }
}
