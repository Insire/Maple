using Maple.Core;
using System.Windows;

namespace Maple
{
    public class Scene : ObservableObject, ISequence
    {
        private readonly ITranslationService _manager;

        private BusyStack _busyStack;
        /// <summary>
        /// Provides IDisposable tokens for running async operations
        /// </summary>
        public BusyStack BusyStack
        {
            get { return _busyStack; }
            private set { SetValue(ref _busyStack, value); }
        }

        private bool _isBusy;
        /// <summary>
        /// Indicates if there is an operation running.
        /// Modified by adding <see cref="BusyToken"/> to the <see cref="BusyStack"/> property
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            private set { SetValue(ref _isBusy, value); }
        }

        private FrameworkElement _content;
        public FrameworkElement Content
        {
            get { return _content; }
            set { SetValue(ref _content, value); }
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetValue(ref _key, value, Changed: UpdateDisplayName); }
        }

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            private set { SetValue(ref _displayName, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        public int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value); }
        }

        public Scene(ITranslationService manager)
        {
            _manager = manager;
            _manager.PropertyChanged += (o, e) =>
                      {
                          if (e.PropertyName == nameof(_manager.CurrentLanguage))
                              UpdateDisplayName();
                      };

            BusyStack = new BusyStack()
            {
                OnChanged = (hasItems) => IsBusy = hasItems
            };
        }

        private void UpdateDisplayName()
        {
            if (Key != null)
                DisplayName = _manager.Translate(Key);
        }
    }
}
