using Maple.Data;
using System.Collections.Generic;

namespace Maple.Core
{
    /// <summary>
    /// ListViewModel implementation for ObservableObjects related to the DataAccessLayer (DB)
    /// </summary>
    /// <typeparam name="T">a wrapper class implementing <see cref="BaseViewModel" /></typeparam>
    /// <typeparam name="S">a DTO implementing <see cref="BaseObject" /></typeparam>
    /// <seealso cref="Maple.Core.BaseListViewModel{T}" />
    public abstract class BaseDataListViewModel<T, S> : BaseListViewModel<T> where T : BaseViewModel<S> where S : BaseObject
    {
        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Remove(T item)
        {
            item.Model.IsDeleted = true;
            base.Remove(item);
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">The items.</param>
        public override void RemoveRange(IEnumerable<T> items)
        {
            base.RemoveRange(items);

            items.ForEach(p => p.Model.IsDeleted = true);
        }
    }
}
