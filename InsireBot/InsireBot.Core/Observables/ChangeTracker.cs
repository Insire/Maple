using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Maple.Core
{
    public class ChangeTracker
    {
        private readonly IDictionary<string, ChangeGroup> _changes;
        public bool HasChanged => _changes.Any();

        public ChangeTracker()
        {
            _changes = new Dictionary<string, ChangeGroup>();
        }

        /// <summary>
        /// Updates the current state of the ChangeTracker
        /// </summary>
        /// <param name="changedValue"></param>
        /// <param name="propertyName"></param>
        /// <returns>true, if the HasChanged Property changed</returns>
        public bool Update(object changedValue, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (_changes.ContainsKey(propertyName))
            {
                var group = _changes[propertyName];
                // change back to original value
                if (ReferenceEquals(group.OriginalValue, changedValue))
                {
                    _changes.Remove(propertyName);
                    return true;
                }
                else
                {
                    // change to new value
                    if (!ReferenceEquals(group.NewValue, changedValue))
                    {
                        group.NewValue = changedValue;
                        _changes[propertyName] = group;
                        return true;
                    }
                }

                return false;
            }
            else
            {
                // add new value
                var group = new ChangeGroup
                {
                    OriginalValue = changedValue,
                    NewValue = changedValue,
                };
                _changes.Add(propertyName, group);
                return true;
            }
        }
    }

    public class ChangeGroup
    {
        public object OriginalValue { get; set; }
        public object NewValue { get; set; }
    }
}
