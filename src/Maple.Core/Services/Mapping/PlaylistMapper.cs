﻿using System;

using AutoMapper;

using FluentValidation;

using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    /// <summary>
    /// Provides logic to map between different domain objects of the Playlisttype
    /// </summary>
    /// <seealso cref="Maple.IPlaylistMapper" />
    public sealed class PlaylistMapper : BaseMapper<Playlist>, IPlaylistMapper
    {
        private readonly IMediaItemMapper _mediaItemMapper;
        private readonly IDialogViewModel _dialogViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistMapper"/> class.
        /// </summary>
        /// <param name="dialogViewModel">The dialog view model.</param>
        public PlaylistMapper(ViewModelServiceContainer container, IMediaItemMapper mediaItemMapper, IDialogViewModel dialogViewModel, IValidator<Playlist> validator)
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
            Mapper = config.CreateMapper();
        }

        public Playlist GetNewPlaylist()
        {
            return new Playlist(_container, _validator, _dialogViewModel, _mediaItemMapper, new PlaylistModel
            {
                Title = _translationService.Translate(nameof(Resources.New)),
                Description = string.Empty,
                Location = string.Empty,
                RepeatMode = 0,
                IsShuffeling = false,
                Sequence = 0,
            });
        }

        public Playlist Get(PlaylistModel model)
        {
            return new Playlist(_container, _validator, _dialogViewModel, _mediaItemMapper, model);
        }

        public PlaylistModel GetData(Playlist viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            return viewModel.Model;
        }
    }
}
