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
        public EventHandler StackCountChanged;

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

        /// <summary>
        /// Tries to take an item from the stack and returns if that action was successful
        /// </summary>
        /// <returns></returns>
        public bool Pull()
        {
            Guid guid;
            var result = Items.TryTake(out guid);

            if (result)
                StackCountChanged?.Invoke(this, new EventArgs());

            return result;
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
        /// adds a new item on the stack
        /// </summary>
        /// <returns></returns>
        public void Push()
        {
            Items.Add(Guid.NewGuid());
            StackCountChanged?.Invoke(this, new EventArgs());
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
