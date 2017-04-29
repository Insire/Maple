using AutoMapper;
using Maple.Core;
using System;

namespace Maple
{
    public class MediaPlayerMapper : IMediaPlayerMapper
    {
        private readonly IMapper _mapper;
        private readonly ITranslationService _translator;
        private readonly IMediaPlayer _mediaPlayer;

        private readonly AudioDevices _devices;

        public MediaPlayerMapper(ITranslationService translator, IMediaPlayer mediaPlayer, AudioDevices devices)
        {
            _translator = translator ?? throw new ArgumentNullException(nameof(translator));
            _mediaPlayer = mediaPlayer ?? throw new ArgumentNullException(nameof(mediaPlayer));
            _devices = devices ?? throw new ArgumentNullException(nameof(devices));

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Data.MediaPlayer, MediaPlayer>();
            //    cfg.CreateMap<Core.MediaPlayer, Data.MediaPlayer>()
            //        .Ignore(p => p.IsDeleted)
            //        .Ignore(p => p.IsNew)
            //        .Ignore(p => p.Playlist);
            //});

            //config.AssertConfigurationIsValid();
            //_mapper = config.CreateMapper();
        }

        public MediaPlayer Get(Data.MediaPlayer player, Playlist playlist)
        {
            return new MediaPlayer(_translator, _mediaPlayer, player, playlist, _devices);
        }
    }
}
