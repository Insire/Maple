using AutoMapper;
using FluentValidation;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    /// <summary>
    /// Provides logic to map between different domain objects of the MediaItemType
    /// </summary>
    /// <seealso cref="Maple.IMediaItemMapper" />
    public sealed class MediaItemMapper : BaseMapper<MediaItem>, IMediaItemMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaItemMapper"/> class.
        /// </summary>
        public MediaItemMapper(ViewModelServiceContainer container, IValidator<MediaItem> validator)
            : base(container, validator)
        {
            InitializeMapper();
        }

        protected override void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
            });

            config.AssertConfigurationIsValid();
            Mapper = config.CreateMapper();
        }

        public MediaItem GetNewMediaItem(int sequence, Playlist playlist)
        {
            return new MediaItem(GetDataNewMediaItem(playlist.Model), _validator, _messenger)
            {
                Title = _translationService.Translate(nameof(Resources.New)),
                Description = string.Empty,
                Playlist = playlist,
            };
        }

        public MediaItemModel GetDataNewMediaItem(PlaylistModel playlist)
        {
            return new MediaItemModel()
            {
                Title = _translationService.Translate(nameof(Resources.New)),
                Description = string.Empty,
                Playlist = playlist,
            };
        }

        public MediaItem Get(MediaItemModel mediaitem)
        {
            return new MediaItem(mediaitem, _validator, _messenger);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="mediaitem">The mediaitem.</param>
        /// <returns></returns>
        public MediaItemModel GetData(MediaItem mediaitem)
        {
            return mediaitem.Model;
        }
    }
}
