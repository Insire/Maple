using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using FluentValidation;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    [DebuggerDisplay("{Title}, {Sequence}")]
    public class Playlist : MapleDomainViewModelBase<PlaylistModel>, IIsSelected, ISequence, IIdentifier
    {
        private readonly IDialogViewModel _dialogViewModel;
        private readonly Stack<int> _history;

        public bool IsNew => Model.IsNew;
        public bool IsDeleted => Model.IsDeleted;

        public ICommand LoadFromFileCommand { get; }
        public ICommand LoadFromFolderCommand { get; }
        public ICommand LoadFromUrlCommand { get; }

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

        private ICollectionView _view;
        public ICollectionView View
        {
            get { return _view; }
            protected set { SetValue(ref _view, value); }
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

        [Bindable(true, BindingDirection.OneWay)]
        public MediaItems MediaItems { get; }

        [Bindable(true, BindingDirection.OneWay)]
        public ReadOnlyObservableCollection<RepeatMode> RepeatModes { get; }

        public Playlist(IMapleCommandBuilder commandBuilder, IValidator<Playlist> validator, IDialogViewModel dialogViewModel, PlaylistModel model)
            : base(commandBuilder, validator)
        {
            MediaItems = new MediaItems(commandBuilder);

            using (BusyStack.GetToken())
            {
                _dialogViewModel = dialogViewModel ?? throw new ArgumentNullException(nameof(dialogViewModel));

                _title = model.Title;
                _description = model.Description;
                _repeatMode = (RepeatMode)model.RepeatMode;
                _isShuffeling = model.IsShuffeling;
                _sequence = model.Sequence;

                RepeatModes = new ReadOnlyObservableCollection<RepeatMode>(new ObservableCollection<RepeatMode>(Enum.GetValues(typeof(RepeatMode)).Cast<RepeatMode>()));
                _history = new Stack<int>();

                View = CollectionViewSource.GetDefaultView(MediaItems); // TODO add sorting by sequence

                LoadFromFileCommand = AsyncCommand.Create(LoadFromFile, () => CanLoadFromFile());
                LoadFromFolderCommand = AsyncCommand.Create(LoadFromFolder, () => CanLoadFromFolder());
                LoadFromUrlCommand = AsyncCommand.Create(LoadFromUrl, () => CanLoadFromUrl());

                Add(Messenger.Subscribe<PlayingMediaItemMessage>(OnPlaybackItemChanged, m => m.PlaylistId == Id && MediaItems.Items.Contains(m.Content)));
            }
        }

        private void OnPlaybackItemChanged(PlayingMediaItemMessage message)
        {
            _history.Push(message.Content.Sequence);
        }

        private async Task LoadFromUrl(CancellationToken token)
        {
            using (BusyStack.GetToken())
            {
                var items = await _dialogViewModel.ShowUrlParseDialog(token).ConfigureAwait(true);
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
                var options = new FileSystemFolderBrowserOptions()
                {
                    IncludeSubFolders = false,
                    CanCancel = true,
                    MultiSelection = false,
                    Title = LocalizationService.Translate(nameof(Resources.SelectFolder)),
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
                    Title = LocalizationService.Translate(nameof(Resources.SelectFiles)),
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

        public virtual Task Clear(CancellationToken token)
        {
            _history.Clear();
            MediaItems.SelectedItem = null;
            return MediaItems.Clear(token);
        }

        public virtual async Task Add(MediaItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), $"{nameof(item)} {Resources.IsRequired}");

            using (BusyStack.GetToken())
            {
                var sequence = SequenceService.Get(MediaItems.Items.Select(p => (ISequence)p).ToList());
                item.Sequence = sequence;

                await AddInternal(item).ConfigureAwait(false);

                if (MediaItems.SelectedItem == null)
                    MediaItems.SelectedItem = MediaItems.Items.First();
            }
        }

        private async Task AddInternal(MediaItem item)
        {
            item.Playlist = this;
            await MediaItems.Add(item);

            if (!Model.MediaItems.Contains(item.Model))
                Model.MediaItems.Add(item.Model);
        }

        public virtual async Task AddRange(IEnumerable<MediaItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), $"{nameof(items)} {Resources.IsRequired}");

            using (BusyStack.GetToken())
            {
                var added = false;
                var sequence = 0;
                var collection = items.ToList();
                var item = default(MediaItem);

                for (var i = 0; i < collection.Count; i++)
                {
                    if (i == 0)
                        sequence = SequenceService.Get(MediaItems.Items.Select(p => (ISequence)p).ToList());

                    item = collection[i];

                    if (item == null)
                        throw new ArgumentNullException(nameof(item), $"{nameof(item)} {Resources.IsRequired}");

                    item.Sequence = sequence;
                    await AddInternal(item).ConfigureAwait(false);
                    sequence++;

                    added = true;
                }

                if (MediaItems.SelectedItem == null && (added || MediaItems.Count > 0))
                    MediaItems.SelectedItem = MediaItems.Items.First();
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
                while (MediaItems.Items.Contains(item))
                    RemoveInternal(item);
            }
        }

        private async Task RemoveInternal(MediaItem item)
        {
            if (MediaItems.SelectedItem == item)
                MediaItems.SelectedItem = Next();

            await MediaItems.Remove(item).ConfigureAwait(false);
            item.IsDeleted = true;
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
            if (MediaItems == null || MediaItems.Count == 0)
                return false;

            var mediaItem = item as MediaItem;
            if (mediaItem == null)
                return false;

            return MediaItems.Items.Contains(mediaItem) && !IsBusy;
        }

        protected virtual bool CanRemoveRange(IEnumerable<MediaItem> items)
        {
            return CanClear() && items != null && items.Any(p => MediaItems.Items.Contains(p));
        }

        /// <summary>
        /// Returns the next MediaItem from the Items collection according to their respective sequence and the current RepeatMode
        /// </summary>
        /// <returns></returns>
        public virtual MediaItem Next()
        {
            using (BusyStack.GetToken())
            {
                if (MediaItems != null && MediaItems.Items.Any())
                {
                    switch (RepeatMode)
                    {
                        case RepeatMode.All:
                            return IsShuffeling ? NextShuffle() : NextRepeatAll();

                        case RepeatMode.None:
                            return IsShuffeling ? NextShuffle() : NextRepeatNone();

                        case RepeatMode.Single: return NextRepeatSingle();

                        default:
                            throw new NotImplementedException(nameof(RepeatMode));
                    }
                }

                return null;
            }
        }

        private MediaItem NextRepeatNone()
        {
            var currentIndex = 0;
            if (MediaItems.SelectedItem?.Sequence != null)
                currentIndex = MediaItems.SelectedItem?.Sequence ?? 0;

            if (MediaItems.Count > 1)
            {
                var nextPossibleItems = MediaItems.Items.Where(p => p.Sequence > currentIndex);

                if (nextPossibleItems?.Any() == true) // try to find items after the current one
                {
                    MediaItems.Items.ToList().ForEach(p => p.IsSelected = false);
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
                return MediaItems.SelectedItem;
            else
                return null;
        }

        private MediaItem NextRepeatAll()
        {
            var currentIndex = 0;
            if (MediaItems.SelectedItem?.Sequence != null)
                currentIndex = MediaItems.SelectedItem?.Sequence ?? 0;

            if (MediaItems.Count > 1)
            {
                var nextPossibleItems = MediaItems.Items.Where(p => p.Sequence > currentIndex);

                if (nextPossibleItems.Any()) // try to find items after the current one
                {
                    MediaItems.Items.ToList().ForEach(p => p.IsSelected = false);
                    var foundItem = nextPossibleItems.Where(q => q.Sequence == nextPossibleItems.Select(p => p.Sequence).Min()).First();
                    foundItem.IsSelected = true;
                    return foundItem;
                }
                else // if there are none, use the first item in the list
                {
                    MediaItems.Items.ToList().ForEach(p => p.IsSelected = false);
                    var foundItem = MediaItems.Items.First();
                    foundItem.IsSelected = true;
                    return foundItem;
                }
            }
            else
                return NextRepeatSingle();
        }

        private MediaItem NextShuffle()
        {
            if (MediaItems.Count > 1)
            {
                var nextItems = MediaItems.Items.Where(p => p.Sequence != MediaItems.SelectedItem?.Sequence); // get all items besides the current one
                MediaItems.Items.ToList().ForEach(p => p.IsSelected = false);
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
                MediaItems.Items.ToList().ForEach(p => p.IsSelected = false);      // deselect all items in the list

                if (_history?.Any() == true)
                {
                    while (_history.Any())
                    {
                        var previous = _history.Pop();

                        //if (previous == SelectedItem?.Sequence) // the most recent item in the history, is the just played item, so we wanna skip that
                        //    continue;

                        if (previous > -1)
                        {
                            var previousItems = MediaItems.Items.Where(p => p.Sequence == previous); // try to get the last played item
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
            return MediaItems != null && MediaItems.Items.Any();
        }

        public bool CanPrevious()
        {
            return _history != null && _history.Any();
        }

        protected override Task UnloadInternal(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
