using Maple.Core;
using System.Collections.Generic;

namespace Maple
{
    public static class MapperExtensions
    {
        public static IEnumerable<MediaItemViewModel> GetMany(this IMediaItemMapper mapper, IEnumerable<Core.MediaItem> items)
        {
            return items.ForEach(mapper.Get);
        }

        public static IEnumerable<MediaItemViewModel> GetMany(this IMediaItemMapper mapper, IEnumerable<Data.MediaItem> items)
        {
            return items.ForEach(mapper.Get);
        }

        public static IEnumerable<Data.MediaItem> GetManyData(this IMediaItemMapper mapper, IEnumerable<Core.MediaItem> items)
        {
            return items.ForEach(mapper.GetData);
        }

        public static IEnumerable<Core.MediaItem> GetManyCore(this IMediaItemMapper mapper, IEnumerable<Data.MediaItem> items)
        {
            return items.ForEach(mapper.GetCore);
        }

        public static IEnumerable<Playlist> GetMany(this IPlaylistMapper mapper, IEnumerable<Core.Playlist> items)
        {
            return items.ForEach(mapper.Get);
        }

        public static IEnumerable<Playlist> GetMany(this IPlaylistMapper mapper, IEnumerable<Data.Playlist> items)
        {
            return items.ForEach(mapper.Get);
        }

        public static IEnumerable<Data.Playlist> GetManyData(this IPlaylistMapper mapper, IEnumerable<Core.Playlist> items)
        {
            return items.ForEach(mapper.GetData);
        }

        public static IEnumerable<Data.Playlist> GetManyData(this IPlaylistMapper mapper, IEnumerable<Playlist> items)
        {
            return items.ForEach(mapper.GetData);
        }

        public static IEnumerable<Core.Playlist> GetManyCore(this IPlaylistMapper mapper, IEnumerable<Data.Playlist> items)
        {
            return items.ForEach(mapper.GetCore);
        }

        public static IEnumerable<Core.Playlist> GetManyCore(this IPlaylistMapper mapper, IEnumerable<Playlist> items)
        {
            return items.ForEach(mapper.GetCore);
        }
    }
}
