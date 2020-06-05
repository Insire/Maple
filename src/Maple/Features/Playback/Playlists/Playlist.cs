using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentValidation;
using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    [DebuggerDisplay("{Title}, {Sequence}")]
    public sealed class Playlist : ViewModelListBase<MediaItem>, IIsSelected, ISequence
    {
        private readonly Stack<int> _history;

        private int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value); }
        }

        private PrivacyStatus _privacyStatus;
        public PrivacyStatus PrivacyStatus
        {
            get { return _privacyStatus; }
            private set { SetValue(ref _privacyStatus, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        private bool _isShuffeling;
        public bool IsShuffeling
        {
            get { return _isShuffeling; }
            set { SetValue(ref _isShuffeling, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }

        private string _thumbnail;
        public string Thumbnail
        {
            get { return _thumbnail; }
            set { SetValue(ref _thumbnail, value); }
        }

        private RepeatMode _repeatMode;
        public RepeatMode RepeatMode
        {
            get { return _repeatMode; }
            set { SetValue(ref _repeatMode, value); }
        }

        public Playlist(IScarletCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
            _history = new Stack<int>();
        }

        public Playlist(Playlist playlist)
            : this(playlist.CommandBuilder)
        {
            Sequence = playlist.Sequence;
            PrivacyStatus = playlist.PrivacyStatus;
            IsSelected = playlist.IsSelected;
            Title = playlist.Title;
            Thumbnail = playlist.Thumbnail;
            RepeatMode = playlist.RepeatMode;

            for (var i = 0; i < playlist.Count; i++)
            {
                var item = playlist[i];

                AddUnchecked(item);
                item.Playlist = this;
            }
        }

        /// <summary>
        /// Returns the next MediaItem from the Items collection according to their respective sequence and the current RepeatMode
        /// </summary>
        /// <returns></returns>
        public MediaItem Next()
        {
            using (BusyStack.GetToken())
            {
                if (Items != null && Items.Any())
                {
                    return RepeatMode switch
                    {
                        RepeatMode.All => IsShuffeling ? NextShuffle() : NextRepeatAll(),

                        RepeatMode.None => IsShuffeling ? NextShuffle() : NextRepeatNone(),

                        RepeatMode.Single => NextRepeatSingle(),

                        _ => throw new NotImplementedException(nameof(RepeatMode)),
                    };
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

        public MediaItem Previous()
        {
            using (BusyStack.GetToken())
            {
                Items.ToList().ForEach(p => p.IsSelected = false);      // deselect all items in the list

                if (_history?.Any() == true)
                {
                    while (_history.Any())
                    {
                        var previous = _history.Pop();

                        //if (previous == SelectedItem?.Sequence) // the most recent item in the history, is the just played item, so we wanna skip that
                        //    continue;

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
