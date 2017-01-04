using MvvmScarletToolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace InsireBot
{
    public class Playlist<T> : RangeObservableCollection<T>, IEnumerable<T>, IList<T>, IIsSelected, ISequence, IIdentifier, INotifyPropertyChanged where T : IMediaItem
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public event RepeatModeChangedEventHandler RepeatModeChanged;
        public event ShuffleModeChangedEventHandler ShuffleModeChanged;
        new public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// contains indices of played <see cref="IMediaItem"/>
        /// </summary>
        public Stack<int> History { get; private set; }

        private IMediaItem _currentItem;
        /// <summary>
        /// is <see cref="Set"/> when a IMediaPlayer picks a <see cref="IMediaItem"/> from this
        /// </summary>
        public IMediaItem CurrentItem
        {
            get { return _currentItem; }
            private set
            {
                if (_currentItem == null)
                {
                    _currentItem = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentItem)));
                }
                else
                {
                    if (!_currentItem.Equals(value))
                    {
                        _currentItem = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentItem)));
                    }
                }
            }
        }

        private int _index;
        /// <summary>
        /// the index of this item if its part of a collection
        /// </summary>
        public int Sequence
        {
            get { return _index; }
            set
            {
                _index = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sequence)));
            }
        }


        private string _privacyStatus;
        /// <summary>
        /// Youtube Property
        /// </summary>
        public string PrivacyStatus
        {
            get { return _privacyStatus; }
            set
            {
                _privacyStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PrivacyStatus)));
            }
        }


        private long _itemCount;
        /// <summary>
        /// Youtube Property
        /// </summary>
        public long ItemCount
        {
            get { return _itemCount; }
            set
            {
                _itemCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemCount)));
            }
        }


        private bool _isSelected;
        /// <summary>
        /// if this list is part of a ui bound collection and selected this should be true
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }


        private bool _isShuffeling;
        /// <summary>
        /// indicates whether the next item is selected randomly from the list of items on a call of Next()
        /// </summary>
        public bool IsShuffeling
        {
            get { return _isShuffeling; }
            set
            {
                if (_isShuffeling != value)
                {
                    _isShuffeling = value;
                    NotifyPropertyChanged(nameof(IsShuffeling));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsShuffeling)));
                    ShuffleModeChanged?.Invoke(this, new ShuffleModeChangedEventEventArgs(value));
                }
            }
        }


        private string _title;
        /// <summary>
        /// the title/name of this playlist (human readable identifier)
        /// </summary>
        public string Title
        {
            get { return _title; }
            private set
            {
                _title = value;
                NotifyPropertyChanged(nameof(Title));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        private string _description;
        /// <summary>
        /// the description of this playlist
        /// </summary>
        public string Description
        {
            get { return _description; }
            private set
            {
                _description = value;
                NotifyPropertyChanged(nameof(Description));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            private set
            {
                _location = value;
                NotifyPropertyChanged(nameof(Location));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Location)));
            }
        }


        private Guid _id;
        public Guid ID
        {
            get { return _id; }
            private set
            {
                _id = value;
                NotifyPropertyChanged(nameof(ID));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
            }
        }


        private RepeatMode _repeatMode;
        /// <summary>
        /// defines what happens when the last <see cref="IMediaItem"/> of <see cref="Items"/> is <see cref="Set"/> and the <see cref="Next"/> is requested
        /// </summary>
        public RepeatMode RepeatMode
        {
            get { return _repeatMode; }
            set
            {
                if (_repeatMode != value)
                {
                    _repeatMode = value;
                    NotifyPropertyChanged(nameof(RepeatMode));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RepeatMode)));
                    RepeatModeChanged?.Invoke(this, new RepeatModeChangedEventEventArgs(RepeatMode));
                }
            }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public Playlist() : base()
        {
            History = new Stack<int>();
            Title = string.Empty;
            ID = Guid.NewGuid();
            RepeatMode = RepeatMode.None;
            IsShuffeling = false;
        }

        public Playlist(IEnumerable<T> items) : this()
        {
            AddRange(items);
        }

        public Playlist(string title, string location, long itemCount, string privacyStatus = "none") : this()
        {
            Title = title;
            Location = location;
            PrivacyStatus = privacyStatus;
            ItemCount = itemCount;
        }

        /// <summary>
        /// Add an <see cref="IMediaItem"/> to <seealso cref="Items"/>
        /// </summary>
        /// <param name="item">the <see cref="IMediaItem"/> to add</param>
        new public void Add(T item)
        {
            if (Items?.Any() == true)
                item.Sequence = Items.Select(p => p.Sequence).Max() + 1;
            else
                item.Sequence = 0;

            if (this.Any() != true)
                History.Push(item.Sequence);

            base.Add(item);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));

            if (CurrentItem == null)
                CurrentItem = Items.First();
        }

        public override void AddRange(IEnumerable<T> items)
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

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));

            if (CurrentItem == null)
                CurrentItem = Items.First();
        }

        new public void Clear()
        {
            base.Clear();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
        }

        /// <summary>
        /// Removes all occurences of an <see cref="IMediaItem"/> from <seealso cref="Items"/>
        /// </summary>
        /// <param name="item"></param>
        new public void Remove(T item)
        {
            while (Items.Contains(item))
                Items.Remove(item);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
        }
        /// <summary>
        /// Returns the next <see cref="IMediaItem"/> after the <seealso cref="CurrentItem"/>
        /// </summary>
        /// <returns></returns>
        public IMediaItem Next()
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
        /// Sets the <see cref="IMediaItem"/> as the <seealso cref="CurrentItem"/>
        /// Adds the <see cref="IMediaItem"/> to the <seealso cref="History"/>
        /// </summary>
        /// <param name="mediaItem"></param>
        public void Set(IMediaItem mediaItem)
        {
            if (mediaItem != null)
            {
                History.Push(mediaItem.Sequence);
                CurrentItem = mediaItem;
            }
        }

        /// <summary>
        /// Removes the last <see cref="IMediaItem"/> from <seealso cref="History"/> and returns it
        /// </summary>
        /// <returns>returns the last <see cref="IMediaItem"/> from <seealso cref="History"/></returns>
        public IMediaItem Previous()
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

        public void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            foreach (var i in Items)
            {
                array.SetValue(i, index);
                index = index + 1;
            }

            throw new NotImplementedException("TODO: CopyTo in Playlist.cs");
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
