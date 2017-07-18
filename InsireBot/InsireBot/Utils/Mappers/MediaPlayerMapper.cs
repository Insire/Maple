using AutoMapper;
using FluentValidation;
using Maple.Core;
using Maple.Localization.Properties;
using System;

namespace Maple
{
    public class MediaPlayerMapper : BaseMapper<MediaPlayer>, IMediaPlayerMapper
    {
        private readonly IMediaPlayer _mediaPlayer;
        private readonly AudioDevices _devices;

        public MediaPlayerMapper(ViewModelServiceContainer container, IMediaPlayer mediaPlayer, AudioDevices devices, IValidator<MediaPlayer> validator)
            : base(container, validator)
        {
            _mediaPlayer = mediaPlayer ?? throw new ArgumentNullException(nameof(mediaPlayer));
            _devices = devices ?? throw new ArgumentNullException(nameof(devices));

            InitializeMapper();
        }

        protected override void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg => // TODO
            {
                cfg.CreateMap<Data.MediaPlayer, Core.MediaPlayer>()
                    .Ignore(p => p.Playlist);

                cfg.CreateMap<Core.MediaPlayer, Data.MediaPlayer>()
                    .Ignore(p => p.Playlist)
                    .Ignore(p => p.IsDeleted)
                    .Ignore(p => p.IsNew);
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        public MediaPlayer GetNewMediaPlayer(int sequence, Playlist playlist = null)
        {
            return new MediaPlayer(_container, _mediaPlayer, _validator, _devices, playlist, new Data.MediaPlayer()
            {
                Sequence = sequence,
                IsPrimary = false,
                Name = _translationService.Translate(nameof(Resources.New)),
                Playlist = playlist?.Model,
            });
        }

        public MainMediaPlayer GetMain(Data.MediaPlayer player, Playlist playlist)
        {
            return new MainMediaPlayer(_container, _mediaPlayer, _validator, _devices, playlist, player);
        }

        public MediaPlayer Get(Data.MediaPlayer player, Playlist playlist)
        {
            return new MediaPlayer(_container, _mediaPlayer, _validator, _devices, playlist, player);
        }

        public MediaPlayer Get(Data.MediaPlayer model)
        {
            throw new NotImplementedException();
            return new MediaPlayer(_container, _mediaPlayer, _validator, _devices, null, model); //TODO
        }

        public MediaPlayer Get(Core.MediaPlayer dto)
        {
            throw new NotImplementedException();
            return new MediaPlayer(_container, _mediaPlayer, _validator, _devices, null, GetData(dto)); //TODO
        }

        public Data.MediaPlayer GetData(MediaPlayer viewModel)
        {
            return viewModel.Model;
        }

        public Data.MediaPlayer GetData(Core.MediaPlayer dto)
        {
            return _mapper.Map<Core.MediaPlayer, Data.MediaPlayer>(dto);
        }

        public Core.MediaPlayer GetCore(MediaPlayer viewModel)
        {
            return GetCore(viewModel.Model);
        }

        public Core.MediaPlayer GetCore(Data.MediaPlayer model)
        {
            return _mapper.Map<Data.MediaPlayer, Core.MediaPlayer>(model);
        }
    }
}
