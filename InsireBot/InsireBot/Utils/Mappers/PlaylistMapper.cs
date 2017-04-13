using AutoMapper;

namespace Maple
{
    /// <summary>
    /// Provides logic to map between different domain objects of the Playlisttype
    /// </summary>
    /// <seealso cref="Maple.IPlaylistMapper" />
    public class PlaylistMapper : IPlaylistMapper
    {
        private readonly IMapper _mapper;

        private DialogViewModel _dialogViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistMapper"/> class.
        /// </summary>
        /// <param name="dialogViewModel">The dialog view model.</param>
        public PlaylistMapper(DialogViewModel dialogViewModel)
        {
            _dialogViewModel = dialogViewModel;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.Playlist, Core.Playlist>();
                cfg.CreateMap<Core.Playlist, Data.Playlist>()
                    .Ignore(p => p.IsDeleted)
                    .Ignore(p => p.IsNew);
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Gets the specified mediaitem.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public Playlist Get(Core.Playlist mediaitem)
        {
            return new Playlist(_dialogViewModel, GetData(mediaitem));
        }

        /// <summary>
        /// Gets the specified mediaitem.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public Playlist Get(Data.Playlist mediaitem)
        {
            return new Playlist(_dialogViewModel, mediaitem);
        }

        /// <summary>
        /// Gets the core.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public Core.Playlist GetCore(Data.Playlist mediaitem)
        {
            return _mapper.Map<Data.Playlist, Core.Playlist>(mediaitem);
        }

        /// <summary>
        /// Gets the core.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public Core.Playlist GetCore(Playlist mediaitem)
        {
            return GetCore(mediaitem.Model);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public Data.Playlist GetData(Playlist mediaitem)
        {
            return mediaitem.Model;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public Data.Playlist GetData(Core.Playlist mediaitem)
        {
            return _mapper.Map<Core.Playlist, Data.Playlist>(mediaitem);
        }
    }
}
