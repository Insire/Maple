using System;
using System.Collections.Generic;

using Maple.Localization.Properties;

namespace Maple.Core
{
    public abstract class ViewModel : ObservableObject, IDisposable
    {
        protected IMessenger Messenger { get; }
        protected BusyStack BusyStack { get; }

        protected bool Disposed { get; set; }
        protected ICollection<SubscriptionToken> MessageTokens { get; private set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetValue(ref _isBusy, value); }
        }

        protected ViewModel(IMessenger messenger)
        {
            BusyStack = new BusyStack();
            BusyStack.OnChanged += (isBusy) => IsBusy = isBusy;
            MessageTokens = new List<SubscriptionToken>();

            Messenger = messenger ?? throw new ArgumentNullException(nameof(messenger), $"{nameof(messenger)} {Resources.IsRequired}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
#pragma warning disable S1066 // Collapsible "if" statements should be merged
                if (MessageTokens != null)
#pragma warning restore S1066 // Collapsible "if" statements should be merged
                {
                    foreach (var token in MessageTokens)
                        Messenger.Unsubscribe<IMapleMessage>(token);

                    MessageTokens = null;
                }

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }
    }
}
