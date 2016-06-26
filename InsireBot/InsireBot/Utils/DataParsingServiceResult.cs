using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using InsireBotCore;

namespace InsireBot
{
    public class DataParsingServiceResult : ObservableObject
    {
        private DataParsingServiceResultType _type;
        public DataParsingServiceResultType Type
        {
            get { return _type; }
            private set
            {
                if (_type != value)
                {
                    _type = value;
                    RaisePropertyChanged(nameof(Type));
                }
            }
        }

        private int _count;
        public int Count
        {
            get { return _count; }
            private set
            {
                if (_count != value)
                {
                    _count = value;
                    RaisePropertyChanged(nameof(Count));
                }
            }
        }

        private IEnumerable<IMediaItem> _mediaItems;
        public IEnumerable<IMediaItem> MediaItems
        {
            get { return _mediaItems; }
            private set
            {
                if (_mediaItems != value)
                {
                    _mediaItems = value;
                    RaisePropertyChanged(nameof(MediaItems));
                }
            }
        }

        private IEnumerable<Playlist<MediaItem>> _playlists;
        public IEnumerable<Playlist<MediaItem>> Playlists
        {
            get { return _playlists; }
            private set
            {
                if (_playlists != value)
                {
                    _playlists = value;
                    RaisePropertyChanged(nameof(Playlists));
                }
            }
        }

        /// <summary>
        /// essentially no data was returned, when using this constructor
        /// </summary>
        public DataParsingServiceResult()
        {
            Type = DataParsingServiceResultType.None;
            Count = 0;
        }

        public DataParsingServiceResult(IEnumerable<Playlist<MediaItem>> items) : this()
        {
            Type = DataParsingServiceResultType.Playlists;
            Count = items.Count();
            Playlists = items;
        }

        public DataParsingServiceResult(Playlist<MediaItem> item) : this()
        {
            Type = DataParsingServiceResultType.Playlists;
            Count = 1;

            Playlists = new List<Playlist<MediaItem>>()
            {
                item
            };
        }

        public DataParsingServiceResult(MediaItem item) : this()
        {
            Type = DataParsingServiceResultType.MediaItems;
            Count = 1;

            MediaItems = new List<MediaItem>()
            {
                item
            };
        }

        public DataParsingServiceResult(IEnumerable<MediaItem> items) : this()
        {
            Type = DataParsingServiceResultType.MediaItems;
            Count = items.Count();
            MediaItems = items;
        }

        public enum DataParsingServiceResultType
        {
            Playlists,
            MediaItems,
            None
        }
    }
}
