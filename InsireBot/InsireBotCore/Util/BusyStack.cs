using System;
using System.Collections.Concurrent;
using GalaSoft.MvvmLight;

namespace InsireBotCore
{
    /// <summary>
    /// BusyStack will handle notifying a viewmodel on if and how many actions are pending
    /// </summary>
    public class BusyStack : ObservableObject
    {
        private ConcurrentBag<Guid> _items;
        public ConcurrentBag<Guid> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }

        public BusyStack()
        {
            Items = new ConcurrentBag<Guid>();
        }

        public bool Pull()
        {
            Guid result;
            return Items.TryTake(out result);
        }

        /// <summary>
        /// checks if the item is still on the "stack", without removing it
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Peek(Guid item)
        {
            Guid result;
            return Items.TryPeek(out result);
        }
        /// <summary>
        /// adds a new item on the stack and returns a reference to it
        /// </summary>
        /// <returns></returns>
        public void Push()
        {
            Items.Add(Guid.NewGuid());
        }

        public bool IsEmpty()
        {
            return Items.Count == 0;
        }

        public bool HasItems()
        {
            return Items.Count > 0;
        }
    }
}
