using System;
using MvvmScarletToolkit;

namespace InsireBot
{
    public class MediaItem : ObservableObject, IMediaItem, ISequence, IIdentifier
    {
        private Guid _id;
        public Guid ID
        {
            get { return _id; }
            private set { SetValue(ref _id, value); }
        }

        private int _index;
        /// <summary>
        /// An Index managed by the collection this <see cref="MediaItem"/> is part of
        /// </summary>
        public int Sequence
        {
            get { return _index; }
            set { SetValue(ref _index, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            private set { SetValue(ref _title, value); }
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            private set { SetValue(ref _duration, value); }
        }

        private bool _isRestricted;
        public bool IsRestricted
        {
            get { return _isRestricted; }
            private set { SetValue(ref _isRestricted, value); }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            private set { SetValue(ref _location, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        private bool _isLocalFile;
        public bool IsLocalFile
        {
            get { return _isLocalFile; }
            private set { SetValue(ref _isLocalFile, value); }
        }

        private MediaItem()
        {
            ID = Guid.NewGuid();
            IsRestricted = false;
            IsSelected = false;
            IsLocalFile = false;
            Sequence = -1;
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

        public override string ToString()
        {
            var result = Title == string.Empty ? Location : Title;
            return result;
        }
    }
}
