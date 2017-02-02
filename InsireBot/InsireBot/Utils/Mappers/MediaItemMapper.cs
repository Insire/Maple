using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maple.Core;
using Maple.Data;

namespace Maple
{
    public class MediaItemMapper : IMediaItemMapper
    {
        public MediaItemViewModel Get(Core.MediaItem mediaitem)
        {
            throw new NotImplementedException();
        }

        public MediaItemViewModel Get(Data.MediaItem mediaitem)
        {
            throw new NotImplementedException();
        }

        public Core.MediaItem GetCore(Data.MediaItem mediaitem)
        {
            throw new NotImplementedException();
        }

        public Core.MediaItem GetCore(MediaItemViewModel mediaitem)
        {
            throw new NotImplementedException();
        }

        public Data.MediaItem GetData(MediaItemViewModel mediaitem)
        {
            throw new NotImplementedException();
        }

        public Data.MediaItem GetData(Core.MediaItem mediaitem)
        {
            throw new NotImplementedException();
        }
    }
}
