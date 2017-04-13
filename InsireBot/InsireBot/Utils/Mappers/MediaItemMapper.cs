using AutoMapper;

namespace Maple
{
    /// <summary>
    /// Provides logic to map between different domain objects of the MediaItemType
    /// </summary>
    /// <seealso cref="Maple.IMediaItemMapper" />
    public class MediaItemMapper : IMediaItemMapper
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaItemMapper"/> class.
        /// </summary>
        public MediaItemMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.MediaItem, Core.MediaItem>();
                cfg.CreateMap<Core.MediaItem, Data.MediaItem>()
                    .Ignore(p => p.IsDeleted)
                    .Ignore(p => p.IsNew)
                    .Ignore(p => p.Playlist);
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Gets the specified mediaitem.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public MediaItem Get(Core.MediaItem mediaitem)
        {
            return new MediaItem(GetData(mediaitem));
        }

        /// <summary>
        /// Gets the specified mediaitem.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public MediaItem Get(Data.MediaItem mediaitem)
        {
            return new MediaItem(mediaitem);
        }

        /// <summary>
        /// Gets the core.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public Core.MediaItem GetCore(Data.MediaItem mediaitem)
        {
            return _mapper.Map<Data.MediaItem, Core.MediaItem>(mediaitem);
        }

        /// <summary>
        /// Gets the core.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public Core.MediaItem GetCore(MediaItem mediaitem)
        {
            return GetCore(mediaitem.Model);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public Data.MediaItem GetData(MediaItem mediaitem)
        {
            return mediaitem.Model;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public Data.MediaItem GetData(Core.MediaItem mediaitem)
        {
            return _mapper.Map<Core.MediaItem, Data.MediaItem>(mediaitem);
        }
    }
}
