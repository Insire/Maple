using InsireBot.Core;
using InsireBot.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace InsireBot
{
    public class PlaylistViewModel : ObservableObject, IIsSelected, ISequence, IIdentifier, INotifyPropertyChanged
    {
        private IBotLog _log;
        private IPlaylistsRepository _repository;
        public int ItemCount => Items?.Count ?? 0;

        /// <summary>
        /// contains indices of played <see cref="IMediaItem"/>
        /// </summary>
        public Stack<int> History { get; private set; }

        public RangeObservableCollection<IMediaItem> Items { get; private set; }

        private IMediaItem _currentItem;
        /// <summary>
        /// is <see cref="SetActive"/> when a IMediaPlayer picks a <see cref="IMediaItem"/> from this
        /// </summary>
        public IMediaItem CurrentItem
        {
            get { return _currentItem; }
            private set { SetValue(ref _currentItem, value); }
        }

        private int _sequence;
        /// <summary>
        /// the index of this item if its part of a collection
        /// </summary>
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value); }
        }

        private string _privacyStatus;
        /// <summary>
        /// Youtube Property
        /// </summary>
        public string PrivacyStatus
        {
            get { return _privacyStatus; }
            private set { SetValue(ref _privacyStatus, value); }
        }

        private bool _isSelected;
        /// <summary>
        /// if this list is part of a ui bound collection and selected this should be true
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            private set { SetValue(ref _isActive, value); }
        }

        private bool _isShuffeling;
        /// <summary>
        /// indicates whether the next item is selected randomly from the list of items on a call of Next()
        /// </summary>
        public bool IsShuffeling
        {
            get { return _isShuffeling; }
            set { SetValue(ref _isShuffeling, value); }
        }

        private string _title;
        /// <summary>
        /// the title/name of this playlist (human readable identifier)
        /// </summary>
        public string Title
        {
            get { return _title; }
            private set { SetValue(ref _title, value); }
        }

        private string _description;
        /// <summary>
        /// the description of this playlist
        /// </summary>
        public string Description
        {
            get { return _description; }
            private set { SetValue(ref _description, value); }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            private set { SetValue(ref _location, value); }
        }

        private Guid _id;
        public Guid ID
        {
            get { return _id; }
            private set { SetValue(ref _id, value); }
        }

        private RepeatMode _repeatMode;
        /// <summary>
        /// defines what happens when the last <see cref="IMediaItem"/> of <see cref="Items"/> is <see cref="SetActive"/> and the <see cref="Next"/> is requested
        /// </summary>
        public RepeatMode RepeatMode
        {
            get { return _repeatMode; }
            set { SetValue(ref _repeatMode, value); }
        }

        public PlaylistViewModel(IPlaylistsRepository repository, IBotLog log) : base()
        {
            Items = new RangeObservableCollection<IMediaItem>();
            Items.CollectionChanged += (o, e) =>
              {
                  OnPropertyChanged(nameof(ItemCount));
              };

            _repository = repository;
            _log = log;

            History = new Stack<int>();
            Title = string.Empty;
            Description = string.Empty;
            ID = Guid.NewGuid();
            RepeatMode = RepeatMode.None;
            IsShuffeling = false;
        }

        public virtual void Clear()
        {
            Items.Clear();
            History.Clear();
            CurrentItem = null;
        }

        /// <summary>
        /// Add an <see cref="IMediaItem"/> to <seealso cref="Items"/>
        /// </summary>
        /// <param name="item">the <see cref="IMediaItem"/> to add</param>
        public virtual void Add(IMediaItem item)
        {
            if (Items?.Any() == true)
                item.Sequence = Items.Select(p => p.Sequence).Max() + 1;
            else
                item.Sequence = 0;

            if (Items.Any() != true)
                History.Push(item.Sequence);

            Items.Add(item);

            if (CurrentItem == null)
                CurrentItem = Items.First();
        }

        public virtual void AddRange(IEnumerable<IMediaItem> items)
        {
            var currentIndex = -1;
            if (Items.Any())
            {
                var indices = Items.Select(p => p.Sequence);
                currentIndex = (indices != null) ? indices.Max() : 0;
            }

            foreach (var item in items)
            {
                currentIndex++;
                item.Sequence = currentIndex;
                Add(item);
            }

            if (CurrentItem == null)
                CurrentItem = Items.First();
        }

        /// <summary>
        /// Removes all occurences of an <see cref="IMediaItem"/> from <seealso cref="Items"/>
        /// </summary>
        /// <param name="item"></param>
        public virtual void Remove(IMediaItem item)
        {
            while (Items.Contains(item))
                Items.Remove(item);
        }

        /// <summary>
        /// Returns the next <see cref="IMediaItem"/> after the <seealso cref="CurrentItem"/>
        /// </summary>
        /// <returns></returns>
        public virtual IMediaItem Next()
        {
            if (Items != null && Items.Any())
            {
                if (IsShuffeling)
                    return NextShuffle();
                else
                {
                    switch (RepeatMode)
                    {
                        case RepeatMode.All: return NextRepeatAll();

                        case RepeatMode.None: return NextRepeatNone();

                        case RepeatMode.Single: return NextRepeatSingle();

                        default:
                            throw new NotImplementedException(nameof(RepeatMode));
                    }
                }
            }
            return null;
        }

        private IMediaItem NextRepeatNone()
        {
            var currentIndex = 0;
            if (CurrentItem?.Sequence != null)
                currentIndex = CurrentItem.Sequence;

            if (Items.Count > 1)
            {
                var nextPossibleItems = Items.Where(p => p.Sequence > currentIndex);

                if (nextPossibleItems?.Any() == true) // try to find items after the current one
                {
                    Items.ToList().ForEach(p => p.IsSelected = false);
                    var foundItem = nextPossibleItems.Where(q => q.Sequence == nextPossibleItems.Select(p => p.Sequence).Min()).First();
                    foundItem.IsSelected = true;
                    return foundItem;
                }

                return null;
                // we dont repeat, so there is nothing to do here
            }
            else
                return NextRepeatSingle();
        }

        private IMediaItem NextRepeatSingle()
        {
            if (RepeatMode != RepeatMode.None)
                return CurrentItem;
            else
                return null;
        }

        private IMediaItem NextRepeatAll()
        {
            var currentIndex = 0;
            if (CurrentItem?.Sequence != null)
                currentIndex = CurrentItem.Sequence;

            if (Items.Count > 1)
            {
                var nextPossibleItems = Items.Where(p => p.Sequence > currentIndex);

                if (nextPossibleItems.Any()) // try to find items after the current one
                {
                    Items.ToList().ForEach(p => p.IsSelected = false);
                    var foundItem = nextPossibleItems.Where(q => q.Sequence == nextPossibleItems.Select(p => p.Sequence).Min()).First();
                    foundItem.IsSelected = true;
                    return foundItem;
                }
                else // if there are none, use the first item in the list
                {
                    Items.ToList().ForEach(p => p.IsSelected = false);
                    var foundItem = Items.First();
                    foundItem.IsSelected = true;
                    return foundItem;
                }
            }
            else
                return NextRepeatSingle();
        }

        private IMediaItem NextShuffle()
        {
            if (Items.Count > 1)
            {
                var nextItems = Items.Where(p => p.Sequence != CurrentItem.Sequence); // get all items besides the current one
                Items.ToList().ForEach(p => p.IsSelected = false);
                var foundItem = nextItems.Random();
                foundItem.IsSelected = true;
                return foundItem;
            }
            else
                return NextRepeatSingle();
        }

        /// <summary>
        /// Removes the last <see cref="IMediaItem"/> from <seealso cref="History"/> and returns it
        /// </summary>
        /// <returns>returns the last <see cref="IMediaItem"/> from <seealso cref="History"/></returns>
        public virtual IMediaItem Previous()
        {
            if (History?.Any() == true)
            {
                Items.ToList().ForEach(p => p.IsSelected = false);      // deselect all items in the list
                while (History.Any())
                {
                    var previous = History.Pop();

                    if (previous == CurrentItem.Sequence) // the most recent item in the history, is the just played item, so we wanna skip that
                        continue;

                    if (previous > -1)
                    {
                        var previousItems = Items.Where(p => p.Sequence == previous); // try to get the last played item
                        if (previousItems.Any())
                        {
                            var foundItem = previousItems.First();
                            foundItem.IsSelected = true;

                            if (previousItems.Count() > 1)
                                _log.Warn("Warning SelectPrevious returned more than one value, when it should only return one");

                            return foundItem;
                        }
                    }
                }
            }
            return null;
        }

        public bool CanNext()
        {
            return Items != null && Items.Any();
        }

        public bool CanPrevious()
        {
            return History != null && History.Any();
        }

        /// <summary>
        /// Sets the <see cref="IMediaItem"/> as the <seealso cref="CurrentItem"/>
        /// Adds the <see cref="IMediaItem"/> to the <seealso cref="History"/>
        /// </summary>
        /// <param name="mediaItem"></param>
        public virtual void SetActive(IMediaItem mediaItem)
        {
            if (mediaItem != null)
            {
                History.Push(mediaItem.Sequence);
                CurrentItem = mediaItem;
            }
        }

        private bool CanSetActive(IMediaItem item)
        {
            return false;
        }
    }
}
