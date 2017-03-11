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
                cfg.CreateMap<Core.MediaItem, Data.MediaItem>()
                    .Ignore(p => p.IsDeleted)
                    .Ignore(p => p.IsNew);
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        public MediaItem Get(Core.MediaItem mediaitem)
        {
            return new MediaItem(GetData(mediaitem));
        }

        public MediaItem Get(Data.MediaItem mediaitem)
        {
            return new MediaItem(mediaitem);
        }

        public Core.MediaItem GetCore(Data.MediaItem mediaitem)
        {
            return _mapper.Map<Data.MediaItem, Core.MediaItem>(mediaitem);
        }

        public Core.MediaItem GetCore(MediaItem mediaitem)
        {
            return GetCore(mediaitem.Model);
        }

        public Data.MediaItem GetData(MediaItem mediaitem)
        {
            return mediaitem.Model;
        }

        public Data.MediaItem GetData(Core.MediaItem mediaitem)
        {
            return _mapper.Map<Core.MediaItem, Data.MediaItem>(mediaitem);
        }
    }
}
