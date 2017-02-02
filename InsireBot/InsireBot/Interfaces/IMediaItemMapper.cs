namespace Maple
{
    public interface IMediaItemMapper
    {
        Core.MediaItem GetCore(MediaItemViewModel mediaitem);

        Core.MediaItem GetCore(Data.MediaItem mediaitem);

        Data.MediaItem GetData(Core.MediaItem mediaitem);

        Data.MediaItem GetData(MediaItemViewModel mediaitem);

        MediaItemViewModel Get(Data.MediaItem mediaitem);

        MediaItemViewModel Get(Core.MediaItem mediaitem);
    }
}
