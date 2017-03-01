using Maple.Core;
using Maple.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Maple
{
    public class Scene : ObservableObject, ISequence
    {
        private readonly ITranslationManager _manager;
        private readonly IPlaylistContext _context;
        public Func<ObservableObject> GetDataContext { get; set; }

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
            set { SetValue(ref _isSelected, value, Changed: async () => await UpdateDataContext()); }
        }

        public int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value, Changed: async () => await UpdateDataContext()); }
        }

        public Scene(ITranslationManager manager, IPlaylistContext context)
        {
            _context = context;
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

        // TODO fiure out a way to call this async and still maintain order
        // maybe blocking collection + cancellationtokensource
        private async Task UpdateDataContext()
        {
            if (Content == null || !IsSelected || GetDataContext == null)
                return;

            // while fetching the dataconext, we will switch IsBusy accordingly
            using (var token = BusyStack.GetToken())
            {
                var currentContext = Content.DataContext as ObservableObject;
                var newContext = GetDataContext.Invoke();

                if (currentContext == null && newContext == null)
                    return;

                if (EqualityComparer<ObservableObject>.Default.Equals(currentContext, newContext))
                    return;

                await _context.SaveChangesAsync();

                Content.DataContext = newContext;
            }
        }

        private void UpdateDisplayName()
        {
            if (Key != null)
                DisplayName = _manager.Translate(Key);
        }
    }
}
