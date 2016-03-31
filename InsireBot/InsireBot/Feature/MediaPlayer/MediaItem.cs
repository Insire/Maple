using System;
using GalaSoft.MvvmLight;

namespace InsireBot.MediaPlayer
{
    public class MediaItem : ObservableObject, IMediaItem
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

        private MediaItem()
        {
            ID = Guid.NewGuid();
        }

        public MediaItem(string title, string location)
        {
            Title = title;
            Location = location;
        }

        public MediaItem(string title, string location, TimeSpan duration) : this(title, location)
        {
            Duration = duration;
        }

        public MediaItem(string title, string location, TimeSpan duration, bool? allowed) : this(title, location, duration)
        {
            IsRestricted = allowed == null ? false : !(bool)allowed;
        }
    }
}
