namespace Maple
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMediaItemMapper
    {
        /// <summary>
        /// Gets the core.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        Core.MediaItem GetCore(MediaItem mediaitem);

        /// <summary>
        /// Gets the core.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        Core.MediaItem GetCore(Data.MediaItem mediaitem);

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        Data.MediaItem GetData(Core.MediaItem mediaitem);

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        Data.MediaItem GetData(MediaItem mediaitem);

        /// <summary>
        /// Gets the specified mediaitem.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        MediaItem Get(Data.MediaItem mediaitem);

        /// <summary>
        /// Gets the specified mediaitem.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        MediaItem Get(Core.MediaItem mediaitem);
    }
}
