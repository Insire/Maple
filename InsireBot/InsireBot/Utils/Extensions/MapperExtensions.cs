using AutoMapper;
using Maple.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Maple
{
    public static class MapperExtensions
    {
        public static IEnumerable<MediaItem> GetMany(this IMediaItemMapper mapper, IEnumerable<Core.MediaItem> items)
        {
            return items.ForEach(mapper.Get);
        }

        public static IEnumerable<MediaItem> GetMany(this IMediaItemMapper mapper, IEnumerable<Data.MediaItem> items)
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

        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(this IMappingExpression<TSource,
            TDestination> map,
            Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}
