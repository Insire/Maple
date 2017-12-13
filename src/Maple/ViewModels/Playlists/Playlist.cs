using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using FluentValidation;
using Maple.Core;
using Maple.Interfaces;
using Maple.Localization.Properties;

namespace Maple
{
    [DebuggerDisplay("{Title}, {Sequence}")]
    public class Playlist : ValidableBaseDataViewModel<Playlist, Data.Playlist>, IIsSelected, ISequence, IIdentifier, IChangeState
    {
        private readonly ISequenceService _sequenceProvider;
        private readonly ILocalizationService _translator;
        private readonly IDialogViewModel _dialogViewModel;
        private readonly object _itemsLock;
        private readonly Stack<int> _history;

        public bool IsNew => Model.IsNew;
        public bool IsDeleted => Model.IsDeleted;
        public int Count => Items?.Count ?? 0;

        public IAsyncCommand LoadFromFileCommand { get; private set; }
        public IAsyncCommand LoadFromFolderCommand { get; private set; }
        public IAsyncCommand LoadFromUrlCommand { get; private set; }
        public ICommand RemoveRangeCommand { get; protected set; }
        public ICommand RemoveCommand { get; protected set; }
        public ICommand ClearCommand { get; protected set; }
        public ICommand AddCommand { get; protected set; }

        public int Id
        {
            get { return Model.Id; }
        }

        public string CreatedBy
        {
            get { return Model.CreatedBy; }
        }

        public string UpdatedBy
        {
            get { return Model.UpdatedBy; }
        }

        public DateTime UpdatedOn
        {
            get { return Model.UpdatedOn; }
        }

        public DateTime CreatedOn
        {
            get { return Model.CreatedOn; }
        }

        public MediaItem this[int index]
        {
            get { return _items[index]; }
        }

        private ICollectionView _view;
        public ICollectionView View
        {
            get { return _view; }
            protected set { SetValue(ref _view, value); }
        }

        private IRangeObservableCollection<MediaItem> _items;
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public IReadOnlyCollection<MediaItem> Items
        {
            get { return (IReadOnlyCollection<MediaItem>)_items; }
            private set { SetValue(ref _items, (IRangeObservableCollection<MediaItem>)value); }
        }

