using System;
using AutoMapper;
using FluentValidation;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    public sealed class MediaPlayerMapper : BaseMapper<MediaPlayer>, IMediaPlayerMapper
    {
        private readonly IMediaPlayer _mediaPlayer;
        private readonly AudioDevices _devices;

        public MediaPlayerMapper(ViewModelServiceContainer container, IMediaPlayer mediaPlayer, AudioDevices devices, IValidator<MediaPlayer> validator)
            : base(container, validator)
        {
            _mediaPlayer = mediaPlayer ?? throw new ArgumentNullException(nameof(mediaPlayer), $"{nameof(mediaPlayer)} {Resources.IsRequired}");
            _devices = devices ?? throw new ArgumentNullException(nameof(devices), $"{nameof(devices)} {Resources.IsRequired}");

            InitializeMapper();
        }

        protected override void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg => // TODO mapper configuration
            {
            });

            config.AssertConfigurationIsValid();
            Mapper = config.CreateMapper();
        }

        public MediaPlayer GetNewMediaPlayer(int sequence, Playlist playlist = null)
        {
            return new MediaPlayer(_container, _mediaPlayer, _validator, _devices, playlist, new MediaPlayerModel()
            {
                Sequence = sequence,
                IsPrimary = false,
                Name = _translationService.Translate(nameof(Resources.New)),
                Playlist = playlist?.Model,
            });
        }

        public MediaPlayer Get(MediaPlayerModel model)
        {
            throw new NotImplementedException(); // by design
        }

        public MediaPlayerModel GetData(MediaPlayer viewModel)
        {
            return viewModel.Model;
        }

        public MainMediaPlayer GetMain(MediaPlayerModel player, Playlist playlist)
        {
            return new MainMediaPlayer(_container, _mediaPlayer, _validator, _devices, playlist, player);
        }

        public MediaPlayer Get(MediaPlayerModel player, Playlist playlist)
        {
            return new MediaPlayer(_container, _mediaPlayer, _validator, _devices, playlist, player);
        }
    }
}
