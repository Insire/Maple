using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using InsireBotCore;

namespace InsireBot
{
    public class DataParsingServiceResult : ObservableObject
    {
        public int Count => RefreshCount();

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

        private IList<IMediaItem> _mediaItems;
        public IList<IMediaItem> MediaItems
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

        private IList<Playlist<IMediaItem>> _playlists;
        public IList<Playlist<IMediaItem>> Playlists
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

        private DataParsingServiceResult()
        {
            Playlists = new List<Playlist<IMediaItem>>();
            MediaItems = new List<IMediaItem>();
        }

        /// <summary>
        /// essentially no data was returned, when using this constructor
        /// </summary>
        public DataParsingServiceResult(DataParsingServiceResultType type = DataParsingServiceResultType.None) : this()
        {
            Type = type;
        }

        public DataParsingServiceResult(IList<Playlist<IMediaItem>> items) : this(DataParsingServiceResultType.Playlists)
        {
            Playlists = items;

            Log();
        }

        public DataParsingServiceResult(IList<Playlist<IMediaItem>> items, DataParsingServiceResultType type) : this(type)
        {
            Playlists = items;

            Log();
        }

        public DataParsingServiceResult(Playlist<IMediaItem> item) : this(DataParsingServiceResultType.Playlists)
        {
            Playlists = new List<Playlist<IMediaItem>>()
            {
                item
            };

            Log();
        }

        public DataParsingServiceResult(Playlist<IMediaItem> item, DataParsingServiceResultType type) : this(type)
        {
            Playlists = new List<Playlist<IMediaItem>>()
            {
                item
            };

            Log();
        }

        public DataParsingServiceResult(IMediaItem item) : this(DataParsingServiceResultType.MediaItems)
        {
            MediaItems = new List<IMediaItem>()
            {
                item
            };

            Log();
        }

        public DataParsingServiceResult(IMediaItem item, DataParsingServiceResultType type) : this(type)
        {
            MediaItems = new List<IMediaItem>()
            {
                item
            };

            Log();
        }

        public DataParsingServiceResult(IList<IMediaItem> items) : this(DataParsingServiceResultType.MediaItems)
        {
            MediaItems = items;

            Log();
        }

        public DataParsingServiceResult(IList<IMediaItem> items, DataParsingServiceResultType type) : this(type)
        {
            MediaItems = items;

            Log();
        }

        private void Log()
        {
            if (Count != 1)
                App.Log.Info($"API Call for {Type} returned {Count} Entries");
            else
                App.Log.Info($"API Call for {Type} returned {Count} Entry");
        }

        private int RefreshCount()
        {
            switch(Type)
            {
                case DataParsingServiceResultType.MediaItems:
                    return MediaItems.Count;

                case DataParsingServiceResultType.Playlists:
                    return Playlists.Count;

                case DataParsingServiceResultType.None:
                    if (MediaItems.Count > 0)
                        return MediaItems.Count;
                    if (Playlists.Count > 0)
                        return Playlists.Count;
                    return 0;
                default:
                    App.Log.Warn("DataParsingServiceResult misses an Implementation of DataParsingServiceResultType");
                    return 0;
            }
        }
    }

    public enum DataParsingServiceResultType
    {
        Playlists,
        MediaItems,
        None
    }
}
