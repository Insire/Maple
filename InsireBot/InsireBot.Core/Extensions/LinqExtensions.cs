using System;
using System.Collections.Generic;
using System.Linq;

namespace Maple.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Randoms the specified items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Random can't generate a result. There were no valid parameters - items</exception>
        public static T Random<T>(this IEnumerable<T> items)
        {
            if (items?.Any() == false)
                throw new ArgumentNullException("Random can't generate a result. There were no valid parameters", nameof(items));


            // note: creating a Random instance each call may not be correct for you,
            // consider a thread-safe static instance
            var r = new Random();
            var list = items as IList<T> ?? items.ToList();
            return list.Count == 0 ? default(T) : list[r.Next(0, list.Count)];
        }

        /// <summary>
        /// determines if some of the submitted items can be added to the collection and returns a collection of these items
        /// </summary>
        /// <param name="baseCollection">The base collection.</param>
        /// <param name="newItems">The new items.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// CanAddRange can't return a result. There were no valid parameters - newItems
        /// or
        /// CanAddRange can't return a result. There were no valid parameters - baseCollection
        /// </exception>
        public static IEnumerable<IIdentifier> CanAddRange(this IList<IIdentifier> baseCollection, IList<IIdentifier> newItems)
        {
            if (newItems?.Any() != true)
                throw new ArgumentNullException("CanAddRange can't return a result. There were no valid parameters", nameof(newItems));

            if (baseCollection == null)
                throw new ArgumentNullException("CanAddRange can't return a result. There were no valid parameters", nameof(baseCollection));

            var excludedIDs = new HashSet<int>(baseCollection.Select(p => p.Id));
            return newItems.Where(p => !excludedIDs.Contains(p.Id));
        }

        /// <summary>
        /// determines if some of the submitted items can be added to the collection and returns a collection of these items
        /// </summary>
        /// <param name="baseCollection">The base collection.</param>
        /// <param name="newItems">The new items.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// CanAddRange can't return a result. There were no valid parameters - newItems
        /// or
        /// CanAddRange can't return a result. There were no valid parameters - baseCollection
        /// </exception>
        public static IEnumerable<IMediaItem> CanAddRange(this IList<IMediaItem> baseCollection, IList<IMediaItem> newItems)
        {
            if (newItems?.Any() != true)
                throw new ArgumentNullException("CanAddRange can't return a result. There were no valid parameters", nameof(newItems));

            if (baseCollection == null)
                throw new ArgumentNullException("CanAddRange can't return a result. There were no valid parameters", nameof(baseCollection));

            var excludedIDs = new HashSet<int>(baseCollection.Select(p => p.Id));
            return newItems.Where(p => !excludedIDs.Contains(p.Id));
        }

        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="sourceItems">The source items.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static IEnumerable<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> sourceItems, Func<TSource, TResult> action)
        {
            foreach (var item in sourceItems)
                yield return action(item);
        }

        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="sourceItems">The source items.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> sourceItems, Action<TSource> action)
        {
            foreach (var item in sourceItems)
                action(item);
        }
    }
}
