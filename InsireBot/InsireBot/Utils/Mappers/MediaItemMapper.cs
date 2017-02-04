using AutoMapper;
using Maple.Core;
using Maple.Data;

namespace Maple
{
    public class MediaItemMapper : IMediaItemMapper
    {
        private readonly IMapper _mapper;
        private readonly IBotLog _log;
        private readonly IMediaItemRepository _mediaItemRepository;


        public MediaItemMapper(IBotLog log, IMediaItemRepository mediaItemRepository)
        {
            _log = log;
            _mediaItemRepository = mediaItemRepository;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.MediaItem, Core.MediaItem>();
                cfg.CreateMap<Core.MediaItem, Data.MediaItem>()
                    .ForMember(nameof(Data.MediaItem.IsDeleted), opt => opt.Ignore());
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        public MediaItemViewModel Get(Core.MediaItem mediaitem)
        {
            return new MediaItemViewModel(_log, _mediaItemRepository, GetData(mediaitem));
        }

        public MediaItemViewModel Get(Data.MediaItem mediaitem)
        {
            return new MediaItemViewModel(_log, _mediaItemRepository, mediaitem);
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
