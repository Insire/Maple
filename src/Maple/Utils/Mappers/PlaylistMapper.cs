using AutoMapper;
using FluentValidation;
using Maple.Core;
using Maple.Localization.Properties;
using System;

namespace Maple
{
    /// <summary>
    /// Provides logic to map between different domain objects of the Playlisttype
    /// </summary>
    /// <seealso cref="Maple.IPlaylistMapper" />
    public class PlaylistMapper : BaseMapper<Playlist>, IPlaylistMapper
    {
        private readonly IMediaItemMapper _mediaItemMapper;
        private readonly DialogViewModel _dialogViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistMapper"/> class.
        /// </summary>
        /// <param name="dialogViewModel">The dialog view model.</param>
        public PlaylistMapper(ViewModelServiceContainer container, IMediaItemMapper mediaItemMapper, DialogViewModel dialogViewModel, IValidator<Playlist> validator)
            : base(container, validator)
        {
            _dialogViewModel = dialogViewModel ?? throw new ArgumentNullException(nameof(dialogViewModel), $"{nameof(dialogViewModel)} {Resources.IsRequired}");
            _mediaItemMapper = mediaItemMapper ?? throw new ArgumentNullException(nameof(mediaItemMapper), $"{nameof(mediaItemMapper)} {Resources.IsRequired}");

            InitializeMapper();
        }

        protected override void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        public Playlist GetNewPlaylist(int sequence)
        {
            return new Playlist(_container, _validator, _dialogViewModel, new Data.Playlist
            {
                Title = _translationService.Translate(nameof(Resources.New)),
                Description = string.Empty,
                Location = string.Empty,
                RepeatMode = 0,
                IsShuffeling = false,
                Sequence = sequence,
            });
        }

        public Playlist Get(Data.Playlist model)
        {
            var result = new Playlist(_container, _validator, _dialogViewModel, model);
            result.AddRange(_mediaItemMapper.GetMany(model.MediaItems));
            return result;
        }

        public Data.Playlist GetData(Playlist mediaitem)
        {
            return mediaitem.Model;
        }
    }
}
