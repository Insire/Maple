using System.Collections.Generic;

using GalaSoft.MvvmLight;

namespace InsireBotCore
{
    public class Playlist : ObservableObject, IIsSelected, IIndex
    {
        private RangeObservableCollection<IMediaItem> _items;
        public RangeObservableCollection<IMediaItem> Items
        {
            get { return _items; }
            private set
            {
                if (_items != value)
                {
                    _items = value;
                    RaisePropertyChanged(nameof(Items));
                }
            }
        }

        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                RaisePropertyChanged(nameof(Index));
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            private set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        private string _id;
        public string ID
        {
            get { return _id; }
            private set
            {
                _id = value;
                RaisePropertyChanged(nameof(ID));
            }
        }

        public Playlist()
        {
            Items = new RangeObservableCollection<IMediaItem>();
            Title = string.Empty;
            ID = string.Empty;
        }

        public Playlist(string title, string id) : this()
        {
            Title = title;
            ID = id;
        }

        public void Add(IMediaItem item)
        {
            Items.Add(item);
            RaisePropertyChanged(nameof(Items));
        }

        public void AddRange(IEnumerable<IMediaItem> items)
        {
            Items.AddRange(items);
            RaisePropertyChanged(nameof(Items));
        }

        public void Clear()
        {
            Items.Clear();
            RaisePropertyChanged(nameof(Items));
        }

        /// <summary>
        /// Removes all occurences of a MediaItem from the internal list
        /// </summary>
        /// <param name="item"></param>
        public void Remove(IMediaItem item)
        {
            while (Items.Contains(item))
                Items.Remove(item);

            RaisePropertyChanged(nameof(Items));
        }
    }
}
