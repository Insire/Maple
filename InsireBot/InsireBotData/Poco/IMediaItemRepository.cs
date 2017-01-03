namespace InsireBot.Data
{
    public interface IMediaItemRepository : IRepository<MediaItem>
    {
        // add specializations here
        void Save(MediaItem item);
    }
}
