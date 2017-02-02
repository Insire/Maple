using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maple.Core;
using Maple.Data;

namespace Maple
{
    public class PlaylistMapper : IPlaylistMapper
    {
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
