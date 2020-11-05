using System;
using System.Collections.Generic;
using System.Linq;

namespace Maple
{
    public static class LinqExtensions
    {
        public static T Random<T>(this IEnumerable<T> baseCollection)
        {
            if (baseCollection is null)
                throw new ArgumentNullException(nameof(baseCollection));

            // note: creating a Random instance each call may not be correct for you,
            // consider a thread-safe static instance
            var r = new Random();
            var list = baseCollection as IList<T> ?? baseCollection.ToList();
            return list.Count == 0 ? default : list[r.Next(0, list.Count)];
        }
    }
}
