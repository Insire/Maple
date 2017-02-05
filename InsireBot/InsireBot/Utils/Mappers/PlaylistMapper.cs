using AutoMapper;
using Maple.Data;

namespace Maple
{
    public class PlaylistMapper : IPlaylistMapper
    {
        private readonly IMapper _mapper;
        private readonly IPlaylistsRepository _playlistsRepository;
        private readonly IMediaItemRepository _mediaItemRepository;
        private DialogViewModel _dialogViewModel;

        public PlaylistMapper(IPlaylistsRepository playlistsRepository, IMediaItemRepository mediaItemRepository, DialogViewModel dialogViewModel)
        {
            _playlistsRepository = playlistsRepository;
            _mediaItemRepository = mediaItemRepository;
            _dialogViewModel = dialogViewModel;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.Playlist, Core.Playlist>();
                cfg.CreateMap<Core.Playlist, Data.Playlist>()
                    .ForMember(nameof(Data.Playlist.IsDeleted), opt => opt.Ignore());
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        public Playlist Get(Core.Playlist mediaitem)
        {
            return new Playlist(_playlistsRepository, _mediaItemRepository, _dialogViewModel, GetData(mediaitem));
        }

        public Playlist Get(Data.Playlist mediaitem)
        {
            return new Playlist(_playlistsRepository, _mediaItemRepository, _dialogViewModel, mediaitem);
        }

        public Core.Playlist GetCore(Data.Playlist mediaitem)
        {
            return _mapper.Map<Data.Playlist, Core.Playlist>(mediaitem);
        }

        public Core.Playlist GetCore(Playlist mediaitem)
        {
            return GetCore(mediaitem.Model);
        }

        public Data.Playlist GetData(Playlist mediaitem)
        {
            return mediaitem.Model;
        }

        public Data.Playlist GetData(Core.Playlist mediaitem)
        {
            return _mapper.Map<Core.Playlist, Data.Playlist>(mediaitem);
        }
    }
}
