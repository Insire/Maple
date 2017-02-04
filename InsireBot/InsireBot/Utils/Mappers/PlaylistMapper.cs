using AutoMapper;
using System;

namespace Maple
{
    public class PlaylistMapper : IPlaylistMapper
    {
        private readonly IMapper _mapper;

        public PlaylistMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.Playlist, Core.Playlist>();
                cfg.CreateMap<Core.Playlist, Data.Playlist>();

                cfg.CreateMap<Data.Playlist, Playlist>();
                cfg.CreateMap<Core.Playlist, Playlist>();

                cfg.CreateMap<Playlist, Core.Playlist>();
                cfg.CreateMap<Playlist, Data.Playlist>();
            });

            _mapper = config.CreateMapper();
        }


        public Playlist Get(Core.Playlist mediaitem)
        {
            throw new NotImplementedException();
        }

        public Playlist Get(Data.Playlist mediaitem)
        {
            throw new NotImplementedException();
        }

        public Core.Playlist GetCore(Data.Playlist mediaitem)
        {
            throw new NotImplementedException();
        }

        public Core.Playlist GetCore(Playlist mediaitem)
        {
            throw new NotImplementedException();
        }

        public Data.Playlist GetData(Playlist mediaitem)
        {
            throw new NotImplementedException();
        }

        public Data.Playlist GetData(Core.Playlist mediaitem)
        {
            throw new NotImplementedException();
        }
    }
}
