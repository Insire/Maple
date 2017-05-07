using AutoMapper;
using Maple.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Maple
{
    /// <summary>
    /// Extensions for <see cref="MediaItem"/>
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IEnumerable<MediaItem> GetMany(this IMediaItemMapper mapper, IEnumerable<Core.MediaItem> items)
        {
            return items.ForEach(mapper.Get);
        }

        public static IEnumerable<MediaItem> GetMany(this IMediaItemMapper mapper, IEnumerable<Data.MediaItem> items, int playlistId)
        {
            return items.ForEach(p =>
            {
                var item = mapper.Get(p);
                item.PlaylistId = playlistId;
                return item;
            });
        }

        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IEnumerable<MediaItem> GetMany(this IMediaItemMapper mapper, IEnumerable<Data.MediaItem> items)
        {
            return items.ForEach(mapper.Get);
        }

        public static IList<MediaItem> GetManyAsList(this IMediaItemMapper mapper, IEnumerable<Data.MediaItem> items)
        {
            return items.ForEach(mapper.Get).ToList();
        }

        /// <summary>
        /// Gets the many data.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IEnumerable<Data.MediaItem> GetManyData(this IMediaItemMapper mapper, IEnumerable<Core.MediaItem> items)
        {
            return items.ForEach(mapper.GetData);
        }

        /// <summary>
        /// Gets the many core.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IEnumerable<Core.MediaItem> GetManyCore(this IMediaItemMapper mapper, IEnumerable<Data.MediaItem> items)
        {
            return items.ForEach(mapper.GetCore);
        }

        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IEnumerable<Playlist> GetMany(this IPlaylistMapper mapper, IEnumerable<Core.Playlist> items)
        {
            return items.ForEach(mapper.Get);
        }

        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IEnumerable<Playlist> GetMany(this IPlaylistMapper mapper, IEnumerable<Data.Playlist> items)
        {
            return items.ForEach(mapper.Get);
        }

        /// <summary>
        /// Gets the many data.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IEnumerable<Data.Playlist> GetManyData(this IPlaylistMapper mapper, IEnumerable<Core.Playlist> items)
        {
            return items.ForEach(mapper.GetData);
        }

        /// <summary>
        /// Gets the many data.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IEnumerable<Data.Playlist> GetManyData(this IPlaylistMapper mapper, IEnumerable<Playlist> items)
        {
            return items.ForEach(mapper.GetData);
        }

        /// <summary>
        /// Gets the many core.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IEnumerable<Core.Playlist> GetManyCore(this IPlaylistMapper mapper, IEnumerable<Data.Playlist> items)
        {
            return items.ForEach(mapper.GetCore);
        }

        /// <summary>
        /// Gets the many core.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IEnumerable<Core.Playlist> GetManyCore(this IPlaylistMapper mapper, IEnumerable<Playlist> items)
        {
            return items.ForEach(mapper.GetCore);
        }

        /// <summary>
        /// Ignores the specified selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="map">The map.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(this IMappingExpression<TSource,
            TDestination> map,
            Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}
