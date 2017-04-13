﻿using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
    public class Playlist : BaseViewModel<Data.Playlist>, IIsSelected, ISequence, IIdentifier, IChangeState
    {
        private readonly DialogViewModel _dialogViewModel;
        public int ItemCount => Items?.Count ?? 0;

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
        /// contains indices of played <see cref="IMediaItem" />
        /// </summary>
        /// <value>
        /// The history.
        /// </value>
        public Stack<int> History { get; private set; }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public RangeObservableCollection<MediaItem> Items { get; private set; }

        private MediaItem _currentItem;
        /// <summary>
        /// is <see cref="SetActive" /> when a IMediaPlayer picks a <see cref="IMediaItem" /> from this
        /// </summary>
        /// <value>
        /// The current item.
        /// </value>
        public MediaItem CurrentItem
        {
            get { return _currentItem; }
            private set { SetValue(ref _currentItem, value); }
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Playlist"/> class.
        /// </summary>
        /// <param name="dialogViewModel">The dialog view model.</param>
        /// <param name="model">The model.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public Playlist(DialogViewModel dialogViewModel, Data.Playlist model)
            : base(model)
        {
            using (_busyStack.GetToken())
            {
                _dialogViewModel = dialogViewModel;

                Items = new RangeObservableCollection<MediaItem>();
                RepeatModes = new ObservableCollection<RepeatMode>(Enum.GetValues(typeof(RepeatMode)).Cast<RepeatMode>().ToList());
                History = new Stack<int>();

                Title = model.Title;
                Description = model.Description;
                Id = model.Id;
                RepeatMode = (RepeatMode)model.RepeatMode;
                IsShuffeling = model.IsShuffeling;

                if (model.MediaItems == null)
                    throw new ArgumentException($"{model.MediaItems} cannot be null");

                model.MediaItems.ForEach(p => Items.Add(new MediaItem(p)));

                Items.CollectionChanged += (o, e) =>
                {
                    OnPropertyChanged(nameof(ItemCount));
                };

                InitializeCommands();
                IntializeValidation();
            }
        }

        private void InitializeCommands()
        {
            LoadFromFileCommand = new AsyncRelayCommand(LoadFromFile, () => CanLoadFromFile());
            LoadFromFolderCommand = new AsyncRelayCommand(LoadFromFolder, () => CanLoadFromFolder());
            LoadFromUrlCommand = new AsyncRelayCommand(LoadFromUrl, () => CanLoadFromUrl());
        }

        private void IntializeValidation()
        {
            AddRule(Title, new NotNullOrEmptyRule(nameof(Title)));
            AddRule(Description, new NotNullOrEmptyRule(nameof(Description)));
            AddRule(CurrentItem, new NotNullRule(nameof(CurrentItem)));
            AddRule(RepeatModes, new NotNullRule(nameof(RepeatModes)));
            AddRule(Items, new NotNullRule(nameof(Items)));
        }

        private async Task LoadFromUrl()
        {
            using (_busyStack.GetToken())
            {
                var items = await _dialogViewModel.ShowUrlParseDialog();
                var result = items.Select(p => new MediaItem(p));
                Items.AddRange(result);
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
                return _dialogViewModel.ShowFolderBrowserDialog();
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
                return _dialogViewModel.ShowFileBrowserDialog();
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
            CurrentItem = null;
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void Add(MediaItem item)
        {
            using (_busyStack.GetToken())
            {
                if (Items?.Any() == true)
                    item.Sequence = Items.Select(p => p.Sequence).Max() + 1;
                else
                    item.Sequence = 0;

                if (Items.Any() != true)
                    History.Push(item.Sequence);

                item.PlaylistId = Id;
                Items.Add(item);

                if (CurrentItem == null)
                    CurrentItem = Items.First();
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
            if (CurrentItem?.Sequence != null)
                currentIndex = CurrentItem?.Sequence ?? 0;

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
                return CurrentItem;
            else
                return null;
        }

        private MediaItem NextRepeatAll()
        {
            var currentIndex = 0;
            if (CurrentItem?.Sequence != null)
                currentIndex = CurrentItem?.Sequence ?? 0;

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
                var nextItems = Items.Where(p => p.Sequence != CurrentItem?.Sequence); // get all items besides the current one
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

                        if (previous == CurrentItem?.Sequence) // the most recent item in the history, is the just played item, so we wanna skip that
                            continue;

                        if (previous > -1)
                        {
                            var previousItems = Items.Where(p => p.Sequence == previous); // try to get the last played item
                            if (previousItems.Any())
                            {
                                var foundItem = previousItems.First();
                                foundItem.IsSelected = true;

                                Assert.IsTrue(previousItems.Count() > 1, "Warning SelectPrevious returned more than one value, when it should only return one");

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
