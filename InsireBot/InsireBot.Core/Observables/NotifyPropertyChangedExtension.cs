using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maple.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class NotifyPropertyChangedExtension
    {
        /// <summary>
        /// Mutates the verbose.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="field">The field.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="raise">The raise.</param>
        /// <param name="propertyName">Name of the property.</param>
        public static void MutateVerbose<TField>(this INotifyPropertyChanged instance, ref TField field, TField newValue, Action<PropertyChangedEventArgs> raise, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TField>.Default.Equals(field, newValue))
                return;

            field = newValue;
            raise?.Invoke(new PropertyChangedEventArgs(propertyName));
        }
    }
}
