using InsireBot.Core;
using InsireBot.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InsireBot
{
    public class Playlist : TrackingBaseViewModel<Data.Playlist>, IIsSelected, ISequence, IIdentifier, INotifyPropertyChanged
    {
        private readonly IBotLog _log;
        public int ItemCount => Items?.Count ?? 0;

        /// <summary>
        /// contains indices of played <see cref="IMediaItem"/>
        /// </summary>
        public Stack<int> History { get; private set; }

        public ChangeTrackingCollection<MediaItemViewModel> Items { get; private set; }

        private MediaItemViewModel _currentItem;
        /// <summary>
        /// is <see cref="SetActive"/> when a IMediaPlayer picks a <see cref="IMediaItem"/> from this
        /// </summary>
        public MediaItemViewModel CurrentItem
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

        private PrivacyStatus _privacyStatus;
        /// <summary>
        /// Youtube Property
        /// </summary>
        public PrivacyStatus PrivacyStatus
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
            set { SetValue(ref _isActive, value); }
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

        private int _id;
        public int Id
        {
            get { return _id; }
            private set { SetValue(ref _id, value); }
        }

        private ObservableCollection<RepeatMode> _repeatModes;
        public ObservableCollection<RepeatMode> RepeatModes
        {
            get { return _repeatModes; }
            private set { SetValue(ref _repeatModes, value); }
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

        public Playlist(IBotLog log, Data.Playlist model) : base(model)
        {
            _log = log;

            RepeatModes = new ObservableCollection<RepeatMode>(Enum.GetValues(typeof(RepeatMode)).Cast<RepeatMode>().ToList());
            History = new Stack<int>();
        }

        protected override void InitializeComplexProperties(Data.Playlist model)
        {
            Title = model.Title;
            Description = model.Description;
            Id = model.Id;
            RepeatMode = (RepeatMode)model.RepeatMode;
            IsShuffeling = model.IsShuffeling;
        }

        protected override void InitializeCollectionProperties(Data.Playlist model)
        {
            if (model.MediaItems == null)
                throw new ArgumentException($"{model.MediaItems} cannot be null");

            Items = new ChangeTrackingCollection<MediaItemViewModel>();
            Items.CollectionChanged += (o, e) =>
            {
                OnPropertyChanged(nameof(ItemCount));
            };

            Items.AddRange(model.MediaItems.Select(p => new MediaItemViewModel(_log, p)));
            RegisterCollection(Items, model.MediaItems);
        }

        public virtual void Clear()
        {
            Items.Clear();
            History.Clear();
            CurrentItem = null;
        }

        public virtual void Add(MediaItemViewModel item)
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

        public virtual void AddRange(IEnumerable<MediaItemViewModel> items)
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

        public virtual void Remove(MediaItemViewModel item)
        {
            while (Items.Contains(item))
                Items.Remove(item);
        }

        public virtual MediaItemViewModel Next()
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

        private MediaItemViewModel NextRepeatNone()
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

        private MediaItemViewModel NextRepeatSingle()
        {
            if (RepeatMode != RepeatMode.None)
                return CurrentItem;
            else
                return null;
        }

        private MediaItemViewModel NextRepeatAll()
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

        private MediaItemViewModel NextShuffle()
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
        /// Removes the last <see cref="MediaItemViewModel"/> from <seealso cref="History"/> and returns it
        /// </summary>
        /// <returns>returns the last <see cref="MediaItemViewModel"/> from <seealso cref="History"/></returns>
        public virtual MediaItemViewModel Previous()
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

        public override IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(Title))
                yield return new ValidationResult($"{nameof(Title)} {Resources.IsRequired}", new[] { nameof(Title) });
        }
    }
}
