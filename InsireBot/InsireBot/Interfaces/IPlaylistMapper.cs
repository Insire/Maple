namespace Maple
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPlaylistMapper
    {
        /// <summary>
        /// Gets the core.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        Core.Playlist GetCore(Playlist mediaitem);

        /// <summary>
        /// Gets the core.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        Core.Playlist GetCore(Data.Playlist mediaitem);

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        Data.Playlist GetData(Core.Playlist mediaitem);

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        Data.Playlist GetData(Playlist mediaitem);

        /// <summary>
        /// Gets the specified mediaitem.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        Playlist Get(Data.Playlist mediaitem);

        /// <summary>
        /// Gets the specified mediaitem.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        Playlist Get(Core.Playlist mediaitem);
    }
}
