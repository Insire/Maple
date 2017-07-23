using FluentValidation;
using Maple.Core;
using Maple.Localization.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.BaseViewModel{Maple.Data.Playlist}" />
    /// <seealso cref="Maple.Core.IIsSelected" />
    /// <seealso cref="Maple.Core.ISequence" />
    /// <seealso cref="Maple.Core.IIdentifier" />
    /// <seealso cref="Maple.Core.IChangeState" />
    [DebuggerDisplay("{Title}, {Sequence}")]
    public class Playlist : ValidableBaseDataViewModel<Playlist, Data.Playlist>, IIsSelected, ISequence, IIdentifier, IChangeState
    {
        private readonly ISequenceProvider _sequenceProvider;
        private readonly ILocalizationService _translator;
        private readonly object _itemsLock;
        private readonly DialogViewModel _dialogViewModel;
        public int Count => Items?.Count ?? 0;

        /// <summary>
        /// The selection changed event
        /// </summary>
        public EventHandler SelectionChanged;
        /// <summary>
        /// The selection changing event
        /// </summary>
        public EventHandler SelectionChanging;

        /// <summary>
        /// Gets a value indicating whether this instance is new.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is new; otherwise, <c>false</c>.
        /// </value>
        public bool IsNew => Model.IsNew;
        /// <summary>
        /// Gets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted => Model.IsDeleted;


        public ICommand LoadFromFileCommand { get; private set; }
        public ICommand LoadFromFolderCommand { get; private set; }
        public ICommand LoadFromUrlCommand { get; private set; }
        public ICommand RemoveRangeCommand { get; protected set; }
        public ICommand RemoveCommand { get; protected set; }
        public ICommand ClearCommand { get; protected set; }
        public ICommand AddCommand { get; protected set; }
        /// <summary>
        /// contains indices of played <see cref="IMediaItem" />
        /// </summary>
        /// <value>
        /// The history.
        /// </value>
        public Stack<int> History { get; private set; }

        private RangeObservableCollection<MediaItem> _items;
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public RangeObservableCollection<MediaItem> Items
        {
            get { return _items; }
            private set { SetValue(ref _items, value); }
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
            set { SetValue(ref _selectedItem, value, OnChanging: OnSelectionChanging, OnChanged: OnSelectionChanged); }
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
        /// <summary>
        /// the title/name of this playlist (human readable identifier)
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value, OnChanged: () => Model.Title = value); }
        }

        private string _description;
        /// <summary>
        /// the description of this playlist
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return _description; }
            set { SetValue(ref _description, value, OnChanged: () => Model.Description = value); }
        }

        private ObservableCollection<RepeatMode> _repeatModes;
        public ObservableCollection<RepeatMode> RepeatModes
        {
            get { return _repeatModes; }
            private set { SetValue(ref _repeatModes, value); }
        }

        private RepeatMode _repeatMode;
        /// <summary>
        /// defines what happens when the last <see cref="IMediaItem" /> of <see cref="Items" /> is <see cref="SetActive" /> and the <see cref="Next" /> is requested
        /// </summary>
        /// <value>
        /// The repeat mode.
        /// </value>
        public RepeatMode RepeatMode
        {
            get { return _repeatMode; }
            set { SetValue(ref _repeatMode, value, OnChanged: () => Model.RepeatMode = (int)value); }
        }

        private ICollectionView _view;
        public ICollectionView View
        {
            get { return _view; }
            protected set { SetValue(ref _view, value); }
        }

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
            get { return Items[index]; }
        }

        public Playlist(ILocalizationService translator, ISequenceProvider sequenceProvider, IValidator<Playlist> validator, DialogViewModel dialogViewModel, Data.Playlist model)
            : base(model, validator)
        {
            using (_busyStack.GetToken())
            {
                _itemsLock = new object();
                _sequenceProvider = sequenceProvider ?? throw new ArgumentNullException(nameof(sequenceProvider));
                _dialogViewModel = dialogViewModel ?? throw new ArgumentNullException(nameof(dialogViewModel));
                _translator = translator ?? throw new ArgumentNullException(nameof(translator));

                Items = new RangeObservableCollection<MediaItem>();
                RepeatModes = new ObservableCollection<RepeatMode>(Enum.GetValues(typeof(RepeatMode)).Cast<RepeatMode>().ToList());
                History = new Stack<int>();

                BindingOperations.EnableCollectionSynchronization(Items, _itemsLock);

                _title = model.Title;
                _description = model.Description;
                _repeatMode = (RepeatMode)model.RepeatMode;
                _isShuffeling = model.IsShuffeling;
                _sequence = model.Sequence;

                if (model.MediaItems == null)
                    throw new ArgumentException($"{model.MediaItems} cannot be null");

                Items.CollectionChanged += (o, e) =>
                {
                    OnPropertyChanged(nameof(Count));
                };

                View = CollectionViewSource.GetDefaultView(Items);
                OnPropertyChanged(nameof(Count));

                InitializeCommands();
                IntializeValidation();

                EnableValidation();
                Validate();
            }
        }

        private void InitializeCommands()
        {
            LoadFromFileCommand = new AsyncRelayCommand(LoadFromFile, () => CanLoadFromFile());
            LoadFromFolderCommand = new AsyncRelayCommand(LoadFromFolder, () => CanLoadFromFolder());
            LoadFromUrlCommand = new AsyncRelayCommand(LoadFromUrl, () => CanLoadFromUrl());

            RemoveCommand = new RelayCommand<MediaItem>(Remove, CanRemove);
            RemoveRangeCommand = new RelayCommand<IList>(RemoveRange, CanRemoveRange);
            ClearCommand = new RelayCommand(() => Clear(), CanClear);
        }

        private void IntializeValidation()
        {

        }

        protected virtual void OnSelectionChanging()
        {
            SelectionChanging?.Raise(this);
        }

        protected virtual void OnSelectionChanged()
        {
            SelectionChanged?.Raise(this);
        }

        private async Task LoadFromUrl()
        {
            using (_busyStack.GetToken())
            {
                var items = await _dialogViewModel.ShowUrlParseDialog();
                Items.AddRange(items);
            }
        }

        private bool CanLoadFromUrl()
        {
            return !IsBusy;
        }

        private Task LoadFromFolder()
        {
            using (_busyStack.GetToken())
            {
                var options = new FileSystemBrowserOptions()
                {
                    CanCancel = true,
                    MultiSelection = false,
                    Title = _translator.Translate(nameof(Resources.SelectFolder)),
                };
                return _dialogViewModel.ShowFolderBrowserDialog(options);
            }
        }

        private bool CanLoadFromFolder()
        {
            return !IsBusy;
        }

        private Task LoadFromFile()
        {
            using (_busyStack.GetToken())
            {
                var options = new FileSystemBrowserOptions()
                {
                    CanCancel = true,
                    MultiSelection = false,
                    Title = _translator.Translate(nameof(Resources.SelectFiles)),
                };
                return _dialogViewModel.ShowFileBrowserDialog(options);
            }
        }

        private bool CanLoadFromFile()
        {
            return !IsBusy;
        }

        public virtual void Clear()
        {
            History.Clear();
            SelectedItem = null;
            RemoveRange(Items.AsEnumerable());
        }

        private bool CanClear()
        {
            return Items?.Any() == true;
        }

        public virtual void Add(MediaItem item)
        {
            using (_busyStack.GetToken())
            {
                var sequence = _sequenceProvider.Get(Items.Select(p => (ISequence)p).ToList());
                item.Sequence = sequence;

                if (Items.Any() != true)
                    History.Push(item.Sequence);

                AddInternal(item);

                if (SelectedItem == null)
                    SelectedItem = Items.First();
            }
        }

        private void AddInternal(MediaItem item)
        {
            item.Playlist = this;
            Items.Add(item);
            Model.MediaItems.Add(item.Model);
        }

        public virtual void AddRange(IEnumerable<MediaItem> items)
        {
            using (_busyStack.GetToken())
            {
                var added = false;
                var sequence = _sequenceProvider.Get(Items.Select(p => (ISequence)p).ToList());

                foreach (var item in items)
                {
                    item.Sequence = sequence;
                    Add(item);
                    sequence++;

                    added = true;
                }

                if (SelectedItem == null && (added || Items.Count > 0))
                    SelectedItem = Items.First();
            }
        }

        public virtual void Remove(MediaItem item)
        {
            using (_busyStack.GetToken())
            {
                while (Items.Contains(item))
                    RemoveInternal(item);
            }
        }

        private void RemoveInternal(MediaItem item)
        {
            Items.Remove(item);
            item.Model.IsDeleted = true;
        }

        public virtual void RemoveRange(IEnumerable<MediaItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (_busyStack.GetToken())
                RemoveRangeInternal(items.ToList());
        }

        public virtual void RemoveRange(IList items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (_busyStack.GetToken())
                RemoveRangeInternal(items.Cast<MediaItem>().ToList());
        }

        private void RemoveRangeInternal(IEnumerable<MediaItem> items)
        {
            foreach (var item in items)
                Remove(item);
        }

        public virtual bool CanRemove(MediaItem item)
        {
            if (Items == null || Items.Count == 0 || item as MediaItem == null)
                return false;

            return Items.Contains(item) && !IsBusy;
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
            using (_busyStack.GetToken())
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
            using (_busyStack.GetToken())
            {
                if (History?.Any() == true)
                {
                    Items.ToList().ForEach(p => p.IsSelected = false);      // deselect all items in the list
                    while (History.Any())
                    {
                        var previous = History.Pop();

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
            return History != null && History.Any();
        }
    }
}
