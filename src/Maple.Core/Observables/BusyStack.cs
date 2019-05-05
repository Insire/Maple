using System;
using System.Collections.Concurrent;

using Maple.Localization.Properties;

namespace Maple.Core
{
    public sealed class BusyStack : ObservableObject
    {
        private readonly ConcurrentBag<BusyToken> _items;
        private readonly Action<bool> _onChanged;

        private BusyStack()
        {
            _items = new ConcurrentBag<BusyToken>();
        }

        public BusyStack(Action<bool> onChanged)
            : this()
        {
            _onChanged = onChanged ?? throw new ArgumentNullException(nameof(onChanged), $"{nameof(onChanged)} {Resources.IsRequired}");
        }

        /// <summary>
        /// Tries to take an item from the stack and returns true if that action was successful
        /// </summary>
        /// <returns></returns>
        public bool Pull()
        {
            var result = _items.TryTake(out var token);

            if (result)
                InvokeOnChanged();

            return result;
        }

        public void Push(BusyToken token)
        {
            _items.Add(token);

            InvokeOnChanged();
        }

        public bool HasItems()
        {
            return _items.TryPeek(out var token);
        }

        public BusyToken GetToken()
        {
            return new BusyToken(this);
        }

        private void InvokeOnChanged()
        {
            DispatcherFactory.Invoke(_onChanged, HasItems());
        }
    }
}
