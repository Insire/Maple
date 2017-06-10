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
        private readonly IMediaItemMapper _mediaItemMapper;
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

        /// <summary>
        /// Gets the load from file command.
        /// </summary>
        /// <value>
        /// The load from file command.
        /// </value>
        public ICommand LoadFromFileCommand { get; private set; }
        /// <summary>
        /// Gets the load from folder command.
        /// </summary>
        /// <value>
        /// The load from folder command.
        /// </value>
        public ICommand LoadFromFolderCommand { get; private set; }
        /// <summary>
        /// Gets the load from URL command.
        /// </summary>
        /// <value>
        /// The load from URL command.
        /// </value>
        public ICommand LoadFromUrlCommand { get; private set; }
        /// <summary>
        /// Gets or sets the remove range command.
        /// </summary>
        /// <value>
        /// The remove range command.
        /// </value>
        public ICommand RemoveRangeCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the remove command.
        /// </summary>
        /// <value>
        /// The remove command.
        /// </value>
        public ICommand RemoveCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the clear command.
        /// </summary>
        /// <value>
        /// The clear command.
        /// </value>
        public ICommand ClearCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the add command.
        /// </summary>
        /// <value>
        /// The add command.
        /// </value>
        public ICommand AddCommand { get; protected set; }
        /// <summary>
        /// contains indices of played <see cref="IMediaItem" />
        /// </summary>
        /// <value>
        /// The history.
        /// </value>
        public Stack<int> History { get; private set; }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public RangeObservableCollection<MediaItem> Items { get; private set; }

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
            set { SetValue(ref _selectedItem, value, OnChanging: () => SelectionChanging?.Raise(this), OnChanged: () => SelectionChanged?.Raise(this)); }
        }

        private int _sequence;
        /// <summary>
        /// the index of this item if its part of a collection
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
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

        private int _id;
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id
        {
            get { return _id; }
            private set { SetValue(ref _id, value, OnChanged: () => Model.Id = value); }
        }

        private ObservableCollection<RepeatMode> _repeatModes;
        /// <summary>
        /// Gets the repeat modes.
        /// </summary>
        /// <value>
        /// The repeat modes.
        /// </value>
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
        /// <summary>
        /// For grouping, sorting and filtering
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public ICollectionView View
        {
            get { return _view; }
            protected set { SetValue(ref _view, value); }
        }

        private string _createdBy;
        public string CreatedBy
        {
            get { return _createdBy; }
            set { SetValue(ref _createdBy, value, OnChanged: () => Model.CreatedBy = value); }
        }

        private string _updatedBy;
        public string UpdatedBy
        {
            get { return _updatedBy; }
            set { SetValue(ref _updatedBy, value, OnChanged: () => Model.UpdatedBy = value); }
        }

        private DateTime _updatedOn;
        public DateTime UpdatedOn
        {
            get { return _updatedOn; }
            set { SetValue(ref _updatedOn, value, OnChanged: () => Model.UpdatedOn = value); }
        }

        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get { return _updatedOn; }
            set { SetValue(ref _updatedOn, value, OnChanged: () => Model.CreatedOn = value); }
        }

        /// <summary>
        /// Gets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public MediaItem this[int index]
        {
            get { return Items[index]; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Playlist" /> class.
        /// </summary>
        /// <param name="translator">The translator.</param>
        /// <param name="mediaItemMapper">The media item mapper.</param>
        /// <param name="dialogViewModel">The dialog view model.</param>
        /// <param name="model">The model.</param>
        /// <exception cref="System.ArgumentNullException">dialogViewModel</exception>
        /// <exception cref="System.ArgumentException"></exception>
        public Playlist(ILocalizationService translator, IMediaItemMapper mediaItemMapper, ISequenceProvider sequenceProvider, IValidator<Playlist> validator, DialogViewModel dialogViewModel, Data.Playlist model)
            : base(model, validator)
        {
            using (_busyStack.GetToken())
            {
                _itemsLock = new object();
                _sequenceProvider = sequenceProvider ?? throw new ArgumentNullException(nameof(sequenceProvider));
                _dialogViewModel = dialogViewModel ?? throw new ArgumentNullException(nameof(dialogViewModel));
                _translator = translator ?? throw new ArgumentNullException(nameof(translator));
                _mediaItemMapper = mediaItemMapper ?? throw new ArgumentNullException(nameof(mediaItemMapper));

                Items = new RangeObservableCollection<MediaItem>();
                RepeatModes = new ObservableCollection<RepeatMode>(Enum.GetValues(typeof(RepeatMode)).Cast<RepeatMode>().ToList());
                History = new Stack<int>();

                BindingOperations.EnableCollectionSynchronization(Items, _itemsLock);

                _title = model.Title;
                _description = model.Description;
                _id = model.Id;
                _repeatMode = (RepeatMode)model.RepeatMode;
                _isShuffeling = model.IsShuffeling;
                _sequence = model.Sequence;
                _createdBy = model.CreatedBy;
                _createdOn = model.CreatedOn;
                _updatedBy = model.UpdatedBy;
                _updatedOn = model.UpdatedOn;

                if (model.MediaItems == null)
                    throw new ArgumentException($"{model.MediaItems} cannot be null");

                Items.AddRange(_mediaItemMapper.GetMany(model.MediaItems));

                Items.CollectionChanged += (o, e) =>
                {
                    OnPropertyChanged(nameof(Count));
                };

                View = CollectionViewSource.GetDefaultView(Items);
                OnPropertyChanged(nameof(Count));

                InitializeCommands();
                IntializeValidation();

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

        private async Task LoadFromUrl()
        {
            using (_busyStack.GetToken())
            {
                var items = await _dialogViewModel.ShowUrlParseDialog();
                Items.AddRange(_mediaItemMapper.GetMany(items, Id));
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
            Items.Clear();
            History.Clear();
            SelectedItem = null;
        }

        /// <summary>
        /// Determines whether this instance can clear.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can clear; otherwise, <c>false</c>.
        /// </returns>
        private bool CanClear()
        {
            return Items?.Any() == true;
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void Add(MediaItem item)
        {
            using (_busyStack.GetToken())
            {
                var sequence = _sequenceProvider.Get(Items.Select(p => (ISequence)p).ToList());
                item.Sequence = sequence;

                if (Items.Any() != true)
                    History.Push(item.Sequence);

                item.PlaylistId = Id;
                Items.Add(item);

                if (SelectedItem == null)
                    SelectedItem = Items.First();
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
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

                if (SelectedItem == null && added)
                    SelectedItem = Items.First();
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void Remove(MediaItem item)
        {
            using (_busyStack.GetToken())
            {
                while (Items.Contains(item))
                    Items.Remove(item);
            }
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public virtual void RemoveRange(IEnumerable<MediaItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (_busyStack.GetToken())
                Items.RemoveRange(items);
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public virtual void RemoveRange(IList items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (_busyStack.GetToken())
                Items.RemoveRange(items);
        }

        /// <summary>
        /// Determines whether this instance can remove the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Determines whether this instance [can remove range] the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can remove range] the specified items; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanRemoveRange(IList items)
        {
            return items == null ? false : CanRemoveRange(items.Cast<MediaItem>());
        }

        /// <summary>
        /// Nexts this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">RepeatMode</exception>
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

        /// <summary>
        /// Removes the last <see cref="MediaItem" /> from <seealso cref="History" /> and returns it
        /// </summary>
        /// <returns>
        /// returns the last <see cref="MediaItem" /> from <seealso cref="History" />
        /// </returns>
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

        /// <summary>
        /// Determines whether this instance can next.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can next; otherwise, <c>false</c>.
        /// </returns>
        public bool CanNext()
        {
            return Items != null && Items.Any();
        }

        /// <summary>
        /// Determines whether this instance can previous.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can previous; otherwise, <c>false</c>.
        /// </returns>
        public bool CanPrevious()
        {
            return History != null && History.Any();
        }
    }
}
