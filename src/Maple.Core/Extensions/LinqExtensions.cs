using System;
using System.Collections.Generic;
using System.Linq;

using Maple.Domain;
using Maple.Localization.Properties;

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
            return list.Count == 0 ? default(T) : list[r.Next(0, list.Count)];
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

        public static IEnumerable<TResult> ForEach<TSource, TResult>(this IRangeObservableCollection<TSource> baseCollection, Func<TSource, TResult> action)
        {
            if (baseCollection == null)
                throw new ArgumentNullException(nameof(baseCollection), $"{nameof(baseCollection)} {Resources.IsRequired}");

            if (action == null)
                throw new ArgumentNullException(nameof(action), $"{nameof(action)} {Resources.IsRequired}");

            foreach (var item in baseCollection)
                yield return action(item);
        }

        public static void ForEach<TSource>(this IRangeObservableCollection<TSource> baseCollection, Action<TSource> action)
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
