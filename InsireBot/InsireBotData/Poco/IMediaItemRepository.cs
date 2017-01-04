namespace InsireBot.Data
{
    public interface IMediaItemRepository : IRepository<MediaItem>
    {
        // add specializations here
        MediaItem Save(MediaItem item);
    }
}