        private MediaItem _selectedItem;
        /// <summary>
        /// is <see cref="SetActive" /> when a IMediaPlayer picks a <see cref="IMediaItem" /> from this
        /// </summary>
        /// <value>
        /// The current item.
        /// </value>
        public MediaItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetValue(ref _selectedItem, value,
                    OnChanging: () => Messenger.Publish(new ViewModelSelectionChangingMessage<MediaItem>(Items, _selectedItem)),
                    OnChanged: () => Messenger.Publish(new ViewModelSelectionChangedMessage<MediaItem>(_items, value)));
            }
        }

        private int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value, OnChanged: () => Model.Sequence = value); }
        }

        private PrivacyStatus _privacyStatus;
        /// <summary>
        /// Youtube Property
        /// </summary>
        /// <value>
        /// The privacy status.
        /// </value>
        public PrivacyStatus PrivacyStatus
        {
            get { return _privacyStatus; }
            private set { SetValue(ref _privacyStatus, value, OnChanged: () => Model.PrivacyStatus = (int)value); }
        }

        private bool _isSelected;
        /// <summary>
        /// if this list is part of a ui bound collection and selected this should be true
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        private bool _isShuffeling;
        /// <summary>
        /// indicates whether the next item is selected randomly from the list of items on a call of Next()
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is shuffeling; otherwise, <c>false</c>.
        /// </value>
        public bool IsShuffeling
        {
            get { return _isShuffeling; }
            set { SetValue(ref _isShuffeling, value, OnChanged: () => Model.IsShuffeling = value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value, OnChanged: () => Model.Title = value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetValue(ref _description, value, OnChanged: () => Model.Description = value); }
        }

        private RepeatMode _repeatMode;
        public RepeatMode RepeatMode
        {
            get { return _repeatMode; }
            set { SetValue(ref _repeatMode, value, OnChanged: () => Model.RepeatMode = (int)value); }
        }

        private IRangeObservableCollection<RepeatMode> _repeatModes;
        public IReadOnlyCollection<RepeatMode> RepeatModes
        {
            get { return (IReadOnlyCollection<RepeatMode>)_repeatModes; }
            private set { SetValue(ref _repeatModes, (IRangeObservableCollection<RepeatMode>)value); }
        }

        public Playlist(ViewModelServiceContainer container, IValidator<Playlist> validator, IDialogViewModel dialogViewModel, Data.Playlist model)
            : base(model, validator, container.Messenger)
        {
            SkipChangeTracking = true;
            using (BusyStack.GetToken())
            {
                _itemsLock = new object();
                _dialogViewModel = dialogViewModel ?? throw new ArgumentNullException(nameof(dialogViewModel), $"{nameof(dialogViewModel)} {Resources.IsRequired}");
                _sequenceProvider = container.SequenceService;

                _translator = container.LocalizationService;
                _title = model.Title;
                _description = model.Description;
                _repeatMode = (RepeatMode)model.RepeatMode;
                _isShuffeling = model.IsShuffeling;
                _sequence = model.Sequence;

                RepeatModes = new RangeObservableCollection<RepeatMode>(Enum.GetValues(typeof(RepeatMode)).Cast<RepeatMode>().ToList());
                _history = new Stack<int>();

                Items = new RangeObservableCollection<MediaItem>();
                _items.CollectionChanged += (o, e) => OnPropertyChanged(nameof(Count));

                BindingOperations.EnableCollectionSynchronization(Items, _itemsLock);
                View = CollectionViewSource.GetDefaultView(Items);
                OnPropertyChanged(nameof(Count));

                LoadFromFileCommand = AsyncCommand.Create(LoadFromFile, () => CanLoadFromFile());
                LoadFromFolderCommand = AsyncCommand.Create(LoadFromFolder, () => CanLoadFromFolder());
                LoadFromUrlCommand = AsyncCommand.Create(LoadFromUrl, () => CanLoadFromUrl());

                RemoveCommand = new RelayCommand<object>(Remove, CanRemove);
                RemoveRangeCommand = new RelayCommand<IList>(RemoveRange, CanRemoveRange);
                ClearCommand = new RelayCommand(() => Clear(), CanClear);

                Validate();
            }
            SkipChangeTracking = false;
        }

        private async Task LoadFromUrl()
        {
            using (BusyStack.GetToken())
            {
                var items = await _dialogViewModel.ShowUrlParseDialog().ConfigureAwait(true);
                AddRange(items);
            }
        }

        private bool CanLoadFromUrl()
        {
            return !IsBusy;
        }

        private async Task LoadFromFolder(CancellationToken token)
        {
            using (BusyStack.GetToken())
            {
                var options = new FileSystemBrowserOptions()
                {
                    CanCancel = true,
                    MultiSelection = false,
                    Title = _translator.Translate(nameof(Resources.SelectFolder)),
                };

                (var Result, var MediaItems) = await _dialogViewModel.ShowMediaItemFolderSelectionDialog(options, token).ConfigureAwait(true);
                if (Result)
                    AddRange(MediaItems);
            }
        }

        private bool CanLoadFromFolder()
        {
            return !IsBusy;
        }

        private async Task LoadFromFile(CancellationToken token)
        {
            using (BusyStack.GetToken())
            {
                var options = new FileSystemBrowserOptions()
                {
                    CanCancel = true,
                    MultiSelection = false,
                    Title = _translator.Translate(nameof(Resources.SelectFiles)),
                };

                (var Result, var MediaItems) = await _dialogViewModel.ShowMediaItemSelectionDialog(options, token).ConfigureAwait(true);
                if (Result)
                    AddRange(MediaItems);
            }
        }

        private bool CanLoadFromFile()
        {
            return !IsBusy;
        }

        public virtual void Clear()
        {
            _history.Clear();
            SelectedItem = null;
            RemoveRange(Items.AsEnumerable());
        }

        private bool CanClear()
        {
            return Items?.Any() == true;
        }

        public virtual void Add(MediaItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), $"{nameof(item)} {Resources.IsRequired}");

            using (BusyStack.GetToken())
            {
                var sequence = _sequenceProvider.Get(Items.Select(p => (ISequence)p).ToList());
                item.Sequence = sequence;

                if (Items.Any() != true)
                    _history.Push(item.Sequence);

                AddInternal(item);

                if (SelectedItem == null)
                    SelectedItem = Items.First();
            }
        }

        private void AddInternal(MediaItem item)
        {
            item.Playlist = this;
            _items.Add(item);
            Model.MediaItems.Add(item.Model);
        }

        public virtual void AddRange(IEnumerable<MediaItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), $"{nameof(items)} {Resources.IsRequired}");

            using (BusyStack.GetToken())
            {
                var added = false;
                var sequence = _sequenceProvider.Get(Items.Select(p => (ISequence)p).ToList());
                var collection = items.ToList();
                var item = default(MediaItem);

                for (var i = 0; i < collection.Count; i++)
                {
                    item = collection[i];

                    if (item == null)
                        throw new ArgumentNullException(nameof(item), $"{nameof(item)} {Resources.IsRequired}");

                    item.Sequence = sequence;
                    Add(item);
                    sequence++;

                    added = true;
                }

                if (SelectedItem == null && (added || Items.Count > 0))
                    SelectedItem = Items.First();
            }
        }

        private void Remove(object item)
        {
            Remove(item as MediaItem);
        }

        public virtual void Remove(MediaItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), $"{nameof(item)} {Resources.IsRequired}");

            using (BusyStack.GetToken())
            {
                while (Items.Contains(item))
                    RemoveInternal(item);
            }
        }

        private void RemoveInternal(MediaItem item)
        {
            _items.Remove(item);
            item.Model.IsDeleted = true;
        }

        public virtual void RemoveRange(IEnumerable<MediaItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), $"{nameof(items)} {Resources.IsRequired}");

            using (BusyStack.GetToken())
                RemoveRangeInternal(items.ToList());
        }

        private void RemoveRange(IList items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), $"{nameof(items)} {Resources.IsRequired}");

            using (BusyStack.GetToken())
                RemoveRangeInternal(items.Cast<MediaItem>().ToList());
        }

        private void RemoveRangeInternal(IEnumerable<MediaItem> items)
        {
            foreach (var item in items)
                Remove(item);
        }

        public virtual bool CanRemove(object item)
        {
            if (Items == null || Items.Count == 0)
                return false;

            var mediaItem = item as MediaItem;
            if (mediaItem == null)
                return false;

            return Items.Contains(mediaItem) && !IsBusy;
        }

        protected virtual bool CanRemoveRange(IEnumerable<MediaItem> items)
        {
            return CanClear() && items != null && items.Any(p => Items.Contains(p));
        }

        protected virtual bool CanRemoveRange(IList items)
        {
            return items == null ? false : CanRemoveRange(items.Cast<MediaItem>());
        }

        public virtual MediaItem Next()
        {
            using (BusyStack.GetToken())
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
        }

        private MediaItem NextRepeatNone()
        {
            var currentIndex = 0;
            if (SelectedItem?.Sequence != null)
                currentIndex = SelectedItem?.Sequence ?? 0;

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

        private MediaItem NextRepeatSingle()
        {
            if (RepeatMode != RepeatMode.None)
                return SelectedItem;
            else
                return null;
        }

        private MediaItem NextRepeatAll()
        {
            var currentIndex = 0;
            if (SelectedItem?.Sequence != null)
                currentIndex = SelectedItem?.Sequence ?? 0;

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

        private MediaItem NextShuffle()
        {
            if (Items.Count > 1)
            {
                var nextItems = Items.Where(p => p.Sequence != SelectedItem?.Sequence); // get all items besides the current one
                Items.ToList().ForEach(p => p.IsSelected = false);
                var foundItem = nextItems.Random();
                foundItem.IsSelected = true;
                return foundItem;
            }
            else
                return NextRepeatSingle();
        }

        public virtual MediaItem Previous()
        {
            using (BusyStack.GetToken())
            {
                if (_history?.Any() == true)
                {
                    Items.ToList().ForEach(p => p.IsSelected = false);      // deselect all items in the list
                    while (_history.Any())
                    {
                        var previous = _history.Pop();

                        if (previous == SelectedItem?.Sequence) // the most recent item in the history, is the just played item, so we wanna skip that
                            continue;

                        if (previous > -1)
                        {
                            var previousItems = Items.Where(p => p.Sequence == previous); // try to get the last played item
                            if (previousItems.Any())
                            {
                                var foundItem = previousItems.First();
                                foundItem.IsSelected = true;

                                return foundItem;
                            }
                        }
                    }
                }
                return null;
            }
        }

        public bool CanNext()
        {
            return Items != null && Items.Any();
        }

        public bool CanPrevious()
        {
            return _history != null && _history.Any();
        }
    }
}
