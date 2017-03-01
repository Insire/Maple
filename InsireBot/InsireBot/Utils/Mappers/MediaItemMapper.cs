using AutoMapper;

namespace Maple
{
    public class MediaItemMapper : IMediaItemMapper
    {
        private readonly IMapper _mapper;

        public MediaItemMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.MediaItem, Core.MediaItem>();
                cfg.CreateMap<Core.MediaItem, Data.MediaItem>();
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        public MediaItemViewModel Get(Core.MediaItem mediaitem)
        {
            return new MediaItemViewModel(GetData(mediaitem));
        }

        public MediaItemViewModel Get(Data.MediaItem mediaitem)
        {
            return new MediaItemViewModel(mediaitem);
        }

        public Core.MediaItem GetCore(Data.MediaItem mediaitem)
        {
            return _mapper.Map<Data.MediaItem, Core.MediaItem>(mediaitem);
        }

        public Core.MediaItem GetCore(MediaItemViewModel mediaitem)
        {
            return GetCore(mediaitem.Model);
        }

        public Data.MediaItem GetData(MediaItemViewModel mediaitem)
        {
            return mediaitem.Model;
        }

        public Data.MediaItem GetData(Core.MediaItem mediaitem)
        {
            return _mapper.Map<Core.MediaItem, Data.MediaItem>(mediaitem);
        }
    }
}
