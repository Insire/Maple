using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsireBot.Core;
using InsireBot.Data;

namespace InsireBot
{
    public class PlaylistMapper : IPlaylistMapper
    {
        public PlaylistViewModel Get(Core.Playlist mediaitem)
        {
            throw new NotImplementedException();
        }

        public PlaylistViewModel Get(Data.Playlist mediaitem)
        {
            throw new NotImplementedException();
        }

        public Core.Playlist GetCore(Data.Playlist mediaitem)
        {
            throw new NotImplementedException();
        }

        public Core.Playlist GetCore(PlaylistViewModel mediaitem)
        {
            throw new NotImplementedException();
        }

        public Data.Playlist GetData(PlaylistViewModel mediaitem)
        {
            throw new NotImplementedException();
        }

        public Data.Playlist GetData(Core.Playlist mediaitem)
        {
            throw new NotImplementedException();
        }
    }
}
