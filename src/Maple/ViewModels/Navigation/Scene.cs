﻿using System;
using System.Windows;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    public class Scene : ObservableObject, ISequence
    {
        private readonly ILocalizationService _manager;
        private readonly BusyStack _busyStack;

        private bool _isBusy;
        /// <summary>
        /// Indicates if there is an operation running.
        /// Modified by adding <see cref="BusyToken" /> to the <see cref="BusyStack" /> property
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
        /// </value>
        public bool IsBusy
        {
            get { return _isBusy; }
            private set { SetValue(ref _isBusy, value); }
        }

        private FrameworkElement _content;
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public FrameworkElement Content
        {
            get { return _content; }
            set { SetValue(ref _content, value); }
        }

        private string _key;
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key
        {
            get { return _key; }
            set { SetValue(ref _key, value, OnChanged: UpdateDisplayName); }
        }

        private string _displayName;
        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName
        {
            get { return _displayName; }
            private set { SetValue(ref _displayName, value); }
        }

        private bool _isSelected;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        public int _sequence;
        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public Scene(ILocalizationService manager)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager), $"{nameof(manager)} {Resources.IsRequired}");
            _manager.PropertyChanged += (o, e) =>
                      {
                          if (e.PropertyName == nameof(_manager.CurrentLanguage))
                              UpdateDisplayName();
                      };

            _busyStack = new BusyStack();
            _busyStack.OnChanged += (hasItems) => IsBusy = hasItems;
        }

        private void UpdateDisplayName()
        {
            if (Key != null)
                DisplayName = _manager.Translate(Key);
        }
    }
}
