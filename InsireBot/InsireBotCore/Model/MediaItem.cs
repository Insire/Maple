using System;
using System.Diagnostics;
using GalaSoft.MvvmLight;

namespace InsireBotCore
{
    public class MediaItem : ObservableObject, IMediaItem, IIndex, IIdentifier
    {
        private Guid _id;
        public Guid ID
        {
            get { return _id; }
            private set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged(nameof(ID));
                }
            }
        }

        private int _index;
        /// <summary>
        /// An Index managed by the collection an item is inside of
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                RaisePropertyChanged(nameof(Index));
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            private set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged(nameof(Title));
                }
            }
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            private set
            {
                if (_duration != value)
                {
                    _duration = value;
                    RaisePropertyChanged(nameof(Duration));
                }
            }
        }

        private bool _isRestricted;
        public bool IsRestricted
        {
            get { return _isRestricted; }
            private set
            {
                if (_isRestricted != value)
                {
                    _isRestricted = value;
                    RaisePropertyChanged(nameof(IsRestricted));
                }
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    RaisePropertyChanged(nameof(Location));
                }
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged(nameof(IsSelected));
                }
            }
        }

        private bool _isLocalFile;
        public bool IsLocalFile
        {
            get { return _isLocalFile; }
            set
            {
                if (_isLocalFile != value)
                {
                    _isLocalFile = value;
                    RaisePropertyChanged(nameof(IsLocalFile));
                }
            }
        }

        private MediaItem()
        {
            ID = Guid.NewGuid();
            IsRestricted = false;
            IsSelected = false;
            IsLocalFile = false;
            Index = -1;
        }

        ~MediaItem()
        {
            Debug.WriteLine($"{Title} was disposed");
        }

        public MediaItem(string title, Uri location) : this()
        {
            Title = title;
            Location = location.OriginalString;
            IsLocalFile = location.IsFile;
        }

        public MediaItem(string title, Uri location, TimeSpan duration) : this(title, location)
        {
            Duration = duration;
        }

        public MediaItem(string title, Uri location, TimeSpan duration, bool? allowed) : this(title, location, duration)
        {
            IsRestricted = allowed == null ? false : !(bool)allowed;
        }
    }
}
