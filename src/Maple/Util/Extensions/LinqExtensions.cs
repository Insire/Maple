using System;
using System.Collections.Generic;
using System.Linq;

namespace Maple
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
                throw new ArgumentNullException(nameof(baseCollection));

            // note: creating a Random instance each call may not be correct for you,
            // consider a thread-safe static instance
            var r = new Random();
            var list = baseCollection as IList<T> ?? baseCollection.ToList();
            return list.Count == 0 ? default : list[r.Next(0, list.Count)];
        }
    }
}
