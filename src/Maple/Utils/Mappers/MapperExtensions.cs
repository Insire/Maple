using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using AutoMapper;

using Maple.Core;

namespace Maple
{
    /// <summary>
    /// Extensions for <see cref="MediaItem"/>
    /// </summary>
    public static class MapperExtensions
    {
        public static IEnumerable<MediaItem> GetMany(this IMediaItemMapper mapper, IEnumerable<Domain.MediaItemModel> items)
        {
            return items.ForEach(mapper.Get);
        }

        public static IEnumerable<Playlist> GetMany(this IPlaylistMapper mapper, IEnumerable<Domain.PlaylistModel> items)
        {
            return items.ForEach(mapper.Get);
        }

        public static IEnumerable<MediaPlayer> GetMany(this IMediaPlayerMapper mapper, IEnumerable<Domain.MediaPlayerModel> items)
        {
            return items.ForEach(mapper.Get);
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
