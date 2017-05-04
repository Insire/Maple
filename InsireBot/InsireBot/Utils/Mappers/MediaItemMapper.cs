using AutoMapper;
using Maple.Core;
using Maple.Data;
using Maple.Localization.Properties;
using System;

namespace Maple
{
    /// <summary>
    /// Provides logic to map between different domain objects of the MediaItemType
    /// </summary>
    /// <seealso cref="Maple.IMediaItemMapper" />
    public class MediaItemMapper : IMediaItemMapper
    {
        private readonly IMapper _mapper;
        private readonly IPlaylistContext _context;
        private readonly ITranslationService _translator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaItemMapper"/> class.
        /// </summary>
        public MediaItemMapper(IPlaylistContext context, ITranslationService translator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _translator = translator ?? throw new ArgumentNullException(nameof(translator));

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.MediaItem, Core.MediaItem>();
                cfg.CreateMap<Core.MediaItem, Data.MediaItem>()
                    .Ignore(p => p.IsDeleted)
                    .Ignore(p => p.IsNew)
                    .Ignore(p => p.Raw)
                    .Ignore(p => p.RawId)
                    .Ignore(p => p.RowVersion)
                    .Ignore(p => p.Playlist);
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        public MediaItem GetNewMediaItem(int sequence, int playlistId)
        {
            return new MediaItem(new Data.MediaItem(), _context)
            {
                Title = _translator.Translate(nameof(Resources.New)),
                Description = string.Empty,
                PlaylistId = playlistId,
            };
        }

        /// <summary>
        /// Gets the specified mediaitem.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public MediaItem Get(Core.MediaItem mediaitem)
        {
            return new MediaItem(GetData(mediaitem), _context);
        }

        /// <summary>
        /// Gets the specified mediaitem.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public MediaItem Get(Data.MediaItem mediaitem)
        {
            return new MediaItem(mediaitem, _context);
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
