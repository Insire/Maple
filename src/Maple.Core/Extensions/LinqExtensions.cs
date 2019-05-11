using System;
using System.Collections.Generic;
using System.Linq;
using Maple.Domain;
using Maple.Localization.Properties;
using MvvmScarletToolkit;

namespace Maple.Core
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Randoms the specified items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Random can't generate a result. There were no valid parameters - items</exception>
        public static T Random<T>(this IEnumerable<T> baseCollection)
        {
            if (baseCollection == null)
                throw new ArgumentNullException(nameof(baseCollection), $"{nameof(baseCollection)} {Resources.IsRequired}");

            // note: creating a Random instance each call may not be correct for you,
            // consider a thread-safe static instance
            var r = new Random();
            var list = baseCollection as IList<T> ?? baseCollection.ToList();
            return list.Count == 0 ? default : list[r.Next(0, list.Count)];
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
            if (baseCollection == null)
                throw new ArgumentNullException(nameof(baseCollection), $"{nameof(baseCollection)} {Resources.IsRequired}");

            if (newItems?.Any() != true)
                throw new ArgumentNullException(nameof(newItems), $"{nameof(newItems)} {Resources.IsRequired}");

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
            if (baseCollection == null)
                throw new ArgumentNullException(nameof(baseCollection), $"{nameof(baseCollection)} {Resources.IsRequired}");

            if (newItems?.Any() != true)
                throw new ArgumentNullException(nameof(newItems), $"{nameof(newItems)} {Resources.IsRequired}");

            var excludedIDs = new HashSet<int>(baseCollection.Select(p => p.Id));
            return newItems.Where(p => !excludedIDs.Contains(p.Id));
        }

        public static IEnumerable<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> baseCollection, Func<TSource, TResult> action)
        {
            if (baseCollection == null)
                throw new ArgumentNullException(nameof(baseCollection), $"{nameof(baseCollection)} {Resources.IsRequired}");

            if (action == null)
                throw new ArgumentNullException(nameof(action), $"{nameof(action)} {Resources.IsRequired}");

            foreach (var item in baseCollection)
                yield return action(item);
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> baseCollection, Action<TSource> action)
        {
            if (baseCollection == null)
                throw new ArgumentNullException(nameof(baseCollection), $"{nameof(baseCollection)} {Resources.IsRequired}");

            if (action == null)
                throw new ArgumentNullException(nameof(action), $"{nameof(action)} {Resources.IsRequired}");

            foreach (var item in baseCollection)
                action(item);
        }

        public static IEnumerable<TResult> ForEach<TSource, TResult>(this RangeObservableCollection<TSource> baseCollection, Func<TSource, TResult> action)
        {
            if (baseCollection == null)
                throw new ArgumentNullException(nameof(baseCollection), $"{nameof(baseCollection)} {Resources.IsRequired}");

            if (action == null)
                throw new ArgumentNullException(nameof(action), $"{nameof(action)} {Resources.IsRequired}");

            foreach (var item in baseCollection)
                yield return action(item);
        }

        public static void ForEach<TSource>(this RangeObservableCollection<TSource> baseCollection, Action<TSource> action)
        {
            if (baseCollection == null)
                throw new ArgumentNullException(nameof(baseCollection), $"{nameof(baseCollection)} {Resources.IsRequired}");

            if (action == null)
                throw new ArgumentNullException(nameof(action), $"{nameof(action)} {Resources.IsRequired}");

            foreach (var item in baseCollection)
                action(item);
        }
    }
}
