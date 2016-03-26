using System;
using GalaSoft.MvvmLight;

namespace InsireBot.MediaPlayer
{
    public class MediaItem : ObservableObject, IMediaItem
    {
        private double _duration;
        public double Duration
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
            private set
            {
                if (_location != value)
                {
                    _location = value;
                    RaisePropertyChanged(nameof(Location));
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

        private MediaItem()
        {
            ID = Guid.NewGuid();
        }

        public MediaItem(string title,string location)
        {
            Title = title;
            Location = location;
        }
    }
}
