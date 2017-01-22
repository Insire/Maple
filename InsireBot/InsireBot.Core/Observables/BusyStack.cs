using System;
using System.Collections.Concurrent;

namespace InsireBot.Core
{
    public class BusyStack : ObservableObject
    {
        private ConcurrentBag<BusyToken> _items;
        protected ConcurrentBag<BusyToken> Items
        {
            get { return _items; }
            set { SetValue(ref _items, value, InvokeOnChanged); }
        }

        private Action<bool> _onChanged;
        public Action<bool> OnChanged
        {
            get { return _onChanged; }
            set { SetValue(ref _onChanged, value); }
        }

        public BusyStack()
        {
            Items = new ConcurrentBag<BusyToken>();
        }

        /// <summary>
        /// Tries to take an item from the stack and returns true if that action was successful
        /// </summary>
        /// <returns></returns>
        public bool Pull()
        {
            var token = default(BusyToken);
            var result = Items.TryTake(out token);

            if (result)
                InvokeOnChanged();

            return result;
        }

        /// <summary>
        /// Adds a new <see cref="BusyToken"/> to the Stack
        /// </summary>
        public void Push(BusyToken token)
        {
            Items.Add(token);

            InvokeOnChanged();
        }

        public bool HasItems()
        {
            var token = default(BusyToken);
            return Items?.TryPeek(out token) ?? false;
        }

        /// <summary>
        /// Returns a new <see cref="BusyToken"/> thats associated with <see cref="this"/> instance of a <see cref="BusyStack"/>
        /// </summary>
        /// <returns>a new <see cref="BusyToken"/></returns>
        public BusyToken GetToken()
        {
            return new BusyToken(this);
        }

        private void InvokeOnChanged()
        {
            DispatcherFactory.Invoke(OnChanged, HasItems());
        }
    }
}
